using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_aswd : MonoBehaviour {
	public float speed ;
	//private Vector3 newpos;
	bool foundplayer;
	bool isMove;
	GameObject player;
	Vector3 newpos;
	// Use this for initialization
	void Start () {
		foundplayer = false;
		isMove = false;
	}

	// Update is called once per frame
	void Update () {
		
		if (foundplayer == false) {
			detectplayer ();
		} else {
			float distx = player.transform.position.x - transform.position.x; //if player left it minus
			float disty = player.transform.position.y - transform.position.y;//if plyer below it minus
			if (isMove == false) {
				
				if (Mathf.Abs (distx) >= Mathf.Abs (disty)) {
					if (distx <= 0) { //if player on left
						if (checkpos ("left") == true) {
							//move left
							newpos = transform.position+ new Vector3(-1.0f,0.0f,0.0f);
							isMove = true;
						} else { //move other
							if (disty <= 0) {
								if (checkpos ("down") == true) {
									//movedown
									newpos = transform.position+ new Vector3(0.0f,-1.0f,0.0f);
									isMove = true;
								}
							} else {
								if (checkpos ("up") == true) {
									//moveup
									newpos = transform.position+ new Vector3(0.0f,1.0f,0.0f);
									isMove = true;
								}
							}	
							
						}
					} else {  //if player on right
						if (checkpos ("right") == true) {
							//move rigth
							newpos = transform.position+ new Vector3(1.0f,0.0f,0.0f);
							isMove = true;
						} else { //move other
							if (disty <= 0) {
								if (checkpos ("down") == true) {
									//movedown
									newpos = transform.position+ new Vector3(0.0f,-1.0f,0.0f);
									isMove = true;
								}
							} else {
								if (checkpos ("up") == true) {
									//moveup
									newpos = transform.position+ new Vector3(0.0f,1.0f,0.0f);
									isMove = true;
								}
							}	

						}
					}
				} else { //go on y direction
					if (disty <= 0) { //if player on below
						if (checkpos ("down") == true) {
							//move 
							newpos = transform.position+ new Vector3(0.0f,-1.0f,0.0f);
							isMove = true;
						} else { //move other
							if (distx <= 0) {
								if (checkpos ("left") == true) {
									//move
									newpos = transform.position+ new Vector3(-1.0f,0.0f,0.0f);
									isMove = true;
								}
							} else {
								if (checkpos ("right") == true) {
									//move
									newpos = transform.position+ new Vector3(1.0f,0.0f,0.0f);
									isMove = true;
								}
							}	

						}
					} else {  //if player on right
						if (checkpos ("up") == true) {
							//move
							newpos = transform.position+ new Vector3(0.0f,1.0f,0.0f);
							isMove = true;
						} else { //move other
							if (distx <= 0) {
								if (checkpos ("left") == true) {
									//move
									newpos = transform.position+ new Vector3(-1.0f,0.0f,0.0f);
									isMove = true;
								}
							} else {
								if (checkpos ("right") == true) {
									//move
									newpos = transform.position+ new Vector3(1.0f,0.0f,0.0f);
									isMove = true;
								}
							}	

						}
					}
				}

			}
			else { 
				if (transform.position != newpos) {
					transform.position = Vector3.MoveTowards (transform.position, newpos, speed * Time.deltaTime);
					//if (checkpos (newpos)) 
					isMove = true;
				} else
					isMove = false;
			}
		}
	}
	void detectplayer ()
	{
		RaycastHit2D hitdown = Physics2D.Raycast (transform.position+new Vector3(0.0f,-0.5f,0.0f), Vector2.down,1.0f);
		Debug.DrawRay(transform.position, Vector2.down, Color.red);
		if (hitdown.collider != null) {
			if (hitdown.collider.tag == "Player") {
				Debug.Log ("FOLLOW HIM");
				foundplayer=true;
				player = GameObject.FindGameObjectWithTag("Player");
			}
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
