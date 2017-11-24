using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlower : MonoBehaviour {

    public Sprite FireOn_Sprite;
    public Sprite FireOff_Sprite;

    private bool isFireOff;
    private SpriteRenderer _SpriteRenderer;

	// Use this for initialization
	void Start () {
        _SpriteRenderer = this.GetComponent<SpriteRenderer>();
        isFireOff = false;
	}

    public void Plate_Interaction(bool SetisOpen)
    {
        if (SetisOpen)
        {
            if (oneTime == false)
            {
                StartCoroutine(DelayOpenFire());
            }
            else
            {
                StopCoroutine(DelayOpenFire());
                StartCoroutine(DelayOpenFire());
            }
            
        }
        /*else
        {
            isFireOff = SetisOpen;
        }*/
        
    }

    public void Lever_Interaction(bool interactFire = false) //ใช้คู่กับ LeverScript โดยการ SendMessage
    {
        isFireOff = !isFireOff;
    }

    bool oneTime;
    IEnumerator DelayOpenFire()
    {
        oneTime = true;
        isFireOff = true;
        yield return new WaitForSeconds(2.5f);
        oneTime = false;
        isFireOff = false;

    }
	// Update is called once per frame
	void Update () {

        if (isFireOff)
        {
            _SpriteRenderer.sprite = FireOff_Sprite;
            gameObject.tag = "Untagged";
            gameObject.layer = 2;
        }
        else
        {
            _SpriteRenderer.sprite = FireOn_Sprite;
            gameObject.tag = "Fire";
            gameObject.layer = 0;
        }
	}
}
