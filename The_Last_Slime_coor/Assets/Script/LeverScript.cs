using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour {

    public GameObject[] Trap;
    public bool switchOff;
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
                    if (Trap[i].tag == "Fire")
                    {
                        if (Trap[i].activeSelf)
                        {
                            Trap[i].SetActive(false);
                        }
                        else
                        {
                            Trap[i].SetActive(true);
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
                    if (Trap[i].tag == "Fire")
                    {
                        if (Trap[i].activeSelf)
                        {
                            Trap[i].SetActive(false);
                        }
                        else
                        {
                            Trap[i].SetActive(true);
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
