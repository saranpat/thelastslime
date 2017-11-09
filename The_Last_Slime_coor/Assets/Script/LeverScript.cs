using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour {

    public GameObject[] Trap;

    private bool[] DoorOnFire;
    private SpriteRenderer _SpriteRenderer;
    private string Lever_Interaction = "Lever_Interaction";
	// Use this for initialization
	void Start () {
        //switchOff = false;
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        DoorOnFire = new bool[Trap.Length];

        for (int i = 0; i < Trap.Length; i++)
        {
            if (Trap[i].GetComponent<DoorScript>() != null)
                DoorOnFire[i] = Trap[i].GetComponent<DoorScript>().isDoorOnFire;
            else
                DoorOnFire[i] = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PullLever()
    {
        _SpriteRenderer.flipX = !_SpriteRenderer.flipX;

        if (Trap != null)
        {
            for (int i = 0; i < Trap.Length; i++)
            {
                Trap[i].SendMessage(Lever_Interaction, DoorOnFire[i]);
            }
        }




        /*if (switchOff)
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
                            Trap[i].layer = 2;
                        }
                        else
                        {
                            Trap[i].GetComponent<SpriteRenderer>().sprite = fireOn;
                            Trap[i].tag = "Fire";
                            Trap[i].layer = 0;
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
                            Trap[i].layer = 2;
                        }
                        else
                        {
                            Trap[i].GetComponent<SpriteRenderer>().sprite = fireOn;
                            Trap[i].tag = "Fire";
                            Trap[i].layer = 0;
                        }
                    }
                    else
                        Trap[i].GetComponent<DoorScript>().isOpen = !Trap[i].GetComponent<DoorScript>().isOpen;
                }
                mysprite.flipX = false;
            }
        }*/
    }
}
