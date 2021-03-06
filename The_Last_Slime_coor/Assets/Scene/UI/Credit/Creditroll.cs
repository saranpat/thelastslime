﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Creditroll : MonoBehaviour {
	public Button button;
	public Image black;
	public Animator anim;
	// Use this for initialization
	void Start () {
		button.onClick.AddListener (delegate {
			StartCoroutine(Fading());
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator Fading()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => black.color.a == 1);
		SceneManager.LoadScene("Open_page");
		//StopCoroutine (Fading());
	}
}
