using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAudioScript : MonoBehaviour {

    public AudioClip CamouSnd;

    private AudioSource AuS;

    private bool inWater;

	// Use this for initialization
	void Start () {
        AuS = GetComponent<AudioSource>();

        inWater = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (inWater)
        {
            inWater = false;
            AuS.clip = CamouSnd;
            AuS.loop = true;
            AuS.Play();
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            inWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            AuS.Stop();
        }
    }
}
