using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public Sprite closeSprite_Onfire;
    public Sprite openSprite;
    public Sprite closeSprite;
    //public Sprite closeSprite_canUseKey;
    private bool isOpen;
    private bool isFireOff;

    public bool isDoorOnFire;
    public bool isCanUseKey;
    public Color DoorColor;
    private BoxCollider2D _BoxCollider2D;
    private SpriteRenderer _SpriteRenderer;

	// Use this for initialization
	void Start () {
        _BoxCollider2D = this.GetComponent<BoxCollider2D>();
        _SpriteRenderer = this.GetComponent<SpriteRenderer>();
        isOpen = false;
        isFireOff = false;
	}

    public void Plate_Interaction(bool SetisOpen)
    {
        isOpen = SetisOpen;
    }


    public void Lever_Interaction(bool interactFire = false) //ใช้คู่กับ LeverScript โดยการ SendMessage
    {
        if(interactFire)
        {
            isFireOff = !isFireOff;
        }
        else
        {
            isOpen = !isOpen;
        }
    }


	// Update is called once per frame
	void Update () {

        if (!isCanUseKey)
        {
            _SpriteRenderer.color = DoorColor;
        }
        else
        {
            _SpriteRenderer.color = Color.white;
        }




        if(isDoorOnFire)
        {
            if (isOpen && isFireOff)
            {
                _SpriteRenderer.sprite = openSprite;
                gameObject.layer = 2;
                gameObject.tag = "Wall";
                _BoxCollider2D.isTrigger = true;
            }
            else if (!isOpen && isFireOff)
            {
                _SpriteRenderer.sprite = closeSprite;
                gameObject.layer = 9; // Obstacle
                gameObject.tag = "Wall";
                _BoxCollider2D.isTrigger = false;
            }
            else if (!isOpen && !isFireOff)
            {
                _SpriteRenderer.sprite = closeSprite_Onfire;
                gameObject.layer = 9; // Obstacle
                gameObject.tag = "Fire";
                _BoxCollider2D.isTrigger = true;
            }
        }
        else
        {
            if (isOpen)
            {
                _SpriteRenderer.sprite = openSprite;
                _BoxCollider2D.isTrigger = true;
                gameObject.layer = 2;
            }
            else
            {
                _SpriteRenderer.sprite = closeSprite;
                _BoxCollider2D.isTrigger = false;
                gameObject.layer = 9; // Obstacle
            }
        }

	}
}
