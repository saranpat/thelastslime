﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBlockScript : MonoBehaviour
{
    public GameObject slimePrefab;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<Movewithmouse>().bulkUp && StageController.slimeCnt < 3 && collision.gameObject.GetComponent<Movewithmouse>().theRealOne == true)
        {
            GameObject miniMe = Instantiate(slimePrefab, transform.position, transform.rotation) as GameObject;
            SoundManager.SlimeSplitRea = true;

            miniMe.gameObject.GetComponent<Movewithmouse>().theRealOne = false;
            miniMe.gameObject.GetComponent<Movewithmouse>().isControl = false;

            Collider2D dummy = miniMe.gameObject.GetComponentInChildren<Collider2D>();

            StartCoroutine(offCollider2D(dummy));

            collision.gameObject.GetComponent<Movewithmouse>().bulkUp = false;
            collision.gameObject.GetComponent<Movewithmouse>().GoToNormalSize();
        }
    }

    IEnumerator offCollider2D(Collider2D dummy)
    {
        yield return new WaitForSeconds(10.0f);
        if (dummy != null)
        {
            dummy.enabled = false;
            dummy.gameObject.GetComponent<Movewithmouse>().NotReal_DeadOrTimeUP(); //play Ani Dead
        }
    }
}
