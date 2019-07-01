using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour {

    System.Type Type
    {
        get { return this.GetType(); }
    }

    public virtual void OnClickClose()
    {
        GameManager.UIMgr.OnClose(this.Type);
    }
}
