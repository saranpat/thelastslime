using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Scenecontroller : MonoBehaviour {

	public Button startbutton;
	public Button startbutton2;
	public Image black;
	public Animator anim;
    public Text txt;
	// Use this for initialization
	void Start () {
		/*startbutton.onClick.AddListener (delegate {
			StartCoroutine(FadingA());
		});
		startbutton2.onClick.AddListener (delegate {
			StartCoroutine(FadingB());
		});*/

        StartCoroutine(TextBlink());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /*IEnumerator FadingA()
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
	}*/

    public void OnTap()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator TextBlink()
    {
        while (true)
        {
            txt.text = "";

            yield return new WaitForSeconds(0.5f);

            txt.text = "Tap to start!!";

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator LoadScene()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
    }
}
