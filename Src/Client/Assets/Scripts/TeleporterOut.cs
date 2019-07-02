using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Services;

public class TeleporterOut : MonoBehaviour {
    public int ID;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        PlayerInputController pc = other.gameObject.GetComponent<PlayerInputController>();
        if (pc && pc.isActiveAndEnabled)
        {
            TeleporterDefine td = DataManager.Instance.Teleporters[this.ID];
            if (td != null)
            {
                MapService.Instance.SendTelepoter(this.ID);
            }
            else
                Debug.LogError("传送点有误：" + this.ID);
        }
    }
}
