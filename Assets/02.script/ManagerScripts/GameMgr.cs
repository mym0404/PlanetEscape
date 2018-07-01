using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatrixState { CAN = 0, PLAYER,AI,CRYSTAL,UFO,OUT,ERROR };

public class GameMgr : MonoBehaviour {

    #region Singleton Setting
    private static GameMgr instance;

    public static GameMgr Instance {
        get { return instance; }
    }
    private void SetSingleton() {
        if (instance != null) {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    
    //클릭을 방지하는 패널을 설정
    public GameObject NonClickPanel;


    //맵의 상태
    private MatrixState[,] mapMatrix;

    //크리스탈들의 상태
    private Vector2Int[ ] crystalState;

    //크리스탈 점수의 상태
    public int score=0;
    public int scoreEnemy = 0;


    private void Awake() {
        SetSingleton();

        mapMatrix = new MatrixState[30 , 30];


        //UFO를 설정한다.
        for (int i = 24 ; i < 30 ; i++) {
            for (int j = 24 ; j < 30 ; j++) {
                mapMatrix[i , j] = MatrixState.UFO;
            }
        }
        mapMatrix[14 , 12] = MatrixState.UFO;

        //x는 푸른 크리스탈 , y는 붉은 크리스탈 개수 
        crystalState = new Vector2Int[1];

        
        for (int i = 0 ; i < crystalState.Length ; i++) {
            crystalState[i].x = 40;
            crystalState[i].y = 10;
        }


        //논클릭패널
        NonClickPanel = GameObject.FindGameObjectWithTag("NONCLICKPANEL");
        NonClickPanel.SetActive(false);
    }
    //크리스탈의 개수를 보는 함수
    public int GetCrystalState(int id , bool isBlue) {
        if (isBlue) {
            return crystalState[id].x;
        } else {
            return crystalState[id].y;
        }
    }
    //크리스탈의 개수를 바꾸는 함수
    public void UpdateCrystalState(int id , bool isBlue , int subtract) {
        if (isBlue) {
            crystalState[id].x -= subtract;
        } else {
            crystalState[id].y -= subtract;
        }
    }


    //맵 매트릭스의 한칸한칸의 상태를 바꾸는 함수
    public void ChangeMatrixState(Position pos, MatrixState state) {
        mapMatrix[pos.R , pos.C] = state;
    }
    //현재 맵 매트릭스의 한칸한칸의 정보를 불러오는 함수, 갈수 있는 곳이면 true를 반환
    public bool GetMatrixState(Position pos, out MatrixState state) {

        if (pos.R >= 30 || pos.R < 0 || pos.C >= 30 || pos.C < 0) {
            state = MatrixState.OUT;
            return false;

        }

        switch (mapMatrix[pos.R , pos.C]) {
            case MatrixState.CAN:
                state = MatrixState.CAN;
                return true;

            case MatrixState.CRYSTAL:
                state = MatrixState.CRYSTAL;
                return false;
 
            case MatrixState.UFO:
                state = MatrixState.UFO;
                return false;
 
            default:
                state = MatrixState.ERROR;
                return false;

        }

    }

}
