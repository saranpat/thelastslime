using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardScript : MonoBehaviour {

    private GameObject[] bot;
    private GameObject[] botNearby;

    private bool isAlarm;

	// Use this for initialization
	void Start () {
        bot = GameObject.FindGameObjectsWithTag("Enemy");

        isAlarm = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isAlarm)
        {
            for (int j = 0; j < bot.Length; j++)
            {
                if (Vector2.Distance(bot[j].transform.position, transform.position) <= 10)
                {
                    bot[j].gameObject.GetComponent<WizardScript>().Set_alertState(this.gameObject);
                    //bot[j].gameObject.GetComponent<WizardScript>().alertState_Point = this.transform;
                    
                }
            }

            //isAlarm = false;
        }
	}

    public bool Get_isAlarm()
    {
        return isAlarm;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isAlarm = false;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isAlarm = true;
        }
    }
}
