using System;

using BetterBeatSaber.Shared.Extensions;

using LiteNetLib.Utils;

namespace BetterBeatSaber.Shared.Packets;

public struct RequestPacket<T> : INetSerializable where T : struct, INetSerializable {

    public Guid Id { get; set; }
    public T Data { get; set; }

    public void Serialize(NetDataWriter writer) {
        writer.Put(Id);
        writer.Put(Data);
    }

    public void Deserialize(NetDataReader reader) {
        Id = reader.GetGuid();
        Data = reader.Get<T>();
    }

}