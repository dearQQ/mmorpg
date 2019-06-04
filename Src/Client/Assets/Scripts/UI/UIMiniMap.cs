﻿using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
public class UIMiniMap : MonoSingleton<UIMiniMap> {
    private Collider mapBox;
    [SerializeField]
    private Image imgMap;
    [SerializeField]
    private Text txtMapName;
    [SerializeField]
    private Image player;
    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this);
	}
    public void Init()
    {
        mapBox = GameObject.Find("mapBox").GetComponent<Collider>();
        if (mapBox == null)
            Debug.LogError("该地图没有地图包围盒");
        txtMapName.text = DataManager.Instance.Maps[MapService.Instance.CurMapID].Name;
    }
	// Update is called once per frame
	void Update ()
    {
        player.transform.eulerAngles = new Vector3(0, 0, User.Instance.CurrentCharacterObject.transform.eulerAngles.y);
        float realWidth = mapBox.bounds.size.x;
        float realHeight = mapBox.bounds.size.z;
        float offset_x = User.Instance.CurrentCharacterObject.transform.position.x - mapBox.bounds.min.x;
        float offset_z = User.Instance.CurrentCharacterObject.transform.position.z - mapBox.bounds.min.z;
        float pivotX = offset_x / realWidth;
        float pivotY = offset_z / realHeight;

        this.imgMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.imgMap.rectTransform.localPosition = Vector2.zero;
        this.player.transform.eulerAngles = new Vector3(0, 0, -User.Instance.CurrentCharacterObject.transform.eulerAngles.y);

    }
}
