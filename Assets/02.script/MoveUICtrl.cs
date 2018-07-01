using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyUIAnimator;

public class MoveUICtrl : MonoBehaviour{

    public GameObject player;

    public Image north;
    public Image east;
    public Image south;
    public Image west;

    private Button[ ] moveButtons;

    private float during = 0.5f;
    private int crystalDirection = -1;
    public int CrystalDirection {
        get { return crystalDirection; }
    }
    public int UFODirection = -1;
    

    private UISpriteAnimation[ ] fadeinAnim;
    private UISpriteAnimation[ ] fadeoutAnim;

    

    private void Start() {
        fadeinAnim = new UISpriteAnimation[4];
        fadeoutAnim = new UISpriteAnimation[4];

        fadeinAnim[0] = UIAnimator.ChangeColor(north ,new Color(1,1,1,0) ,new Color(1 , 1 , 1 , 1) , during);
        fadeinAnim[1] = UIAnimator.ChangeColor(east ,new Color(1,1,1,0) , new Color(1 , 1 , 1 , 1) , during);
        fadeinAnim[2] = UIAnimator.ChangeColor(south , new Color(1,1,1,0) ,new Color(1 , 1 , 1 , 1) , during);
        fadeinAnim[3] = UIAnimator.ChangeColor(west ,new Color(1,1,1,0) , new Color(1 , 1 , 1 , 1) , during);
        fadeoutAnim[0] = UIAnimator.ChangeColorTo(north ,new Color(1 , 1 , 1 , 0) , during);
        fadeoutAnim[1] = UIAnimator.ChangeColorTo(east , new Color(1 , 1 , 1 , 0) , during);
        fadeoutAnim[2] = UIAnimator.ChangeColorTo(south ,new Color(1 , 1 , 1 , 0) , during);
        fadeoutAnim[3] = UIAnimator.ChangeColorTo(west , new Color(1 , 1 , 1 , 0) , during);


        moveButtons = new Button[4];
        moveButtons[0] = north.GetComponent<Button>();
        moveButtons[1] = east.GetComponent<Button>();
        moveButtons[2] = south.GetComponent<Button>();
        moveButtons[3] = west.GetComponent<Button>();
    }



    public void OnClickNorth() {
        UIAnimationOut();
        player.SendMessage("ClickMoveBtns",0,SendMessageOptions.DontRequireReceiver);
    }
    public void OnClickEast() {
        UIAnimationOut();
        player.SendMessage("ClickMoveBtns",1,SendMessageOptions.DontRequireReceiver);
    }
    public void OnClickSouth() {
        UIAnimationOut();
         player.SendMessage("ClickMoveBtns",2,SendMessageOptions.DontRequireReceiver);
    }
    public void OnClickWest() {
        UIAnimationOut();
        player.SendMessage("ClickMoveBtns",3,SendMessageOptions.DontRequireReceiver);
    }

    void UIAnimationOut() {
        for (int i = 0 ; i < 4 ; i++) {
            if (moveButtons[i].interactable) {
                moveButtons[i].interactable = false;
                fadeoutAnim[i].Play();
            }
        }
    }

    //이동이 끝나고 다시 이동 버튼을 키는 함수
    public void UIAnimationIn() {



        PlayerCtrl pc = player.GetComponent<PlayerCtrl>();

        MatrixState[ ] states = new MatrixState[4];
        states[0] = pc.pos.GetNorthState();
        states[1] = pc.pos.GetEastState();
        states[2] = pc.pos.GetSouthState();
        states[3] = pc.pos.GetWestState();


        //크리스탈과 ufo 방향을 초기화
        crystalDirection = -1;
        UFODirection = -1;

        //갈수 있는 곳이면 버튼을 키기
        for (int i = 0 ; i < 4 ; i++) {

            Debug.Log(states[i]);

            if (states[i] == MatrixState.CAN) {
                moveButtons[i].interactable = true;
                fadeinAnim[i].Play();
            }
            if (states[i] == MatrixState.CRYSTAL)
                crystalDirection = i;
            if (states[i] == MatrixState.UFO)
                UFODirection = i;
        }

        

        if (crystalDirection!=-1 && PlayerMgr.Instance.item==PlayerItem.EMPTY) {//크리스탈 옆에 있고 크리스탈을 안갖고 있다면
            
            GameObject.Find("Canvas-Overlay").SendMessage("ChangeMineButtonState" , true , 
                SendMessageOptions.DontRequireReceiver);

        }
        
        if (UFODirection != -1 && (PlayerMgr.Instance.item==PlayerItem.BLUECRYSTAL
            ||PlayerMgr.Instance.item==PlayerItem.REDCRYSTAL)) {//UFO가 옆에 있고 크리스탈을 갖고 있다면
            
            GameObject.Find("Canvas-Overlay").SendMessage("ChangeDropButtonState" , true , 
                SendMessageOptions.DontRequireReceiver);

        }


        GameMgr.Instance.NonClickPanel.SetActive(false);
    }
}
