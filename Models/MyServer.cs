using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;



namespace WebApplication1.Models
{
    class MyServer
    {
        private DataModel data;
        private bool isConneted;
        private NetworkStream stream;
        private StreamReader sr;
        private TcpClient client;
        static MyServer instance = null;
        private string serverIp ;
        private int serverPort;
        private readonly string latStr = "get /position/latitude-deg\r\n";
        private readonly string lonStr= "get /position/longitude-deg\r\n";
        private readonly string throttleStr = "get /controls/engines/current-engine/throttle\r\n";
        private readonly string rudderStr = "get /controls/flight/rudder\r\n";
        private MyServer()
        {
            data = DataModel.getInstance();
            isConneted = false;
        }
        public string ServerIp { get { return serverIp; } set { ServerIp = value; } }
        public int Port { get { return serverPort; } set { serverPort = value; } }
        public static MyServer getInstance()
        {
            if (instance == null)
            {
                instance = new MyServer();
                return instance;
            }
            else return instance;
        }

        public void connect_server()
        {
            if (isConneted) return;
            serverIp = data.Ip;
            Port = data.Port;    
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            client = new TcpClient();
            client.Connect(ep);
            stream = client.GetStream();
            sr = new StreamReader(stream);
            isConneted = true;
        }

        public double getData(string message)
        {
            double data=0;
            byte[] strMessage = Encoding.ASCII.GetBytes(message);
            try
            {
                stream.Write(strMessage, 0, strMessage.Length);
                string e = sr.ReadLine();
                data = Double.Parse(e.Split('=')[1].Split(' ')[1].Split('\'')[1]);
            }
            catch (Exception ex)
            {
                Console.Write("cannot connect\n");
            }
            return data;
        }
        public void open()
        {
            if (client == null) return;
            data.Lat = getData(latStr);
            data.Lon = getData(lonStr);
            data.Throttle = getData(throttleStr);
            data.Rudder = getData(rudderStr);
        }
    }
}






