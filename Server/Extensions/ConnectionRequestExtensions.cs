using System.Text;

using LiteNetLib;

namespace BetterBeatSaber.Server.Extensions;

public static class ConnectionRequestExtensions {

    public static void Reject(this ConnectionRequest request, string message) =>
        request.Reject(Encoding.UTF8.GetBytes(message));

}