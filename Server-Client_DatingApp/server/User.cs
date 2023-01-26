using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace server
{
    internal class User
    {
        public static List<User> Users = new List<User>();
        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }


        public static User LoginTry(string username, string password)
        {
            return Users.Find(u => u.Username == username && u.Password == password);
        }
        public static void LoadUsers(string Filename)
        {
            Users.Clear();
            XDocument xml = XDocument.Load(Filename);
            foreach (var user in xml.Descendants("user"))
            {
                User newUser = new User(
                    (string)user.Attribute("username"), 
                    (string)user.Attribute("password")
                    );
                Users.Add(newUser);
            }
        }
        public static void SaveUsers(string Filename)
        {
            XElement root = new XElement("users");

            foreach (User u in Users)
            {
                root.Add(
                    new XElement(
                        "user",
                        new XAttribute((XName)"username", u.Username),
                        new XAttribute((XName)"password", u.Password)
                        )
                    );
            }
            XDocument xml = new XDocument(root);
            xml.Save(Filename);
        }
    }
}
