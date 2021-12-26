using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebApplication1.Models
{
    public class DataModel
    {
        public static DataModel instance = null;
        private System.IO.StreamReader file;
        private double lon1;
        private double lat1;
        private int time;
        private int duration;
        private string fileName;
        private string ip;
        private int port;
        private double throttle;
        private double rudder;
        private bool eof;
        private DataModel() {
            eof = false;
        }
        public static DataModel getInstance()
        {
            if (instance == null)
            {
                instance = new DataModel();
            }
            return instance;
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }
        public string Ip
        {
            get
            {
                return ip;
            }
            set
            {
                ip = value;
            }
        }

        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time= value;
            }
        }

        public int Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
            }
        }

        public double Throttle
        {
            get
            {
                return throttle;
            }
            set
            {
                throttle = value;
            }
        }

        public double Rudder
        {
            get
            {
                return rudder;
            }
            set
            {
                rudder = value;
            }
        }
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }
        public double Lon
        {
            get
            {
                return lon1;
            }
            set
            {
                lon1 = value;
            }
        }
        public double Lat
        {
            get
            {
                return lat1;
            }
            set
            {
                lat1 = value;
            }
        }

        public bool Eof
        {
            get
            {
                return eof;
            }
            set
            {
                eof = value;
            }
        }

        public void initialize()
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, FileName));
            file = new System.IO.StreamReader(path);
        }
        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";
        public void fileWriting()
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, FileName));
            string[] lines = { cut(Lon), cut(Lat), cut(Throttle),cut(Rudder) };
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path, true))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
                file.Close();
            }

        }

        public string cut(double val)
        {
            string str = val.ToString();
            string[] arr = str.Split('.');
            if (arr.Length > 1)
            {
                try
                {
                    arr[1] = arr[1].Substring(0, 3);
                }catch(Exception ex)
                {

                }
                str = arr[0] +"."+ arr[1];
            }
            return str;
        }

        public void fileReading()
        {
            int counter = 0;
            string line=null;
           
            while ((counter < 4)&&(line = file.ReadLine()) != null)
            {
                setData(counter,line);
                counter++;
            }
            if (line == null)
            {
                Eof = true;
            }
        }

        public void setData(int counter,string value)
        {
            try
            {
                switch (counter)
                {
                    case 0:
                        Lon = Convert.ToSingle(value);
                        break;
                    case 1:
                        Lat = Convert.ToSingle(value);
                        break;
                    case 2:
                        Throttle = Convert.ToSingle(value);
                        break;
                    case 3:
                        Rudder = Convert.ToSingle(value);
                        break;
                    default:
                        throw new System.ArgumentException("illegal choice!");


                }
            }catch(Exception ex)
            {
                Console.WriteLine("wrong argument");
            }
        }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Val");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("EndOfFile", Eof.ToString());
            writer.WriteEndElement();
        }
    }
   
}