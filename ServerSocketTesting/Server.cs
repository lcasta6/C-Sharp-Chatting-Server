using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ServerSocketTesting
{
    public class Server
    {
        public Server(int inPort, Action<String> inCall)
        {
            /*ipAddress, is only here for my own experimentation
             I also created the socket using the IPAdress and port, this 
             does the Binding as well.*/
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var serverSocket = new TcpListener(ipAddress, inPort);
            
            /*Declare a new Thread and start it, this Thread will star the server
             Notice how this needs the lambda to instantiate the an instance the the server class*/
            var serverThread = new Thread (() => TheServer.StartServer(serverSocket,inCall));
            serverThread.Start();
        }

        private static class TheServer
        {
            /*I'm doing copies of the variables I'm going to need from the outer Server
             class, I am unsure of better options than this as of now*/
            private static int _counter;
            private static Action<String> _localCall;
            
            private static TcpListener _localServer;
            
            private static readonly List<TcpClient> Clients = new List<TcpClient>();
            private static byte[] _buffer;

            private static NetworkStream _inStream;

            public static void StartServer(TcpListener serverSocket, Action<string> inCall)
            {
                /*Make a local reference to the TCPClient and start the listener
                 The server will start taking in clients in the "startGame()" method*/
                _localServer = serverSocket;
                _localServer.Start();
                
                _buffer = new byte[100025];
                
                _localCall = inCall;
                _localCall("The server has started, waiting for clients.");
                
                StartGame();
            }

            private static void StartGame()
            {
                while (true)
                {
                    /*We accept the client that connects to the port,
                     add it to our List, and display it on the output*/
                    _counter++;
                    TcpClient clientSocket = _localServer.AcceptTcpClient();
                    Clients.Add(clientSocket);
                    
                    /*Let all the Clients know that someone new joined*/
                    _localCall("New Client Connected #"+_counter);

                    try
                    {
                        /*Get Initial message from the client*/
                        _inStream = clientSocket.GetStream();
                        _inStream.Read(_buffer, 0, 10025);
                        string inMessage = System.Text.Encoding.ASCII.GetString(_buffer);

                        _localCall(inMessage);
                        
                        /*Have this client run on it's own threat*/
                        Thread newClientThread = new Thread(HandleClient);
                        newClientThread.Start();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    


                }
            }

            private static void HandleClient()
            {
                while (true)
                {
                    _inStream.Read(_buffer,0,10025);
                    /*Notice the difference here from the Client side, on the client
                     side we're getting the bytes of a String to transmit it through
                     our socket.  Here we're taking a byte stream and converting it
                     to a String.*/
                    /*How would this be done another data type?*/
                    string inMessage = System.Text.Encoding.ASCII.GetString(_buffer);

                    _localCall(inMessage);
                    _inStream.Flush();
                }
                
            }
            
            /*Updates all the clients in our client Array*/
            private static void UpdateClients(String message)
            {
                foreach (TcpClient i in Clients)
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
                
            }//End of updateClients
        }
    }
}
