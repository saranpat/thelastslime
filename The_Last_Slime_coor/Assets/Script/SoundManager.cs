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

    public AudioClip Dead;
    public AudioClip Unlocked;
    public AudioClip FireBall;
    public AudioClip Button;
    public AudioClip Lever;
    public AudioClip SlimeSplit;
    public AudioClip Detected;
   

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
        if(LeverRea)
        {
            LeverRea = false;
            monk.PlayOneShot(Lever);
        }
        if(SlimeSplitRea)
        {
            SlimeSplitRea = false;
            monk.PlayOneShot(SlimeSplit);
        }
        if(DetectedRea)
        {
            DetectedRea = false;
            monk.PlayOneShot(Detected);
        }

	}

    IEnumerator Waitsound(float length)
    {
        yield return new WaitForSeconds(length);
        
    }
}
