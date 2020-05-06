
using System.Net;
using System.Net.Sockets;

namespace ServerSocketTesting
{
    public class Client
    {
        /*These two lines initialize*/
        private static TcpClient _socketClient = default(TcpClient);
        private static NetworkStream _clientStream = default(NetworkStream);

        public Client()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            _socketClient.Connect(ipAddress,5555);
            _clientStream = _socketClient.GetStream();
        }
    }
}