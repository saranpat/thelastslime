using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSoundScript : MonoBehaviour {
    [HideInInspector] public float[] volumeOffset;
    [HideInInspector] public bool isMute = false;
    public float volume = 0.5f;

    private Camera MainCam;

    private AudioSource[] AuS;

    private GameObject[] waterBlk;
    private GameObject[] fireBlk;
    private GameObject[] enemy;

    private bool[] audioPlayed;

    private GameObject closestWaterBlk;
    [HideInInspector] public GameObject closestFireBlk;
    private GameObject closestGuard;
    private GameObject closestHero_orWizard;
    private GameObject target;

    private bool waterSndPlay;
    private bool fireSndPlay;
    private bool camouSndPlay;
    private bool guardSndPlay;
    private bool bootSndPlay;

    private float waterTiming;
    private float fireTiming;
    private float camouTiming;
    private float guardTiming;
    private float bootTiming;
    private float delay;
    private float range;

    // Use this for initialization
    void Start () {
        MainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        AuS = GetComponents<AudioSource>();

        volumeOffset = new float[AuS.Length];

        waterSndPlay = false;
        fireSndPlay = false;
        camouSndPlay = false;
        guardSndPlay = false;
        bootSndPlay = false;

        waterTiming = Time.time;
        fireTiming = Time.time;
        camouTiming = Time.time;
        guardTiming = Time.time;
        bootTiming = Time.time;

        for (int i = 0; i < AuS.Length; i++)
        {
            volumeOffset[i] = AuS[i].volume;
            AuS[i].Stop();
        }

        audioPlayed = new bool[AuS.Length];

        delay = 1.0f;
        range = 4.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (isMute)
        {
            for (int i = 0; i < AuS.Length; i++)
            {
                AuS[i].mute = true;
            }
        }
        else
        {
            for (int i = 0; i < AuS.Length; i++)
            {
                AuS[i].mute = false;
            }
        }

        for (int i = 0; i < AuS.Length; i++)
        {
            float f = (volume * 2) * volumeOffset[i];

            if (f > 1)
                f = 1;

            AuS[i].volume = f;
        }

        target = MainCam.gameObject.GetComponent<CameraScript>().target;

        if(target != null)
            transform.position = target.transform.position;

        waterBlk = GameObject.FindGameObjectsWithTag("Water");
        fireBlk = GameObject.FindGameObjectsWithTag("Fire");
        enemy = GameObject.FindGameObjectsWithTag("Enemy"); 

        CheckClosest();
        CheckPlaying();
        PlayAudio();

        closestFireBlk = null;
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
                else if (enemy[i].gameObject.GetComponent<AI_Move>().Wizard_AI || 
                            enemy[i].gameObject.GetComponent<AI_Move>().Hero_AI)
                {
                    if (closestHero_orWizard == null)
                        closestHero_orWizard = enemy[i];

                    if (Vector2.Distance(transform.position, enemy[i].transform.position) <
                            Vector2.Distance(transform.position, closestHero_orWizard.transform.position))
                        closestHero_orWizard = enemy[i];
                }
            }
        }
    }

    void CheckPlaying()
    {
        if (closestWaterBlk != null)
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
        }

        if (closestFireBlk != null)
        {
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
        }

        if (closestWaterBlk != null)
        {
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
        }

        if (closestGuard != null)
        {
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

        if (closestHero_orWizard != null)
        {
            if (Vector2.Distance(transform.position, closestHero_orWizard.transform.position) <= range + 1.0f)
            {
                if (bootTiming < Time.time)
                {
                    if (!AuS[4].isPlaying)
                        bootSndPlay = true;
                    bootTiming = Time.time + delay;
                }
            }
            else
            {
                AuS[4].Stop();
            }
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

        if (bootSndPlay)
        {
            AuS[4].Play();
            bootSndPlay = false;
        }
    }

    public void PauseSound ()
    {
        for (int i = 0; i < AuS.Length; i++)
        {
            if (AuS[i].isPlaying)
            {
                audioPlayed[i] = true;
                AuS[i].Pause();
            }
            else
            {
                audioPlayed[i] = false;
            }
        }
    }

    public void UnPauseSound ()
    {
        for (int i = 0; i < AuS.Length; i++)
        {
            if (audioPlayed[i])
                AuS[i].UnPause();
        }
    }
}
