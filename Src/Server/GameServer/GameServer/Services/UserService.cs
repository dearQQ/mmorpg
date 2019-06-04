using System;
using System.Collections.Generic;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using System.Linq;
using GameServer.Managers;
namespace GameServer.Services
{
    class UserService :Singleton<UserService>
    {
        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
        }
        public void Init()
        {

        }
        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            NetMessage message= new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userRegister = new UserRegisterResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                message.Response.userRegister.Result = Result.Failed;
                message.Response.userRegister.Errormsg = "用户已存在.";
            }
           
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                message.Response.userRegister.Result = Result.Success;
                message.Response.userRegister.Errormsg = "用户注册成功";
            }

            
            sender.SendData(message);
        }
        void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userLogin = new UserLoginResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                if (user.Password != request.Passward)
                {
                    message.Response.userLogin.Result = Result.Failed;
                    message.Response.userLogin.Errormsg = "密码不正确";
                }
                else
                {
                    sender.Session.User = user;
                    message.Response.userLogin.Result = Result.Success;
                    message.Response.userLogin.Errormsg = "none";
                    message.Response.userLogin.Userinfo = new NUserInfo();
					message.Response.userLogin.Userinfo.Id = 1;
                    message.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                    message.Response.userLogin.Userinfo.Player.Id = user.Player.ID;

                    foreach (var c in user.Player.Characters)
                    {
                        NCharacterInfo info = new NCharacterInfo();
                        info.Id = c.ID;
                        info.Name = c.Name;
                        info.Class = (CharacterClass)c.Class;
                        info.Tid = c.TID;
                        message.Response.userLogin.Userinfo.Player.Characters.Add(info);
                    }
                }
            }
            else
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "用户不存在";
            }

            
            sender.SendData(message);
        }
        void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("OnCreateCharacter:昵称{0}，角色职业{1}", request.Name,request.Class);
            TCharacter character = new TCharacter();
            character.Name = request.Name;
            character.TID = (int)request.Class;
            character.MapID = 1;
            character.Class = (int)request.Class;
            character.MapPosX = 4700;
            character.MapPosY = 4500;
            character.MapPosZ = 800;
            DBService.Instance.Entities.Characters.Add(character);
            sender.Session.User.Player.Characters.Add(character);
            DBService.Instance.Entities.SaveChanges();


            NetMessage message = new NetMessage();
            message.Response = new  NetMessageResponse();
            message.Response.createChar = new UserCreateCharacterResponse();
            message.Response.createChar.Result = Result.Success;
            message.Response.createChar.Errormsg = "none";
            foreach (var c in sender.Session.User.Player.Characters)
            {
                NCharacterInfo info = new NCharacterInfo();
                info.Id = c.ID;
                info.Name = c.Name;
                info.Type = CharacterType.Player;
                info.Class = (CharacterClass)c.Class;
                info.Tid = c.TID;
                message.Response.createChar.Characters.Add(info);
            }
            sender.SendData(message);
        }
        void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter Tchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Character character = CharacterManager.Instance.AddCharacter(Tchar);
            if (character != null)
            {
                NetMessage message = new NetMessage();
                message.Response = new NetMessageResponse();
                message.Response.gameEnter = new UserGameEnterResponse();
                message.Response.gameEnter.Result = Result.Success;
                message.Response.gameEnter.Errormsg = "none";
                sender.SendData(message);
            }
            MapManager.Instance[Tchar.MapID].CharacterEnter(sender, character);
        }
    }
}
