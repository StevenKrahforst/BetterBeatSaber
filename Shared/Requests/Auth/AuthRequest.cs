using LiteNetLib.Utils;

namespace BetterBeatSaber.Shared;

public struct AuthRequest : INetSerializable {

    public string Ticket { get; set; }

    public void Serialize(NetDataWriter writer) =>
        writer.Put(Ticket);

    public void Deserialize(NetDataReader reader) =>
        Ticket = reader.GetString();

}