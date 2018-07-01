using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    public GameObject GridImage;

    private List<Image> tiles=new List<Image>();

    private int xPos, yPos;

	// Use this for initialization
	void Start () {
        StartCoroutine(SetGrid());
	}


    IEnumerator SetGrid() {

        xPos = -1450;
        yPos = -1450;

        for (int i = 0 ; i < 30 ; i++) {
            for (int j = 0 ; j < 30 ; j++) {
                GameObject obj=Instantiate(GridImage , new Vector3(0 , 0 , -5f),Quaternion.LookRotation(Vector3.up) ,transform);
                obj.transform.localPosition = new Vector3(xPos + (j * 100) , yPos + (i * 100) , 0.01f);

                obj.GetComponent<Image>().raycastTarget = false;
                
                tiles.Add(obj.GetComponent<Image>());




                yield return new WaitForSeconds(0.01f);
            }
        }
    }


	// Update is called once per frame
	void Update () {
		
	}
}
