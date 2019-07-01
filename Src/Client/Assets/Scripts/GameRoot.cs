using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager.UIMgr.OpenUI<UILoading>("MainUI");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
