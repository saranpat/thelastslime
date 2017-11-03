using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSoundScript : MonoBehaviour {
    public AudioClip WaterSnd;
    public Camera maincamera;

    private AudioSource AuS;
    private GameObject target;

    private bool waterFound;

    // Use this for initialization
    void Start () {
        AuS = GetComponent<AudioSource>();

        waterFound = false;
    }
	
	// Update is called once per frame
	void Update () {
        target = maincamera.gameObject.GetComponent<CameraScript>().target;
        transform.position = target.transform.position;

        if (waterFound)
        {
            waterFound = false;
            AuS.clip = WaterSnd;
            AuS.loop = true;
            AuS.volume = 0.5f;
            AuS.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            waterFound = true;
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
