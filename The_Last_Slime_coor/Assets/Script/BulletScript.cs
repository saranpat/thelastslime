using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    private float Speed = 2;
	// Use this for initialization
	void Start () {
		
	}
	
    public void Set_Speed(float s)
    {
        Speed = s;
    }

	// Update is called once per frame
	void Update () {
        this.transform.Translate(0, Speed * Time.deltaTime, 0);
  
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Door")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            SoundManager.DeadRea = true;
            Movewithmouse.isDead = true;
            Destroy(this.gameObject,0.1f);
        }
    }
}
