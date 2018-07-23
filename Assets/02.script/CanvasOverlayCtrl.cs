using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyUIAnimator;

public class CanvasOverlayCtrl : MonoBehaviour {
    //채굴, 수거 버튼
    public Button mineBlue; //1
    public Button mineRed; //2 
    public Button dropBtn; //3
    //발사 버튼
    public Button fireBtn; //4

    public GameObject player;
    public MoveUICtrl uiCtrl;


    //채굴, 수거 버튼 애니메이션
    private UISpriteAnimation fadeinBlue;
    private UISpriteAnimation fadeoutBlue;
    private UISpriteAnimation fadeinRed;
    private UISpriteAnimation fadeoutRed;
    private UISpriteAnimation fadeinDrop;
    private UISpriteAnimation fadeoutDrop;
    //발사 애니메이션
    private UISpriteAnimation fadeinFire;
    private UISpriteAnimation fadeoutFire;


    public Text scoreText;


    private void Start() {
        fadeinBlue=UIAnimator.ChangeColorTo(mineBlue.gameObject.GetComponent<Image>() , new Color(1 , 1 , 1 , 1) , 0.5f);
        
        fadeoutBlue = UIAnimator.ChangeColorTo(mineBlue.gameObject.GetComponent<Image>()
            , new Color(1 , 1 , 1 , 100/255f) , 0.5f);
        
        fadeinRed=UIAnimator.ChangeColorTo(mineRed.gameObject.GetComponent<Image>() , new Color(1 , 1 , 1 , 1) , 0.5f);
        
        fadeoutRed = UIAnimator.ChangeColorTo(mineRed.gameObject.GetComponent<Image>()
            , new Color(1 , 1 , 1 , 100/255f) , 0.5f);

        fadeinDrop=UIAnimator.ChangeColorTo(dropBtn.gameObject.GetComponent<Image>() , new Color(1 , 1 , 1 , 1) , 0.5f);
        fadeoutDrop= UIAnimator.ChangeColorTo(dropBtn.gameObject.GetComponent<Image>()
            , new Color(1 , 1 , 1 , 100/255f) , 0.5f);

        fadeinFire=UIAnimator.ChangeColorTo(fireBtn.gameObject.GetComponent<Image>() , new Color(1 , 1 , 1 , 1) , 0.5f);
        fadeoutFire= UIAnimator.ChangeColorTo(fireBtn.gameObject.GetComponent<Image>()
            , new Color(1 , 1 , 1 , 100/255f) , 0.5f);
    }


    #region Button State Change

    //채굴 버튼의 상태가 변할 때
    void ChangeMineButtonState(bool On) {

        if (PlayerMgr.Instance.item != PlayerItem.EMPTY)
            return;
        
        if (On) {
            mineBlue.interactable = true;
            fadeinBlue.Play();

            mineRed.interactable = true;
            fadeinRed.Play();
        } else {
            fadeoutBlue.Play();
            fadeoutRed.Play();
           
            mineBlue.interactable = false;
            mineRed.interactable = false;
            

        }
    }
    //드랍 버튼의 상태가 변할 때
    void ChangeDropButtonState(bool On) {
        if (On) {
            dropBtn.interactable = true;
            fadeinDrop.Play();
        } else {
            dropBtn.interactable = false;
            fadeoutDrop.Play();
        }
    }

    //발사 버튼의 상태가 변할 때
    void ChangeFireButtonState(bool On) {
        if (On) {
            fireBtn.interactable = true;
            fadeinFire.Play();
        } else {
            fireBtn.interactable = false;
            fadeinFire.Play();
        }
    }

    #endregion

    #region ButtonClick
    //채굴버튼 클릭
    public void OnClickBlueMine() {
        Collider[ ] colliders = Physics.OverlapSphere(player.transform.position , 5 , 1<<11);
        //주변에 있는 크리스탈에 광물이 남아있는지 체크한다
        if (colliders.Length>0) {

            if (colliders.Length > 1) {
                Debug.Log("두개이상의 크리스탙이 감지됩니다.");
                return;
            }
            int remainCrystal=GameMgr.Instance.GetCrystalState(colliders[0].gameObject.GetComponent<CrystalInitialization>().CrystalID , true);       
            Debug.Log("남아있는 푸른 크리스탈의 개수 : " + remainCrystal.ToString());
            if (remainCrystal <0) {
                Debug.Log("남아있는 크리스탈이 없습니다.");
                return;
            }
        }
        
        //현재어떤걸 캐야하는 상황인지 업데이트한다
        PlayerMgr.Instance.ClickCrystalID = colliders[0].gameObject.GetComponent<CrystalInitialization>().CrystalID;
        PlayerMgr.Instance.ClickBlue = true;
        PlayerMgr.Instance.ClickRed = false;

        //캐는 중이라고 업데이트 하고 플레이어의 함수를 실행한다
        PlayerMgr.Instance.Mining = true;
        player.SendMessage("ClickMoveBtns" , uiCtrl.CrystalDirection , SendMessageOptions.DontRequireReceiver);
    }
    public void OnClickRedMine() {
        Collider[ ] colliders = Physics.OverlapSphere(player.transform.position , 5 , 1<<11);
        //주변에 있는 크리스탈에 광물이 남아있는지 체크한다
        if (colliders.Length>0) {

            if (colliders.Length > 1) {
                Debug.Log("두개이상의 크리스탙이 감지됩니다.");
                return;
            }
            int remainCrystal=GameMgr.Instance.GetCrystalState(colliders[0].gameObject.GetComponent<CrystalInitialization>().CrystalID , false);
            Debug.Log("남아있는 붉은 크리스탈의 개수 : " + remainCrystal.ToString());
            if (remainCrystal <0) {
                Debug.Log("남아있는 크리스탈이 없습니다.");
                return;
            }
        }

        //현재어떤걸 캐야하는 상황인지 업데이트한다
        
        PlayerMgr.Instance.ClickCrystalID = colliders[0].gameObject.GetComponent<CrystalInitialization>().CrystalID;
        PlayerMgr.Instance.ClickBlue = false;
        PlayerMgr.Instance.ClickRed = true;

        //캐는 중이라고 업데이트 하고 플레이어의 함수를 실행한다
        PlayerMgr.Instance.Mining = true;
        player.SendMessage("ClickMoveBtns" , uiCtrl.CrystalDirection , SendMessageOptions.DontRequireReceiver);
    }

    //드랍버튼 클릭
    public void OnClickDrop() {

        if (PlayerMgr.Instance.item == PlayerItem.BLUECRYSTAL) {
            UpdateScore(true,100);
            GameMgr.Instance.score += 100;
            
        } else if (PlayerMgr.Instance.item == PlayerItem.REDCRYSTAL) {
            UpdateScore(true,300);
            GameMgr.Instance.score += 300;
        }

        player.SendMessage("DropCrystal" , SendMessageOptions.DontRequireReceiver);
        
        

        
        //Drop버튼 비활성화
        dropBtn.interactable = false;
        fadeoutDrop.Play();

        
    }

    //발사버튼 클릭
    public void OnClickFire() {
        GameMgr.Instance.FireRobot(GameMgr.Instance.myID , PlayerMgr.Instance.HUD_Target);
    }


    #endregion

    #region ScoreUpdate Routine
    //스코어를 업데이트한다.
    public void UpdateScore(bool myScore, int add) {
        Debug.Log("UpdateScore 함수 호출");
        StartCoroutine(ScoreCoroutine(myScore ,  add));
        
    }
    bool isScoreCoroutining = false;
    IEnumerator ScoreCoroutine(bool myScore,int add) {

        Debug.Log("코루틴 호출");
        if (isScoreCoroutining == true) {
            yield return new WaitForSeconds(0.1f);
            UpdateScore(myScore , add);
           
            yield break;
        }
        
        isScoreCoroutining = true;
        
        int lastScore,displayScore;

        if (myScore) {
            lastScore = GameMgr.Instance.score + add;
            displayScore = GameMgr.Instance.score;
        } else {
            lastScore = GameMgr.Instance.scoreEnemy + add;
            displayScore = GameMgr.Instance.scoreEnemy;
        }

        Debug.Log("lastscore = " + lastScore + " displayScore=" + displayScore + "add = " + add);

        if (myScore) {
            while(displayScore<lastScore) {
                displayScore+=Random.Range(8,13);
                displayScore = Mathf.Clamp(displayScore , 0 , lastScore);
                scoreText.text = "<color=#C9FFFE>" + displayScore.ToString() + "</color> : <color=#FF97BC>" +
                GameMgr.Instance.scoreEnemy.ToString() + "</color>";
                
                yield return null;
            }
        } else {
            while(displayScore<lastScore) {
                displayScore += Random.Range(8 , 13);
                displayScore = Mathf.Clamp(displayScore , 0 , lastScore);
                scoreText.text = "<color=#C9FFFE>" + displayScore.ToString() + "</color> : <color=#FF97BC>" +
                GameMgr.Instance.scoreEnemy.ToString() + "</color>";
                
                yield return null;
            }
        }

        

        isScoreCoroutining = false;
    }
    #endregion
}
