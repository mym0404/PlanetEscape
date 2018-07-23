using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScriptMgr : MonoBehaviour {
    #region Set Singleton
    private static ScriptMgr instance;

    public static ScriptMgr Instance {
        get { return instance; }
        set { }
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

    void Awake() {
        SetSingleton();
    }

    public void UpdateTransform(int id) {
        GameObject unit = GameMgr.Instance.GetObjectWithID(id);

        int dir;
        int pos_r, pos_c;

        float pos_z, pos_y, pos_x; ;

        float angle;

        if (id == 1 || id == 3) { //만약 플레이어라면
            PlayerCtrl playerctrl = unit.GetComponent<PlayerCtrl>();
            dir = playerctrl.GetDirection();

            pos_r = playerctrl.pos.R;
            pos_c = playerctrl.pos.C;
        } else { //인공지능이라면

            AICtrl aictrl = unit.GetComponent<AICtrl>();
            dir = aictrl.GetDirection();

            pos_r = aictrl.pos.R;
            pos_c = aictrl.pos.C;
        }
    

        pos_z = 14.5f - pos_r;
        pos_x = -14.5f + pos_c;
        pos_y = 0.3f;

        
        switch (dir) {
            case 0:
                angle = 180;
                break;

            case 1:
                angle = 270;
                break;

            case 2:
                angle = 0;
                break;

            case 3:
                angle = 90;
                break;
            default:
                angle = 0;
                break;
        }

        unit.transform.position = new Vector3(pos_x, pos_y, pos_z);
        unit.transform.eulerAngles = new Vector3(0 , angle , 0);
    }

}
