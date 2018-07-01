using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerRaycastTarget : MonoBehaviour,IPointerClickHandler {
    Transform tr;
    Vector2 UIPos;

    public GameObject ProfileCanvas;
    public GameObject ProfileImage;

    private GameObject profile;

    private void Awake() {
        tr = transform;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (PlayerMgr.Instance.isProfileImageOn == false) {
            UIPos = RectTransformUtility.WorldToScreenPoint(Camera.main , tr.position + Vector3.up * 5);
            profile=Instantiate(ProfileImage , UIPos , Quaternion.identity , ProfileCanvas.GetComponent<RectTransform>());
            PlayerMgr.Instance.isProfileImageOn = true;
        } else {
            DestroyImmediate(profile);
            PlayerMgr.Instance.isProfileImageOn = false;
        }
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
