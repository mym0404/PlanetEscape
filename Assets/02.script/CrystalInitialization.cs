using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalInitialization : MonoBehaviour {

    public Position pos;
    public int CrystalID;


    private Color crystalColor = new Color(200 / 255f , 244 / 255f , 244 / 255f , 1);
    private float maxIntensity = 30f;
    private float minIntensity = 10f;
    
    
    private Light crystalLight;
    private int crystalCount = 50;

    private void Awake() {

        pos = new Position(13,13);

        crystalLight = GetComponent<Light>();
    }

    // Use this for initialization
    void Start () {
        crystalLight.intensity = maxIntensity;

        //크리스탈이 있는 지역 MatrixState를 변경
        for (int i = 0 ; i < 4 ; i++) {
            for (int j = 0 ; j < 4 ; j++) {
                GameMgr.Instance.ChangeMatrixState(new Position(pos.R+i , pos.C+j) , MatrixState.CRYSTAL);
            }
        }

        
	}
	
	
    void ChangeCrystalCount(int n) {
        crystalCount -= n;
        if (n < 10)
            return;
        //크리스탈 빛의 세기를 남아있는 크리스탈 개수에 비례해서 바꾼다.
        crystalLight.intensity = 30 - (30 / (crystalCount));
        crystalLight.intensity = Mathf.Clamp(crystalLight.intensity , minIntensity , maxIntensity);
    }
}
