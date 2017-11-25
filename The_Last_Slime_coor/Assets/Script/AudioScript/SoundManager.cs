using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [HideInInspector] public AudioSource[] monk;

    public static bool DeadRea;
    public static bool UnlockedRea;
    public static bool FireBallRea;
    public static bool ButtonRea;
    public static bool LeverRea;
    public static bool SlimeSplitRea;
    public static bool DetectedRea;
    public static bool NormalRea;
    public static bool UIOverRea;
    public static bool WinRea;
    public static bool GetItemRea;

    public AudioClip Dead;
    public AudioClip Unlocked;
    public AudioClip FireBall;
    public AudioClip Button;
    public AudioClip Lever;
    public AudioClip SlimeSplit;
    public AudioClip Detected;
    public AudioClip Win;
    public AudioClip[] BGM;
    public AudioClip UIOver;
    public AudioClip GetItem;

    private int i;

    // Use this for initialization
    void Start () {
        monk = GetComponents<AudioSource>();
        DeadRea = false;
        UnlockedRea = false;
        FireBallRea = false;
        ButtonRea = false;
        LeverRea = false;
        SlimeSplitRea = false;
        DetectedRea = false;
        WinRea = false;
        GetItemRea = false;

        i = Random.Range(0, BGM.Length - 1);

        monk[0].clip = BGM[i];
        monk[0].loop = true;
        monk[0].Play();

        if (PlayerPrefs.GetInt("BGMMute") == 1)
            monk[0].mute = true;
        else
            monk[0].volume = PlayerPrefs.GetFloat("BGMVolume");

        if (PlayerPrefs.GetInt("SFXMute") == 1)
            monk[1].mute = true;
        else
            monk[1].volume = PlayerPrefs.GetFloat("SFXVolume");
    }
	
	// Update is called once per frame
	void Update () {
        
		if(DeadRea)
        {
            DeadRea = false;
            monk[1].PlayOneShot(Dead);
            //StartCoroutine(Waitsound(Dead.length));            
        }
        if(UnlockedRea)
        {
            UnlockedRea = false;
            monk[1].PlayOneShot(Unlocked);
        }
        if(FireBallRea)
        {
            FireBallRea = false;
            monk[1].PlayOneShot(FireBall);
        }
        if(ButtonRea)
        {
            ButtonRea = false;
            monk[1].PlayOneShot(Button);
        }
        if (UIOverRea)
        {
            UIOverRea = false;
            monk[1].PlayOneShot(UIOver);
        }
        if (LeverRea)
        {
            LeverRea = false;
            monk[1].PlayOneShot(Lever);
        }
        if (SlimeSplitRea)
        {
            SlimeSplitRea = false;
            monk[1].PlayOneShot(SlimeSplit);
        }
        if (GetItemRea)
        {
            GetItemRea = false;
            monk[1].PlayOneShot(GetItem);
        }
        if (DetectedRea)
        {
            DetectedRea = false;
            monk[0].clip = Detected;
            monk[0].loop = true;
            monk[0].Play();
        }
        if (NormalRea)
        {
            NormalRea = false;
            monk[0].clip = BGM[i];
            monk[0].loop = true;
            monk[0].Play();
        }
        if (WinRea)
        {
            WinRea = false;
            monk[0].clip = Win;
            monk[0].loop = false;
            monk[0].Play();
        }
    }

    IEnumerator Waitsound(float length)
    {
        yield return new WaitForSeconds(length);
        
    }

    /*public void PauseGame()
    {
        if (monk.isPlaying)
            monk.Pause();
        else
            monk.UnPause();
    }*/
}
