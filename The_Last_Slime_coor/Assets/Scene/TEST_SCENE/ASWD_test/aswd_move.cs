using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aswd_move : MonoBehaviour {
	public float speed ;
	private Vector3 newpos;
	bool isMove;
	// Use this for initialization
	void Start () {
		isMove = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (isMove==false) {
			if (Input.GetKey (KeyCode.W)) {
				
				newpos = transform.position + new Vector3 (0.0f, 1.0f, 0.0f);
				if (checkpos("up")==true) 
				isMove = true;
				

				//transform.position = Vector3.MoveTowards (transform.position, newpos, speed * Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.A)) {
				newpos = transform.position + new Vector3 (-1.0f, 0.0f, 0.0f);
				if (checkpos("left")==true) 
				isMove = true;
			}
			if (Input.GetKey (KeyCode.D)) {
				newpos = transform.position + new Vector3 (1.0f, 0.0f, 0.0f);
				if (checkpos("right")==true) 
				isMove = true;
			}
			if (Input.GetKey (KeyCode.S)) {
				newpos = transform.position + new Vector3 (0.0f, -1.0f, 0.0f);
				if (checkpos("down")==true) 
				isMove = true;
			}
		} else { 
			if (transform.position != newpos) {
				transform.position = Vector3.MoveTowards (transform.position, newpos, speed * Time.deltaTime);
				//if (checkpos (newpos)) 
				isMove = true;
			} else
				isMove = false;
		}
	}
	bool checkpos(string direction)
	{
		RaycastHit2D hitup = Physics2D.Raycast (transform.position+new Vector3(0.0f,0.5f,0.0f), Vector2.up,1.0f);
		Debug.DrawRay(transform.position, Vector2.up, Color.white);
		RaycastHit2D hitdown = Physics2D.Raycast (transform.position+new Vector3(0.0f,-0.5f,0.0f), Vector2.down,1.0f);
		Debug.DrawRay(transform.position, Vector2.down, Color.white);
		RaycastHit2D hitleft = Physics2D.Raycast (transform.position+new Vector3(-0.5f,0.0f,0.0f), Vector2.left,1.0f);
		Debug.DrawRay(transform.position, Vector2.left, Color.white);
		RaycastHit2D hitright = Physics2D.Raycast (transform.position+new Vector3(0.5f,0.0f,0.0f), Vector2.right,1.0f);
		Debug.DrawRay(transform.position, Vector2.right, Color.white);
		if (direction == "up") {
			if (hitup.collider != null) {
				Debug.Log ("found " + hitup.collider.tag);
				if (hitup.collider.tag == "Wall") {
					return false;
				}
			}
		} else if (direction == "down") {
			if (hitdown.collider != null) {
				Debug.Log ("found " + hitdown.collider.tag);
				if (hitdown.collider.tag == "Wall") {

					return false;
				}
			}
		} else if (direction == "left") {
			if (hitleft.collider != null) {
				Debug.Log ("found " + hitleft.collider.tag);
				if (hitleft.collider.tag == "Wall") {
					return false;

				}
			}
		} else if (direction == "right") {
			if (hitright.collider != null) {
				Debug.Log ("found " + hitright.collider.tag);
				if (hitright.collider.tag == "Wall") {
					return false;

				}
			}
		}
		return true;
	}

}
