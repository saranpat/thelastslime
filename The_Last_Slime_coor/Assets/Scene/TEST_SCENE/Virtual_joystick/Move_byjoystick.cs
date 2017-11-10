using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_byjoystick : MonoBehaviour {
	public Joystick joy;
	public float speed = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector2 dir = Vector2.zero;
		float angle;
		dir.x = joy.Horizontal ();
		dir.y = joy.Vertical ();
		Vector3 dirformove = new Vector3 (transform.position.x + dir.x, transform.position.y + dir.y, transform.position.z);
		angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		//transform.Translate (dir * speed * Time.deltaTime);
		if (Time.timeScale != 0)transform.rotation = Quaternion.AngleAxis(angle-90, transform.forward);

		transform.position = Vector3.MoveTowards(transform.position, dirformove, speed * Time.deltaTime);
		//transform.rotation = rot;

	}
}
/*target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
target.z = transform.position.z;
target2d = new Vector2(target.x, target.y);

var dir = target - transform.position;
var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

transform.rotation = Quaternion.AngleAxis(angle - 90, transform.forward); //-90 for face toward mouse
transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);*/