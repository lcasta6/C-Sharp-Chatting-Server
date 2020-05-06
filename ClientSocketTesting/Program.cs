using System;

namespace ClientSocketTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a Client Port");
            Client clientConnection;

            try
            {
                Action<String> action = s => Console.WriteLine(s);
                int clientPort = Int32.Parse(Console.ReadLine() ?? "5555");
                
                clientConnection = new Client(clientPort,action);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
        }
    }
}
