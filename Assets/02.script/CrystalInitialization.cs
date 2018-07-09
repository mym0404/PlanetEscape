using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalInitialization : MonoBehaviour {
    //위치
    public Position pos;
    //크리스탈 ID
    public int CrystalID;

    
    
    //크리스탈 빛 컴포넌트
    public Light lightRed;
    public Light lightBlue;
    //남은 크리스탈 개수
    [HideInInspector]
    public int crystalCount_Blue = 40;
    [HideInInspector]
    public int crystalCount_Red = 10;

    //다음 가져와야할 크리스탈 인덱스
    private int blueIndex = 0;
    private int redIndex = 40;

    private void Awake() {

        pos = new Position(13,13);
        
    }

    // Use this for initialization
    void Start () {
        

        //크리스탈이 있는 지역 MatrixState를 변경
        for (int i = 0 ; i < 4 ; i++) {
            for (int j = 0 ; j < 4 ; j++) {
                GameMgr.Instance.ChangeMatrixState(new Position(pos.R+i , pos.C+j) , MatrixState.CRYSTAL);
            }
        }

        
	}
	
	
    public void ChangeCrystalCount(bool isBlue, int change) {
        if (isBlue) {//만약 파랑 크리스탈을 줄어들게 한다면
            if (crystalCount_Blue > 0) {
                crystalCount_Blue -= change;
                Destroy(transform.GetChild(blueIndex++).gameObject);
                lightBlue.intensity -= change*0.5f;
            }
        } else { //만약 붉은 크리스탈을 줄어들게 한다면
            if (crystalCount_Red > 0) {
                crystalCount_Red -= change;
                Destroy(transform.GetChild(redIndex++).gameObject);
                lightRed.intensity -= change*0.5f;
            }
        }
        //모든 크리스탈 소진시
        if (crystalCount_Blue + crystalCount_Red == 0) {
            Destroy(lightBlue);
            Destroy(lightRed);
        }

    }
}
