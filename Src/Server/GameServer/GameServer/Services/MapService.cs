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
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleport);
        }

        public void Init()
        {
            MapManager.Instance.Init();
            TCharecterItems charecterItems = new TCharecterItems();
            charecterItems.ID = 1;
            charecterItems.ItemID = 100;
            charecterItems.ItemCount = 99;
            charecterItems.TCharacterID = 1;
            DBService.Instance.Entities.TCharecterItems.Add(charecterItems);
            DBService.Instance.Entities.SaveChanges();
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
        private void OnMapTeleport(NetConnection<NetSession> sender, MapTeleportRequest message)
        {
            Character character = sender.Session.Character;
            Common.Data.TeleporterDefine td = DataManager.Instance.Teleporters[message.teleporterId];
            if (td == null)
            {
                Log.WarningFormat("OnMapTeleport not exist character name{0}, teleporterId id:{1}", character.Info.Name, message.teleporterId);
                return;
            }

            Common.Data.MapDefine targetMap = DataManager.Instance.Maps[td.LinkTo];
            if (targetMap == null)
            {
                Log.WarningFormat("OnMapTeleport target map not exist teleporterId:{0}", message.teleporterId);
                return;
            }
            MapManager.Instance[td.MapID].CharacterLeave(character);
            MapManager.Instance[targetMap.ID].CharacterEnter(sender, character);


            //Common.Data.TeleporterDefine taget = DataManager.Instance.Teleporters[td.LinkTo];

            //if (taget == null)
            //{
            //    Log.WarningFormat("OnMapTeleport target not exist teleporterId:{0}", td.LinkTo);
            //    return;
            //}
            //MapManager.Instance[td.MapID].CharacterLeave(character);
            //MapManager.Instance[taget.MapID].CharacterEnter(sender, character);
            NetMessage res = new NetMessage();
            res.Response = new NetMessageResponse();
            res.Response.mapTeleportRes = new MapTeleportResponse();
            res.Response.mapTeleportRes.teleporterId = message.teleporterId;
            sender.SendData(res);
        }
    }
}
