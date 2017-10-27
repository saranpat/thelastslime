using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardScript : MonoBehaviour {

    private FieldOfView _FieldOfView;
    private bool isAlarm;
    private GameObject NearPlayer;
    private Transform targetPoint;
    private int curPathIndex = 0;
    private bool rotating;

    public GameObject[] point;
    public float speed = 5;

	// Use this for initialization
	void Start () {

        _FieldOfView = this.gameObject.GetComponent<FieldOfView>();
        isAlarm = false;
	}
	
	// Update is called once per frame
	void Update () {

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, _FieldOfView.viewRadius, _FieldOfView.targetMask);

        if (_FieldOfView.visibleTargets.Count > 0)
        {
            if (_FieldOfView.visibleTargets.Count == 1)
            {
                NearPlayer = _FieldOfView.visibleTargets[0].gameObject;
            }
            else if (_FieldOfView.visibleTargets.Count == 2)
            {
                float A1 = Vector2.Distance(_FieldOfView.visibleTargets[0].transform.position, this.transform.position);
                float A2 = Vector2.Distance(_FieldOfView.visibleTargets[1].transform.position, this.transform.position);
                if (A1 < A2)
                {
                    NearPlayer = _FieldOfView.visibleTargets[0].gameObject;
                }
                else
                {
                    NearPlayer = _FieldOfView.visibleTargets[1].gameObject;
                }

            }
            else if (_FieldOfView.visibleTargets.Count == 3)
            {
                float A1 = Vector2.Distance(_FieldOfView.visibleTargets[0].transform.position, this.transform.position);
                float A2 = Vector2.Distance(_FieldOfView.visibleTargets[1].transform.position, this.transform.position);
                float A3 = Vector2.Distance(_FieldOfView.visibleTargets[2].transform.position, this.transform.position);
                if (A1 < A2 && A1 < A3)
                {
                    NearPlayer = _FieldOfView.visibleTargets[0].gameObject;
                }
                else if (A2 < A1 && A2 < A3)
                {
                    NearPlayer = _FieldOfView.visibleTargets[1].gameObject;
                }
                else
                {
                    NearPlayer = _FieldOfView.visibleTargets[2].gameObject;
                }
            }
            /*if (isAlarm == false)
            {

            }*/
            isAlarm = true;
        }
        else
        {
            if (isAlarm)
                targetPoint = point[curPathIndex].transform;
            isAlarm = false;
        }

        if (curPathIndex < point.Length ) //&& !isAlarm
        {
            float CurSpeed = speed;

            if (targetPoint == null)
                targetPoint = point[curPathIndex].transform;

            Vector2 dir = targetPoint.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion dummyRotation = Quaternion.AngleAxis(angle - 90, transform.forward);
            Vector2 dirToTarget = (targetPoint.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, dirToTarget) < 1)
            {
                if (!isAlarm)
                rotating = false;
            }
            else
            {
                rotating = true;
            }


            if (isAlarm && NearPlayer != null)
            {
                targetPoint = NearPlayer.transform;

                CurSpeed = speed *2;
            }
            else if (rotating == false )//&& !isAlarm
            {
                curPathIndex++;
                if (curPathIndex == point.Length)
                {
                    curPathIndex = 0;
                }
                targetPoint = point[curPathIndex].transform;
                CurSpeed = speed / 50;
            }

            float FinalSpeed = CurSpeed / 100;

            Debug.Log(speed);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, dummyRotation, FinalSpeed);



        }


	}

    public bool Get_isAlarm()
    {
        return isAlarm;
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isAlarm = false;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isAlarm = true;
        }
    }*/
}
