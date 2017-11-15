using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emo : MonoBehaviour {

    public GameObject AI_head;
    private AI_Move _AI_Move;
    private Animator _Animator;

    private string Detect = "Detect";
    private string Alert = "Alert";
    private string NotDetect = "NotDetect";
    private bool oneTimeNotDetect;
	// Use this for initialization
	void Start () {
        oneTimeNotDetect = true;
        _Animator = this.gameObject.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

        if (AI_head != null)
        {
            if (_AI_Move != AI_head.GetComponent<AI_Move>())
            {
                _AI_Move = AI_head.GetComponent<AI_Move>();
            }


            if (_AI_Move.isDetect)
            {
                oneTimeNotDetect = false;
                _Animator.SetBool(Alert, false);
                _Animator.SetBool(NotDetect, false);
                _Animator.SetBool(Detect, true);
            }
            else if (_AI_Move.alertState)
            {
                _Animator.SetBool(NotDetect, false);
                _Animator.SetBool(Detect, false);
                _Animator.SetBool(Alert, true);
            }
            else
            {
                _Animator.SetBool(Detect, false);
                _Animator.SetBool(Alert, false);

                if(!oneTimeNotDetect)
                {
                    oneTimeNotDetect = true;
                    _Animator.SetBool(NotDetect, true);
                    Invoke("offNotDetect", 1.0f);
                }

            }

            this.transform.position = AI_head.transform.position + (Vector3.up * 0.7f);
        }

	}

    void offNotDetect()
    {
        _Animator.SetBool(NotDetect, false);
    }

}
