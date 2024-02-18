using System.Net;
using System.Net.Sockets;

namespace BetterBeatSaber.Online.Manager;

public sealed class NotificationManager {

    private struct XSOMessage
    {
        public int messageType { get; set; }
        public int index { get; set; }
        public float volume { get; set; }
        public string audioPath { get; set; }
        public float timeout { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string icon { get; set; }
        public float height { get; set; }
        public bool useBase64Icon { get; set; }
        public string sourceApp { get; set; }
    }

    private const int Port = 42069;

    private static void Main(string[] args)
    {
        IPAddress broadcastIP = IPAddress.Parse("127.0.0.1");
        Socket broadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint endPoint = new IPEndPoint(broadcastIP, Port);

        XSOMessage msg = new XSOMessage();
        msg.messageType = 1;
        msg.title = "Example Notification!";
        msg.content = "It's an example!";
        msg.height = 120f;
        msg.sourceApp = "XSOverlay_Example_UDP";
        msg.timeout = 6;
        msg.volume = 0.5f;
        msg.audioPath = "default";
        msg.useBase64Icon = false;
        msg.icon = "default";

        //byte[] byteBuffer = JsonSerializer.SerializeToUtf8Bytes(msg);
        //broadcastSocket.SendTo(byteBuffer, endPoint);
    }

}