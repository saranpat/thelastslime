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

    public SpriteRenderer _SpriteRenderer;
    public Sprite[] _Sprite;
    public GameObject[] point;
    public LayerMask targetMask;
    public float speed = 5;
    public float viewRadius = 5;
	// Use this for initialization
	void Start () {

        _FieldOfView = this.gameObject.GetComponent<FieldOfView>();
        isAlarm = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (PauseMenuScript.paused == false)
        {
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


                Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(this.transform.position, viewRadius, targetMask);
                for (int i = 0; i < targetsInViewRadius.Length; i++)
                {
                    if (targetsInViewRadius[i].tag == "Enemy")
                    {
                        Debug.Log(targetsInViewRadius[i].gameObject.name);
                        bool alertState = targetsInViewRadius[i].gameObject.GetComponent<AI_Move>().Get_alertState();
                        if (!alertState && !isAlarm)
                        {
                            Debug.Log("Send");
                            targetsInViewRadius[i].gameObject.SendMessage("Set_alertState", this.gameObject);
                        }

                    }
                }
                isAlarm = true;

            }
            else
            {
                if (isAlarm)
                    targetPoint = point[curPathIndex].transform;
                isAlarm = false;
            }

            if (curPathIndex < point.Length) //&& !isAlarm
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

                    CurSpeed = speed * 2;
                }
                else if (rotating == false)//&& !isAlarm
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
                transform.rotation = Quaternion.Lerp(this.transform.rotation, dummyRotation, FinalSpeed);
            }

            if (this.transform.localRotation.z < 0.35f && this.transform.localRotation.z > -0.35)
            {
                _SpriteRenderer.sprite = _Sprite[0];
            }
            else if (this.transform.localRotation.z < -0.35f && this.transform.localRotation.z > -0.7)
            {
                _SpriteRenderer.sprite = _Sprite[2];
            }
            else if (this.transform.localRotation.z < 0.7f && this.transform.localRotation.z > 0.35)
            {
                _SpriteRenderer.sprite = _Sprite[1];
            }
        }
	}
    public GameObject Get_GameObject()
    {
        if (NearPlayer != null)
            return NearPlayer;
        else
            return null;
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
