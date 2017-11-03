using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSoundScript : MonoBehaviour {
    public Camera MainCam;

    private AudioSource[] AuS;
    private GameObject[] waterBlk;
    private GameObject[] fireBlk;
    private GameObject closestWaterBlk;
    private GameObject closestFireBlk;
    private GameObject target;

    private bool waterSndPlay;
    private bool fireSndPlay;
    private bool camouSndPlay;

    private float waterTiming;
    private float fireTiming;
    private float camouTiming;
    private float waterDelay;
    private float fireDelay;
    private float camouDelay;
    private float range;

    // Use this for initialization
    void Start () {
        AuS = GetComponents<AudioSource>();

        waterSndPlay = false;
        fireSndPlay = false;
        camouSndPlay = false;

        waterTiming = Time.time;
        fireTiming = Time.time;
        camouTiming = Time.time;

        for (int i = 0; i < AuS.Length; i++)
        {
            AuS[i].Stop();
        }

        waterDelay = AuS[0].clip.length;
        fireDelay = AuS[1].clip.length;
        camouDelay = AuS[2].clip.length;

        range = 4.0f;
    }
	
	// Update is called once per frame
	void Update () {
        target = MainCam.gameObject.GetComponent<CameraScript>().target;
        transform.position = target.transform.position;

        waterBlk = GameObject.FindGameObjectsWithTag("Water");
        fireBlk = GameObject.FindGameObjectsWithTag("Fire");

        if (waterBlk != null)
        {
            for (int i = 0; i < waterBlk.Length; i++)
            {
                if (closestWaterBlk == null)
                    closestWaterBlk = waterBlk[i];

                if (Vector2.Distance(transform.position, waterBlk[i].transform.position) <
                        Vector2.Distance(transform.position, closestWaterBlk.transform.position))
                    closestWaterBlk = waterBlk[i];
            }
        }

        if (fireBlk != null)
        {
            for (int i = 0; i < fireBlk.Length; i++)
            {
                if (closestFireBlk == null)
                    closestFireBlk = fireBlk[i];

                if (Vector2.Distance(transform.position, fireBlk[i].transform.position) <
                        Vector2.Distance(transform.position, closestFireBlk.transform.position))
                    closestFireBlk = fireBlk[i];
            }
        }

        if (Vector2.Distance(transform.position, closestWaterBlk.transform.position) <= range)
        {
            if (waterTiming < Time.time)
            {
                waterSndPlay = true;
                waterTiming = Time.time + waterDelay;
            }
        }
        else
        {
            AuS[0].Stop();
        }

        if (Vector2.Distance(transform.position, closestFireBlk.transform.position) <= range)
        {
            if (fireTiming < Time.time)
            {
                fireSndPlay = true;
                fireTiming = Time.time + fireDelay;
            }
        }
        else
        {
            AuS[1].Stop();
        }

        if (Vector2.Distance(transform.position, closestWaterBlk.transform.position) <= 0.75f)
        {
            if (camouTiming < Time.time)
            {
                camouSndPlay = true;
                camouTiming = Time.time + camouDelay;
            }
        }
        else
        {
            AuS[2].Stop();
        }

        if (waterSndPlay)
        {
            AuS[0].Play();
            waterSndPlay = false;
        }

        if (fireSndPlay)
        {
            AuS[1].Play();
            fireSndPlay = false;
        }

        if (camouSndPlay)
        {
            AuS[2].Play();
            camouSndPlay = false;
        }
    }
}
