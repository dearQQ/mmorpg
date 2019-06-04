using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using GameServer;
using Common;

namespace Network
{
    class NetService
    {
        static TcpSocketListener ServerListener;
        public bool Init(int port)
        {
            ServerListener = new TcpSocketListener("127.0.0.1", GameServer.Properties.Settings.Default.ServerPort, 10);
            ServerListener.SocketConnected += OnSocketConnected;
            return true;
        }


        public void Start()
        {
            //启动监听
            Log.Warning("Starting Listener...");
            ServerListener.Start();

            MessageDistributer<NetConnection<NetSession>>.Instance.Start(8);
            Log.Warning("NetService Started");
        }


        public void Stop()
        {
            Log.Warning("Stop NetService...");

            ServerListener.Stop();

            Log.Warning("Stoping Message Handler...");
            MessageDistributer<NetConnection<NetSession>>.Instance.Stop();
        }

        private void OnSocketConnected(object sender, Socket e)
        {
            IPEndPoint clientIP = (IPEndPoint)e.RemoteEndPoint;
            //可以在这里对IP做一级验证,比如黑名单

            NetConnection<NetSession> connection = new NetConnection<NetSession>(e);
            Log.WarningFormat("Client[{0}]] Connected", clientIP);
        }
    }
}
