using System;

using LiteNetLib.Utils;

namespace BetterBeatSaber.Shared.Extensions;

public static class NetDataWriterExtensions {

    public static void Put(this NetDataWriter writer, Guid guid) =>
        writer.Put(guid.ToByteArray());
    
    public static Guid GetGuid(this NetDataReader reader) {
        var data = new byte[16];
        reader.GetBytes(data, 16);
        return new Guid(data);
    }

}