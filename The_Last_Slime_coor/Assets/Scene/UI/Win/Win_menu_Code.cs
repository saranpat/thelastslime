using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Win_menu_Code : MonoBehaviour {

	public Button button;

	public Image black;
	//public Image slimepic;
	public Animator anim;
	// Use this for initialization
	void Start () {
		StartCoroutine(buttonshow());
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
		SceneManager.LoadScene("Credit");
	}
	IEnumerator buttonshow()
	{
		yield return new WaitForSeconds (1);
		button.gameObject.SetActive(true);
		//button2.gameObject.SetActive(true);
		StopCoroutine (buttonshow());
	}
}
