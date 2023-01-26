using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace server
{
    internal class dates
    {
        public static List<dates> DatesList = new List<dates>();
        public dates(string owner, string whom, int pending, int accepted, int rejected)
        {
            this.Owner = owner;
            this.Whom = whom;
            this.Pending = pending;
            this.Accepted = accepted;
            this.Rejected = rejected;
        }
        private string owner;
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        private string whom;
        public string Whom
        {
            get { return whom; }
            set { whom = value; }
        }

        private int pending;
        public int Pending
        {
            get { return pending; }
            set { pending = value; }
        }
        private int accepted;
        public int Accepted
        {
            get { return accepted; }
            set { accepted = value; }
        }
        private int rejected;
        public int Rejected
        {
            get { return rejected; }
            set { rejected = value; }
        }


        public static void LoadResources(string Filename)
        {
            DatesList.Clear();
            XDocument xml = XDocument.Load(Filename);
            foreach (var res in xml.Descendants("dates"))
            {
                string owner = (string)res.Attribute("owner");
                string whom = (string)res.Attribute("whom");
                int pending = (int)res.Attribute("pending");
                int accepted = (int)res.Attribute("accepted");
                int rejected = (int)res.Attribute("rejected");
                dates newRes = new dates(owner, whom, pending, accepted, rejected);
                DatesList.Add(newRes);
            }
        }
        public static void SaveResources(string Filename)
        {
            XElement root = new XElement("data");

            foreach (dates r in DatesList)
            {
                root.Add(
                    new XElement(
                        "dates",
                        new XAttribute((XName)"owner", r.Owner),
                        new XAttribute((XName)"whom", r.Whom),
                        new XAttribute((XName)"pending", r.Pending),
                        new XAttribute((XName)"accepted", r.Accepted),
                        new XAttribute((XName)"rejected", r.Rejected)
                        )
                    );
            }
            XDocument xml = new XDocument(root);
            xml.Save(Filename);
        }
    }
}
