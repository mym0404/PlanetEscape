using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationEndHandler : MonoBehaviour {

    

    public GameObject parentObject;

    


    public void MoveEnd() {

        parentObject.SendMessage("MoveEnd" , SendMessageOptions.DontRequireReceiver);

    }
    public void TurnEnd() {

        parentObject.SendMessage("TurnEnd" , SendMessageOptions.DontRequireReceiver);
    }
    public void MineEnd() {

        parentObject.SendMessage("MineEnd" , SendMessageOptions.DontRequireReceiver);
    }
}
