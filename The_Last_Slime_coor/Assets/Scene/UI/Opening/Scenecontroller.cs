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

    private GameObject startPanel;
    private GameObject selectPanel;

    private Button[] lvlBtn = new Button[13];

	// Use this for initialization
	void Start () {
        /*startbutton.onClick.AddListener (delegate {
			StartCoroutine(FadingA());
		});
		startbutton2.onClick.AddListener (delegate {
			StartCoroutine(FadingB());
		});*/

        startPanel = GameObject.Find("StartPanel");
        selectPanel = GameObject.Find("SelectPanel");

        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 2);

        for (int i = 0; i < 13; i++)
        {
            if (i == 0)
                lvlBtn[i] = GameObject.Find("Button").GetComponent<Button>();
            else
                lvlBtn[i] = GameObject.Find("Button (" + i.ToString() + ")").GetComponent<Button>();
            
            if (i > PlayerPrefs.GetInt("Level") - 2)
                lvlBtn[i].GetComponentInChildren<Text>().text = "lock";
        }

        selectPanel.SetActive(false);

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

    public void LevelSelect(Button btn)
    {
        string s = btn.GetComponentInChildren<Text>().text;

        if (s != "lock")
            StartCoroutine(LoadLevel(s));
    }

    IEnumerator TextBlink()
    {
        if (startPanel.activeSelf)
        {
            while (true)
            {
                txt.text = "";

                yield return new WaitForSeconds(0.5f);

                txt.text = "Tap to start!!";

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator LoadLevel(string s)
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);

        int i = int.Parse(s);
        i++;

        SceneManager.LoadScene("Scene" + i.ToString());
    }

    IEnumerator LoadScene()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        startPanel.SetActive(false);
        anim.SetBool("Fade", false);
        selectPanel.SetActive(true);
    }
}
