using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallColliderScript : MonoBehaviour {

    private Button leverBtn;

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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Lever")
        {
            leverBtn.gameObject.SetActive(false);
        }
    }

    public void LeverTrigger(Collider2D lever)
    {
        if (!lever.gameObject.GetComponent<LeverScript>().switchOff)
            lever.gameObject.GetComponent<LeverScript>().switchOff = true;
        else
            lever.gameObject.GetComponent<LeverScript>().switchOff = false;
    }
}
