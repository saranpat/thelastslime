using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenuScript : MonoBehaviour {
	//public Button button;
	public static bool paused;

    private GameObject audio;
    private GameObject bgm;

    private GameObject pausePanel;

	// Use this for initialization
	void Start () {
		paused = false;

        audio = GameObject.Find("CheckSound");
        bgm = GameObject.Find("SoundManager");

        pausePanel = GameObject.Find("PausePanel");

        pausePanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		//button.onClick.AddListener (stopgame);
	}

	public void stopgame()
	{
		paused = !paused;
		if (paused) {
			Time.timeScale = 0;
            audio.GetComponent<CheckSoundScript>().PauseSound();
            //bgm.GetComponent<SoundManager>().PauseGame();
            pausePanel.SetActive(true);
		} else if (!paused) {
			Time.timeScale = 1;
            audio.GetComponent<CheckSoundScript>().UnPauseSound();
            //bgm.GetComponent<SoundManager>().PauseGame();
            pausePanel.SetActive(false);
        }
	}
}
