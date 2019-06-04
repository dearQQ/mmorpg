using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Managers;
namespace GameServer.Services
{
    class MapService :Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapCharacterEnterRequest>(this.MapCharacterEnter);
        }

        public void Init()
        {
            MapManager.Instance.Init();
        }
        private void MapCharacterEnter(NetConnection<NetSession> sender, MapCharacterEnterRequest request)
        {

        }
    }
}
