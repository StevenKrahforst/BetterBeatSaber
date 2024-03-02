using LiteNetLib.Utils;

namespace BetterBeatSaber.Shared.Models;

public struct Player : INetSerializable {

    public ulong Id { get; set; }
    public string Name { get; set; }
    
    public void Serialize(NetDataWriter writer) {
        writer.Put(Id);
        writer.Put(Name);
    }

    public void Deserialize(NetDataReader reader) {
        Id = reader.GetULong();
        Name = reader.GetString();
    }

}