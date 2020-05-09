using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
            
            /*At this point we can have the Client run on it's own Thread.*/
            Thread clientThread = new Thread(() => clientChat(_outMessage,_outStream));
            clientThread.Start();
            
            /*Now I'm going to attempt to write out a message to the server
            _outMessage = System.Text.Encoding.ASCII.GetBytes("Hello Server");

            _outStream.Write(_outMessage);
            _outStream.Flush();*/
            
        }

        public void clientChat(byte[] inMessage, NetworkStream inStream)
        {
            while (true)
            {
                /*Get a message from the Client and send it to the Server*/
                Console.Write("Message to Server: ");
                inMessage = System.Text.Encoding.ASCII.GetBytes(Console.ReadLine());
                
                inStream.Write(inMessage);
                inStream.Flush();
            }
            
        }
    }
}