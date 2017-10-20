using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour {

    public static int slimeCnt;

    public static GameObject[] slime;

	// Use this for initialization
	void Start () {
        slimeCnt = 1;
	}
	
	// Update is called once per frame
	void Update () {
        slime = GameObject.FindGameObjectsWithTag("Player");

        slimeCnt = slime.Length;
	}
}
