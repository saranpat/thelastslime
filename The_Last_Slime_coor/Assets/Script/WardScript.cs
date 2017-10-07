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
            int i;

            for (int j = 0; j < bot.Length; j++)
            {
                if (Vector2.Distance(bot[j].transform.position, transform.position) <= 10)
                {

                }
            }
        }
	}

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            print("Alerttttt");
        }
    }
}
