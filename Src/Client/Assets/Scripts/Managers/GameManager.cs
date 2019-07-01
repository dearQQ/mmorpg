using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
public class GameManager : MonoBehaviour
{

    private static SceneManager _sceneMgr;

    private static GameObjectManager _gameObjectMgr;

    private static CameraManager _cameraMgr;

    private static UINameBarManager _nameMgr;

    private static UIManager _uiMgr;

    public static SceneManager SceneMgr
    {
        get { return _sceneMgr; }
    }
    public static GameObjectManager GameObjectMgr
    {
        get { return _gameObjectMgr; }
    }

    public static CameraManager CameraMgr
    {
        get { return _cameraMgr; }
        set { _cameraMgr = value; }
    }
    public static UINameBarManager NameMgr
    {
        get { return _nameMgr; }
        set { _nameMgr = value; }
    }

    public static UIManager UIMgr
    {
        get { return _uiMgr; }
    }
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        GameManager._sceneMgr = this.gameObject.AddComponent<SceneManager>();
        GameManager._gameObjectMgr = this.gameObject.AddComponent<GameObjectManager>();
        GameManager._uiMgr = GameObject.Find("UIRoot").GetComponent<UIManager>();
    }
}