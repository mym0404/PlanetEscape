using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPos : MonoBehaviour {
    private Transform tr;
    private Transform parentTr;
    private bool isSetting = false;
	// Use this for initialization
	void Start () {
        tr = transform;
        
        StartCoroutine(WaitSet());
	}
    IEnumerator WaitSet() {
        yield return new WaitUntil(() => { return tr.parent; });
        parentTr = tr.parent;
    }
	
	// Update is called once per frame
	void Update () {
        if(isSetting==true)
            tr.position = new Vector3(parentTr.position.x , 0 , parentTr.position.z);
	}
}
