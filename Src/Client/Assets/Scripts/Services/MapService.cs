using Common.Data;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Managers;
namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {
        private int curMapId;

        public int CurMapID
        {
            get { return curMapId; }
        }

        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.MapEntitySyncResponse);
            MessageDistributer.Instance.Subscribe<MapTeleportResponse>(this.MapTeleportResponse);
        }

       

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.MapEntitySyncResponse);
        }

        

        public void Init()
        {

        }

        public void MapCharacterEnter(int mapId)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapCharacterEnter = new MapCharacterEnterRequest();
            message.Request.mapCharacterEnter.mapId = mapId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:{1}", response.mapId, response.Characters.Count);
            foreach (var character in response.Characters)
            {
                if (Models.User.Instance.CurrentCharacter.Id == character.Id)
                    Models.User.Instance.CurrentCharacter = character;
                if (CharacterManager.Instance.Characters.ContainsKey(character.Entity.Id))
                {
                    CharacterManager.Instance.RemoveCharacter(character.Entity.Id);
                }
                CharacterManager.Instance.AddCharacter(character);
            }
            if (this.curMapId != response.mapId)
            {
                this.EnterMap(response.mapId);
                this.curMapId = response.mapId;
            }
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                //Models.User.Instance.CurrentMapData = map;
                GameManager.SceneMgr.LoadScene(map.Resource,
                    delegate
                    {
                        GameManager.UIMgr.OpenUI<UIMainCity>("Main");
                        GameManager.UIMgr.OpenUI<UIMiniMap>("Main");
                    });
                
            }
            else
                Debug.LogError("该地图不存在:"+ mapId);
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave id:{0}", response.characterId);
            if (response.characterId == Models.User.Instance.CurrentCharacter.Entity.Id)
                CharacterManager.Instance.Clear();
            else
                CharacterManager.Instance.RemoveCharacter(response.characterId);
            this.curMapId = 0;
        }

        public void MapEntitySyncRequst(EntityEvent entityEvent,NEntity nEntity)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync();
            message.Request.mapEntitySync.entitySync.Id = nEntity.Id;
            message.Request.mapEntitySync.entitySync.Event = entityEvent;
            message.Request.mapEntitySync.entitySync.Entity = nEntity;
            NetClient.Instance.SendMessage(message);
        }

        public void MapEntitySyncResponse(object sender,MapEntitySyncResponse response)
        {
            foreach (var entity in response.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(entity);
            }
        }
        internal void SendTelepoter(int linkTo)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleportReq = new MapTeleportRequest();
            message.Request.mapTeleportReq.teleporterId = linkTo;
            NetClient.Instance.SendMessage(message);
        }
        private void MapTeleportResponse(object sender, MapTeleportResponse message)
        {
            Models.User.Instance.TeleportID = message.teleporterId;
        }
    }
}
