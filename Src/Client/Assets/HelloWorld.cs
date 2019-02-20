using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;

public class HelloWorld : MonoBehaviour {
	// Use this for initialization
	void Start () {
        Network.NetClient.Instance.Init("127.0.0.1",8000);
        Network.NetClient.Instance.Connect();
        NetMessage msg = new NetMessage();
        msg.Request = new NetMessageRequest();
        msg.Request.firstTestReq = new FirstTestRequest();
        msg.Request.firstTestReq.Text = "Hello World";
        Network.NetClient.Instance.SendMessage(msg);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
