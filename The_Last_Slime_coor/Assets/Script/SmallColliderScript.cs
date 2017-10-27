using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallColliderScript : MonoBehaviour
{

    private GameObject leverBtnObj;
    private Button leverBtn;

    private Collider2D lever;
    private bool OneTimes = false;
    // Use this for initialization
    void Start()
    {
        leverBtnObj = GameObject.FindGameObjectWithTag("LeverBtn");
    }

    // Update is called once per frame
    void Update()
    {
        if (leverBtnObj != null && OneTimes == false)
        {
            leverBtn = leverBtnObj.GetComponent<Button>();
            leverBtn.gameObject.SetActive(false);
            OneTimes = true;
        }
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
        {
            lever.gameObject.GetComponent<LeverScript>().switchOff = true;
            SoundManager.LeverRea = true;
        }
        else
        {
            lever.gameObject.GetComponent<LeverScript>().switchOff = false;
            SoundManager.LeverRea = true;
        }
    }
}
