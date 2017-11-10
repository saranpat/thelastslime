using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    AudioSource monk;

    public static bool DeadRea;
    public static bool UnlockedRea;
    public static bool FireBallRea;
    public static bool ButtonRea;
    public static bool LeverRea;
    public static bool SlimeSplitRea;
    public static bool DetectedRea;
    public static bool NormalRea;
    public static bool UIOverRea;

    public AudioClip Dead;
    public AudioClip Unlocked;
    public AudioClip FireBall;
    public AudioClip Button;
    public AudioClip Lever;
    public AudioClip SlimeSplit;
    public AudioClip Detected;
    public AudioClip[] BGM;
    public AudioClip UIOver;

    // Use this for initialization
    void Start () {
        monk = GetComponent<AudioSource>();
        DeadRea = false;
        UnlockedRea = false;
        FireBallRea = false;
        ButtonRea = false;
        LeverRea = false;
        SlimeSplitRea = false;
        DetectedRea = false;

        int i = Random.Range(0, BGM.Length - 1);

        monk.clip = BGM[i];
        monk.loop = true;
        monk.Play();
    }
	
	// Update is called once per frame
	void Update () {
        
		if(DeadRea)
        {
            DeadRea = false;
            monk.PlayOneShot(Dead);
            //StartCoroutine(Waitsound(Dead.length));            
        }
        if(UnlockedRea)
        {
            UnlockedRea = false;
            monk.PlayOneShot(Unlocked);
        }
        if(FireBallRea)
        {
            FireBallRea = false;
            monk.PlayOneShot(FireBall);
        }
        if(ButtonRea)
        {
            ButtonRea = false;
            monk.PlayOneShot(Button);
        }
        if (UIOverRea)
        {
            UIOverRea = false;
            monk.PlayOneShot(UIOver);
        }
        if (LeverRea)
        {
            LeverRea = false;
            monk.PlayOneShot(Lever);
        }
        if (SlimeSplitRea)
        {
            SlimeSplitRea = false;
            monk.PlayOneShot(SlimeSplit);
        }
        if (DetectedRea)
        {
            DetectedRea = false;
            monk.clip = Detected;
            monk.loop = true;
            monk.Play();
        }
        if (NormalRea)
        {
            int i = Random.Range(0, BGM.Length - 1);

            NormalRea = false;
            monk.clip = BGM[i];
            monk.loop = true;
            monk.Play();
        }
    }

    IEnumerator Waitsound(float length)
    {
        yield return new WaitForSeconds(length);
        
    }

    public void PauseGame()
    {
        if (monk.isPlaying)
            monk.Pause();
        else
            monk.UnPause();
    }
}
