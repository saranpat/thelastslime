using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour {

    public GameObject[] Trap;
    public static bool switchOff;

	// Use this for initialization
	void Start () {
        switchOff = false;
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
            }
        }
	}
}
