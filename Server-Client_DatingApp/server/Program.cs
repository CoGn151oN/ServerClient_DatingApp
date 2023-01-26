using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

namespace server
{
    internal class Program
    {
        
        private static bool listen = true;
        private static TcpListener listener;
        static void Main(string[] args)
        {
            User.LoadUsers("Users.xml");
            Console.WriteLine("Users loaded successfully!");

            dates.LoadResources("dates.xml");
            Console.WriteLine("Resources loaded successfully!");

            IPAddress ip = IPAddress.Parse(ConfigurationManager.AppSettings["ip"].ToString());
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            Console.WriteLine("Configuration loaded!\n - IP: {0}\n - Port: {1}", ip, port);

            listener = new TcpListener(ip, port);
            listener.Start();
            Thread listenerThread = new Thread(WaitingForClients);
            listenerThread.Start();
            Console.WriteLine("Listener started!");

            Console.WriteLine("Press ENTER to close the program!");
            Console.ReadLine();

            listen = false;
            listener.Stop();
            Client.CloseAllClient();
        }
        static private void WaitingForClients()
        {
            while (listen)
            {
                if (listener.Pending())
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Client c = new Client(client);
                }
            }
        }
    }
}
