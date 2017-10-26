using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScript : MonoBehaviour {

    public GameObject DoorToOpen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "CheckWater")
            DoorToOpen.gameObject.GetComponent<DoorScript>().isOpen = true;
        else
            DoorToOpen.gameObject.GetComponent<DoorScript>().isOpen = false;
    }
}
