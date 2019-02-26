using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UIRegister : MonoBehaviour
{

    [SerializeField]
    private GameObject RegisterUI;
    [SerializeField]
    private Button btnRegister;

    [SerializeField]
    private InputField txtUser;
    [SerializeField]
    private InputField txtPassword;
    [SerializeField]
    private InputField txtPasswordAffirm;

    [SerializeField]
    private InputField txtLoginUser;
    [SerializeField]
    private InputField txtLoginPassword;
    [SerializeField]
    private Toggle isRemember;

    private void Start()
    {
        UserService.Instance.OnRegister = this.OnRegister;
    }
    private void Awake()
    {
    }
    private void OnEnable()
    {
        txtUser.text = "";
        txtPassword.text = "";
        txtPasswordAffirm.text = "";
    }
    public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(txtUser.text))
        {
            MessageBox.Show("请输入用户名","用户注册");
            return;
        }
        if (string.IsNullOrEmpty(txtPassword.text))
        {
            MessageBox.Show("请输入密码", "用户注册");
            return;
        }
        if (string.IsNullOrEmpty(txtPasswordAffirm.text))
        {
            MessageBox.Show("请再次输入密码", "用户注册");
            return;
        }
        if (!string.Equals(txtPassword.text,txtPasswordAffirm.text))
        {
            MessageBox.Show("两次输入的密码不一致", "用户注册");
            return;
        }
        UserService.Instance.Register(txtUser.text, txtPassword.text);

    }
    void OnRegister(Result result, string message)
    {
        if (result == Result.Success)
        {
            MessageBox.Show("注册成功", "用户注册");
            RegisterUI.SetActive(false);
            txtLoginUser.text = txtUser.text;
            txtLoginPassword.text = txtPassword.text;
            isRemember.isOn = true;
        }
        else
        {
            MessageBox.Show("注册失败:"+ message, "用户注册",MessageBoxType.Error);
        }
    }





}
