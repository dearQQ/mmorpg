using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();


        internal Map(MapDefine define)
        {
            this.Define = define;
        }

        internal void Update()
        {
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.entityId);
            character.Info.mapId = this.ID;
            //通知自己这个地图有多少玩家
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            message.Response.mapCharacterEnter.mapId = this.ID;
			message.Response.mapCharacterEnter.Characters.Add(character.Info);

            //通知所有玩家我进入地图了
            NetMessage bordcast = new NetMessage();
            bordcast.Response = new NetMessageResponse();
            bordcast.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            bordcast.Response.mapCharacterEnter.mapId = this.ID;
			
            foreach (var kv in this.MapCharacters)
            {
                //添加这个地图的所以角色信息
                message.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);

                //通知每个玩家我进入地图了
                bordcast.Response.mapCharacterEnter.Characters.Clear();
                bordcast.Response.mapCharacterEnter.Characters.Add(character.Info);
                //给其他玩家发消息
                kv.Value.connection.SendData(bordcast);
            }
            this.MapCharacters[character.entityId] = new MapCharacter(conn, character);
            //给自己发消息
            conn.SendData(message);
        }

        internal void CharacterLeave(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterId:{1}", this.Define.ID, character.entityId);
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            foreach (var kv in this.MapCharacters)
            {
                message.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
                message.Response.mapCharacterLeave.characterId = character.entityId;
                kv.Value.connection.SendData(message);
            }
            this.MapCharacters.Remove(character.entityId);
        }

        internal void UpdateEntity(NEntitySync nEntity)
        {
            foreach (var item in this.MapCharacters)
            {
                if (item.Value.character.entityId == nEntity.Id)
                {
                    item.Value.character.Position = nEntity.Entity.Position;
                    item.Value.character.Direction = nEntity.Entity.Direction;
                    item.Value.character.Speed = nEntity.Entity.Speed;
                    TCharacter tcha = DBService.Instance.Entities.Characters.Where(u => u.Name == item.Value.character.Info.Name).FirstOrDefault();
                    tcha.MapPosX = nEntity.Entity.Position.X;
                    tcha.MapPosY = nEntity.Entity.Position.Y;
                    tcha.MapPosZ = nEntity.Entity.Position.Z;
                    DBService.Instance.Entities.SaveChanges();
                }
                else
                    MapService.Instance.SendEntityUpdate(item.Value.connection,nEntity);
            }
        }
    }
}
