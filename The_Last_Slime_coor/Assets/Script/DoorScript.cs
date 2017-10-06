using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public int keyLeftToOpen;
    public Sprite openSprite;
    public bool isOpen;

	// Use this for initialization
	void Start () {
        isOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Movewithmouse.keyCnt == keyLeftToOpen)
        {
            isOpen = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = openSprite;
            gameObject.tag = "Door";
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
	}
}
