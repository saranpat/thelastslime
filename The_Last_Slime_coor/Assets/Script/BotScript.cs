using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotScript : MonoBehaviour {

    public GameObject[] targetPin;
    public float speed;
    public float dist; // distance the enemy can "see" in front of him

    private int curPathIndex;
    private Transform targetPoint;
    private Vector2 dir;

    private bool isDetect;

    // Use this for initialization
    void Start () {
        curPathIndex = 0;

        isDetect = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // check if we have somewere to walk
        if (curPathIndex < targetPin.Length && !isDetect)
        {
            if (targetPoint == null)
                targetPoint = targetPin[curPathIndex].transform;
            walk();
        }

        DetectPlayer();
        print(isDetect);
	}

    void walk()
    {
        // rotate towards the target
        dir = targetPoint.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle - 90, transform.forward);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (transform.position == targetPoint.position)
        {
            curPathIndex++;

            if (curPathIndex == targetPin.Length)
                curPathIndex = 0;

            targetPoint = targetPin[curPathIndex].transform;
        }
    }

    void DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist);

        if(hit.collider.tag == "Player")
        {
            isDetect = true;
        }
        else
        {
            isDetect = false;
        }
    }
}
