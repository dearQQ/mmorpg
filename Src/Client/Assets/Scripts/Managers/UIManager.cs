using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public class UIGroup
    {
        public Dictionary<string, GameObject> Groups = new Dictionary<string, GameObject>();
        private List<string> _groupName = new List<string>
        {
            "Main",
            "Box",
            "HpBar",
        };
        public UIGroup(Transform parent)
        {
            for (int i = 0; i < _groupName.Count; i++)
            {
                GameObject go = new GameObject();
                go.name = "UIGroup-" + _groupName[i];
                RectTransform rt = go.AddComponent<RectTransform>();
                go.transform.SetParent(parent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                rt.sizeDelta = new Vector2(Screen.width, Screen.height);
                Groups[_groupName[i]] = go;
            }
        }
    }

    public class UIElement
    {
        public string path;

        public GameObject instance;

        public bool isGlobal;
        //隐藏的时间
        public DateTime hideTime;
    }

    private float timer = 0f;
    public float RecyclingTime = 120f;
    private Dictionary<Type, UIElement> _uiElements = new Dictionary<Type, UIElement>();
    private UIGroup _uiGroup;

    public UIManager()
    {
        //注册UI
        //_uiElements.Add(UITest, new UIElement() { path = "UITest"});
        _uiElements.Add(typeof(UIMiniMap), new UIElement() { path = "UI/UIMiniMap", isGlobal = true });
        _uiElements.Add(typeof(UIMainCity), new UIElement() { path = "UI/UIMainCity", isGlobal = true });
    }

    private void Start()
    {
        _uiGroup = new UIGroup(this.transform);
        DontDestroyOnLoad(this);
    }
    /// <summary>
    /// 打开UI界面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T OpenUI<T>(string group)
    {
        Type type = typeof(T);
        UIElement uiInfo;
        if (_uiElements.TryGetValue(type, out uiInfo))
        {
            if (uiInfo.instance != null)
            {
                uiInfo.instance.SetActive(true);
            }
            else
            {
                UnityEngine.Object obj = Resources.Load(uiInfo.path);
                if (obj == null)
                {
                    Debug.LogError("打开的UI路径不存在:" + uiInfo.path);
                    return default(T);
                }
                uiInfo.instance = GameObject.Instantiate(obj as GameObject);
            }
            uiInfo.instance.transform.SetParent(_uiGroup.Groups[group].transform);
            uiInfo.instance.transform.SetAsLastSibling();
            uiInfo.instance.transform.localPosition = Vector3.zero;
            uiInfo.instance.transform.localScale = Vector3.one;
            return uiInfo.instance.GetComponent<T>();
        }
        Debug.LogError("打开的UI未注册,Type = " + type);
        return default(T);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="type"></param>
    public void OnClose(Type type)
    {
        if (_uiElements[type] != null)
        {
            _uiElements[type].instance.SetActive(false);
            _uiElements[type].hideTime = DateTime.Now;
        }
    }

    //2分钟回收一次，超过2分钟没有使用的界面会被销毁
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= RecyclingTime)
        {
            foreach (var ui in _uiElements.Values)
            {
                if (ui.instance && ui.instance.activeSelf == false)
                {
                    if (DateTime.Now.Ticks - ui.hideTime.Ticks >= 120000f)
                    {
                        Destroy(ui.instance);
                        ui.instance = null;
                    }
                }
            }
            timer = 0f;
        }
    }
}
