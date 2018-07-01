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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
