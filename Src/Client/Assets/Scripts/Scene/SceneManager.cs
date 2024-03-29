﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneManager : MonoBehaviour
{
    UnityAction<float> onProgress = null;

    UnityAction _OnComplete;
    // Update is called once per frame
    void Update () {
		
	}

    public void LoadScene(string name,UnityAction callback = null)
    {
        _OnComplete = callback;
        StartCoroutine(LoadLevel(name));
    }

    IEnumerator LoadLevel(string name)
    {
        Debug.LogFormat("LoadLevel: {0}", name);
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = true;
        async.completed += LevelLoadCompleted;
        while (!async.isDone)
        {
            if (onProgress != null)
                onProgress(async.progress);
            yield return null;
        }
    }

    private void LevelLoadCompleted(AsyncOperation obj)
    {
        if (onProgress != null)
            onProgress(1f);
        Debug.Log("LevelLoadCompleted:" + obj.progress);
        if (_OnComplete != null)
        {
            _OnComplete();
            _OnComplete = null;
        }
    }
}
