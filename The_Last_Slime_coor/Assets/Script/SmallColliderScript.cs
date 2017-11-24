using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallColliderScript : MonoBehaviour
{
    public Movewithmouse _Movewithmouse;
    private GameObject leverBtnObj;
    private Button leverBtn;

    private Collider2D lever;
    private bool OneTimes = false;
    // Use this for initialization
    void Start()
    {
        _Movewithmouse = this.GetComponentInParent<Movewithmouse>();
        if (_Movewithmouse.theRealOne)
        {
            leverBtnObj = GameObject.FindGameObjectWithTag("LeverBtn");
        }
        
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
            if (leverBtn!=null)
            leverBtn.gameObject.SetActive(true);
            lever = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Lever")
        {
            if (leverBtn != null)
            leverBtn.gameObject.SetActive(false);
        }
    }

    public void LeverTrigger()
    {
        if (PlayerPrefs.GetInt("SFXMute") == 0)
        {
            SoundManager.ButtonRea = true;
            SoundManager.LeverRea = true;
        }

        if (leverBtn != null)
        lever.gameObject.GetComponent<LeverScript>().PullLever();
        /*if (!lever.gameObject.GetComponent<LeverScript>().switchOff)
        {
            lever.gameObject.GetComponent<LeverScript>().switchOff = true;
            lever.gameObject.GetComponent<LeverScript>().PullLever();
            SoundManager.LeverRea = true;
        }
        else
        {
            lever.gameObject.GetComponent<LeverScript>().switchOff = false;
            lever.gameObject.GetComponent<LeverScript>().PullLever();
            SoundManager.LeverRea = true;
        }*/
    }
}
