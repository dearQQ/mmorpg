using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UICreateCharacter : MonoBehaviour
{
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
    private int JobID = 1;
    // Use this for initialization
    void Start ()
    {
        this.InitCreateCharacterUI();
        UserService.Instance.OnCreateCharacter = this.OnCreateCharacter;
    }

    private void InitCreateCharacterUI()
    {
        if (jobs.Count != jobsInfo.Count || jobs.Count != jobModel.Count || jobsInfo.Count != jobModel.Count)
            Debug.LogError("角色数量和角色信息数量不相等");
        foreach (var job in jobs)
        {
            job.onValueChanged.AddListener((bool isOn) =>
            {
                int index = jobs.IndexOf(job);
                jobsInfo[index].SetActive(isOn);
                jobModel[index].SetActive(isOn);
                JobID = index + 1;
            });
            int idx = jobs.IndexOf(job);
            jobsInfo[idx].transform.Find("Info").GetComponent<Text>().text = DataManager.Instance.Characters[idx + 1].Description;
        }
    }
   
    public void OnClickEnterGame()
    {
        if (string.IsNullOrEmpty(name.text))
        {
            MessageBox.Show("请输入昵称","新建角色",MessageBoxType.Error);
            return;
        }
        UserService.Instance.CreateCharacter(name.text, JobID);
    }
    void OnCreateCharacter(SkillBridge.Message.Result result,string msg)
    {
        if (result == SkillBridge.Message.Result.Success)
        {
            MessageBox.Show("创建角色成功", "新建角色");
        }
        else
            MessageBox.Show(msg, "新建角色");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
