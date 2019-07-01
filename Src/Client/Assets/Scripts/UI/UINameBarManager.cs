using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UINameBarManager:MonoBehaviour {
    [SerializeField]
    GameObject hpBar;
    Dictionary<string, GameObject> m_headBars = new Dictionary<string, GameObject>();
    // Use this for initialization
    private void Start()
    {
        hpBar = Resloader.Load<GameObject>("UI/UIHeadBar");
        GameManager.NameMgr = this;
    }
    // Update is called once per frame
    void Update ()
    {
		
	}
    public void AddHeadBar(Transform owner,Character chara)
    {
        GameObject go = Instantiate(hpBar, this.transform);
        go.transform.position = owner.position;
        go.transform.GetComponent<UIHeadBar>().owner = owner;
        go.transform.GetComponent<UIHeadBar>().cha = chara;
        go.name = chara.entityId + "_" + chara.Name;
        go.SetActive(true);
        m_headBars[chara.Name] =  go;
    }

    public void RemoveHearBar(string name)
    {
        if (m_headBars.ContainsKey(name))
        {
            Destroy(m_headBars[name]);
            m_headBars.Remove(name);
        }
    }
}
