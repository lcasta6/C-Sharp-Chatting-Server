using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ServerSocketTesting
{
    public class Server
    {
        private TcpListener serverSocket = default(TcpListener);
        public Server(int inPort, Action<String> inCall)
        {
            /*ipAddress, is only here for my own experimentation
             I also created the socket using the IPAdress and port, this 
             does the Binding as well.*/
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            serverSocket = new TcpListener(ipAddress, inPort);
            
            /*Declare a new Thread and start it, this Thread will star the server*/
            Thread serverThread = new Thread (() => TheServer.StartServer(serverSocket,inCall));
            serverThread.Start();
        }

        public class TheServer
        {
            /*I'm doing copies of the variables I'm going to need from the outer Server
             class, I am unsure of better options than this as of now*/
            private static int _counter;
            private static Action<String> _localCall;
            private static TcpListener _localServer;
            private static List<TcpClient> clients = new List<TcpClient>();

            public static void StartServer(TcpListener serverSocket, Action<String> inCall)
            {
                /*Make a local reference to the TCPClient and start the listener
                 The server will start taking in clients in the "startGame()" method*/
                _localServer = serverSocket;
                _localServer.Start();
                
                _localCall = inCall;
                _localCall("The server has started, waiting for clients.");
                
                startGame();
            }

            public static void startGame()
            {
                while (true)
                {
                    /*We accept the client that connects to the port,
                     add it to our List, and display it on the output*/
                    _counter++;
                    TcpClient clientSocket = _localServer.AcceptTcpClient();
                    clients.Add(clientSocket);
                    
                    /*Let all the Clients know that someone new joined*/
                    _localCall("New Client Connected #"+_counter);

                    /*Start a new Client Thread*/
                }
            }
            
            /*Updates all the clients in our client Array*/
            private static void UpdateClients(String message)
            {
                foreach (TcpClient i in clients)
                {
                    /*Setting up the byte stream to send out*/
                    NetworkStream outStream = i.GetStream();
                    Byte[] broadcastBytes = default(Byte[]);

                    /*Setting up the value of the outBytes, this is
                     what's going to be send out using the outStream*/
                    broadcastBytes = Encoding.ASCII.GetBytes(message);
                    
                    /*Send out the message to our client(s)*/
                    outStream.Write(broadcastBytes,0,broadcastBytes.Length);
                    outStream.Flush();
                }
                
            }
            

        }




    }
}
