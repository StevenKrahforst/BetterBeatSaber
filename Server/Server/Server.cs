using System.Collections.Concurrent;
using System.Net;
using System.Security.Authentication;

using BetterBeatSaber.Server.Extensions;
using BetterBeatSaber.Server.Models;
using BetterBeatSaber.Server.Server.Interfaces;
using BetterBeatSaber.Server.Services.Interfaces;
using BetterBeatSaber.Server.Services.Steam;

using LiteNetLib;
using LiteNetLib.Utils;

using Microsoft.Extensions.Options;

namespace BetterBeatSaber.Server.Server;

public sealed class Server : BackgroundService, IServer {

    public ServerOptions Options { get; }

    private ILogger<Server> Logger { get; }
    private IServiceScopeFactory ServiceScopeFactory { get; }
    
    private EventBasedNetListener Listener { get; } = new();
    private NetManager Manager { get; }
    public NetPacketProcessor PacketProcessor { get; } = new();

    public IList<IConnection> Connections { get; } = new List<IConnection>();
    
    private ConcurrentDictionary<IPEndPoint, Player> ConnectionQueue { get; } = new();
    
    public Server(IOptions<ServerOptions> options, ILogger<Server> logger, IServiceScopeFactory serviceScopeFactory) {
        Options = options.Value;
        Logger = logger;
        ServiceScopeFactory = serviceScopeFactory;
        Manager = new NetManager(Listener);
    }

    #region Event Handlers

    private void OnConnectionRequest(ConnectionRequest request) {
        Logger.LogInformation("Connection request from {Ip}", request.RemoteEndPoint.Address);
        if (request.Data.GetByte() != 0x25 || Connections.Count >= Options.MaxConnections)
            request.Reject();
        else
            Task.Run(() => HandleConnectionRequest(request));
    }

    private async Task HandleConnectionRequest(ConnectionRequest request) {
        
        using var scope = ServiceScopeFactory.CreateScope();

        var steamService = scope.ServiceProvider.GetRequiredService<ISteamService>();
        
        var ticket = request.Data.GetString();
        if (ticket == null) {
            request.Reject("Invalid ticket");
            return;
        }

        var steamId = 0ul;
        ProfileResponsePlayer? playerSummary = null;
        try {

            var (authParams, authError) = await steamService.Authenticate(620980u, ticket);
            if (authParams == null || authError != null)
                throw new AuthenticationException(authError?.ErrorDescription ?? "Unknown error");

            if(!ulong.TryParse(authParams.SteamId, out steamId) || steamId == 0ul)
                throw new Exception("Failed to parse Steam ID");
            
            playerSummary = await steamService.GetPlayerSummary(steamId);

        } catch (AuthenticationException exception) {
            request.Reject(exception.Message);
            return;
        } catch (Exception exception) {
            Logger.LogWarning("An unknown Exception occured while authenticating ({Ip})", request.RemoteEndPoint.Address);
            Logger.LogWarning("Exception: {Exception}", exception);
        }

        if (playerSummary == null || steamId == 0ul) {
            request.Reject("Unknown error");
            return;
        }
        
        var playerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();

        var player = await playerService.GetAndUpdateOrCreatePlayer(steamId, playerSummary.PersonaName);
        if (player == null) {
            request.Reject("Failed to create player");
            return;
        }

        ConnectionQueue.TryAdd(request.RemoteEndPoint, player);
        
        request.Accept();
        
    }
    
    private void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
        var connection = GetConnectionByPeer(peer);
        if (connection != null) {
            Connections.Remove(connection);
            Logger.LogInformation("Player {Name} disconnected ({Reason})", connection.Player.Name, disconnectInfo.Reason.ToString());
        } else
            Logger.LogWarning("Peer {Id} ({Ip}) disconnected without a Connection being removed", peer.Id, peer.Address);
    }

    private void OnPeerConnected(NetPeer peer) {
        if (ConnectionQueue.TryGetValue(peer, out var player)) {
            Connections.Add(new Connection(this, peer, player));
            Logger.LogInformation("Player {Name} connected", player.Name);
        } else
            peer.Disconnect();
    }
    
    private void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte _, DeliveryMethod __) {
        var connection = GetConnectionByPeer(peer);
        if(connection != null)
            PacketProcessor.ReadAllPackets(reader, connection);
    }

    #endregion

    #region Private

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        Listen();
        while (!stoppingToken.IsCancellationRequested) {
            Manager.PollEvents();
            Thread.Sleep(25);
        }
        Unlisten();
        return Task.CompletedTask;
    }

    private void Listen() {

        Manager.Start();
        
        Listener.ConnectionRequestEvent += OnConnectionRequest;
        Listener.PeerConnectedEvent += OnPeerConnected;
        Listener.PeerDisconnectedEvent += OnPeerDisconnected;
        Listener.NetworkReceiveEvent += OnNetworkReceive;
        
    }

    private void Unlisten() {
        
        Manager.Stop();
        
        Listener.ConnectionRequestEvent -= OnConnectionRequest;
        Listener.PeerConnectedEvent -= OnPeerConnected;
        Listener.PeerDisconnectedEvent -= OnPeerDisconnected;
        Listener.NetworkReceiveEvent -= OnNetworkReceive;
        
    }

    private IConnection? GetConnectionByPeer(NetPeer peer) =>
        Connections.FirstOrDefault(connection => connection.Peer.Id == peer.Id);

    #endregion
    
    public record ServerOptions {

        public required int Port { get; init; }
        public required int MaxConnections { get; init; }
        public required string Address { get; init; }

    }

}