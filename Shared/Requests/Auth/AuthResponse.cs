using BetterBeatSaber.Shared.Models;

using LiteNetLib.Utils;

namespace BetterBeatSaber.Shared;

public struct AuthResponse : INetSerializable {

    public bool Success { get; set; }
    public Player? Player { get; set; }
    
    public void Serialize(NetDataWriter writer) {
        writer.Put(Success);
        if(Success && Player.HasValue)
            writer.Put(Player.Value);
    }

    public void Deserialize(NetDataReader reader) {
        Success = reader.GetBool();
        if(Success)
            Player = reader.Get<Player>();
    }

}