using LiteNetLib.Utils;

namespace BetterBeatSaber.Server.Server.Interfaces;

public interface IServer : IHostedService {

    public Server.ServerOptions Options { get; }
    
    public IList<IConnection> Connections { get; }
    
    internal NetPacketProcessor PacketProcessor { get; }

}