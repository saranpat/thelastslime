using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Move : MonoBehaviour {

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

    public bool JeeTest;
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

        if (!JeeTest) //กันไว้ก่อนเดี๋ยวบัค ซีนอื่นๆจะยังใช้ได้เหมือนเดิมยกเว้นซีนของเรา จะให้ไปจับผู้เล่นจากอีกโค้ดแทน
        {
            DetectPlayer();

            if (isDetect)
            {
                if (Vector2.Distance(transform.position, player.transform.position) <= dist
                    && Vector2.Distance(transform.position, player.transform.position) > 0.7f)
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
                        curPathIndex = 0;
                        targetPoint = targetPin[curPathIndex].transform;
                    }

                }

            }
        }
        else // เพิ่ม
        {

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
                    if(A1 < A2)
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

                /*if (Movewithmouse.cantDetect && _FieldOfView.visibleTargets.Count >= 1)
                {
                    if (isDetect == true)
                    {
                        targetPoint = targetPin[curPathIndex].transform;
                        //DummyTime = 0;
                    }
                    isDetect = false;
                }
                else 
                {
                    if (isDetect == false)
                        targetPoint = player.transform;
                    isDetect = true;
                }*/

            }
            else
            {
                /*if (isDetect == true)
                {
                    //targetPoint = targetPin[curPathIndex].transform;
                    //DummyTime = 0;
                }
                //isDetect = false;
                 if (Movewithmouse.cantDetect)
                 {
                     if (isDetect == true)
                     {
                         targetPoint = targetPin[curPathIndex].transform;
                         //DummyTime = 0;
                     }
                     isDetect = false;
                 }*/


            }

            if (isDetect)
            {
                if (player != null)
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
                        curPathIndex = 0;
                        targetPoint = targetPin[curPathIndex].transform;
                    }
                }

            }
        }

    }

    void BackToTheOriginal()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPoint.position - transform.position, 0.5f); //ชนกำแพงมากว่ากี่วิ
        Debug.DrawRay(transform.position, targetPoint.position - transform.position, Color.white);

        if (hit.collider != null)// && ifNewtargetPoint == false
        {
            //Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire" || hit.collider.tag == "Water")
            {

                DummyTime += Time.deltaTime;
                Debug.Log(DummyTime);
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
                //Debug.Log(Old_DummytargetPoint);
                if (i == Old_DummytargetPoint)
                {
                    //Debug.Log("OldPoint");
                    Old_DummytargetPoint = 0;
                    continue;
                }

                float Dummydis = Vector2.Distance(NodePosition[i].transform.position, this.transform.position);
                float DummytargetPointDis = Vector2.Distance(targetPoint.position, NodePosition[i].transform.position);
                float H = Dummydis + DummytargetPointDis;

                RaycastHit2D I_hit_wall = Physics2D.Raycast(NodePosition[i].transform.position, targetPoint.position - NodePosition[i].transform.position , DummytargetPointDis);
                if (I_hit_wall.collider != null)
                {
                    if (I_hit_wall.collider.tag == "Wall" || I_hit_wall.collider.tag == "Fire" || I_hit_wall.collider.tag == "Water")
                    {
                        H += DummytargetPointDis;
                        //H += Dummydis;
                        /*if (H < disNearest)
                        {
                            disNearest = H;
                            Dummy = NodePosition[i];
                            DummyOldPoint = i;
                        }*/
                    }
                }



                RaycastHit2D Nothit = Physics2D.Raycast(transform.position, NodePosition[i].transform.position - this.transform.position, Dummydis);

                if (Nothit.collider != null)
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
            else
            {
                targetPoint = targetPin[curPathIndex].transform;
            }
            DummyTime = 0;
        }
    }
    void BackToTheOriginal_AIGoToPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPoint.position - transform.position, 0.5f); //ชนกำแพงมากว่ากี่วิ

        if (targetPoint.position == player.transform.position)
        {
            Debug.DrawRay(transform.position, targetPoint.position - transform.position, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, targetPoint.position - transform.position, Color.yellow);
        }


        if (hit.collider != null && ifNewtargetPoint == false) //
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire" || hit.collider.tag == "Water")
            {
                DummyTime += Time.deltaTime;
            }

        }
        else if (hit.collider != null && targetPoint.position == player.transform.position)
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire" || hit.collider.tag == "Water")
            {
                DummyTime += Time.deltaTime;
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
                //Debug.Log(Old_DummytargetPoint);
                if (i == Old_DummytargetPoint)
                {
                    //Debug.Log("OldPoint");
                    Old_DummytargetPoint = 0;
                    continue;
                }

                float Dummydis = Vector2.Distance(NodePosition[i].transform.position, this.transform.position);
                float DummytargetPointDis = Vector2.Distance(NodePosition[i].transform.position, player.transform.position);

                float H = Dummydis + DummytargetPointDis;

                RaycastHit2D I_hit_wall = Physics2D.Raycast(NodePosition[i].transform.position, NodePosition[i].transform.position - player.transform.position, DummytargetPointDis);
                if (I_hit_wall.collider != null)
                {
                    if (I_hit_wall.collider.tag == "Wall" || I_hit_wall.collider.tag == "Fire" || I_hit_wall.collider.tag == "Water")
                    {
                        H += DummytargetPointDis;
                        //H += Dummydis;
                        /*if (H < disNearest)
                        {
                            disNearest = H;
                            Dummy = NodePosition[i];
                            DummyOldPoint = i;
                        }*/
                    }
                }



                //Debug.Log("I : " + i + " : " +H);

                RaycastHit2D Nothit = Physics2D.Raycast(transform.position, NodePosition[i].transform.position - this.transform.position, Dummydis);

                if (Nothit.collider != null)
                {
                    if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Fire" || Nothit.collider.tag == "Water")//
                    {
                        if (Nothit.collider.tag == "Water")
                        {
                            /*H = Dummydis;
                            if (H < disNearest)
                            {
                                disNearest = H;
                                Dummy = NodePosition[i];
                                DummyOldPoint = i;
                            }*/
                        }
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
                //Debug.Log("Null");
                disNearest = Mathf.Infinity;
                for (int i = 1; i < NodePosition.Length; i++)
                {
                    float Dummydis = Vector2.Distance(NodePosition[i].transform.position, this.transform.position)*2;
                    //float H = Dummydis;

                    float DummytargetPointDis = Vector2.Distance(NodePosition[i].transform.position, player.transform.position);

                    float H = Dummydis + DummytargetPointDis;

                    RaycastHit2D Nothit = Physics2D.Raycast(transform.position, NodePosition[i].transform.position - this.transform.position, Dummydis);
                    if (Nothit.collider != null)
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
                }


               
            }
            DummyTime = 0;
        }
    }

    public bool rotating;

    void walk()
    {

        BackToTheOriginal(); // เดินกลับไปหาเส้นของมัน

        // rotate towards the target
        dir = targetPoint.position - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion dummyRotation = Quaternion.AngleAxis(angle - 90, transform.forward);
        Vector2 dirToTarget = (targetPoint.position - transform.position).normalized;

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

            /// <BackToTheOriginal>
            if (ifNewtargetPoint)
            {
                targetPoint = Old_targetPoint;
                ifNewtargetPoint = false;
            }
            /// </BackToTheOriginal>


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

    void DetectPlayer()
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
    }

    private bool StopMoveing;
    IEnumerator ReturnToPatrol()
    {
        Movewithmouse.isDead = true;
        StopMoveing = true;
        yield return new WaitForSeconds(2.0f);
        StopMoveing = false;
        isDetect = false;
        curPathIndex = 0;
        targetPoint = targetPin[curPathIndex].transform;
        transform.position = targetPin[curPathIndex].transform.position;
    }
}
