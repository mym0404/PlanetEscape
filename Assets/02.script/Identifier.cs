using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identifier : MonoBehaviour {

    public int unitID;

    private void Awake() {
        StartCoroutine(SetID());
    }

    IEnumerator SetID() {
        yield return new WaitForSecondsRealtime(Random.Range(0f , 1f));
        unitID = Random.Range(0 , 100000);
        
        //만약 unitID가 중복되서 false가 반환된 경우 다시 시작한다.
        if (GameMgr.Instance.RegisterID(unitID) == false) {
            StartCoroutine(SetID());
        } 
    }

}
