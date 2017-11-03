using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSoundScript : MonoBehaviour {
    public Camera MainCam;

    private AudioSource[] AuS;
    private GameObject[] waterBlk;
    private GameObject[] fireBlk;
    private GameObject[] enemy;

    private GameObject closestWaterBlk;
    private GameObject closestFireBlk;
    private GameObject closestGuard;
    private GameObject target;

    private bool waterSndPlay;
    private bool fireSndPlay;
    private bool camouSndPlay;
    private bool guardSndPlay;

    private float waterTiming;
    private float fireTiming;
    private float camouTiming;
    private float guardTiming;
    private float delay;
    private float range;

    // Use this for initialization
    void Start () {
        AuS = GetComponents<AudioSource>();

        waterSndPlay = false;
        fireSndPlay = false;
        camouSndPlay = false;
        guardSndPlay = false;

        waterTiming = Time.time;
        fireTiming = Time.time;
        camouTiming = Time.time;
        guardTiming = Time.time;

        for (int i = 0; i < AuS.Length; i++)
        {
            AuS[i].Stop();
        }

        delay = 1.0f;
        range = 4.0f;
    }
	
	// Update is called once per frame
	void Update () {
        target = MainCam.gameObject.GetComponent<CameraScript>().target;
        transform.position = target.transform.position;

        waterBlk = GameObject.FindGameObjectsWithTag("Water");
        fireBlk = GameObject.FindGameObjectsWithTag("Fire");
        enemy = GameObject.FindGameObjectsWithTag("Enemy");

        CheckClosest();
        CheckPlaying();
        PlayAudio();
    }

    void CheckClosest()
    {
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

        if (enemy != null)
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                if (enemy[i].gameObject.GetComponent<AI_Move>().Guard_AI)
                {
                    if (closestGuard == null)
                        closestGuard = enemy[i];

                    if (Vector2.Distance(transform.position, enemy[i].transform.position) <
                            Vector2.Distance(transform.position, closestGuard.transform.position))
                        closestGuard = enemy[i];
                }
            }
        }
    }

    void CheckPlaying()
    {
        if (Vector2.Distance(transform.position, closestWaterBlk.transform.position) <= range)
        {
            if (waterTiming < Time.time)
            {
                if (!AuS[0].isPlaying)
                    waterSndPlay = true;
                waterTiming = Time.time + delay;
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
                if (!AuS[1].isPlaying)
                    fireSndPlay = true;
                fireTiming = Time.time + delay;
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
                if (!AuS[2].isPlaying)
                    camouSndPlay = true;
                camouTiming = Time.time + delay;
            }
        }
        else
        {
            AuS[2].Stop();
        }

        if (Vector2.Distance(transform.position, closestGuard.transform.position) <= range + 1.0f)
        {
            if (guardTiming < Time.time)
            {
                if (!AuS[3].isPlaying)
                    guardSndPlay = true;
                guardTiming = Time.time + delay;
            }
        }
        else
        {
            AuS[3].Stop();
        }
    }

    void PlayAudio()
    {
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

        if (guardSndPlay)
        {
            AuS[3].Play();
            guardSndPlay = false;
        }
    }
}
