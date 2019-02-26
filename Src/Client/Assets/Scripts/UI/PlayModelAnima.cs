using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModelAnima : MonoBehaviour {

    Animator anima;
    // Use this for initialization
    void Start () {
        anima = this.transform.GetComponent<Animator>();
	}
    private void OnEnable()
    {
        if(anima != null)
            anima.SetTrigger("SkillA");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
