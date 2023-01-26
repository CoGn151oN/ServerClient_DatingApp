using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace server
{
    internal class Client
    {
        public static List<Client> clients = new List<Client>();
        public static void CloseAllClient()
        {
            foreach (Client c in new List<Client>(Client.clients))
            {
                c.Close();
            }
        }
        private User currentUser = null;
        public Client(TcpClient TcpClient)
        {
            this.tcpClient = TcpClient;
            reader = new StreamReader(TcpClient.GetStream());
            writer = new StreamWriter(TcpClient.GetStream());
            clients.Add(this);
            ClientThread = new Thread(ReadCommands);
            ClientThread.Start();
            Console.WriteLine("New client started!");
        }
        private TcpClient tcpClient;
        private StreamReader reader;
        private StreamWriter writer;
        public Thread ClientThread;
        private void ReadCommands()
        {
            try
            {
                while (true)
                {
                    string command = reader.ReadLine();
                    if (command == null) break;
                    Interpret(command);
                    Console.WriteLine("Command recived: {0}", command);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Close();
                Console.WriteLine("Client disconnected");
            }
        }

        private void Interpret(string cmd)
        {        
            string[] data = cmd.Split('|');
            switch (data[0])
            {
                case "LOGIN":
                    if (data.Length != 3)
                    {
                        SendInformation("Incorrect parameter list!");
                        return;
                    }
                    Login(data[1], data[2]);
                    break;
                case "LOGOUT":
                    Logout();
                    break;
                case "EXIT":
                    Close();
                    break;
                case "INVITE":
                    // TESTING PARAM:
                    // LOGIN|jack|jackpwd
                    // INVITE|jenna|movies
                    // LOGOUT
                    InviteUser(data);
                    break;
                case "ACCEPT":
                    // TESTING PARAM:
                    // LOGIN|jenna|jennapwd
                    // ACCEPT|jack
                    AcceptInvite(data);
                    break;
                case "REJECT":
                    // TESTING PARAM:
                    // LOGIN|jenna|jennapwd
                    // REJECT|jack
                    RejectInvite(data);
                    break;
                case "INVITATIONS":
                    Invitations();
                    break;
                default:
                    SendInformation("Unknown command!");
                    break;
            }
        }

        private void Invitations()
        {
            if (currentUser == null)
            {
                SendInformation("You need to log in to use this feature");
                return;
            }
            string curus = currentUser.Username;
            dates p = dates.DatesList.Find(us => us.Whom == curus);
            if (p == null) 
            {
                SendInformation("Sorry you don't have any invites");
                return;
            }
            SendInformation(string.Format("DATA:\nInviter: {0} Invitee: {1}", p.Owner, p.Whom)); 
        }

        private void RejectInvite(string[] data)
        {
            //check bejelentkezes
            if (currentUser == null)
            {
                SendInformation("You need to log in to use this feature");
                return;
            }
            if (data.Length != 2)
            {
                SendInformation("Incorrect parameters entered, you must enter REJECT|<whom>");
                return;
            }
            //tudom h ezt a dates xmlbol lenne jobb kiszedni, azert csinalom igy h ellenorizze h a login ossze van e kotve
            //mivel erre nincs eleg ido igy oldottam meg
            User u = User.Users.Find(d => d.Username == data[1]);
            string iWannaDate = u.Username;
            dates p = dates.DatesList.Find(us => us.Owner == iWannaDate);
            if (p == null)
            {
                SendInformation("That invite doesn't exist");
                return;
            }
            //ha van egyaltalan ajanlat
            int stat = p.Pending;
            if (stat == 0) 
            {
                SendInformation("There's no pending invite");
                return;
            }
            else
            {
                p.Pending = 0;
                p.Rejected = 1;
                SendInformation(string.Format("You turned down a date with {0}", p.Owner));
                return;
            }

        }

        private void AcceptInvite(string[] data) 
        {
            //check bejelentkezes
            if (currentUser == null)
            {
                SendInformation("You need to log in to use this feature");
                return;
            }
            if (data.Length != 2)
            {
                SendInformation("Incorrect parameters entered, you must enter ACCEPT|<whom>");
                return;
            }
            //tudom h ezt a dates xmlbol lenne jobb kiszedni, azert csinalom igy h ellenorizze h a login ossze van e kotve
            //mivel erre nincs eleg ido igy oldottam meg
            User u = User.Users.Find(d => d.Username == data[1]);
            string iWannaDate = u.Username;
            dates p = dates.DatesList.Find(us => us.Owner == iWannaDate);
            if (p == null)
            {
                SendInformation("That invite doesn't exist");
                return;
            }
            //ha van egyaltalan ajanlat
            int stat = p.Pending;
            if (stat == 0)
            {
                SendInformation("There's no pending invite");
                return;
            }
            else
            {
                p.Pending = 0;
                p.Accepted = 1;
                SendInformation("You're going on a date");
            }

        }


        private void InviteUser(string[] data)
        {
            User u = User.Users.Find(d => d.Username == data[1]);
            string iWannaDate = u.Username;
            //ha nincs bejelentkezve v nem megfelelo a param lista
            if (currentUser == null)
            {
                SendInformation("You need to log in to use this feature");
                return;
            }
            if (data.Length != 3)
            {
                SendInformation("Incorrect parameters entered, you must enter INVITE|<whom>|where");
                return;
            }

            ////hozzaadni dates.xml-hez ha meg nincs benne adott ket ember kapcsolata
            dates p = dates.DatesList.Find(us => us.Whom == iWannaDate);
            ////who
            //if (p == null)
            //{
            //    dates.DatesList.Add(new dates("emma", data[1], 0, 0, 0));
            //}

            //ha az adott ket ember kozott mar van xml file letrehozva
            if (p.Pending > 0)
            {
                SendInformation(String.Format("User {0} has already received a request from you.", p.Whom));
                return;
            }
            if (p.Accepted > 0)
            {
                SendInformation(String.Format("User {0} has already accepted your invitation.", p.Whom));
                return;
            }
            p.Pending = 1;
            SendInformation(String.Format("You have invited user {0} on a date to {1}", p.Whom, data[2]));
            return;
        }

        private void Login(string username, string password)
        {
            if (currentUser != null)
            {
                SendInformation("You are already logged in!");
                return;
            }
            User u = User.LoginTry(username, password);
            if (u == null)
            {
                SendInformation("Incorrect username or password!");
            }
            else
            {
                SendInformation(String.Format("User {0} logged in.", u.Username));
                currentUser = u;
            }
        }
        private void Logout()
        {
            if (currentUser == null)
            {
                SendInformation("You are not logged in!");
            }
            else
            {
                currentUser = null;
                SendInformation("Logging out");
            }
        }
        private void SendInformation(string info)
        {
            writer.WriteLine(info);
            writer.Flush();
        }
        public void Close()
        {
            reader.Close();
            writer.Close();
            clients.Remove(this);
        }
    }
}
