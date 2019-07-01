using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UICharacterSelect : MonoBehaviour {

    [SerializeField]
    private GameObject CreateUI;
    [SerializeField]
    private Transform characterList;
    [SerializeField]
    private GameObject characterItem;
    [SerializeField]
    private List<GameObject> jobModel = new List<GameObject>();
    private List<GameObject> uiCharacterInfo = new List<GameObject>();
    int curIndex = 0;
    // Use this for initialization
    void Start ()
    {
        
    }

    void OnEnable()
    {
        InitUI();
    }
    public void InitUI()
    {
        List<NCharacterInfo> characters = Models.User.Instance.Info.Player.Characters;
        Debug.Log("现有角色数量：" + characters.Count);
        foreach (var go in uiCharacterInfo)
        {
            Destroy(go);
        }
        uiCharacterInfo.Clear();
        if (characters.Count > 0)
            CreateUI.SetActive(false);
        else
        {
            CreateUI.SetActive(true);
            this.gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            curIndex = curIndex > characters.Count ? characters.Count : curIndex;

            GameObject item = GameObject.Instantiate(characterItem);
            item.SetActive(true);
            item.transform.SetParent(characterList);
            item.transform.localScale = Vector3.one;
            item.transform.Find("bg").gameObject.SetActive(i == 0);
            item.transform.Find("lv").GetComponent<Text>().text = i < characters.Count ? DataManager.Instance.Characters[characters[i].Tid].Name : "";
            item.transform.Find("name").GetComponent<Text>().text = i < characters.Count ? characters[i].Name : "创建新角色";
            item.transform.Find("Add_img").gameObject.SetActive(i > characters.Count - 1);

            foreach (var model in jobModel)
            {
                model.SetActive(characters[0].Tid - 1 == jobModel.IndexOf(model));
            }
            int index = i;
            item.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                Debug.Log("点击了角色：" + index);
                //characterList.GetChild(curIndex).Find("bg").gameObject.SetActive(false);
                
                if (index > characters.Count - 1)
                {
                    CreateUI.SetActive(true);
                    this.gameObject.SetActive(false);
                }
                else
                {
                    foreach (var model in jobModel)
                    {
                        characterList.GetChild(jobModel.IndexOf(model)).Find("bg").gameObject.SetActive(jobModel.IndexOf(model) == index);
                        model.SetActive(characters[index].Tid - 1 == jobModel.IndexOf(model));
                    }
                }
                curIndex = index;
            });
            uiCharacterInfo.Add(item);
        }
    }
    //进入游戏
    public void EnterGame()
    {
        UserService.Instance.OnGameEnterRequest(curIndex);
        Models.User.Instance.CurrentCharacter = Models.User.Instance.Info.Player.Characters[curIndex];
    }
    
    // Update is called once per frame
    void Update ()
    {
		
	}
}
