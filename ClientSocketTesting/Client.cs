using System;
using System.Net;
using System.Net.Sockets;

namespace ClientSocketTesting
{
    public class Client
    {
        private TcpClient clientSocket = new TcpClient();
        public Client(int port,Action<String> inAction)
        { 
            /*This is my attempt at creating a socket and Binding,
             Notice the difference between and the use of the Server*/
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            clientSocket.Connect(ipAddress,5555);
        }
    }
}