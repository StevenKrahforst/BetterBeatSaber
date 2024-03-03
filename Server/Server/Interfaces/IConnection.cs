using BetterBeatSaber.Server.Models;

using LiteNetLib;
using LiteNetLib.Utils;

namespace BetterBeatSaber.Server.Server.Interfaces;

public interface IConnection {

    public IServer Server { get; }
    public NetPeer Peer { get; }
    
    public Player Player { get; set; }
    
    public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : INetSerializable;

}