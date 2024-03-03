using BetterBeatSaber.Server.Models;
using BetterBeatSaber.Server.Server.Interfaces;

using LiteNetLib;
using LiteNetLib.Utils;

namespace BetterBeatSaber.Server.Server;

public sealed class Connection(
    IServer server,
    NetPeer peer,
    Player player
) : IConnection {

    public IServer Server { get; } = server;
    public NetPeer Peer { get; } = peer;

    public Player Player { get; set; } = player;

    public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : INetSerializable {
        var writer = new NetDataWriter();
        Server.PacketProcessor.WriteNetSerializable(writer, ref packet);
        Peer.Send(writer, deliveryMethod);
    }

}