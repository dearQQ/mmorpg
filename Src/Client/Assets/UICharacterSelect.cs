using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UICharacterSelect : MonoBehaviour {

    [SerializeField]
    private GameObject CreateUI;
    [SerializeField]
    private Transform characterList;
    [SerializeField]
    private GameObject characterItem;
    [SerializeField]
    private List<GameObject> jobModel = new List<GameObject>();
    int curIndex = 0;
    // Use this for initialization
    void Start ()
    {
        UserService.Instance.OnGameEnter = this.OnGameEnter;
        int characterCount = Models.User.Instance.Info.Player.Characters.Count;
        Debug.Log("现有角色数量：" + characterCount);
        if (characterCount > 0)
        {
            CreateUI.SetActive(false);
            for (int i = 0; i < 3; i++)
            {
                GameObject item = GameObject.Instantiate(characterItem);
                item.SetActive(true);
                item.transform.SetParent(characterList);
                item.transform.localScale = Vector3.one;
                item.transform.Find("bg").gameObject.SetActive(i == curIndex);
                //有角色
                if (i < characterCount)
                {
                    item.transform.Find("lv").GetComponent<Text>().text = Models.User.Instance.Info.Player.Characters[i].Level.ToString();
                    item.transform.Find("name").GetComponent<Text>().text = Models.User.Instance.Info.Player.Characters[i].Name;
                }
                else //没角色
                {
                    item.transform.Find("lv").GetComponent<Text>().text = "";
                    item.transform.Find("name").GetComponent<Text>().text = "创建新角色";
                }
                int index = i;
                item.GetComponent<Button>().onClick.AddListener(delegate()
                {
                    Debug.Log("点击了角色：" + index);
                    if (index > characterCount - 1)
                    {
                        CreateUI.SetActive(true);
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        characterList.GetChild(curIndex).Find("bg").gameObject.SetActive(false);
                        characterList.GetChild(index).Find("bg").gameObject.SetActive(true);
                        curIndex = index;
                        foreach (var model in jobModel)
                        {
                            model.SetActive(jobModel.IndexOf(model) == index);
                        }
                    }
                });
            }
        }
        else
        {
            CreateUI.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    //进入游戏
    public void EnterGame()
    {
        UserService.Instance.GameEnter(curIndex);
    }
    void OnGameEnter(SkillBridge.Message.Result result,string msg)
    {
        if (result == SkillBridge.Message.Result.Success)
        {
            MessageBox.Show("进入游戏", "选择角色");
        }
    }
    // Update is called once per frame
    void Update ()
    {
		
	}
}
