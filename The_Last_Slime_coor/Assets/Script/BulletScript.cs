using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public GameObject prefabEff;
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

            Instantiate(prefabEff, this.transform.position, Quaternion.identity);
            SoundManager.FireBallRea = true;
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Movewithmouse>().theRealOne == true)
            {
                Movewithmouse.isDead = true;
            }
            else
            {
                collision.gameObject.GetComponent<Movewithmouse>().NotReal_DeadOrTimeUP();
            }

            SoundManager.FireBallRea = true;
            //SoundManager.DeadRea = true;
            Instantiate(prefabEff, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject,0.1f);
        }
    }
}
