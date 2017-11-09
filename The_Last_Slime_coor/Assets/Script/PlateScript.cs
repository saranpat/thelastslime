using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScript : MonoBehaviour
{

    public GameObject ObjToOpen;
    private DoorScript _DoorScript;
    private FireBlower _FireBlower;

    // Use this for initialization
    void Start()
    {

        if (ObjToOpen != null)
        {
            if(ObjToOpen.GetComponent<DoorScript>() != null)
                _DoorScript = ObjToOpen.GetComponent<DoorScript>();
            if (ObjToOpen.GetComponent<FireBlower>() != null)
                _FireBlower = ObjToOpen.GetComponent<FireBlower>();
        }
            

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
            if (_DoorScript != null)
            {
                _DoorScript.Plate_Interaction(true);
            }

            if (_FireBlower != null)
            {
                _FireBlower.Plate_Interaction(true);
            }
                

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "CheckWater")
        {
            if (_DoorScript != null)
            {
                _DoorScript.Plate_Interaction(false);
                SoundManager.UnlockedRea = true;
            }

            if (_FireBlower != null)
            {
                _FireBlower.Plate_Interaction(false);
            }
        }
    }

}
