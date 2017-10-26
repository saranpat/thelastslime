using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallColliderScript : MonoBehaviour {

    private Button leverBtn;

    private Collider2D lever;

	// Use this for initialization
	void Start () {
        leverBtn = GameObject.Find("LeverBtn").gameObject.GetComponent<Button>();

        leverBtn.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Lever")
        {
            leverBtn.gameObject.SetActive(true);
            lever = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Lever")
        {
            leverBtn.gameObject.SetActive(false);
        }
    }

    public void LeverTrigger()
    {
        if (!lever.gameObject.GetComponent<LeverScript>().switchOff)
            lever.gameObject.GetComponent<LeverScript>().switchOff = true;
        else
            lever.gameObject.GetComponent<LeverScript>().switchOff = false;
    }
}
