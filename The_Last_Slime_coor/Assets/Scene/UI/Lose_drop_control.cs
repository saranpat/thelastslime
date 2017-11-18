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
		yield return new WaitForSeconds(1);
		anim.SetBool ("Lose", true);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).length <= anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //yield return new WaitForSeconds(3);
        Time.timeScale = 1;
		PlayerPrefs.SetString ("lastLoadedScene", SceneManager.GetActiveScene ().name);
		SceneManager.LoadScene("Lose_UI");
	}

}
