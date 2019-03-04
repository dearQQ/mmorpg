using Common.Data;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {
        private int curMapId;
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            
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
            foreach (var character in response.Characters)
            {
                if (Models.User.Instance.CurrentCharacter.Id == character.Id)
                    Models.User.Instance.CurrentCharacter = character;
                if (CharacterManager.Instance.Characters.ContainsKey(character.Id))
                {
                    CharacterManager.Instance.RemoveCharacter(character.Id);
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
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
                Debug.LogError("该地图不存在:"+ mapId);
        }
    }
}
