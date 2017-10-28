using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scenecontroller : MonoBehaviour {

	public Button startbutton;
	public Button startbutton2;
	public Image black;
	public Animator anim;
	// Use this for initialization
	void Start () {
		startbutton.onClick.AddListener (delegate {
			StartCoroutine(FadingA());
		});
		startbutton2.onClick.AddListener (delegate {
			StartCoroutine(FadingB());
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator FadingA()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => black.color.a == 1);
		SceneManager.LoadScene("alpha_stage");
	}
	IEnumerator FadingB()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => black.color.a == 1);
		SceneManager.LoadScene("playable_stage");
	}
}
