using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerRaycastTarget : MonoBehaviour,IPointerClickHandler {
    private Transform tr;
    public GameObject HUD;
    private GameObject hud_temp;

    private bool isOn=false;

    private void Awake() {
        tr = transform;
    }

    public void OnPointerClick(PointerEventData eventData) {
        SetHUD();
        
    }

    private void SetHUD() {
        if (!isOn) {//만약 HUD 이펙트가 꺼져있다면 
            isOn = true;
            hud_temp = Instantiate(HUD , tr.transform.position , Quaternion.identity , tr);
        } else {//만약 HUD 이펙트가 켜져있다면
            isOn = false;
            DestroyImmediate(hud_temp);
        }
    }



}
