using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movewithmouse : MonoBehaviour {
	public float speed = 1.5f;

    public static bool cantDetect;
    public static bool isDead;

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

        isLeavingWater = false;
	}

	void Update () {
		if (Input.GetMouseButton(0) && !isDead) {
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target.z = transform.position.z;
			target2d = new Vector2 (target.x, target.y);

            var dir = target - transform.position;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.AngleAxis(angle-90, transform.forward); //-90 for face toward mouse
			transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
			//rb2d.velocity =target2d.normalized * speed;
		}

        if (isDead)
            StartCoroutine(Respawn());

        if (isLeavingWater)
            StartCoroutine(LeavingWater());
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
            isLeavingWater = false;
            cantDetect = true;
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            isLeavingWater = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Lever")
        {
            if (Input.GetMouseButtonDown(0))
            {
                LeverScript.switchOff = true;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Alerttttt");
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
        yield return new WaitForSeconds(0.1f);

        if (isLeavingWater)
        {
            cantDetect = false;
            gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/big_slime");
            transform.localScale = new Vector2(0.6f, 0.6f);
            isLeavingWater = false;

            yield return new WaitForSeconds(5.0f);

            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Slime");
            transform.localScale = new Vector2(0.3f, 0.3f);
        }
    }
}
