using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identifier : MonoBehaviour {

    [SerializeField]
    private int unitID;

    private void Awake() {
      
    }

    public void SetID(int id) {
        unitID = id;
    }
    public int GetID() {
        return unitID;
    }

}
