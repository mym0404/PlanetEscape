using System.Collections;
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
        /*
        if (height * (1.6) > width) {//만약 세로가 게임에 필요한 비율상 더 길다면 수정요명
            
            height = (int)(Screen.currentResolution.width * (800 / 1280f));
        } else {
            
            width = (int)(Screen.currentResolution.height * (1280 / 800f));
        }
        */
        //Screen.SetResolution(width,height,FullScreenMode.FullScreenWindow,60);
        Screen.SetResolution(1280,800,FullScreenMode.FullScreenWindow,60);

    }
    public Text resolutionText;
    private void OnGUI() {
        //resolutionText.text = Screen.currentResolution.width + ", " + Screen.currentResolution.height;
    }
    #endregion

    #region UnitID Setting

    private int ID_TEAM1_PLAYER { get; set; }
    private int ID_TEAM1_AI1 { get; set; }
    private int ID_TEAM1_AI2 { get; set; }
    private int ID_TEAM2_PLAYER { get; set; }
    private int ID_TEAM2_AI1 { get; set; }
    private int ID_TEAM2_AI2 { get; set; }

    [HideInInspector]
    public int myTeam=1; //수정 요망 네트워크로 받게 해야함 
    [HideInInspector]
    public int myID=1; //수정 요망

    private void SetUnitID() {
        ID_TEAM1_PLAYER = 1;
        ID_TEAM1_AI1 = 2;
        ID_TEAM1_AI2 = 3;
        ID_TEAM2_PLAYER = 4;
        ID_TEAM2_AI1 = 5;
        ID_TEAM2_AI2 = 6;
    }
    //아이디를 가지고 해당 로봇 객체를 리턴해주는 함수
    public GameObject GetObjectWithID(int id) {

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("AI")) {
            if (o.GetComponent<Identifier>().GetID() == id) {
                return o;
            }
        }
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("PLAYER")) {
            if (o.GetComponent<Identifier>().GetID() == id) {
                return o;
            }
        }
        return null;
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
        
    }

    private void Awake() {
        SetSingleton();
        SetResolution();
        SetUnitID(); //없앨수도

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

    #region crystal state get,set
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
    #endregion

    #region map matrix state get,set
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
    #endregion

    #region fire

    public void FireRobot(int IDfrom,int IDto) {
        GameObject Obj_from = GetObjectWithID(IDfrom);
        GameObject Obj_to = GetObjectWithID(IDto);

        if (Obj_from == null || Obj_to == null) {
            throw new System.Exception("발사를 하려는데 객체가 없습니다");
            return;
        }

        Debug.Log(IDfrom.ToString() + " fire=> " + IDto.ToString());

    }
    #endregion

}
