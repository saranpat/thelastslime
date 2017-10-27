using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lose_drop_control : MonoBehaviour {
//	public GameObject slime;
//	public static bool ded;
	public Animator anim;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Movewithmouse.isDead) {
			StartCoroutine(DropCurtain());
		}
	}
	IEnumerator DropCurtain()
	{
		anim.SetBool ("Lose", true);
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene("Lose_UI");
	}

}
