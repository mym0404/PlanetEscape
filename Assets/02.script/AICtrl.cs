using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class AICtrl : MonoBehaviour {

    

    //캐릭터 상태
    private bool Moving = false;
    private bool Turning = false;
    private bool Mining = false;

    
    //캐릭터의 위치
    public Position pos;
    //애니메이터
    private Animator moveAnim;
    private Animator robotAnim;

    //방향 정보
    private int direction; //0 북 1 동 2 남 3 서
    private int crystalDirection = -1;
    //private int UFODirection = -1;


    //광물캐는 이펙트
    public GameObject mineEffect;
    public Transform mineEffectPos;
    public Transform robotTr;

    //현재 캐는 크리스탈 ID
    private int ClickCrystalID = -1;

    //크리스탈 프리팹
    public GameObject blueCrystal;
    public GameObject redCrystal;
    private GameObject haveCrystal = null;

    //AI의 상태
    PlayerItem item = PlayerItem.EMPTY;



    //플레이어의 방향을 알아내는 함수
    public int GetDirection() {
        return direction;
    }

    public GameObject moveCanvas;

    private void Awake() {


        
        pos = new Position(0 , 0);




        moveAnim = GetComponentsInChildren<Animator>()[0];
        robotAnim = GetComponentsInChildren<Animator>()[1];

    }


    public void SetPosition(int r , int c) {
        pos.R = r;
        pos.C = c;
    }


    // Use this for initialization
    void Start() {
        //방향 지정
        direction = 2;
        //현재 위치를 지정하고 이 위치의 타일 상태를 AI로 변경한다.
        SetPosition(12 , 10);
        GameMgr.Instance.ChangeMatrixState(pos , MatrixState.AI);

        StartCoroutine(GoToCoordinate(new Position(25 , 25)));
    }

    /// <summary>
    /// AI Function
    /// </summary>
    /// <param name="destPos"></param>
    /// <returns></returns>
    IEnumerator GoToCoordinate(Position destPos) { 

        int remain; //행,열 합쳐서 몇번 이동해야 하는지
        int moveDir;
        
        //처음에 갈 거리가 0이면 함수를 종료한다
        remain=pos.GetDistance(destPos ,out moveDir);
        if (remain == 0)
            yield break;
        
        //반복문
        while (remain > 0) {
            remain=pos.GetDistance(destPos ,out moveDir);
        
            Debug.Log("남은 거리 : " + remain + " 가야하는 방향 : "+moveDir);

            if (!TryMove(moveDir)) { //이동 실패시

                if (moveDir == 0) {//만약 가야하는 방향이 북쪽이었는데 이동이 실패했다면,
                    //동쪽으로 이동 시도
                    if (!TryMove(1)) {
                        //남쪽으로 이동 시도
                        if (!TryMove(2)) {
                            //서쪽으로 이동 시도
                            if (!TryMove(3)) {
                                Debug.Log("이동 실패");
                            }
                        }
                    }
                } else if (moveDir == 1) {
                    if (!TryMove(2)) {
                        if (!TryMove(3)) {
                            if (!TryMove(0)) {
                                Debug.Log("이동 실패");
                            }
                        }
                    }
                }else if (moveDir == 2) {
                    if (!TryMove(3)) {
                        if (!TryMove(0)) {
                            if (!TryMove(1)) {
                                Debug.Log("이동 실패");
                            }
                        }
                    }
                }else if (moveDir == 3) {
                    if (!TryMove(0)) {
                        if (!TryMove(1)) {
                            if (!TryMove(2)) {
                                Debug.Log("이동 실패");
                            }
                        }
                    }
                }
                

                
                
                
            }

            yield return new WaitForSeconds(4.0f);
        }

        

        yield break;
    }



    //이동 시도 이동했으면 true 이동 실패시 false 반환
    public bool TryMove(int dir) {

        //해당 방향이 갈수없는 곳인지 체크
        switch (dir) {
            case 0:
                if (pos.GetNorthState() != MatrixState.CAN)
                    return false;
                break;
            case 1:
                if (pos.GetEastState() != MatrixState.CAN)
                    return false;
                break;
            case 2:
                if (pos.GetSouthState() != MatrixState.CAN)
                    return false;
                break;
            case 3:
                if (pos.GetWestState() != MatrixState.CAN)
                    return false;
                break;
            default:

                break;
        }



        //현재 방향과 클릭한 UI의 방향에 따라 어떤 함수를실행시킬것인지 결정한다
        if (dir == direction) {
            StartCoroutine(MoveMove());
            return true;
        } else if (dir + 1 == direction || (dir == 3 && direction == 0)) {//좌회전
            StartCoroutine(LeftMove());
            return true;
        } else if (dir - 1 == direction || (dir == 0 && direction == 3)) {//우회전
            StartCoroutine(RightMove());
            return true;
        } else { // 180도 회전
            StartCoroutine(BackMove());
            return true;
        }
    }
    IEnumerator MoveMove() {



        MoveForward();
        yield return new WaitUntil(() => { return !Moving; });
    }
    IEnumerator LeftMove() {

        Turn(0);
        //턴을 기다리는 루틴을 넣어야 함
        yield return new WaitUntil(() => { return !Turning; });



        MoveForward();
        yield return new WaitUntil(() => { return !Moving; });

    }
    IEnumerator RightMove() {

        Turn(1);
        yield return new WaitUntil(() => { return !Turning; });


        MoveForward();
        yield return new WaitUntil(() => { return !Moving; });
    }
    IEnumerator BackMove() {

        Turn(2);
        yield return new WaitUntil(() => { return !Turning; });



        MoveForward();
        yield return new WaitUntil(() => { return !Moving; });
    }

    IEnumerator Mine() {
        //만약 주위에 크리스탈이 없다면 종료
        if (crystalDirection == -1)
            yield break;

        
        Collider[ ] colliders = Physics.OverlapSphere(transform.position , 5 , 1 << 11);
        //주변에 있는 크리스탈에 광물이 남아있는지 체크한다
        if (colliders.Length > 0) {

            if (colliders.Length > 1) {
                Debug.Log("두개이상의 크리스탙이 감지됩니다.");
                yield break;
            }
            int remainCrystal = GameMgr.Instance.GetCrystalState(colliders[0].gameObject.GetComponent<CrystalInitialization>().CrystalID , true);
            Debug.Log("남아있는 푸른 크리스탈의 개수 : " + remainCrystal.ToString());
            if (remainCrystal < 0) {
                Debug.Log("남아있는 크리스탈이 없습니다.");
                yield break;
            }
        }

        //현재어떤걸 캐야하는 상황인지 업데이트한다
        ClickCrystalID = colliders[0].gameObject.GetComponent<CrystalInitialization>().CrystalID;









        //현재 방향과 클릭한 UI의 방향에 따라 어떤 함수를실행시킬것인지 결정한다
        if (crystalDirection == direction) {

        } else if (crystalDirection + 1 == direction || (crystalDirection == 3 && direction == 0)) {//좌회전
            Turn(0);
            yield return new WaitUntil(() => { return !Turning; });
        } else if (crystalDirection - 1 == direction || (crystalDirection == 0 && direction == 3)) {//우회전
            Turn(1);
            yield return new WaitUntil(() => { return !Turning; });
        } else { // 180도 회전
            Turn(2);
            yield return new WaitUntil(() => { return !Turning; });
        }





        //캐는 중이라고 업데이트
        Mining = true;
        robotAnim.SetTrigger("MINE");

        GameObject effect = Instantiate(mineEffect , mineEffectPos.position , Quaternion.LookRotation(robotTr.forward) , mineEffectPos);

        //광물 캐는걸 기다리는 루틴
        yield return new WaitUntil(() => { return !Mining; });


        robotAnim.SetTrigger("MINEEND");

        Destroy(effect , 2.0f);


        //클릭크리스탈 아이디를 AI 각각 따로 만들어주자
        GameMgr.Instance.UpdateCrystalState(ClickCrystalID , true , 1); // 푸른 크리스탈을 줄어들게한다



        //크리스탈을 AI의 머리 위에 생성한다
        haveCrystal = Instantiate(blueCrystal , new Vector3(0 , 3000f , 0) , Quaternion.identity , transform);
        haveCrystal.transform.localPosition = new Vector3(0 , 2.15f , 0);

        //AI의 상태를 크리스탈을 든 상태로 변경한다
        item = PlayerItem.BLUECRYSTAL;

        yield break;
    }

    //앞으로 이동 함수
    public void MoveForward() {

        Moving = true;

        Transform robotTr = robotAnim.gameObject.GetComponent<Transform>();
        robotTr.localPosition = Vector3.zero;

        //원래 있던 곳을 CAN으로 상태를 바꾼다
        GameMgr.Instance.ChangeMatrixState(pos , MatrixState.CAN);

        switch (direction) {
            case 0:
                pos.R--;
                moveAnim.SetTrigger("MOVE_NORTH");
                break;
            case 1:
                pos.C++;
                moveAnim.SetTrigger("MOVE_EAST");
                break;
            case 2:
                pos.R++;
                moveAnim.SetTrigger("MOVE_SOUTH");
                break;
            case 3:
                pos.C--;
                moveAnim.SetTrigger("MOVE_WEST");
                break;
            default:
                break;
        }

        GameMgr.Instance.ChangeMatrixState(pos , MatrixState.AI);

        //주위 상황을 업데이트한다
        UpdateAround();

        robotAnim.SetTrigger("MOVE");
    }


    //로봇 회전 함수
    public void Turn(int turnDir) {

        Turning = true;

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

    //상태변경함수 (애니메이션으로부터)
    public void MoveEnd() {
        Moving = false;
    }
    public void TurnEnd() {
        Turning = false;
    }
    public void MineEnd() {
        Mining = false;
    }

    public void DropCrystal() {
        DestroyImmediate(haveCrystal);
        haveCrystal = null;

        //플레이어의 상태 변경
        item = PlayerItem.EMPTY;
    }


    private void UpdateAround() {
        crystalDirection = -1;


        if (pos.GetNorthState() == MatrixState.CRYSTAL) {
            crystalDirection = 0;
        } else if (pos.GetEastState() == MatrixState.CRYSTAL) {
            crystalDirection = 1;
        }else if (pos.GetSouthState() == MatrixState.CRYSTAL) {
            crystalDirection = 2;
        }else if (pos.GetWestState() == MatrixState.CRYSTAL) {
            crystalDirection = 3;
        }
    }
}
