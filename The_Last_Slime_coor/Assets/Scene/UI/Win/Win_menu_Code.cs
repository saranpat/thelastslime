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
	public GameObject seq1;
	public GameObject seq2;
	public GameObject seq3;
	public GameObject seq4;
	public GameObject seq5;
	public GameObject seq6;
	public GameObject seq7;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        button.gameObject.SetActive(false);
		StartCoroutine(playsequence());
		//StartCoroutine(buttonshow());
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
	//	StopCoroutine (Fading());
	}
	IEnumerator playsequence()
	{
		
		seq1.gameObject.SetActive (true);
		yield return new WaitForSeconds (2.7f);
		//yield return new WaitUntil (() => black.color.a == 1);
		seq2.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.3f);
		seq3.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.17f);
		seq4.gameObject.SetActive (true);
		yield return new WaitForSeconds (2);
		seq5.gameObject.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		seq6.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.1f);
		seq1.gameObject.SetActive (false);
		seq2.gameObject.SetActive (false);
		seq3.gameObject.SetActive (false);
		seq4.gameObject.SetActive (false);
		seq5.gameObject.SetActive (false);
		yield return new WaitForSeconds (0.1f);
	
		seq7.gameObject.SetActive (true);
		yield return new WaitForSeconds (1);

		StartCoroutine(buttonshow());
		StopCoroutine (playsequence());
	}
	IEnumerator buttonshow()
	{
		yield return new WaitForSeconds (3);
		button.gameObject.SetActive(true);
		//button2.gameObject.SetActive(true);
		StopCoroutine (buttonshow());
	}
}
