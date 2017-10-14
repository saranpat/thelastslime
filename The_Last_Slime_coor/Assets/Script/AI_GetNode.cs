using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_GetNode : MonoBehaviour {

    public Transform[] NodePosition;

	// Use this for initialization
	void Awake () {
        NodePosition = this.gameObject.GetComponentsInChildren<Transform>();




	}
	
	// Update is called once per frame
	void Update () {
		
	}



}
