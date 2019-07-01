using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform viewPoint;

    public GameObject player;

    private GameObject _mainCityCamera;

    public GameObject MainCamera
    {
        get {return _mainCityCamera; }
    }

    private void Start()
    {
        _mainCityCamera = GameObject.FindGameObjectWithTag("MainCityCamera");
        DontDestroyOnLoad(_mainCityCamera);
        GameManager.CameraMgr = this;
    }
    private void LateUpdate()
    {
        if (player == null || _mainCityCamera == null)
            return;

        _mainCityCamera.transform.position = player.transform.position;
        _mainCityCamera.transform.rotation = player.transform.rotation;
    }
}
