using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movewithmouse : MonoBehaviour {
	public float speed = 1.5f;

    public static bool cantDetect;
    public static bool isDead;

    public bool bulkUp; //check state of slime (small or big)
    public bool theRealOne;
    public bool isControl;

    private Vector3 target;
	private Vector2 target2d;
    private Vector2 startPos;

	private Rigidbody2D rb2d;

    private bool isLeavingWater; // check if out of water

	void Start () {
		target = transform.position;
        startPos = target;

		rb2d = GetComponent<Rigidbody2D> ();

        isDead = false;
        cantDetect = false;

        bulkUp = false;

        isLeavingWater = false;
	}

	void Update () {
		if (Input.GetMouseButton(0) && !isDead && isControl) {
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target.z = transform.position.z;
			target2d = new Vector2 (target.x, target.y);

            var dir = target - transform.position;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.AngleAxis(angle-90, transform.forward); //-90 for face toward mouse
			transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
			//rb2d.velocity =target2d.normalized * speed;
		}

        if (isDead & theRealOne)
            StartCoroutine(Respawn());

        if (isDead & !theRealOne)
            Destroy(this.gameObject);

        if (isLeavingWater)
            StartCoroutine(LeavingWater());

        if (!bulkUp) //Return to normal size
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Slime");
            transform.localScale = new Vector2(0.25f, 0.25f);
        }

	}

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Fire")
        {
            isDead = true;
        }

        if (other.tag == "Key")
        {
            other.gameObject.SetActive(false);
        }

        if (other.tag == "Door")
        {
            if (other.gameObject.GetComponent<DoorScript>().isOpen == true)
            {
                //print("To the next Level");
            }
        }

        if (other.tag == "Water")
        {
            cantDetect = true;
            this.gameObject.layer = 11; // layer 11 PlayerInWater
            isLeavingWater = false;
            StopAllCoroutines();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            if (theRealOne)
                isLeavingWater = true;

            cantDetect = false;
            this.gameObject.layer = 10; // layer 10 Player
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            cantDetect = true;
            this.gameObject.layer = 11; // layer 11 PlayerInWater
            isLeavingWater = false;
        }


        if (collision.tag == "Lever")
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!collision.gameObject.GetComponent<LeverScript>().switchOff)
                    collision.gameObject.GetComponent<LeverScript>().switchOff = true;
                else
                    collision.gameObject.GetComponent<LeverScript>().switchOff = false;
            }
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        
        transform.position = startPos;
        isDead = false;
        //Application.LoadLevel();
    }

    IEnumerator LeavingWater()
    {
        //gameObject.GetComponent<CircleCollider2D>().isTrigger = false;

        //yield return new WaitForSeconds(0.1f);

        if (isLeavingWater)
        {
            //cantDetect = false;
            bulkUp = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/big_slime");
            transform.localScale = new Vector2(0.4f, 0.4f);
            isLeavingWater = false;

            yield return new WaitForSeconds(5.0f);

            bulkUp = false;
        }
    }

}
