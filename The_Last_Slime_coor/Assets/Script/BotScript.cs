using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotScript : MonoBehaviour {

    public GameObject[] targetPin;
    public float speed;
    public float dist; // distance the enemy can "see" in front of him
    public float visionAngle;

    private int curPathIndex;
    private Transform targetPoint;
    private Vector2 dir;
    private float angle;

    private bool isDetect;
    private GameObject player;
    private RaycastHit checkWall;

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

        if (isDetect)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= dist 
                && Vector2.Distance(transform.position, player.transform.position) > 0.7f)
            {
                dir = player.transform.position - transform.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.AngleAxis(angle - 90, transform.forward);
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
            else
            {
                StartCoroutine(ReturnToPatrol());
            }
        }
    }

    void walk()
    {
        // rotate towards the target
        dir = targetPoint.position - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

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
        Vector2 offsetR = Quaternion.AngleAxis(visionAngle, transform.forward) * dir;
        Vector2 offsetL = Quaternion.AngleAxis(-visionAngle, transform.forward) * dir;

        /*Debug.DrawRay(transform.position, dir, Color.red);
        Debug.DrawRay(transform.position, offsetR, Color.green);
        Debug.DrawRay(transform.position, offsetL, Color.blue);*/

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, offsetR, dist);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, offsetL, dist);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire")
            {
                isDetect = false;
            }
            else if (hit.collider.tag == "Player")
            {
                isDetect = true;
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
        if (hit2.collider != null)
        {
            if (hit2.collider.tag == "Player")
            {
                isDetect = true;
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
        if (hit3.collider != null)
        {
            if (hit3.collider.tag == "Player")
            {
                isDetect = true;
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
    }

    IEnumerator ReturnToPatrol()
    {
        Movewithmouse.isDead = true;
 
        yield return new WaitForSeconds(2.0f);

        isDetect = false;
        curPathIndex = 0;
    }
}
