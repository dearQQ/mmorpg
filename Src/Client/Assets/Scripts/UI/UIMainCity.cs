using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainCity : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GameLeave()
    {
        GameManager.SceneMgr.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
    }
}
