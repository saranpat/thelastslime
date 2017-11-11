using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Lose_menu_controller : MonoBehaviour {

	public Button button;
	public Button button2;
	public Image black;
	public Image slimepic;
	public Sprite grilled;
	private Image buttonimg;
	//public Image slime;
	public Animator anim;
	// Use this for initialization
	void Start () {
		StartCoroutine(buttonshow());
		button.onClick.AddListener (delegate {
			StartCoroutine(Fading());
		});
		button2.onClick.AddListener (delegate {
			StartCoroutine(FadingB());
		});
	//	slimepic.GetComponent<Image> ();
		if (Movewithmouse.isGrilled == true)
			slimepic.sprite = grilled;
	}

	// Update is called once per frame
	void Update () {

	}
	IEnumerator Fading()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => black.color.a == 1);
		string sceneback= PlayerPrefs.GetString("lastLoadedScene");
		SceneManager.LoadScene(sceneback);
	}
	IEnumerator FadingB()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => black.color.a == 1);
		SceneManager.LoadScene("Open_page");
	}
	IEnumerator buttonshow()
	{
		yield return new WaitForSeconds (1);
		button.gameObject.SetActive(true);
		button2.gameObject.SetActive(true);
		StopCoroutine (buttonshow());
	}
}
