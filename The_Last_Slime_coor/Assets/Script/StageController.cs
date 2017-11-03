using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour {

    public static int slimeCnt;

    public static GameObject[] slime;

    public Camera maincamera;

    private GameObject[] panel = new GameObject[3];

    private Button[] splitSprite = new Button[3];

	// Use this for initialization
	void Start () {
        slimeCnt = 1;

        for (int i = 1; i <= 3; i++)
        {
            splitSprite[i - 1] = GameObject.Find("Split" + i.ToString()).GetComponent<Button>();
            panel[i - 1] = GameObject.Find("Split" + i.ToString() + " panel");
        }

        panel[0].gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        slime = GameObject.FindGameObjectsWithTag("Player");

        slimeCnt = slime.Length;

        if (slimeCnt > 1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i < slimeCnt)
                    panel[i].gameObject.SetActive(true);
                else
                    panel[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                panel[i].gameObject.SetActive(false);
            }
        }

        if (maincamera.gameObject.GetComponent<CameraScript>().target == null)
        {
            maincamera.gameObject.GetComponent<CameraScript>().target = slime[0];
            slime[0].GetComponent<Movewithmouse>().isControl = true;
        }
	}

    public void SpriteButtonClick(Button btn)
    {
        SoundManager.ButtonRea = true;

        if(btn.name == "Split1")
        {
            slime[0].GetComponent<Movewithmouse>().isControl = true;

            if (panel[1].gameObject.activeSelf)
                slime[1].GetComponent<Movewithmouse>().isControl = false;
            else
                return;

            if (panel[2].gameObject.activeSelf)
                slime[2].GetComponent<Movewithmouse>().isControl = false;

            maincamera.gameObject.GetComponent<CameraScript>().target = slime[0];
        }
        else if (btn.name == "Split2")
        {
            slime[0].GetComponent<Movewithmouse>().isControl = false;
            slime[1].GetComponent<Movewithmouse>().isControl = true;

            if (panel[2].gameObject.activeSelf)
                slime[2].GetComponent<Movewithmouse>().isControl = false;

            maincamera.gameObject.GetComponent<CameraScript>().target = slime[1];
        }
        else if (btn.name == "Split3")
        {
            slime[0].GetComponent<Movewithmouse>().isControl = false;
            slime[1].GetComponent<Movewithmouse>().isControl = false;
            slime[2].GetComponent<Movewithmouse>().isControl = true;

            maincamera.gameObject.GetComponent<CameraScript>().target = slime[2];
        }
    }
}
