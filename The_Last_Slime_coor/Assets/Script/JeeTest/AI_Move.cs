using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Move : MonoBehaviour
{
    //[HideInInspector]
    public bool alertState; //เตือนจาก ward ถ้าผู้เล่นเข้าใกล้
    [HideInInspector]
    public GameObject alertState_Obj; //รับตำแหน่งของ ward มาเพื่อให้บอทเดินไปดูตรง Ward นั้นๆ

    public GameObject[] targetPin;
    public float speed;
    private float dist; // distance the enemy can "see" in front of him
    public float visionAngle;
    public bool isLoop;

    private int curPathIndex;
    private Transform targetPoint;
    private Vector2 dir;
    private float angle;
    private bool plus;

    private FieldOfView _FieldOfView;
    private Animator _Animator;

    [HideInInspector]
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
    private float TimeAlertState;

    public float Time_for_Chase = 15;


    public bool Hero_AI;
    public bool Wizard_AI;
    public bool Guard_AI;
    public GameObject firePrefab; // magic shot prefab
    public float fireSpeed; //magic shot speed
    public float fireRange; // shooting distance
    private bool isRecharge; //recharge magic

    private bool playDetectSound;
    private bool playDeadSound;

    public GameObject EmoObj;


    // Use this for initialization
    void Start()
    {
        if (this.gameObject.GetComponent<FieldOfView>() != null)
            _FieldOfView = this.gameObject.GetComponent<FieldOfView>();
        if (this.gameObject.GetComponentInChildren<Animator>() != null)
            _Animator = this.GetComponentInChildren<Animator>();

        dist = _FieldOfView.viewRadius;

        //player = GameObject.FindGameObjectWithTag("Player");
        if (GameObject.FindGameObjectWithTag("Node") != null)
        _AI_GetNode = GameObject.FindGameObjectWithTag("Node").GetComponent<AI_GetNode>();
        curPathIndex = 0;

        isDetect = false;
        alertState = false;
        plus = true;

        playDetectSound = false;
        playDeadSound = false;

        GameObject E = Instantiate(EmoObj, this.transform.position, Quaternion.identity) as GameObject;
        E.GetComponent<Emo>().AI_head = this.gameObject;

    }

    public void Set_alertState(GameObject WardTransfrom)
    {
        if (alertState == false && isDetect == false)
        {
            TimeAlertState = 0;
            alertState_Obj = WardTransfrom;
            targetPoint = WardTransfrom.transform;
            alertState = true;
        }
    }
    public bool Get_alertState()
    {
       return alertState;
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
                DummyTime += Time.deltaTime;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        }

        if(Wizard_AI)
            if (Vector2.Distance(transform.position, player.transform.position) <= fireRange && !isRecharge)
            {
                Vector2 dirToTarget = (player.transform.position - this.transform.position).normalized;
                if (Vector2.Angle(transform.up, dirToTarget) < 10 / 1.75f)
                {
                    StartCoroutine(Fire());
                }
               
                
            }


        BackToTheOriginal_AIGoToPlayer();

        if (transform.position == targetPoint.position)
        {
            if (ifNewtargetPoint)
            {
                if (player.transform.gameObject.layer == 11 && Wizard_AI == false) // 11 = PlayerInWater
                {
                    targetPoint = targetPin[curPathIndex].transform;
                    DummyTime = 0;
                    isDetect = false;
                    alertState = false;
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
        if (TimeDetect > Time_for_Chase )//|| Vector2.Distance(transform.position, player.transform.position) > dist + 1)
        {
            //ถ้าเวลาตามหมดแต่ยังเห็นผู้เล่นอยู่ก็ตามไปจนกว่าผู้เล่นจะหลบมุม
            if (targetPoint.position == player.transform.position)
            {

            }
            else
            {
                if(Wizard_AI)
                {
                    if(player.transform.gameObject.layer == 11)
                    {
                        TimeDetect = 0;
                    }
                    else
                    {
                        isDetect = false;
                        alertState = false;
                        TimeDetect = 0;

                        if (playDetectSound)
                            SoundManager.NormalRea = true;

                        playDetectSound = false;
                    }
                }
                else
                {
                    isDetect = false;
                    alertState = false;
                    TimeDetect = 0;

                    if (playDetectSound)
                        SoundManager.NormalRea = true;

                    playDetectSound = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check if we have somewere to walk
        if (curPathIndex < targetPin.Length && !isDetect)
        {
            if (targetPoint == null && !alertState)
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
                        player.GetComponent<Collider2D>().enabled = false;
                        player.gameObject.GetComponent<Movewithmouse>().NotReal_DeadOrTimeUP();
                        //Destroy(player.gameObject, 0.1f);


                        StopMoveing = false;
                        isDetect = false;
                        alertState = false;

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
                alertState = false;
                TimeDetect = 0;
                curPathIndex = 0;
                targetPoint = targetPin[curPathIndex].transform;

                if (playDetectSound)
                    SoundManager.NormalRea = true;

                playDetectSound = false;
            }
        }

        AI_PlayAnimation();
    }

    void AI_PlayAnimation()
    {
        if (isDetect)
        {
            if (_Animator != null)
            _Animator.speed = 2f;
        }
        else
        {
            if (_Animator != null)
            _Animator.speed = 1;
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
        Vector2 offsetR = Quaternion.AngleAxis(30, transform.forward) * dir;
        Vector2 offsetL = Quaternion.AngleAxis(-30, transform.forward) * dir;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPoint.position - transform.position, 0.5f); //ชนกำแพงมากว่ากี่วิ
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, offsetR, 0.5f);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, offsetL, 0.5f);
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
        if (hit2.collider != null) //&& ifNewtargetPoint == false
        {
            if (Hero_AI)
            {
                if (hit2.collider.tag == "Wall" || hit2.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
            else
            {
                if (hit2.collider.tag == "Wall" || hit2.collider.tag == "Fire" || hit2.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
        }
        else if (hit2.collider != null && targetPoint.position == player.transform.position)
        {
            if (Hero_AI)
            {
                if (hit2.collider.tag == "Wall" || hit2.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
            else
            {
                if (hit2.collider.tag == "Wall" || hit2.collider.tag == "Fire" || hit2.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
        }
        if (hit3.collider != null) //&& ifNewtargetPoint == false
        {
            if (Hero_AI)
            {
                if (hit3.collider.tag == "Wall" || hit3.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
            else
            {
                if (hit3.collider.tag == "Wall" || hit3.collider.tag == "Fire" || hit3.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
        }
        else if (hit3.collider != null && targetPoint.position == player.transform.position)
        {
            if (Hero_AI)
            {
                if (hit3.collider.tag == "Wall" || hit3.collider.tag == "Water")
                {
                    DummyTime += Time.deltaTime;
                }
            }
            else
            {
                if (hit3.collider.tag == "Wall" || hit3.collider.tag == "Fire" || hit3.collider.tag == "Water")
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
                    alertState = false;
                    TimeDetect = 0;
                }



            }
            DummyTime = 0;
        }
    }
    [HideInInspector]
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

        if(alertState)
        {
            TimeAlertState += Time.deltaTime;
            if (TimeAlertState > 15)
            {
                TimeAlertState = 0;
                alertState = false;
                targetPoint = targetPin[curPathIndex].transform;
            }
        }



        float AI_Dis_Ward = 0;
        if (alertState_Obj != null && alertState)
        {
            AI_Dis_Ward = Vector2.Distance(alertState_Obj.transform.position, this.transform.position);
        }


        if (alertState_Obj != null && AI_Dis_Ward <= 1.0f && alertState)
        {
            if (alertState_Obj.GetComponent<WardScript>().Get_isAlarm())
            {
                //ifNewtargetPoint = true;
                if (player != null)
                {
                    targetPoint = player.transform;
                }
                else
                {
                    targetPoint = alertState_Obj.GetComponent<WardScript>().Get_GameObject().transform;
                }
                alertState = false;  
                isDetect = true;

            }
            else
            {
                Debug.Log("false");
                alertState = false;
                targetPoint = targetPin[curPathIndex].transform;
            }

        }
        else if (transform.position == targetPoint.position)
        {
            if(alertState)
            {
                targetPoint = alertState_Obj.transform;
            }
            else
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
        /*if (!playDeadSound)
            SoundManager.DeadRea = true;*/

        playDeadSound = true;
        Movewithmouse.isDead = true;
        StopMoveing = true;
        yield return new WaitForSeconds(2.0f);
        StopMoveing = false;
        isDetect = false;
        alertState = false;
        TimeDetect = 0;
        curPathIndex = 0;
        targetPoint = targetPin[curPathIndex].transform;
        transform.position = targetPin[curPathIndex].transform.position;
    }

    IEnumerator Fire()
    {
        isRecharge = true;
        var bullet = (GameObject)Instantiate(firePrefab, transform.position, transform.rotation);
        //Vector2 dirforbullet = targetPoint.position - transform.position;
        //bullet.GetComponent<Rigidbody2D>().velocity = dirforbullet * fireSpeed;
        bullet.gameObject.SendMessage("Set_Speed", fireSpeed*1.5f);
        Destroy(bullet, 2.5f);
        yield return new WaitForSeconds(2.5f);
        isRecharge = false;
    }
    public GameObject Get_PlayerTran()
    {
        return player;
    }

    private void OnCollisionEnter2D(Collision2D collision) //เอาไว้ไม่ให้บอทชนกันเอง
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if(isDetect)
            {
                ifNewtargetPoint = false;
                DummyTime = 50;
                
            }
            else
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
            }



        }
    }

}
