using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerItem { EMPTY = 0, BLUECRYSTAL, REDCRYSTAL };

public class PlayerMgr : MonoBehaviour {
    #region Singleton Setting
    private static PlayerMgr instance;
    public static PlayerMgr Instance {
        get { return instance; }
    }
    private void SetSingleton() {
        if (instance != null) {//만약 인스턴스가 이미 존재한다면
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region PlayerState
    private bool turning = false;
    public bool Turning {
        get { return turning; }
        set { turning = value; }
    }


    private bool moving = false;
    public bool Moving {
        get { return moving; }
        set { moving = value; }
    }

    private bool mining = false;
    public bool Mining {
        get { return mining; }
        set { mining = value; }
    }
    #endregion



    public PlayerItem item = PlayerItem.EMPTY;

    //어떤컬 캔건지 표시하는 변수들
    public bool ClickBlue = false;
    public bool ClickRed = false;
    public int ClickCrystalID = -1;

    //프로필이 떠있는지 체크한다
    public bool isProfileImageOn = false;


    private void Awake() {
        SetSingleton();
    }


}
