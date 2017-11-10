using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenuScript : MonoBehaviour {
	public Button button;
	public bool paused;
	// Use this for initialization
	void Start () {
		paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		button.onClick.AddListener (stopgame);
	}

	public void stopgame()
	{
		paused = !paused;
		if (paused) {
			Time.timeScale = 0;
		} else if (!paused) {
			Time.timeScale = 1;
		}
	}
}
