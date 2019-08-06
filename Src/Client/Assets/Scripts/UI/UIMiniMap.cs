using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
public class UIMiniMap : MonoBehaviour {
    private int mapID;
    private GameObject mapBox;
    BoxCollider box;
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
    public void UpdateMiniMap()
    {
        Common.Data.MapDefine mapCfg = DataManager.Instance.Maps[MapService.Instance.CurMapID];

        //根据配置决定小地图是否显示
        if (string.IsNullOrEmpty(mapCfg.MiniMap))
        {
            this.gameObject.SetActive(false);
            return;
        }
        else if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
        txtMapName.text = mapCfg.Name;
        imgMap.sprite = Resloader.Load<Sprite>("MiniMap/" + mapCfg.MiniMap);
    }
	// Update is called once per frame
	void Update ()
    {
        if (User.Instance.CurrentCharacterObject == null)
            return;
        if (mapBox == null)
        {
            mapBox = GameObject.FindGameObjectWithTag("MapBox");
            return;
        }
        
        if (mapBox == null)
        {
            Debug.LogError("该场景没有mapbox");
            return;
        }
        if (box == null)
            box = mapBox.GetComponent<BoxCollider>();

        if (box == null)
        {
            Debug.LogErrorFormat("该场景的box没有BoxCollider,地图id:{0}", this.mapID);
            return;
        }

        if (MapService.Instance.CurMapID > 0 && this.mapID != MapService.Instance.CurMapID)
        {
            this.mapID = MapService.Instance.CurMapID;
            UpdateMiniMap();
        }

        float realWidth = box.bounds.size.x;
        float realHeight = box.bounds.size.z;
        float offset_x = User.Instance.CurrentCharacterObject.transform.position.x - box.bounds.min.x;
        float offset_z = User.Instance.CurrentCharacterObject.transform.position.z - box.bounds.min.z;
        float pivotX = offset_x / realWidth;
        float pivotY = offset_z / realHeight;

        this.imgMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.imgMap.rectTransform.localPosition = Vector2.zero;
        this.player.transform.eulerAngles = new Vector3(0, 0, -User.Instance.CurrentCharacterObject.transform.eulerAngles.y);
    }
}
