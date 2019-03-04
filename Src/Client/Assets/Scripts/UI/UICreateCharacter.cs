using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UICreateCharacter : MonoBehaviour
{
    [SerializeField]
    private GameObject SelectCharacterUI;
    [SerializeField]
    private List<Toggle> jobs = new List<Toggle>();
    [SerializeField]
    private List<GameObject> jobsInfo = new List<GameObject>();
    [SerializeField]
    private List<GameObject> jobModel = new List<GameObject>();
    [SerializeField]
    private InputField name;
    [SerializeField]
    private Button startGame;
    private void Awake()
    {
        UserService.Instance.OnCreateCharacter = this.OnCreateCharacter;
    }

    // Use this for initialization
    void Start ()
    {
        this.InitCreateCharacterUI();
    }
    private void OnEnable()
    {
        this.InitCreateCharacterUI();
    }

    private void InitCreateCharacterUI()
    {
        if (jobs.Count != jobsInfo.Count || jobs.Count != jobModel.Count || jobsInfo.Count != jobModel.Count)
            Debug.LogError("角色数量和角色信息数量不相等");
        foreach (var job in jobs)
        {
            int idx = jobs.IndexOf(job);
            jobsInfo[idx].SetActive(idx == 0);
            jobModel[idx].SetActive(idx == 0);
            jobs[idx].isOn = idx == 0;
            jobsInfo[idx].transform.Find("Info").GetComponent<Text>().text = DataManager.Instance.Characters[idx + 1].Description;
            int index = jobs.IndexOf(job);
            job.onValueChanged.AddListener((bool isOn) =>
            {
                jobsInfo[index].SetActive(isOn);
                jobModel[index].SetActive(isOn);
            });
            
        }
    }
   
    public void OnClickEnterGame()
    {
        if (string.IsNullOrEmpty(name.text))
        {
            MessageBox.Show("请输入昵称","新建角色",MessageBoxType.Error);
            return;
        }

        int jobID = 1;
        foreach (var job in jobs)
        {
            if (job.isOn)
                jobID = jobs.IndexOf(job) + 1;
        }
        UserService.Instance.CreateCharacter(name.text, jobID);
    }
    void OnCreateCharacter(SkillBridge.Message.Result result,string msg)
    {
        if (result == SkillBridge.Message.Result.Success)
        {
            MessageBox.Show("创建角色成功", "新建角色");
            SelectCharacterUI.GetComponent<UICharacterSelect>().InitUI();
            SelectCharacterUI.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
            MessageBox.Show(msg, "新建角色");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
