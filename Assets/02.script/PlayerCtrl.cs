using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerCtrl : MonoBehaviour {

    //캐릭터의 위치
    public Position pos;
    public int startPos_R;
    public int startPos_C;

    //애니메이터
    private Animator moveAnim;
    private Animator robotAnim;
    
    //방향 정보
    private int direction; //0 북 1 동 2 남 3 서
    
    //카메라 정보
    private CameraCtrl camCtrl;

    
    //광물캐는 이펙트
    public GameObject mineEffect;
    public Transform mineEffectPos;
    public Transform robotTr;


    //광물캐기 등에 사용되는 로딩 패널
    public GameObject LoadingPanel;

    //크리스탈 프리팹
    public GameObject blueCrystal;
    public GameObject redCrystal;
    private GameObject haveCrystal=null;

    

    //플레이어의 방향을 알아내는 함수
    public int GetDirection() {
        return direction;
    }

    //포지션을 지정하는 함수
    public void SetPosition(int r , int c) {
        pos.R = r;
        pos.C = c;
    }

    public GameObject moveCanvas;

    private void Awake() {

        pos = new Position(0,0);
        //방향 지정
        direction = 2;
        //현재 위치를 지정하고 이 위치의 타일 상태를 플레이어로 변경한다.
        SetPosition(startPos_R , startPos_C);
        


        moveAnim = GetComponentsInChildren<Animator>()[0];
        robotAnim = GetComponentsInChildren<Animator>()[1];

        camCtrl = Camera.main.GetComponent<CameraCtrl>();
    }


    

    // Use this for initialization
    void Start () {
        ScriptMgr.Instance.UpdateTransform(GetComponent<Identifier>().GetID());


        GameMgr.Instance.ChangeMatrixState(pos , MatrixState.PLAYER);
	}
	

    //이동 버튼을 눌렀을 때
    public void ClickMoveBtns(int dir) {
        
        
        //MatrixState destState;
        //GameMgr.Instance.GetMatrixState(pos , out destState);

        //갈수없는 곳인지 체크
        switch (dir) {
            case 0:
                if (pos.GetNorthState() != MatrixState.CAN && PlayerMgr.Instance.Mining == false) {
                    moveCanvas.GetComponent<MoveUICtrl>().UIAnimationIn();
                    return;
                }
                break;
                
            case 1:
                if (pos.GetEastState() != MatrixState.CAN && PlayerMgr.Instance.Mining == false) {
                    moveCanvas.GetComponent<MoveUICtrl>().UIAnimationIn();
                    return;
                }
                break;
            case 2:
                if (pos.GetSouthState() != MatrixState.CAN && PlayerMgr.Instance.Mining == false) {
                    moveCanvas.GetComponent<MoveUICtrl>().UIAnimationIn();
                    return;
                }
                break;
            case 3:
                if (pos.GetWestState() != MatrixState.CAN && PlayerMgr.Instance.Mining == false) {
                    moveCanvas.GetComponent<MoveUICtrl>().UIAnimationIn();
                    return;
                }
                break;
            default:

                break;
        }
        
        //채굴버튼을 끈다
        GameObject.FindGameObjectWithTag("CANVASOVERLAY").SendMessage
            ("ChangeMineButtonState" , false , SendMessageOptions.DontRequireReceiver);
        //드랍버튼을 끈다
        GameObject.FindGameObjectWithTag("CANVASOVERLAY").SendMessage
            ("ChangeDropButtonState" , false , SendMessageOptions.DontRequireReceiver);


        //논클릭패널을 킨다
        GameMgr.Instance.NonClickPanel.SetActive(true);


        //현재 방향과 클릭한 UI의 방향에 따라 어떤 함수를실행시킬것인지 결정한다
        if (dir == direction) {
            StartCoroutine(MoveMove());
            return;
        } else if (dir + 1 == direction || (dir == 3 && direction == 0)) {//좌회전
            StartCoroutine(LeftMove());
            return;
        } else if (dir - 1 == direction || (dir == 0 && direction == 3)) {//우회전
            StartCoroutine(RightMove());
            return;
        } else { // 180도 회전
            StartCoroutine(BackMove());
            return;
        }
    }


    
    IEnumerator MoveMove() {

        if (PlayerMgr.Instance.Mining) {
            StartCoroutine(Mine());
            yield break;

        }

        
        MoveForward();
        yield return new WaitUntil(() => { return !PlayerMgr.Instance.Moving; });
        moveCanvas.SendMessage("UIAnimationIn" , SendMessageOptions.DontRequireReceiver);
    }
    IEnumerator LeftMove() {
        
        Turn(0);
        yield return new WaitUntil(() => { return !PlayerMgr.Instance.Turning; });

        if (PlayerMgr.Instance.Mining) {
            StartCoroutine(Mine());
            yield break;

        }

        
        MoveForward();
        yield return new WaitUntil(() => { return !PlayerMgr.Instance.Moving; });
        moveCanvas.SendMessage("UIAnimationIn" , SendMessageOptions.DontRequireReceiver);
    }
    IEnumerator RightMove() {
        
        Turn(1);
        yield return new WaitUntil(() => { return !PlayerMgr.Instance.Turning; });

        if (PlayerMgr.Instance.Mining) {
            StartCoroutine(Mine());
            yield break;

        }

       
        MoveForward();
        yield return new WaitUntil(() => { return !PlayerMgr.Instance.Moving; });

        moveCanvas.SendMessage("UIAnimationIn" , SendMessageOptions.DontRequireReceiver);
    }
    IEnumerator BackMove() {

        Turn(2);
        yield return new WaitUntil(() => { return !PlayerMgr.Instance.Turning; });

        if (PlayerMgr.Instance.Mining) {
            StartCoroutine(Mine());
            yield break;

        }

        
        MoveForward();
        yield return new WaitUntil(() => { return !PlayerMgr.Instance.Moving; });

        moveCanvas.SendMessage("UIAnimationIn" , SendMessageOptions.DontRequireReceiver);
    }
    //채굴 코루틴
    IEnumerator Mine() {
        Debug.Log("채집 시작");
        
        robotAnim.SetTrigger("MINE");

        GameObject effect=Instantiate(mineEffect , mineEffectPos.position , Quaternion.LookRotation(robotTr.forward),mineEffectPos);

        //로딩패널
        LoadingPanel.SetActive(true);
        Slider mineSlider = LoadingPanel.GetComponentInChildren<Slider>();
        mineSlider.value = 0;

        mineSlider.GetComponent<Animation>().Play();


        //더이상 광물을 캐지 않는 상태가 될때까지 기다린다
        yield return new WaitUntil(() => { return !PlayerMgr.Instance.Mining; });


        //논클릭패널을 비활성화시킨다
        GameMgr.Instance.NonClickPanel.SetActive(false);

        robotAnim.SetTrigger("MINEEND");

        Destroy(effect,2.0f);

        //로딩패널 비활성화
        LoadingPanel.SetActive(false);

        Debug.Log("mine end");

        //플레이어, 크리스탈 상태를 업데이트한다.
        if (PlayerMgr.Instance.ClickBlue == true && PlayerMgr.Instance.ClickRed==false) { //푸른 크리스탈을 캤을 때
            Debug.Log("푸른 크리스탈을 캤습니다! 크리스탈 ID : "
                + PlayerMgr.Instance.ClickCrystalID.ToString());

            GameMgr.Instance.UpdateCrystalState(PlayerMgr.Instance.ClickCrystalID , true , 1); // 푸른 크리스탈을 줄어들게한다
            PlayerMgr.Instance.item = PlayerItem.BLUECRYSTAL;

            //크리스탈을 플레이어의 머리 위에 생성한다
            haveCrystal = Instantiate(blueCrystal,new Vector3(0 , 3000f , 0) , Quaternion.identity,transform);
            haveCrystal.transform.localPosition = new Vector3(0 , 2.15f , 0);

        } else if (PlayerMgr.Instance.ClickRed == true && PlayerMgr.Instance.ClickBlue==false) { //붉은 크리스탈을 캤을 때
            Debug.Log("붉은 크리스탈을 캤습니다! 크리스탈 ID : "
                + PlayerMgr.Instance.ClickCrystalID.ToString());

            GameMgr.Instance.UpdateCrystalState(PlayerMgr.Instance.ClickCrystalID , false , 1); // 붉은 크리스탈을 줄어들게한다
            PlayerMgr.Instance.item = PlayerItem.REDCRYSTAL;

            //크리스탈을 플레이어의 머리 위에 생성한다
            haveCrystal = Instantiate(redCrystal,new Vector3(0 , 3000f , 0) , Quaternion.identity,transform);
            haveCrystal.transform.localPosition = new Vector3(0 , 2.15f , 0);
        }

        
    }

    //앞으로 이동 함수
    public void MoveForward() {

        PlayerMgr.Instance.Moving = true;

        Transform robotTr = robotAnim.gameObject.GetComponent<Transform>();
        robotTr.localPosition = Vector3.zero;

        GameMgr.Instance.ChangeMatrixState(pos , MatrixState.CAN);
        switch(direction){
            case 0:
                pos.R--;
                camCtrl.Move(CamMoveDirection.NORTH);
                moveAnim.SetTrigger("MOVE_NORTH");
                break;
            case 1:
                pos.C++;
                camCtrl.Move(CamMoveDirection.EAST);
                moveAnim.SetTrigger("MOVE_EAST");
                break;
            case 2:
                pos.R++;
                camCtrl.Move(CamMoveDirection.SOUTH);
                moveAnim.SetTrigger("MOVE_SOUTH");
                break;
            case 3:
                pos.C--;
                camCtrl.Move(CamMoveDirection.WEST);
                moveAnim.SetTrigger("MOVE_WEST");
                break;
            default:
                break;
        }

        Debug.Log("R = " + pos.R.ToString() + " C = " + pos.C.ToString());
        GameMgr.Instance.ChangeMatrixState(pos , MatrixState.PLAYER);


        robotAnim.SetTrigger("MOVE");
    }
    //로봇 회전 함수
    public void Turn(int turnDir){
        PlayerMgr.Instance.Turning = true;
        
        Transform robotTr = robotAnim.gameObject.GetComponent<Transform>();
        robotTr.localPosition = Vector3.zero;
        
        switch (turnDir) {
            case 0: //좌회전

                direction--;
                if (direction == -1)
                    direction = 3;

                robotAnim.SetTrigger("LEFT");
                break;
            case 1: //우회전
              
                direction++;
                if (direction == 4)
                    direction = 0;

                robotAnim.SetTrigger("RIGHT");
                break;
            case 2: //180도 회전

                direction--;
                if (direction == -1)
                    direction = 3;
                direction--;
                if (direction == -1)
                    direction = 3;

                robotAnim.SetTrigger("BACK");
                break;
            default:

                break;
        }
      
    }


    public void MoveEnd() {
        PlayerMgr.Instance.Moving = false;
    }
    public void TurnEnd() {
        PlayerMgr.Instance.Turning = false;
    }
    public void MineEnd() {
        PlayerMgr.Instance.Mining = false;
    }

    public void DropCrystal() {
        DestroyImmediate(haveCrystal);
        haveCrystal = null;

        //플레이어의 상태 변경
        PlayerMgr.Instance.item = PlayerItem.EMPTY;
    }
}
