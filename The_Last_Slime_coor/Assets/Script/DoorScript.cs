using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public GameObject keyToOpen;
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isOpen;
    public bool isDoorOnFire;
	// Use this for initialization
	void Start () {
        isOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (keyToOpen != null)
        {
            if (!keyToOpen.activeSelf)
            {
                isOpen = true;

            }
        }

        if (isOpen)
        {
            
            gameObject.GetComponent<SpriteRenderer>().sprite = openSprite;
            if (!isDoorOnFire)
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            else
            {
                gameObject.tag = "DoorOnFireOpen";
            }
            gameObject.layer = 2;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = closeSprite;
            if (!isDoorOnFire)
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            else
            {
                gameObject.tag = "Fire";
            }
            gameObject.layer = 9;
        }
	}
}
