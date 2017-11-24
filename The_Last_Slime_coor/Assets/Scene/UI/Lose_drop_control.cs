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
        onetime = false;
	}
    bool onetime;
	// Update is called once per frame
	void Update () {
		if (Movewithmouse.isDead) {
            if (onetime == false)
            {
                StartCoroutine(DropCurtain());
            }
			
		}
	}
	IEnumerator DropCurtain()
    {
        Debug.Log("Debug 00");
        onetime = true;
		yield return new WaitForSeconds(1);
        Time.timeScale = 0.0f;
		anim.SetBool ("Lose", true);
        Debug.Log("Debug after WaitForSeconds(1)");
        yield return new WaitUntil(() => anim.IsInTransition(0) && anim.GetNextAnimatorStateInfo(0).IsName("Finish"));
        Debug.Log("Debug after WaitUntil");
        //yield return new WaitForSeconds(2.0f);
        Time.timeScale = 1;
        onetime = false;
		PlayerPrefs.SetString ("lastLoadedScene", SceneManager.GetActiveScene ().name);
		SceneManager.LoadScene("Lose_UI");
	}

}
