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
		if (switchOff)
        {
            if (Trap != null)
            {
                for (int i = 0; i < Trap.Length; i++)
                {
                    Trap[i].SetActive(false);
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
                    Trap[i].SetActive(true);
                }
                mysprite.flipX = false;
            }
        }
	}
}
