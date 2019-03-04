using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour {

    [SerializeField]
    private GameObject InitUI;
    [SerializeField]
    private GameObject LoadingUI;
    [SerializeField]
    private GameObject TipsUI;
    [SerializeField]
    private GameObject LoginUI;
    [SerializeField]
    private Image progressBar;
    [SerializeField]
    private Text progressTxt;

    // Use this for initialization
    IEnumerator Start () {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        InitUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        TipsUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        LoadingUI.SetActive(true);
        InitUI.SetActive(false);
        yield return new WaitForSeconds(1f);
        TipsUI.SetActive(false);
        yield return DataManager.Instance.LoadData();
        MapService.Instance.Init();
        
        for (int i = 0; i < 100; i++)
        {
            progressBar.fillAmount = (i + 1) / 100f;
            progressTxt.text = "正在加载:" + (i + 1) + "%";

            yield return new WaitForEndOfFrame();
        }
        
        LoadingUI.SetActive(false);
        LoginUI.SetActive(true);
        yield return 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
