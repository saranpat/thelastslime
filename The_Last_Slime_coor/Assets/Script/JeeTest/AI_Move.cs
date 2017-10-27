﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Move : MonoBehaviour
{

    public GameObject[] targetPin;
    public float speed;
    public float dist; // distance the enemy can "see" in front of him
    public float visionAngle;
    public bool isLoop;

    private int curPathIndex;
    private Transform targetPoint;
    private Vector2 dir;
    private float angle;
    private bool plus;

    FieldOfView _FieldOfView;


    public bool isDetect;
    private GameObject player;
    private GameObject NearPlayer;
    private GameObject NearPlayerOld;
    private RaycastHit checkWall;

    /// <BackToTheOriginal>
    /// ใช้กับฟังก์ชั่น BackToTheOriginal
    private AI_GetNode _AI_GetNode;
    private Transform[] NodePosition;
    private Transform Old_targetPoint;
    private int Old_DummytargetPoint;
    private float DummyTime = 0;
    private bool ifNewtargetPoint = false;
    /// </BackToTheOriginal>

    private float TimeDetect;
    public float Time_for_Chase = 15;


    public bool Hero_AI;

    private bool playDetectSound;

    // Use this for initialization
    void Start()
    {
        if (this.gameObject.GetComponent<FieldOfView>() != null)
            _FieldOfView = this.gameObject.GetComponent<FieldOfView>();

        //player = GameObject.FindGameObjectWithTag("Player");

        _AI_GetNode = GameObject.FindGameObjectWithTag("Node").GetComponent<AI_GetNode>();
        curPathIndex = 0;

        isDetect = false;
        plus = true;

        playDetectSound = false;
    }

    void AI_Chase()
    {
        // rotate towards the target
        dir = targetPoint.position - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion dummyRotation = Quaternion.AngleAxis(angle - 90, transform.forward);
        transform.rotation = Quaternion.Slerp(this.transform.rotation, dummyRotation, 0.05f);
        // move towards the target

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPoint.position - transform.position, 0.75f);
        if (hit.collider != null) //
        {
            if (hit.collider.tag != "Water")
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
            }
            else
            {
                //DummyTime += Time.deltaTime;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        }

        //transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        BackToTheOriginal_AIGoToPlayer();

        if (transform.position == targetPoint.position)
        {
            if (ifNewtargetPoint)
            {
                if (player.transform.gameObject.layer == 11)
                {
                    targetPoint = targetPin[curPathIndex].transform;
                    DummyTime = 0;
                    isDetect = false;
                    TimeDetect = 0;
                    ifNewtargetPoint = false;

                    if (playDetectSound)
                        SoundManager.NormalRea = true;

                    playDetectSound = false;
                }
                else
                {
                    targetPoint = player.transform;
                    ifNewtargetPoint = false;
                }
            }
        }



        TimeDetect += Time.deltaTime;
        if (TimeDetect > Time_for_Chase || Vector2.Distance(transform.position, player.transform.position) > dist + 1)
        {
            //ถ้าเวลาตามหมดแต่ยังเห็นผู้เล่นอยู่ก็ตามไปจนกว่าผู้เล่นจะหลบมุม
            if (targetPoint.position == player.transform.position)
            {

            }
            else
            {
                isDetect = false;
                TimeDetect = 0;

                if (playDetectSound)
                    SoundManager.NormalRea = true;

                playDetectSound = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check if we have somewere to walk
        if (curPathIndex < targetPin.Length && !isDetect)
        {
            if (targetPoint == null)
                targetPoint = targetPin[curPathIndex].transform;
            walk();
        }

        if (_FieldOfView.visibleTargets.Count > 0)
        {
            if (_FieldOfView.visibleTargets.Count == 1)
            {
                Debug.Log("1");
                NearPlayer = _FieldOfView.visibleTargets[0].gameObject;
            }
            else if (_FieldOfView.visibleTargets.Count == 2)
            {
                Debug.Log("2");
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
                Debug.Log("2");
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
            if (isDetect == false || NearPlayer != NearPlayerOld)
            {
                NearPlayerOld = NearPlayer;
                player = NearPlayer.gameObject;
                targetPoint = player.transform;
            }
            isDetect = true;

        }
           
        if (isDetect)
        {
            if (player != null)
            {
                if (!playDetectSound)
                    SoundManager.DetectedRea = true;

                playDetectSound = true;

                if (Vector2.Distance(transform.position, player.transform.position) > 0.7f)
                {
                    if (StopMoveing == false)
                    {
                        AI_Chase();
                    }
                }
                else
                {
                    if (player.gameObject.GetComponent<Movewithmouse>().theRealOne == true)
                    {
                        StartCoroutine(ReturnToPatrol());
                    }
                    else
                    {
                        Destroy(player.GetComponent<Collider2D>());
                        Destroy(player.gameObject, 0.1f);
                        StopMoveing = false;
                        isDetect = false;

                        if (playDetectSound)
                            SoundManager.NormalRea = true;

                        playDetectSound = false;
                        TimeDetect = 0;
                        curPathIndex = 0;
                        targetPoint = targetPin[curPathIndex].transform;
                    }
                }
            }
            else
            {
                StopMoveing = false;
                isDetect = false;
                TimeDetect = 0;
                curPathIndex = 0;
                targetPoint = targetPin[curPathIndex].transform;

                if (playDetectSound)
                    SoundManager.NormalRea = true;

                playDetectSound = false;
            }


        }
    }

    void BackToTheOriginal()
    {
        float LengthForRay = 0.5f;
        for (int k = 0; k < targetPin.Length; k++)
        {
            if (targetPoint.position == targetPin[k].transform.position)
            {
                LengthForRay = 0.5f;
                break;
            }
            else
            {
                LengthForRay = 0.5f;
                continue;
            }
        }

        Vector2 offsetR = Quaternion.AngleAxis(30, transform.forward) * dir;
        Vector2 offsetL = Quaternion.AngleAxis(-30, transform.forward) * dir;


        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPoint.position - transform.position, LengthForRay); //ชนกำแพงมากว่ากี่วิ
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, offsetR, LengthForRay);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, offsetL, LengthForRay);


        Debug.DrawRay(transform.position, targetPoint.position - transform.position, Color.white);
        //Debug.DrawRay(transform.position, offsetR, Color.green);
        //Debug.DrawRay(transform.position, offsetL, Color.blue);

        if (hit.collider != null)// && ifNewtargetPoint == false
        {
            if(Hero_AI)
            {
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Water")
                {
                    DummyTime = 50;
                }
            }
            else
            {
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire" || hit.collider.tag == "Water")
                {
                    DummyTime = 50;
                }
            }

        }
        if (hit2.collider != null)// && ifNewtargetPoint == false
        {
            if (Hero_AI)
            {
                if (hit2.collider.tag == "Wall" || hit2.collider.tag == "Water")
                {
                    DummyTime = 50;
                }
            }
            else
            {
                if (hit2.collider.tag == "Wall" || hit2.collider.tag == "Fire" || hit2.collider.tag == "Water")
                {
                    DummyTime = 50;
                }
            }
        }
        if (hit3.collider != null)// && ifNewtargetPoint == false
        {
            if (Hero_AI)
            {
                if (hit3.collider.tag == "Wall" || hit3.collider.tag == "Water")
                {
                    DummyTime = 50;
                }
            }
            else
            {
                if (hit3.collider.tag == "Wall" || hit3.collider.tag == "Fire" || hit3.collider.tag == "Water")
                {
                    DummyTime = 50;
                }
            }
        }



        Old_targetPoint = targetPoint;

        if (DummyTime > 0.01f)
        {
            this.NodePosition = _AI_GetNode.NodePosition;
            float disNearest;
            disNearest = Mathf.Infinity;
            Transform Dummy = null;
            int DummyOldPoint = 0;

            for (int i = 1; i < NodePosition.Length; i++)
            {
                if (i == Old_DummytargetPoint)
                {
                    Old_DummytargetPoint = 0;
                    continue;
                }

                float Dummydis = Vector2.Distance(NodePosition[i].transform.position, this.transform.position);
                float DummytargetPointDis = Vector2.Distance(targetPoint.position, NodePosition[i].transform.position);
                float H = Dummydis + DummytargetPointDis;

                RaycastHit2D I_hit_wall = Physics2D.Raycast(NodePosition[i].transform.position, targetPoint.position - NodePosition[i].transform.position, DummytargetPointDis);
                if (I_hit_wall.collider != null)
                {
                    if (Hero_AI)
                    {
                        if (I_hit_wall.collider.tag == "Wall" || I_hit_wall.collider.tag == "Water")
                        {
                            H += DummytargetPointDis;
                        }
                    }
                    else
                    {
                        if (I_hit_wall.collider.tag == "Wall" || I_hit_wall.collider.tag == "Fire" || I_hit_wall.collider.tag == "Water")
                        {
                            H += DummytargetPointDis;
                        }
                    }

                }

                RaycastHit2D Nothit = Physics2D.Raycast(transform.position, NodePosition[i].transform.position - this.transform.position, Dummydis);

                if (Nothit.collider != null)
                {
                    if (Hero_AI)
                    {
                        if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Water")
                        {
                            continue;
                        }
                        else
                        {
                            if (H < disNearest)
                            {
                                disNearest = H;
                                Dummy = NodePosition[i];
                                DummyOldPoint = i;
                            }
                        }
                    }
                    else
                    {
                        if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Fire" || Nothit.collider.tag == "Water")
                        {
                            continue;
                        }
                        else
                        {
                            if (H < disNearest)
                            {
                                disNearest = H;
                                Dummy = NodePosition[i];
                                DummyOldPoint = i;
                            }
                        }
                    }
                }
                else
                {
                    if (H < disNearest)
                    {
                        disNearest = H;
                        Dummy = NodePosition[i];
                        DummyOldPoint = i;
                    }
                }
            }
            if (targetPoint != Dummy && Dummy != null)
            {
                ifNewtargetPoint = true;
                targetPoint = Dummy;
                Old_DummytargetPoint = DummyOldPoint;
            }
            else if (Dummy == null)
            {
                //targetPoint = targetPin[curPathIndex].transform;

                disNearest = Mathf.Infinity;
                for (int i = 1; i < NodePosition.Length; i++)
                {
                    float Dummydis = Vector2.Distance(NodePosition[i].transform.position, this.transform.position) * 2;
                    //float H = Dummydis;

                    float DummytargetPointDis = Vector2.Distance(NodePosition[i].transform.position, targetPoint.transform.position);

                    float H = Dummydis + DummytargetPointDis;

                    RaycastHit2D Nothit = Physics2D.Raycast(transform.position, NodePosition[i].transform.position - this.transform.position, Dummydis);
                    if (Nothit.collider != null)
                    {
                        if (Hero_AI)
                        {
                            if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Water")//
                            {
                                continue;
                            }
                            else
                            {
                                if (H < disNearest)
                                {
                                    disNearest = H;
                                    Dummy = NodePosition[i];
                                    DummyOldPoint = i;
                                }

                            }
                        }
                        else
                        {
                            if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Fire" || Nothit.collider.tag == "Water")//
                            {
                                continue;
                            }
                            else
                            {
                                if (H < disNearest)
                                {
                                    disNearest = H;
                                    Dummy = NodePosition[i];
                                    DummyOldPoint = i;
                                }

                            }
                        }
                    }
                    else
                    {
                        if (H < disNearest)
                        {
                            disNearest = H;
                            Dummy = NodePosition[i];
                            DummyOldPoint = i;
                        }

                    }
                }
                if (targetPoint != Dummy && Dummy != null)// && _FieldOfView.visibleTargets.Count >= 0
                {
                    ifNewtargetPoint = true;
                    targetPoint = Dummy;
                    Old_DummytargetPoint = DummyOldPoint;
                }

            }
            DummyTime = 0;
        }
    }
    void BackToTheOriginal_AIGoToPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPoint.position - transform.position, 1f); //ชนกำแพงมากว่ากี่วิ

        if (targetPoint.position == player.transform.position)
        {
            Debug.DrawRay(transform.position, targetPoint.position - transform.position, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, targetPoint.position - transform.position, Color.yellow);
        }

        if (hit.collider != null ) //&& ifNewtargetPoint == false
        {
            if (Hero_AI)
            {
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
            else
            {
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire" || hit.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
        }
        else if (hit.collider != null && targetPoint.position == player.transform.position)
        {
            if (Hero_AI)
            {
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
            else
            {
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire" || hit.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
        }

        Old_targetPoint = targetPoint;

        if (DummyTime > 0.1f)
        {
            this.NodePosition = _AI_GetNode.NodePosition;
            float disNearest;
            disNearest = Mathf.Infinity;
            Transform Dummy = null;//NodePosition[1];
            int DummyOldPoint = 0;

            for (int i = 1; i < NodePosition.Length; i++)
            {
                if (i == Old_DummytargetPoint)
                {
                    Old_DummytargetPoint = 0;
                    continue;
                }

                float Dummydis = Vector2.Distance(NodePosition[i].transform.position, this.transform.position);
                float DummytargetPointDis = Vector2.Distance(NodePosition[i].transform.position, player.transform.position);

                float H = Dummydis + DummytargetPointDis;

                RaycastHit2D I_hit_wall = Physics2D.Raycast(NodePosition[i].transform.position,  player.transform.position - NodePosition[i].transform.position, DummytargetPointDis);
                if (I_hit_wall.collider != null)
                {
                    if (Hero_AI)
                    {
                        if (I_hit_wall.collider.tag == "Wall" || I_hit_wall.collider.tag == "Water")
                        {
                            H += DummytargetPointDis;
                        }
                    }
                    else
                    {
                        if (I_hit_wall.collider.tag == "Wall" || I_hit_wall.collider.tag == "Fire" || I_hit_wall.collider.tag == "Water")
                        {
                            H += DummytargetPointDis;
                        }
                    }
                }
                RaycastHit2D Nothit = Physics2D.Raycast(this.transform.position, NodePosition[i].transform.position - this.transform.position, Dummydis);

                if (Nothit.collider != null)
                {
                    if (Hero_AI)
                    {
                        if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Water")//
                        {
                            continue;
                        }
                        else
                        {
                            if (H < disNearest)
                            {
                                disNearest = H;
                                Dummy = NodePosition[i];
                                DummyOldPoint = i;
                            }

                        }
                    }
                    else
                    {
                        if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Fire" || Nothit.collider.tag == "Water")//
                        {
                            continue;
                        }
                        else
                        {
                            if (H < disNearest)
                            {
                                disNearest = H;
                                Dummy = NodePosition[i];
                                DummyOldPoint = i;
                            }

                        }
                    }
                }
                else
                {
                    if (H < disNearest)
                    {
                        disNearest = H;
                        Dummy = NodePosition[i];
                        DummyOldPoint = i;
                    }

                }
            }
            if (targetPoint != Dummy && Dummy != null)
            {
                ifNewtargetPoint = true;
                targetPoint = Dummy;
                Old_DummytargetPoint = DummyOldPoint;
            }
            else if (Dummy == null)
            {
                Debug.Log("Null BackToTheOriginal_AI");
                disNearest = Mathf.Infinity;
                for (int i = 1; i < NodePosition.Length; i++)
                {
                    float Dummydis = Vector2.Distance(NodePosition[i].transform.position, this.transform.position) * 2;
                    //float H = Dummydis;

                    float DummytargetPointDis = Vector2.Distance(NodePosition[i].transform.position, player.transform.position);

                    float H = Dummydis + DummytargetPointDis;

                    RaycastHit2D Nothit = Physics2D.Raycast(this.transform.position, NodePosition[i].transform.position - this.transform.position, Dummydis);
                    if (Nothit.collider != null)
                    {
                        if (Hero_AI)
                        {
                            if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Water")//
                            {
                                continue;
                            }
                            else
                            {
                                if (H < disNearest)
                                {
                                    disNearest = H;
                                    Dummy = NodePosition[i];
                                    DummyOldPoint = i;
                                }

                            }
                        }
                        else
                        {
                            if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Fire" || Nothit.collider.tag == "Water")//
                            {
                                continue;
                            }
                            else
                            {
                                if (H < disNearest)
                                {
                                    disNearest = H;
                                    Dummy = NodePosition[i];
                                    DummyOldPoint = i;
                                }

                            }
                        }
                    }
                    else
                    {
                        if (H < disNearest)
                        {
                            disNearest = H;
                            Dummy = NodePosition[i];
                            DummyOldPoint = i;
                        }

                    }
                }

                if (targetPoint != Dummy && Dummy != null && _FieldOfView.visibleTargets.Count >= 0)// 
                {
                    ifNewtargetPoint = true;
                    targetPoint = Dummy;
                    Old_DummytargetPoint = DummyOldPoint;
                }
                else if (_FieldOfView.visibleTargets.Count > 0)
                {

                }

                if (_FieldOfView.visibleTargets.Count <= 0)
                {
                    targetPoint = targetPin[curPathIndex].transform;
                    DummyTime = 0;
                    isDetect = false;
                    TimeDetect = 0;
                }



            }
            DummyTime = 0;
        }
    }

    public bool rotating;

    void walk()
    {
        // rotate towards the target
        dir = targetPoint.position - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion dummyRotation = Quaternion.AngleAxis(angle - 90, transform.forward);
        Vector2 dirToTarget = (targetPoint.position - transform.position).normalized;

        BackToTheOriginal(); // เดินกลับไปหาเส้นของมัน

        if (Vector2.Angle(transform.up, dirToTarget) < 1)
        {
            rotating = false;
        }
        else
        {
            //Debug.Log((int)dummyRotation.eulerAngles.z + "  " + (int)transform.rotation.eulerAngles.z);
            rotating = true;
        }

        if (rotating == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        }

        transform.rotation = Quaternion.Slerp(this.transform.rotation, dummyRotation, 0.07f);


        if (transform.position == targetPoint.position)
        {


            if (isLoop)
            {
                curPathIndex++;

                if (curPathIndex == targetPin.Length)
                {
                    curPathIndex = 0;
                }
            }
            else
            {
                if (plus)
                {
                    curPathIndex++;

                    if (curPathIndex == targetPin.Length)
                    {
                        plus = false;
                        curPathIndex = targetPin.Length - 2;
                    }
                }
                else
                {
                    curPathIndex--;

                    if (curPathIndex == -1)
                    {
                        plus = true;
                        curPathIndex = 1;
                    }
                }
            }

            targetPoint = targetPin[curPathIndex].transform;

            /// <BackToTheOriginal>
            if (ifNewtargetPoint)
            {
                targetPoint = Old_targetPoint;
                ifNewtargetPoint = false;
            }
            /// </BackToTheOriginal>

        }
    }

    /*void DetectPlayer()
    {
        Vector2 offsetR = Quaternion.AngleAxis(visionAngle, transform.forward) * dir;
        Vector2 offsetL = Quaternion.AngleAxis(-visionAngle, transform.forward) * dir;

        //Debug.DrawRay(transform.position, dir, Color.white);
        //Debug.DrawRay(transform.position, offsetR, Color.green);
        //Debug.DrawRay(transform.position, offsetL, Color.blue);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, offsetR, dist);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, offsetL, dist);

        if (Movewithmouse.cantDetect)
        {
            isDetect = false;
        }

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire")
            {
                //isDetect = false;
                //isDetect_ButWall = true;
            }
            else if (hit.collider.tag == "Player" && !Movewithmouse.cantDetect)
            {
                if (!isDetect)
                {
                    targetPoint = player.transform;
                    isDetect = true;
                }

            }
        }
        if (hit2.collider != null)
        {
            if (hit2.collider.tag == "Player" && !Movewithmouse.cantDetect)
            {
                if (!isDetect)
                {
                    targetPoint = player.transform;
                    isDetect = true;
                }

            }
        }
        if (hit3.collider != null)
        {
            if (hit3.collider.tag == "Player" && !Movewithmouse.cantDetect)
            {
                if (!isDetect)
                {
                    targetPoint = player.transform;
                    isDetect = true;
                }

            }
        }
    }*/

    private bool StopMoveing;
    IEnumerator ReturnToPatrol()
    {
        Movewithmouse.isDead = true;
        StopMoveing = true;
        yield return new WaitForSeconds(2.0f);
        StopMoveing = false;
        isDetect = false;
        TimeDetect = 0;
        curPathIndex = 0;
        targetPoint = targetPin[curPathIndex].transform;
        transform.position = targetPin[curPathIndex].transform.position;
    }
}
