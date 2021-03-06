﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;

namespace GameServer.Managers
{
    class CharacterManager : Singleton<CharacterManager>
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public CharacterManager()
        {
        }

        public void Dispose()
        {
        }

        public void Init()
        {

        }

        public void Clear()
        {
            this.Characters.Clear();
        }

        public Character AddCharacter(TCharacter cha)
        {
            Character character = new Character(CharacterType.Player, cha);
            EntityManager.Instance.AddEntity(cha.MapID, character);
            this.Characters[character.entityId] = character;
            return character;
        }


        public void RemoveCharacter(int characterId)
        {
            if (this.Characters[characterId] != null)
                return;
            EntityManager.Instance.RemoveEntity(this.Characters[characterId].Data.MapID, this.Characters[characterId]);
            this.Characters.Remove(characterId);
        }
    }
}
