using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModelAnima : MonoBehaviour {

    Animator anima;
    private void Awake()
    {
        anima = this.transform.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
       
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
