using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Win_drop_down : MonoBehaviour {

	public Animator anim;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Movewithmouse.isWin) {
			StartCoroutine(DropCurtain());
		}
	}
	IEnumerator DropCurtain()
	{
		anim.SetBool ("Lose", true);
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene("Win_UI");
	}
}
