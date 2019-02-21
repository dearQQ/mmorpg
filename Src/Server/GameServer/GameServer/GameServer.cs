using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

using System.Threading;

using Network;
using GameServer.Services;

namespace GameServer
{
    class GameServer
    {
        Thread thread;
        bool running = false;
        NetService netService;
        public bool Init()
        {
            DBService.Instance.Init();

            //HelloWorldService.Instance.Init();
            UserService.Instance.Init();
            netService = new NetService();
            netService.Init(8000);
            thread = new Thread(new ThreadStart(this.Update));
            return true;
        }

        public void Start()
        {
            netService.Start();
            //HelloWorldService.Instance.Start();
            running = true;
            thread.Start();
        }


        public void Stop()
        {
            netService.Stop();
            running = false;
            thread.Join();
        }

        public void Update()
        {
            while (running)
            {
                Time.Tick();
                Thread.Sleep(100);
                //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
            }
        }
    }
}
