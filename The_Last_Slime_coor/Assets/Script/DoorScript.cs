using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public GameObject keyToOpen;
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isOpen;

	// Use this for initialization
	void Start () {
        isOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (keyToOpen != null)
            if (!keyToOpen.activeSelf)
                isOpen = true;

        if (isOpen)
        {
            Debug.Log("Open");
            gameObject.GetComponent<SpriteRenderer>().sprite = openSprite;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.layer = 2;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = closeSprite;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            gameObject.layer = 0;
        }
	}
}
