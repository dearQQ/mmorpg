using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
public class UIHeadBar : MonoBehaviour {
    [SerializeField]
    private Text _name;
    [SerializeField]
    private Text _lv;
    [SerializeField]
    private Image _hp;
    public Character cha;
    private GameObject _camera;
    public Transform owner;
	// Use this for initialization
	void Start ()
    {
        _camera = GameManager.CameraMgr.MainCamera;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (cha != null && owner != null && _camera != null)
        {
            _name.text = "Lv." + cha.Info.Level + " " + cha.Name;
            _lv.text = "Lv." + cha.Info.Level;
            _hp.fillAmount = 1f;
            this.transform.position = owner.position + Vector3.up * 2;
            this.transform.forward = _camera.transform.forward;
        }
	}
}
