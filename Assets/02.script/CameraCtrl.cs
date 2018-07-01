using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CamMoveDirection { NORTH, EAST, SOUTH, WEST };

public class CameraCtrl : MonoBehaviour {
    private Transform tr;

    public GameObject Player;

    private void Awake() {
        tr = GetComponent<Transform>();
    }

    private void Start() {
        tr.position = new Vector3(Player.transform.position.x , 1.5f , Player.transform.position.z);
    }


    public void Move(CamMoveDirection direction) {
        Vector3 moveDir=Vector3.zero;
        switch (direction) {
            case CamMoveDirection.NORTH:
                moveDir = new Vector3(0 , 0 , 1);
                break;
            case CamMoveDirection.EAST:
                moveDir = new Vector3(1 , 0 , 0);
                break;
            case CamMoveDirection.SOUTH:
                moveDir = new Vector3(0 , 0 , -1);
                break;
            case CamMoveDirection.WEST:
                moveDir = new Vector3(-1 , 0 , 0);
                break;
        }
        StartCoroutine(MoveCam(moveDir));
    }

    IEnumerator MoveCam(Vector3 moveDir) {
        float time = 0;
        Vector3 startPos = tr.position;

        while (time<1.5f) {
            tr.position = startPos + (moveDir * time)*(2/3f);
            
            time += Time.unscaledDeltaTime;

            yield return null;

        }
        tr.position = startPos + moveDir;
    }

}
