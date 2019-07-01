using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Managers;
using GameServer.Entities;

namespace GameServer.Services
{
    class MapService :Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.MapEntitySync);
        }

        public void Init()
        {
            MapManager.Instance.Init();
        }
        private void MapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest request)
        {
            Character character = sender.Session.Character;
            MapManager.Instance[character.Info.mapId].UpdateEntity(request.entitySync);
        }
        public void SendEntityUpdate(NetConnection<NetSession> sender,NEntitySync entitySync)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapEntitySync = new MapEntitySyncResponse();
            message.Response.mapEntitySync.entitySyncs.Add(entitySync);
            sender.SendData(message);
        }
    }
}
