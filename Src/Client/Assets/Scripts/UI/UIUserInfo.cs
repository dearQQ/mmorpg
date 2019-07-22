using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIUserInfo : MonoSingleton<UIUserInfo> {

    int playerLv;
    [SerializeField]
    private Text textLevel;
    [SerializeField]
    private Text textName;
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
        if (User.Instance.CurrentCharacter == null)
            return;
        if (User.Instance.CurrentCharacter.Level != playerLv)
        {
            playerLv = User.Instance.CurrentCharacter.Level;
            textLevel.text = playerLv.ToString();
            textName.text = User.Instance.CurrentCharacter.Name;
        }
	}
}
