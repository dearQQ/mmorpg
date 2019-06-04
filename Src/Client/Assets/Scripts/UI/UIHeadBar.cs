using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
public class UIHeadBar : MonoBehaviour {
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text lv;
    [SerializeField]
    private Image hp;
    public Character cha;
    private GameObject camera;
    public Transform owner;
	// Use this for initialization
	void Start ()
    {
        camera = GameObject.FindGameObjectWithTag("MainCityCamera");

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (cha != null)
        {
            name.text = "Lv." + cha.Info.Level + " " + cha.Name;
            lv.text = "Lv." + cha.Info.Level;
            hp.fillAmount = 1f;
            this.transform.position = owner.position + Vector3.up * 2;
            this.transform.forward = camera.transform.forward;
        }
	}
}
