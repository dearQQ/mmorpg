using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Services;
using SkillBridge.Message;
using System;

public class TeleporterIn : MonoBehaviour {

    bool first_in = true;
    public int ID;
    // Use this for initialization
    void Start () {
        this.GetComponent<MeshRenderer>().enabled = false;
        
    }
    private void Update()
    {
        if (Models.User.Instance.TeleportID == this.ID)
        {
            User.Instance.CurrentCharacterObject.transform.position = this.transform.position;
            User.Instance.CurrentCharacterObject.transform.forward = this.transform.forward;
            PlayerInputController pc = User.Instance.CurrentCharacterObject.GetComponent<PlayerInputController>();
            pc.character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
            pc.character.SetPosition(GameObjectTool.WorldToLogic(this.transform.position));
            pc.SendEntityEvent(EntityEvent.None);
            Models.User.Instance.TeleportID = 0;
        }
    }
}
