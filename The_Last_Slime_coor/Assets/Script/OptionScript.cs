using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour {

    private Toggle bgmT;
    private Toggle sfxT;

    private Slider bgmS;
    private Slider sfxS;

    private AudioSource[] sndManager;
    private GameObject sndCheck;
    private GameObject slimeSnd;

	// Use this for initialization
	void Start () {
        bgmT = GameObject.Find("BGMToggle").GetComponent<Toggle>();
        bgmS = GameObject.Find("BGMSlider").GetComponent<Slider>();
        sfxT = GameObject.Find("SFXToggle").GetComponent<Toggle>();
        sfxS = GameObject.Find("SFXSlider").GetComponent<Slider>();
        sndManager = GameObject.Find("SoundManager").GetComponents<AudioSource>();
        sndCheck = GameObject.Find("CheckSound");
        slimeSnd = GameObject.Find("Slime");

        if (!PlayerPrefs.HasKey("BGMMute"))
            PlayerPrefs.SetInt("BGMMute", 0);

        if (!PlayerPrefs.HasKey("SFXMute"))
            PlayerPrefs.SetInt("SFXMute", 0);

        if (!PlayerPrefs.HasKey("BGMVolume"))
            PlayerPrefs.SetFloat("BGMVolume", sndManager[0].volume);

        if (!PlayerPrefs.HasKey("SFXVolume"))
            PlayerPrefs.SetFloat("SFXVolume", sndCheck.GetComponent<CheckSoundScript>().volume);

        if (PlayerPrefs.GetInt("BGMMute") == 1)
        {
            bgmT.isOn = false;
            bgmS.value = 0;
            sndManager[0].mute = true;
        }
        else
        {
            bgmT.isOn = true;
            bgmS.value = PlayerPrefs.GetFloat("BGMVolume");
        }
        
        if (PlayerPrefs.GetInt("SFXMute") == 1)
        {
            sfxT.isOn = false;
            sfxS.value = 0;
            sndCheck.GetComponent<CheckSoundScript>().isMute = true;
            sndManager[1].mute = true;
            slimeSnd.GetComponent<AudioSource>().mute = true;
        }
        else
        {
            sfxT.isOn = true;
            sfxS.value = PlayerPrefs.GetFloat("SFXVolume"); ;
        }


    }
	
	// Update is called once per frame
	void Update () {
        if (!bgmT.isOn)
        {
            bgmS.value = 0;
            sndManager[0].mute = true;

            PlayerPrefs.SetInt("BGMMute", 1);
        }
        else
        {
            sndManager[0].mute = false;
            sndManager[0].volume = bgmS.value;

            PlayerPrefs.SetInt("BGMMute", 0);
            PlayerPrefs.SetFloat("BGMVolume", bgmS.value);
        }

        if (!sfxT.isOn)
        {
            sfxS.value = 0;
            sndCheck.GetComponent<CheckSoundScript>().isMute = true;
            sndManager[1].mute = true;
            slimeSnd.GetComponent<AudioSource>().mute = true;

            PlayerPrefs.SetInt("SFXMute", 1);
        }
        else
        {
            sndCheck.GetComponent<CheckSoundScript>().isMute = false;
            sndCheck.GetComponent<CheckSoundScript>().volume = sfxS.value;
            sndManager[1].mute = false;
            sndManager[1].volume = sfxS.value;
            slimeSnd.GetComponent<AudioSource>().mute = false;
            slimeSnd.GetComponent<AudioSource>().volume = sfxS.value;

            PlayerPrefs.SetInt("SFXMute", 0);
            PlayerPrefs.SetFloat("SFXVolume", sfxS.value);
        }
    }
}
