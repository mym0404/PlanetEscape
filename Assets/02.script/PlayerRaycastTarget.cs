using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerRaycastTarget : MonoBehaviour,IPointerClickHandler {
    private Transform tr;
    public GameObject HUD;

    
    private void Awake() {
        tr = transform;
    }

    public void OnPointerClick(PointerEventData eventData) {
        SetHUD();
        
    }
    //이상함
    private void SetHUD() {
        if (PlayerMgr.Instance.HUD_Target==-1) {//만약 HUD 이펙트가 꺼져있다면 



            DestroyImmediate(PlayerMgr.Instance.HUD_Object);

            PlayerMgr.Instance.HUD_Object = Instantiate(HUD , tr.transform.position , Quaternion.identity , tr);

            PlayerMgr.Instance.HUD_Target = GetComponent<Identifier>().GetID();

            

            Debug.Log("HUD_Target : " + PlayerMgr.Instance.HUD_Target.ToString());
        } else {//만약 HUD 이펙트가 켜져있다면

            DestroyImmediate(PlayerMgr.Instance.HUD_Object);

            if (GetComponent<Identifier>().GetID() == PlayerMgr.Instance.HUD_Target) {//만약 같은 사람을 클릭했을 경우 이펙트 제거
                
                PlayerMgr.Instance.HUD_Target = -1;
                
            } else { //다른사람 일 경우
                PlayerMgr.Instance.HUD_Object = Instantiate(HUD , tr.transform.position , Quaternion.identity , tr);
                PlayerMgr.Instance.HUD_Target = GetComponent<Identifier>().GetID();
                
            }

            Debug.Log("HUD_Targetelse : " + PlayerMgr.Instance.HUD_Target.ToString());
            

            
        }

        if (PlayerMgr.Instance.HUD_Target == -1) {
            //버튼 끄기
            GameObject.FindGameObjectWithTag("CANVASOVERLAY").SendMessage("ChangeFireButtonState" , false);
        } else {
            //버튼 키기
            GameObject.FindGameObjectWithTag("CANVASOVERLAY").SendMessage("ChangeFireButtonState" , true);
        }
    }



}
