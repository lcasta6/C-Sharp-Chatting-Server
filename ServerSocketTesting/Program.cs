using System;


namespace ServerSocketTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            /*This is just the start field where we'll simple get information for the Server Class*/
            Console.WriteLine("Please enter a Server port: ");
            Server serverConnection;
            try
            {
                Action<String> action = s => Console.WriteLine(s);
                int serverPort = Int32.Parse(Console.ReadLine() ?? "5555");
                
                /*our Server will control what gets printed out into the Console.*/
                serverConnection = new Server(serverPort, action);
                
            }catch(Exception ie){Console.WriteLine(ie.Message);}

        }
    }
}
