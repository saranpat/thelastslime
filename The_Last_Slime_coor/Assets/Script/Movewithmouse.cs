﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movewithmouse : MonoBehaviour {
	public float speed = 1.5f;

    public int keyCount;
    public static int keyCnt;

    public static bool cantDetect;
    public static bool isDead;

    private Vector3 target;
	private Vector2 target2d;
    private Vector2 startPos;

	private Rigidbody2D rb2d;

	void Start () {
		target = transform.position;
        startPos = target;

        keyCnt = keyCount;

		rb2d = GetComponent<Rigidbody2D> ();

        isDead = false;
        cantDetect = false;
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
	}

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Fire")
        {
            isDead = true;
        }

        if (other.tag == "Key")
        {
            keyCnt--;

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
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cantDetect = false;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            cantDetect = true;
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);

        transform.position = startPos;
        isDead = false;
    }
}
