using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPosition : MonoBehaviour {
    private RectTransform tr;
    public Transform playerTr;
	// Use this for initialization
	void Awake () {
        tr = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        tr.localPosition = new Vector3(playerTr.position.x , 0.02f , playerTr.position.z);
	}
}
