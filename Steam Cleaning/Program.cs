using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

namespace Steam_Cleaning
{
    class Program
    {
        static List<Vacuum> Vacuums = new List<Vacuum>();

        static void Main(string[] args)
        {
            ParseXML();
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        private static void ParseXML()
        {
            XmlDocument reader = new XmlDocument();

            try { reader.Load("data.xml"); }
            catch (FileNotFoundException e)
            {
                using (XmlWriter xW = XmlWriter.Create("data.xml"))
                {
                    xW.WriteStartDocument();
                    xW.WriteStartElement("Carpets");
                    xW.WriteEndElement();
                    reader.Load("data.xml");
                }
            }

            XmlElement root = reader.DocumentElement;
            XmlNodeList nodeList = root.GetElementsByTagName("Carpet");
            foreach (XmlNode node in nodeList)
                Vacuums.Add(new Vacuum(new WatchData(node)));
        }
    }

    class Vacuum
    {
        WatchData Data;
        public Vacuum(WatchData data)
        {
            Data = data;
            watch();
        }

        private void watch()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Data.Path;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = Data.Filter;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                Directory.GetFiles(Path.GetDirectoryName(e.FullPath), Data.Filter).Select(x => new FileInfo(x))
                 .OrderByDescending(x => x.LastWriteTime)
                 .Skip(Data.Keep)
                 .ToList()
                 .ForEach(f => f.Delete());
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }

    struct WatchData
    {
        public string Path { get; private set; }
        public string Filter { get; private set; }
        public int Keep { get; private set; }

        public WatchData(XmlNode xmlData)
        {
            Path = xmlData.Attributes["Path"].InnerText;
            Filter = xmlData.Attributes["Filter"].InnerText;
            Keep = Convert.ToInt32(xmlData.Attributes["Keep"].InnerText);
        }
    }
}