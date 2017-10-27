using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour {

    public GameObject[] Trap;
    public bool DoorOnFire;
    public bool switchOff;

    public Sprite fireOn;
    public Sprite fireOff;

	private SpriteRenderer mysprite;
	// Use this for initialization
	void Start () {
        switchOff = false;
		mysprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PullLever()
    {
        if (switchOff)
        {
            if (Trap != null)
            {
                
                for (int i = 0; i < Trap.Length; i++)
                {
                    if ((DoorOnFire && Trap[i].tag == "Fire") || (DoorOnFire && Trap[i].tag == "DoorOnFireOpen"))
                    {
                        Trap[i].GetComponent<DoorScript>().isOpen = !Trap[i].GetComponent<DoorScript>().isOpen;
                    }
                    else if (Trap[i].tag == "Fire" || Trap[i].tag == "Untagged")
                    {
                        if (Trap[i].GetComponent<SpriteRenderer>().sprite == fireOn)
                        {
                            Trap[i].GetComponent<SpriteRenderer>().sprite = fireOff;
                            Trap[i].tag = "Untagged";
                        }
                        else
                        {
                            Trap[i].GetComponent<SpriteRenderer>().sprite = fireOn;
                            Trap[i].tag = "Fire";
                        }
                    }
                    else
                    {
                        Trap[i].GetComponent<DoorScript>().isOpen = !Trap[i].GetComponent<DoorScript>().isOpen;
                    }
                }
                mysprite.flipX = true;
            }
        }
        else
        {
            if (Trap != null)
            {
                for (int i = 0; i < Trap.Length; i++)
                {

                    if (DoorOnFire && Trap[i].tag == "Fire")
                    {
                        Trap[i].GetComponent<DoorScript>().isOpen = !Trap[i].GetComponent<DoorScript>().isOpen;
                    }
                    else if (Trap[i].tag == "Fire" || Trap[i].tag == "Untagged")
                    {
                        if (Trap[i].GetComponent<SpriteRenderer>().sprite == fireOn)
                        {
                            Trap[i].GetComponent<SpriteRenderer>().sprite = fireOff;
                            Trap[i].tag = "Untagged";
                        }
                        else
                        {
                            Trap[i].GetComponent<SpriteRenderer>().sprite = fireOn;
                            Trap[i].tag = "Fire";
                        }
                    }
                    else
                        Trap[i].GetComponent<DoorScript>().isOpen = !Trap[i].GetComponent<DoorScript>().isOpen;
                }
                mysprite.flipX = false;
            }
        }
    }
}
