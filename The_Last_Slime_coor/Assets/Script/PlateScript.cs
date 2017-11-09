using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlateScript : MonoBehaviour
{

    public GameObject ObjToOpen;
    public LayerMask targetMask;
    public bool IsTreading;

    public GameObject On_Eff;
    public GameObject Off_Eff;

    private DoorScript _DoorScript;
    private FireBlower _FireBlower;
    private bool playSoundOneTime_Enter;
    private bool playSoundOneTime_Exit;

    [HideInInspector]
    public float Radius;

    // Use this for initialization
    void Start()
    {
        Radius = 0.4f;
        playSoundOneTime_Enter = false;
        playSoundOneTime_Exit = true;

        IsTreading = false;
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
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, Radius, targetMask);

        if (targetsInViewRadius.Length > 1) // 1 เพราะตัวมันเองไม่นับ
        {
            IsTreading = true;
        }
        else
        {
            IsTreading = false;
        }

        if (_DoorScript != null)
        {
            _DoorScript.Plate_Interaction(IsTreading);
        }
        if (_FireBlower != null)
        {
            _FireBlower.Plate_Interaction(IsTreading);
        }

        On_Eff.SetActive(!IsTreading);
        Off_Eff.SetActive(IsTreading);
        Plate_PlaySound();
    }


    void Plate_PlaySound()
    {
        if (IsTreading && !playSoundOneTime_Enter)
        {
            playSoundOneTime_Enter = true;
            playSoundOneTime_Exit = false;
            SoundManager.UnlockedRea = true;
        }
        else if (!IsTreading && !playSoundOneTime_Exit)
        {
            playSoundOneTime_Exit = true;
            playSoundOneTime_Enter = false;
            SoundManager.UnlockedRea = true;
        }
        
    }

    

}
[CustomEditor(typeof(PlateScript))]
public class PlateScriptEditor : Editor
{
    void OnSceneGUI()
    {
        PlateScript fow = (PlateScript)target;
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.Radius);
    }
}