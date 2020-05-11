using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApp2
{
    class Program
    {
        //Will be used to transmit messages out to the clients
        private static List<Socket> clients = new List<Socket>();
        
        static void Main(string[] args)
        {
            //Keeps count of the clients
            int count = 0;
            
            /*We get the information fo the server and set up a listener for it*/
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint serverConnection = new IPEndPoint(ipAddress,5555);
            
            Socket listener = new Socket(SocketType.Stream,ProtocolType.Tcp);

            /*This is where the client and server diverge in their implementation,
             this is the point where the listener is binned to a port and is listening*/
            try
            {
                listener.Bind(serverConnection);
                listener.Listen(5);
            }catch (ArgumentNullException e)
            {
                Console.WriteLine("Null Connection: {0}", e.ToString());

            }catch (SocketException e)
            {
                Console.WriteLine("Socket could not be accessed: {0}", e.ToString());
            }catch (Exception e)
            {
                Console.WriteLine("Unspecified Exceptions: {0}", e.ToString());
            }

            //We wait for our first client to come in
            //firstClient Socket will be associated with that one client
            Console.WriteLine("Waiting for the client to come in");
            while (true)
            {
                //Get the next client
                Socket inClient = listener.Accept();
                clients.Add(inClient);
                
                //Start a new Thread for this Client
                Thread newClient = new Thread(()=>HandleClients(inClient, count));
                newClient.Start();
                count++;
            }
            
            
            
        }//End of Main

        private static void HandleClients(Socket inClient, int count)
        {
            //After we get a client we can begin our execution
            Console.WriteLine("New client connected");
            byte[] clientMessage = new byte[10025];
            while (true)
            {
                try
                {
                    //Receive from the Client
                    inClient.Receive(clientMessage);
                    
                    //If the client has disconnected then we just end the program
                    if (clientMessage[0] == default(byte))
                    {
                        throw new SystemException();
                    }
                    
                    Console.Write($"Client #{count} said: ");
                    Console.WriteLine(System.Text.Encoding.ASCII.GetString(clientMessage) + "\n");
                    MessageToClients(count,System.Text.Encoding.ASCII.GetString(clientMessage));
                    Array.Clear(clientMessage,0,clientMessage.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong, closed Socket");
                    clients.Remove(inClient);
                    break;
                }
            }//End of While(true) loop
        }//End of HandleClients

        private static void MessageToClients(int clientNumber,string message)
        {
            foreach (var i in clients)
            {
                i.Send(System.Text.Encoding.ASCII.GetBytes($"Client#{clientNumber} said {message}"));
            }
        }
        

        
    }
}