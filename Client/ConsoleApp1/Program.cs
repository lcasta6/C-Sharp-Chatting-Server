using System;
using System.Buffers.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set up an ipAdress to local host and then
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint clientConnection = new IPEndPoint(ipAddress,5555);
            Socket clientSocket = new Socket(SocketType.Stream,ProtocolType.Tcp);
            
            //Try to connect to the remote device and throw exceptions if necessary
            //Besides ArgumentNull And SocketException all will be handled by the last catch
            try
            {
                clientSocket.Connect(clientConnection);
                Console.WriteLine("Connection Established");
            }catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNull: {0}", e.Message);
            }catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e.Message);
            }catch (Exception e)
            {
                Console.WriteLine("Some other exception occured: ");
                Console.WriteLine($"{e.GetType()} was thrown message {e.Message}");
            }
            
            //At this point the client should be connected so they should be able to receive and
            //Transmit messages to the server.
            Thread receiveMessages = new Thread(()=>ReceiveMessages(clientSocket));           
            receiveMessages.Start();
            
            //For the rest of this code we'll set up the reception back
            byte[] serverMessages = new byte[10025];
            while (true)
            {
                clientSocket.Receive(serverMessages);
                Console.WriteLine(System.Text.Encoding.ASCII.GetString(serverMessages));
                Array.Clear(serverMessages,0,serverMessages.Length);
            }
        }//End of Main

        private static void ReceiveMessages(Socket clientSocket)
        {
            //Sending Messages to the server
            while (true)
            {
                string stringMessage = Console.ReadLine();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(stringMessage);
                clientSocket.Send(msg);
                Thread.Sleep(200);
            }
        }
        
    }
}