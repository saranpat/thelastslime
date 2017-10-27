using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScript : MonoBehaviour
{

    public GameObject DoorToOpen;
    DoorScript _DoorScript;
    // Use this for initialization
    void Start()
    {

        if (DoorToOpen != null)
            _DoorScript = DoorToOpen.GetComponent<DoorScript>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "CheckWater")
        {
           
            SoundManager.UnlockedRea = true;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "CheckWater")
        {
            //Debug.Log("GGGGGGGGGGGg");
            _DoorScript.isOpen = true;
           
            //Debug.Log("GGGGGGGGGGGffffffffffg");
        }
        /*else //ถึงว่าติดบัคเพราะมันเข้า False ตลอดก่อนที่จะไป True
        {
            Debug.Log("False??");
            _DoorScript.isOpen = false;
        }*/

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "CheckWater")
        {
            //Debug.Log("False??");
            _DoorScript.isOpen = false;
            SoundManager.UnlockedRea = true;
        }
    }

}
