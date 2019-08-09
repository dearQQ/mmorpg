using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using Models;
using SkillBridge.Message;
using Managers;

namespace Services
{
    class UserService : Singleton<UserService>,IDisposable
    {
        public Action<Result, string> OnRegister;
        public Action<Result, string> OnLogin;
        public Action<Result, string> OnCreateCharacter;

        public UserService()
        {
            NetClient.Instance.OnConnect += this.OnConnect;
            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnRegisterResponse);
            MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnLoginResponse);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnCreateCharacterResponse);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnGameEnterResponse);
            MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(this.OnGameLeave);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnRegisterResponse);
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnLoginResponse);
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnCreateCharacterResponse);
            MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(this.OnGameEnterResponse);
            MessageDistributer.Instance.Unsubscribe<UserGameLeaveResponse>(this.OnGameLeave);
        }

        public void Connect()
        {
            NetClient.Instance.Init("127.0.0.1",8000);
            NetClient.Instance.Connect();
        }
        void OnConnect(int result, string reason)
        {
            if (!NetClient.Instance.Connected)
            {
                MessageBox.Show(string.Format("网络错误，无法连接到服务器！\n RESULT:{0} ERROR:{1}", result, reason), "错误", MessageBoxType.Error);
            }
        }
        
        public void OnRegisterRequest(string user,string psw)
        {
            NetMessage netMessage = new NetMessage();
            netMessage.Request = new NetMessageRequest();
            netMessage.Request.userRegister = new UserRegisterRequest();
            netMessage.Request.userRegister.User = user;
            netMessage.Request.userRegister.Passward = psw;
            NetClient.Instance.SendMessage(netMessage);
        }
       
        void OnRegisterResponse(object sender, UserRegisterResponse response)
        {
            if (OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);
            }
        }


        public void OnLoginRequest(string user, string psw)
        {
            NetMessage netMessage = new NetMessage();
            netMessage.Request = new NetMessageRequest();
            netMessage.Request.userLogin = new UserLoginRequest();
            netMessage.Request.userLogin.User = user;
            netMessage.Request.userLogin.Passward = psw;
            NetClient.Instance.SendMessage(netMessage);
        }

        void OnLoginResponse(object sender, UserLoginResponse response)
        {
            if (response.Result == Result.Success)
                User.Instance.SetupUserInfo(response.Userinfo);
            if (OnLogin != null)
            {
                this.OnLogin(response.Result, response.Errormsg);
            }
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type">角色职业</param>
        public void OnCreateCharacterRequest(string name,int job)
        {
            NetMessage netMessage = new NetMessage();
            netMessage.Request = new NetMessageRequest();
            netMessage.Request.createChar = new UserCreateCharacterRequest();
            netMessage.Request.createChar.Class = (CharacterClass)job;
            netMessage.Request.createChar.Name = name;
            NetClient.Instance.SendMessage(netMessage);
        }
        void OnCreateCharacterResponse(object sender,UserCreateCharacterResponse response)
        {
            if(response.Result == Result.Success)
            {
                Models.User.Instance.Info.Player.Characters.Clear();
                Models.User.Instance.Info.Player.Characters.AddRange(response.Characters);
            }
            if (this.OnCreateCharacter != null)
                this.OnCreateCharacter(response.Result,response.Errormsg);
        }

        public void OnGameEnterRequest(int index)
        {
            NetMessage netMessage = new NetMessage();
            netMessage.Request = new NetMessageRequest();
            netMessage.Request.gameEnter = new UserGameEnterRequest();
            netMessage.Request.gameEnter.characterIdx = index;
            NetClient.Instance.SendMessage(netMessage);
        }
        void OnGameEnterResponse(object sender,UserGameEnterResponse response)
        {
            if (response.Result == Result.Success)
            {
                if (response.Character != null)
                    ItemManager.Instance.Init(response.Character.Items);
            }
        }
        public void SendGameLeave()
        {
            Debug.Log("UserGameLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameLeave = new UserGameLeaveRequest();
            
            NetClient.Instance.SendMessage(message);
        }

        void OnGameLeave(object sender, UserGameLeaveResponse response)
        {
            Debug.LogFormat("OnGameLeave:{0} [{1}]", response.Result, response.Errormsg);
        }
    }
}
