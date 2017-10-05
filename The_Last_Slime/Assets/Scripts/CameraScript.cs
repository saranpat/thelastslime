using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private GameObject slime;

	// Use this for initialization
	void Start () {
        slime = GameObject.Find("Slime");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = slime.transform.position + new Vector3(0, 0, -1);
	}
}
