using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBlockScript : MonoBehaviour {

    public GameObject slimePrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Movewithmouse.bulkUp)
        {
            GameObject miniMe = Instantiate(slimePrefab, transform.position, transform.rotation);

            Movewithmouse.bulkUp = false;
        }
    }
}
