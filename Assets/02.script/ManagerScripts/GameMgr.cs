﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    #region Resolution Setting
    private void SetResolution() {
        Screen.fullScreen = true;

        int height = Screen.currentResolution.height;
        int width = Screen.currentResolution.width;

        if (height * (1280 / 800f) > width) {//만약 세로가 게임에 필요한 비율상 더 길다면 수정요명
            
            height = (int)(Screen.currentResolution.width * (800 / 1280f));
        } else {
            
            width = (int)(Screen.currentResolution.height * (1280 / 800f));
        }

        Screen.SetResolution(width,height,FullScreenMode.FullScreenWindow,60);
        

    }
    public Text resolutionText;
    private void OnGUI() {
        //resolutionText.text = Screen.currentResolution.width + ", " + Screen.currentResolution.height;
    }
    #endregion

    //클릭을 방지하는 패널을 설정
    [HideInInspector]
    public GameObject NonClickPanel;


    //맵의 상태
    private MatrixState[,] mapMatrix;

    //크리스탈들의 상태
    public CrystalInitialization[ ] crystalState;
    

    //크리스탈 점수의 상태
    [HideInInspector]
    public int score=0;
    [HideInInspector]
    public int scoreEnemy = 0;

    //인공지능 자동 이동 시간
    public float GetRandomMoveDeltaTime() {
        return Random.Range(4f , 6f);
    }

    private void Update() {
        resolutionText.text = Screen.currentResolution.width + ", " + Screen.currentResolution.height;
    }

    private void Awake() {
        SetSingleton();
        SetResolution();

        mapMatrix = new MatrixState[30 , 30];


        //UFO를 설정한다.
        for (int i = 24 ; i < 30 ; i++) {
            for (int j = 24 ; j < 30 ; j++) {
                mapMatrix[i , j] = MatrixState.UFO;
            }
        }
        mapMatrix[12 , 13] = MatrixState.UFO; //테스트용 UFO 위치

         
        


        //논클릭패널
        NonClickPanel = GameObject.FindGameObjectWithTag("NONCLICKPANEL");
        NonClickPanel.SetActive(false);
    }
    //크리스탈의 개수를 보는 함수
    public int GetCrystalState(int id , bool isBlue) {
        if (isBlue) {
            return crystalState[id].crystalCount_Blue;
        } else {
            return crystalState[id].crystalCount_Red;
        }
    }
    //크리스탈의 개수를 바꾸는 함수
    public void UpdateCrystalState(int id , bool isBlue , int subtract) {
        if (isBlue) {
            crystalState[id].ChangeCrystalCount(true,subtract); //수정요망
            
            
        } else {
            crystalState[id].ChangeCrystalCount(false,subtract);
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

            case MatrixState.PLAYER:
                state = MatrixState.PLAYER;
                return false;
            case MatrixState.AI:
                state = MatrixState.AI;
                return false;

            case MatrixState.OUT:
                state = MatrixState.OUT;
                return false;

            default:
                state = MatrixState.ERROR;
                return false;

        }

    }

}
