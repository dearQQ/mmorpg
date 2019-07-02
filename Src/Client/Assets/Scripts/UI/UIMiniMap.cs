using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
public class UIMiniMap : MonoBehaviour {
    int mapID;
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
    public void Init()
    {
        Common.Data.MapDefine mapDefine = DataManager.Instance.Maps[MapService.Instance.CurMapID];
        txtMapName.text = mapDefine.Name;
        mapID = MapService.Instance.CurMapID;

        if (string.IsNullOrEmpty(mapDefine.MiniMap))
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
            this.gameObject.SetActive(true);

        imgMap.sprite = Resloader.Load<Sprite>("MiniMap/" + mapDefine.MiniMap);

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
        else if (MapService.Instance.CurMapID > 0 && this.mapID != MapService.Instance.CurMapID)
        {
            Init();
        }
        if (box == null)
            box = mapBox.GetComponent<BoxCollider>();
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
