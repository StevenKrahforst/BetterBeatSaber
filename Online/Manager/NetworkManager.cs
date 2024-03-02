using System;
using System.Threading;

using LiteNetLib;
using LiteNetLib.Utils;

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
        Listener.PeerConnectedEvent += OnConnectedEvent;
        Listener.PeerDisconnectedEvent += OnDisconnectedEvent;
        Listener.NetworkReceiveEvent += OnReceiveEvent;
    }

    #region Event Handlers
    
    private void OnConnectedEvent(NetPeer _) =>
        OnConnected?.Invoke();
    
    private void OnDisconnectedEvent(NetPeer _, DisconnectInfo __) =>
        OnDisconnected?.Invoke();

    private void OnReceiveEvent(NetPeer _, NetPacketReader reader, byte __, DeliveryMethod ___) =>
        PacketProcessor.ReadAllPackets(reader);

    #endregion
    
    public void Initialize() =>
        Connect();

    public void Dispose() =>
        Disconnect();
    
    public void Tick() {
        while (!Console.KeyAvailable) {
            //client.PollEvents();
            Thread.Sleep(15);
        }
    }
    
    public void RegisterPacketListener<T>(Action<T> listener) where T : INetSerializable, new() =>
        PacketProcessor.SubscribeNetSerializable(listener);

    public void UnregisterPacketHandler<T>() where T : INetSerializable =>
        PacketProcessor.RemoveSubscription<T>();

    public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : INetSerializable {
        Writer.Reset();
        PacketProcessor.WriteNetSerializable(Writer, ref packet);
        Peer?.Send(Writer, deliveryMethod);
    }

    private void Connect() {
        
        Listener.NetworkReceiveEvent -= OnReceiveEvent;
        Listener.PeerConnectedEvent -= OnConnectedEvent;
        Listener.PeerDisconnectedEvent -= OnDisconnectedEvent;
        
        Manager.Start();
        Peer = Manager.Connect("", 2, "");
        
    }

    private void Disconnect() {
        Manager.Stop();
        Listener.NetworkReceiveEvent -= OnReceiveEvent;
    }

}