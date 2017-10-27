using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardScript : MonoBehaviour {

    private FieldOfView _FieldOfView;
    private bool isAlarm;
    private GameObject NearPlayer;

    private int curPathIndex = 0;

    public GameObject[] point;


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
            isAlarm = false;
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
