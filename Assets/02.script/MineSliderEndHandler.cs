using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSliderEndHandler : MonoBehaviour {

    public void MineSliderEnd() {
        Debug.Log("MineSliderEnd()");
        PlayerMgr.Instance.Mining = false;
    }
}
