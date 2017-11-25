using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public Sprite closeSprite_Onfire;
    public Sprite openSprite;
    public Sprite closeSprite;
    public Sprite FireOn_Sprite;
    public GameObject FireEff;
    //public Sprite closeSprite_canUseKey;
    private bool isOpen;
    private bool isFireOff;

    public bool isDoorOnFire;
    public bool isCanUseKey;
    public Color DoorColor;
    private BoxCollider2D _BoxCollider2D;
    private SpriteRenderer _SpriteRenderer;
    private Animator anim;
	// Use this for initialization
	void Start () {
        _BoxCollider2D = this.GetComponent<BoxCollider2D>();
        _SpriteRenderer = this.GetComponent<SpriteRenderer>();

        if (GetComponent<Animator>() != null)
            anim = GetComponent<Animator>();

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isCanUseKey)
            if (collision.gameObject.tag == "Player")
            {

                Movewithmouse _Movewithmouse = collision.gameObject.GetComponent<Movewithmouse>();
                if (_Movewithmouse.GetKey)
                {
                    _Movewithmouse.GetKey = false;
                    SoundManager.UnlockedRea = true;
                    isOpen = true;
                }
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
                anim.enabled = false;
                _SpriteRenderer.sprite = openSprite;
                gameObject.layer = 2;
                gameObject.tag = "Wall";
                _BoxCollider2D.isTrigger = true;
                FireEff.SetActive(false);
            }
            else if (isOpen && !isFireOff)
            {
                anim.enabled = true;
                _SpriteRenderer.sprite = FireOn_Sprite;
                gameObject.tag = "Fire";
                gameObject.layer = 0;
                _BoxCollider2D.isTrigger = true;
                FireEff.SetActive(true);
            }
            else if (!isOpen && isFireOff)
            {
                anim.enabled = false;
                _SpriteRenderer.sprite = closeSprite;
                gameObject.layer = 9; // Obstacle
                gameObject.tag = "Wall";
                _BoxCollider2D.isTrigger = false;
                FireEff.SetActive(false);
            }
            else if (!isOpen && !isFireOff)
            {
                anim.enabled = false;
                _SpriteRenderer.sprite = closeSprite_Onfire;
                gameObject.layer = 9; // Obstacle
                gameObject.tag = "Fire";
                _BoxCollider2D.isTrigger = true;
                FireEff.SetActive(true);
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
