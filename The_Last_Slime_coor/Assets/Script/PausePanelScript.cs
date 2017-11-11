using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanelScript : MonoBehaviour {
    private string sceneName;

    private GameObject fade;
    private GameObject option;

    // Use this for initialization
    void Start () {
		sceneName = SceneManager.GetActiveScene().name;

        fade = GameObject.Find("Fade");
        option = GameObject.Find("Option");

        option.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Restart ()
    {
        StartCoroutine(loadScene(sceneName));
    }

    public void Title()
    {
        StartCoroutine(loadScene("Open_page"));
    }

    public void Option()
    {
        if (option.activeSelf)
            option.SetActive(false);
        else
            option.SetActive(true);
    }

    IEnumerator loadScene(string s)
    {
        Time.timeScale = 1;
        fade.GetComponent<Animator>().SetBool("Fade", true);
        yield return new WaitUntil(() => fade.GetComponent<Image>().color.a == 1);

        SceneManager.LoadScene(s);
    }
}
