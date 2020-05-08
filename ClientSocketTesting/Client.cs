using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ClientSocketTesting
{
    public class Client
    {
        private readonly TcpClient _clientSocket = new TcpClient();
        private NetworkStream _outStream;
        private byte[] _outMessage;
        public Client(int port,Action<String> inAction)
        {
            /*This is my attempt at creating a socket and Binding,
             Notice the difference with the use of the Server*/
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            _clientSocket.Connect(ipAddress,5555);
            _outStream = _clientSocket.GetStream();
            
            /*Now I'm going to attempt to write out a message to the server*/
            _outMessage = System.Text.Encoding.ASCII.GetBytes("little man looser"+"$");

            _outStream.Write(_outMessage);
            _outStream.Flush();
            
        }
    }
}