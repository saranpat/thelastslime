using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject target;

    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position + new Vector3(0, offset.y, 0);
	}
}
