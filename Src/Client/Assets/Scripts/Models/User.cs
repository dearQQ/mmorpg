using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Entities;
namespace Models
{
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;
        public Character curCharacter;
        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }
        public GameObject CurrentCharacterObject { get; set; }
        public int TeleportID;
        
    }
}
