using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using Models;
using SkillBridge.Message;

namespace Services
{
    class UserService : Singleton<UserService>
    {
        public Action<Result, string> OnRegister;
        public Action<Result, string> OnLogin;
        public Action<Result, string> OnCreateCharacter;
        public Action<Result, string> OnGameEnter;

        public UserService()
        {
            NetClient.Instance.OnConnect += this.OnConnect;
            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnRegisterResponse);
            MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnLoginResponse);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnCreateCharacterResponse);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnGameEnterResponse);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnRegisterResponse);
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnLoginResponse);
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnCreateCharacterResponse);
            MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(this.OnGameEnterResponse);
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
        
        public void Register(string user,string psw)
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
        public void Login(string user, string psw)
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
        public void CreateCharacter(string name,int job)
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
            if (this.OnCreateCharacter != null)
                this.OnCreateCharacter(response.Result,response.Errormsg);
        }

        public void GameEnter(int index)
        {
            NetMessage netMessage = new NetMessage();
            netMessage.Request = new NetMessageRequest();
            netMessage.Request.gameEnter = new UserGameEnterRequest();
            netMessage.Request.gameEnter.characterIdx = index;
            NetClient.Instance.SendMessage(netMessage);
        }
        void OnGameEnterResponse(object sender,UserGameEnterResponse response)
        {
            if (this.OnGameEnter != null)
                this.OnGameEnter(response.Result, response.Errormsg);
        }
    }


    //class UserService : Singleton<UserService>, IDisposable
    //{

    //    public UnityEngine.Events.UnityAction<Result, string> OnLogin;
    //    public UnityEngine.Events.UnityAction<Result, string> OnRegister;
    //    public UnityEngine.Events.UnityAction<Result, string> OnCharacterCreate;

    //    NetMessage pendingMessage = null;

    //    bool connected = false;

    //    public UserService()
    //    {
    //        NetClient.Instance.OnConnect += OnGameServerConnect;
    //        NetClient.Instance.OnDisconnect += OnGameServerDisconnect;
    //        MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnUserLogin);
    //        MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
    //        MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            
    //    }

    //    public void Dispose()
    //    {
    //        MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnUserLogin);
    //        MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
    //        MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
    //        NetClient.Instance.OnConnect -= OnGameServerConnect;
    //        NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
    //    }

    //    public void Init()
    //    {

    //    }

    //    public void ConnectToServer()
    //    {
    //        Debug.Log("ConnectToServer() Start ");
    //        //NetClient.Instance.CryptKey = this.SessionId;
    //        NetClient.Instance.Init("127.0.0.1", 8000);
    //        NetClient.Instance.Connect();
    //    }


    //    void OnGameServerConnect(int result, string reason)
    //    {
    //        Log.InfoFormat("LoadingMesager::OnGameServerConnect :{0} reason:{1}", result, reason);
    //        if (NetClient.Instance.Connected)
    //        {
    //            this.connected = true;
    //            if(this.pendingMessage!=null)
    //            {
    //                NetClient.Instance.SendMessage(this.pendingMessage);
    //                this.pendingMessage = null;
    //            }
    //        }
    //        else
    //        {
    //            if (!this.DisconnectNotify(result, reason))
    //            {
    //                MessageBox.Show(string.Format("网络错误，无法连接到服务器！\n RESULT:{0} ERROR:{1}", result, reason), "错误", MessageBoxType.Error);
    //            }
    //        }
    //    }

    //    public void OnGameServerDisconnect(int result, string reason)
    //    {
    //        this.DisconnectNotify(result, reason);
    //        return;
    //    }

    //    bool DisconnectNotify(int result,string reason)
    //    {
    //        if (this.pendingMessage != null)
    //        {
    //            if (this.pendingMessage.Request.userLogin!=null)
    //            {
    //                if (this.OnLogin != null)
    //                {
    //                    this.OnLogin(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
    //                }
    //            }
    //            else if(this.pendingMessage.Request.userRegister!=null)
    //            {
    //                if (this.OnRegister != null)
    //                {
    //                    this.OnRegister(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
    //                }
    //            }
    //            else
    //            {
    //                if (this.OnCharacterCreate != null)
    //                {
    //                    this.OnCharacterCreate(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
    //                }
    //            }
    //            return true;
    //        }
    //        return false;
    //    }

    //    public void SendLogin(string user, string psw)
    //    {
    //        Debug.LogFormat("UserLoginRequest::user :{0} psw:{1}", user, psw);
    //        NetMessage message = new NetMessage();
    //        message.Request = new NetMessageRequest();
    //        message.Request.userLogin = new UserLoginRequest();
    //        message.Request.userLogin.User = user;
    //        message.Request.userLogin.Passward = psw;

    //        if (this.connected && NetClient.Instance.Connected)
    //        {
    //            this.pendingMessage = null;
    //            NetClient.Instance.SendMessage(message);
    //        }
    //        else
    //        {
    //            this.pendingMessage = message;
    //            this.ConnectToServer();
    //        }
    //    }

    //    void OnUserLogin(object sender, UserLoginResponse response)
    //    {
    //        Debug.LogFormat("OnLogin:{0} [{1}]", response.Result, response.Errormsg);

    //        if (response.Result == Result.Success)
    //        {//登陆成功逻辑
    //            Models.User.Instance.SetupUserInfo(response.Userinfo);
    //        };
    //        if (this.OnLogin != null)
    //        {
    //            this.OnLogin(response.Result, response.Errormsg);

    //        }
    //    }


    //    public void SendRegister(string user, string psw)
    //    {
    //        Debug.LogFormat("UserRegisterRequest::user :{0} psw:{1}", user, psw);
    //        NetMessage message = new NetMessage();
    //        message.Request = new NetMessageRequest();
    //        message.Request.userRegister = new UserRegisterRequest();
    //        message.Request.userRegister.User = user;
    //        message.Request.userRegister.Passward = psw;

    //        if (this.connected && NetClient.Instance.Connected)
    //        {
    //            this.pendingMessage = null;
    //            NetClient.Instance.SendMessage(message);
    //        }
    //        else
    //        {
    //            this.pendingMessage = message;
    //            this.ConnectToServer();
    //        }
    //    }

    //    void OnUserRegister(object sender, UserRegisterResponse response)
    //    {
    //        Debug.LogFormat("OnUserRegister:{0} [{1}]", response.Result, response.Errormsg);

    //        if (this.OnRegister != null)
    //        {
    //            this.OnRegister(response.Result, response.Errormsg);

    //        }
    //    }

    //    public void SendCharacterCreate(string name, CharacterClass cls)
    //    {
    //        Debug.LogFormat("UserCreateCharacterRequest::name :{0} class:{1}", name, cls);
    //        NetMessage message = new NetMessage();
    //        message.Request = new NetMessageRequest();
    //        message.Request.createChar = new UserCreateCharacterRequest();
    //        message.Request.createChar.Name = name;
    //        message.Request.createChar.Class = cls;

    //        if (this.connected && NetClient.Instance.Connected)
    //        {
    //            this.pendingMessage = null;
    //            NetClient.Instance.SendMessage(message);
    //        }
    //        else
    //        {
    //            this.pendingMessage = message;
    //            this.ConnectToServer();
    //        }
    //    }

    //    void OnUserCreateCharacter(object sender, UserCreateCharacterResponse response)
    //    {
    //        Debug.LogFormat("OnUserCreateCharacter:{0} [{1}]", response.Result, response.Errormsg);

    //        if(response.Result == Result.Success)
    //        {
    //            Models.User.Instance.Info.Player.Characters.Clear();
    //            Models.User.Instance.Info.Player.Characters.AddRange(response.Characters);
    //        }

    //        if (this.OnCharacterCreate != null)
    //        {
    //            this.OnCharacterCreate(response.Result, response.Errormsg);

    //        }
    //    }


    //}
}
