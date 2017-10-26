using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scenecontroller : MonoBehaviour {

	public Button startbutton;
	public Image black;
	public Animator anim;
	// Use this for initialization
	void Start () {
		startbutton.onClick.AddListener (delegate {
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
		SceneManager.LoadScene("Oct-24 New Sprites _Dandy");
	}
}
