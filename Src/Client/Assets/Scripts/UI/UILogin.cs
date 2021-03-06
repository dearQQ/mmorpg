﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;
using System;

public class UILogin : MonoBehaviour
{
    [SerializeField]
    private Toggle isRemember;
    [SerializeField]
    private InputField txtUser;
    [SerializeField]
    private InputField txtPassword;
    [SerializeField]
    private Button btnLogin;

    private void Start()
    {
        bool bRemember = PlayerPrefs.GetInt("isRemember") == 1;
        isRemember.isOn = bRemember;
        if (bRemember)
        {
            txtUser.text = PlayerPrefs.GetString("user","");
            txtPassword.text = PlayerPrefs.GetString("password","");
        }
        UserService.Instance.Connect();
        UserService.Instance.OnLogin = this.OnLogin;
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(txtUser.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(txtPassword.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        
        PlayerPrefs.SetInt("isRemember",isRemember.isOn ? 1 : 0);
        UserService.Instance.OnLoginRequest(txtUser.text,txtPassword.text);
    }

    void OnLogin(Result res,string msg)
    {
        if (res == Result.Success)
        {
            if (isRemember.isOn)
            {
                PlayerPrefs.SetString("user", txtUser.text);
                PlayerPrefs.SetString("password", txtPassword.text);
            }
            GameManager.SceneMgr.LoadScene("CharSelect");
        }
        else
            MessageBox.Show(msg, "用户登录", MessageBoxType.Error);
    }

}
