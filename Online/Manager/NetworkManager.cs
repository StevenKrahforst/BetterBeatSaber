using System;
using System.Collections;
using System.Net;

using BetterBeatSaber.Online.Api;
using BetterBeatSaber.Shared.Models;

using JetBrains.Annotations;

using LiteNetLib;
using LiteNetLib.Utils;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Online.Manager;

internal sealed class NetworkManager : IInitializable, IDisposable, ITickable {

    public event Action? OnConnected;
    public event Action? OnDisconnected;
    
    private EventBasedNetListener Listener { get; } = new();
    private NetManager Manager { get; }
    private NetPacketProcessor PacketProcessor { get; } = new();
    private NetDataWriter Writer { get; } = new();
    private NetPeer? Peer { get; set; }
    
    [Inject, UsedImplicitly]
    private readonly SteamPlatformUserModel _platformUserModel = null!;
    
    public Player? Player { get; private set; }
    
    public NetworkManager() {
        Manager = new NetManager(Listener) {
            EnableStatistics = true,
            AutoRecycle = true
            #if DEBUG
            ,
            BroadcastReceiveEnabled = true,
            SimulateLatency = true,
            SimulationMinLatency = 1,
            SimulationMaxLatency = 50
            #endif
        };
    }

    #region Event Handlers

    private void OnConnectedEvent(NetPeer _) {
        OnConnected?.Invoke();
        Console.WriteLine("CONNNNNNNNNNN");
    }
    
    private void OnDisconnectedEvent(NetPeer _, DisconnectInfo __) =>
        OnDisconnected?.Invoke();

    private void OnReceiveEvent(NetPeer _, NetPacketReader reader, byte __, DeliveryMethod ___) =>
        PacketProcessor.ReadAllPackets(reader);

    #endregion

    #region Init & Exit

    public void Initialize() =>
        global::BetterBeatSaber.Utilities.SharedCoroutineStarter.Instance.StartCoroutine(Connect());

    public void Dispose() =>
        Disconnect();
    
    #endregion
    
    public void Tick() =>
        Manager.PollEvents();

    #region Public
    
    public void RegisterPacketListener<T>(Action<T> listener) where T : INetSerializable, new() =>
        PacketProcessor.SubscribeNetSerializable(listener);

    public void UnregisterPacketHandler<T>() where T : INetSerializable =>
        PacketProcessor.RemoveSubscription<T>();

    public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : INetSerializable {
        Writer.Reset();
        PacketProcessor.WriteNetSerializable(Writer, ref packet);
        Peer?.Send(Writer, deliveryMethod);
    }
    
    #endregion

    #region Private

    private IEnumerator Connect() {
        
        Console.WriteLine("CONNECTING TO SERVER...");
        
        yield return new WaitUntil(() => SteamManager.Initialized);

        var ticketTask = _platformUserModel.GetUserAuthToken();
        yield return new WaitUntil(() => ticketTask.IsCompleted);

        if(string.IsNullOrEmpty(ticketTask.Result.token))
            yield break;

        var request = new ApiRequest("/server");
        yield return request.SendWebRequest();
        
        if(request.Failed)
            yield break;
        
        var addressSplit = request.ContentString.Split(':');
        if(addressSplit.Length != 2 || !IPAddress.TryParse(addressSplit[0], out var address) || !int.TryParse(addressSplit[1], out var port))
            yield break;
        
        Connect(new IPEndPoint(address, port), ticketTask.Result.token);
        
    }
    
    private void Connect(IPEndPoint endpoint, string ticket) {
        
        Listener.PeerConnectedEvent += OnConnectedEvent;
        Listener.PeerDisconnectedEvent += OnDisconnectedEvent;
        Listener.NetworkReceiveEvent += OnReceiveEvent;
        
        Manager.Start();

        var writer = new NetDataWriter();
        writer.Put((byte) 0x25);
        writer.Put(ticket);

        Peer = Manager.Connect(endpoint, writer);
        
    }

    private void Disconnect() {
        Manager.Stop();
        Listener.PeerConnectedEvent -= OnConnectedEvent;
        Listener.PeerDisconnectedEvent -= OnDisconnectedEvent;
        Listener.NetworkReceiveEvent -= OnReceiveEvent;
    }
    
    #endregion

}