using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardScript : MonoBehaviour {

    public bool alertState; //เตือนจาก ward ถ้าผู้เล่นเข้าใกล้
    public GameObject alertState_Obj; //รับตำแหน่งของ ward มาเพื่อให้บอทเดินไปดูตรง Ward นั้นๆ

    public GameObject firePrefab; // magic shot prefab
    public float fireSpeed; //magic shot speed

    public GameObject[] targetPin;
    public float speed;
    public float dist; // distance the enemy can "see" in front of him
    public float visionAngle;
    public float fireRange; // shooting distance
    public bool isLoop;

    private int curPathIndex;
    private Transform targetPoint;
    private Vector2 dir;
    private float angle;
    private bool plus;

    private bool isDetect;
    private GameObject player;
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

    private bool isRecharge; //recharge magic

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _AI_GetNode = GameObject.FindGameObjectWithTag("Node").GetComponent<AI_GetNode>();
        curPathIndex = 0;

        isDetect = false;
        plus = true;

        alertState = false;

        isRecharge = false;
    }


    public void Set_alertState(GameObject WardTransfrom)
    {
        if(alertState == false)
        {
            alertState_Obj = WardTransfrom;
            targetPoint = WardTransfrom.transform;
            alertState = true;
        }
    }

    void AI_Chase()
    {
        // rotate towards the target
        dir = targetPoint.position - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle - 90, transform.forward);
        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (transform.position == targetPoint.position)
        {
            if (ifNewtargetPoint)
            {
                targetPoint = player.transform;
                ifNewtargetPoint = false;
            }
        }

        BackToTheOriginal_AIGoToPlayer();

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
        if (curPathIndex < targetPin.Length && !isDetect && !alertState)
        {
            if (targetPoint == null)
                targetPoint = targetPin[curPathIndex].transform;
            walk();
        }

        if (alertState && !isDetect)
        {
            //pathfinding to ward
            walkToWard();
            
        }

        DetectPlayer();

        if (isDetect)
        {
            if (Movewithmouse.isDead)
            {
                StartCoroutine(ReturnToPatrol());
            }

            if (Vector2.Distance(transform.position, player.transform.position) <= fireRange && !isRecharge)
            {
                dir = targetPoint.position - transform.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.AngleAxis(angle - 90, transform.forward);

                StartCoroutine(Fire());
            }

            if (Vector2.Distance(transform.position, player.transform.position) <= dist
                            && Vector2.Distance(transform.position, player.transform.position) >= fireRange - 1)
            {
                if (StopMoveing == false)
                {
                    AI_Chase();
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

            if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire")
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
            Transform Dummy = NodePosition[1];
            int DummyOldPoint = 0;

            for (int i = 1; i < NodePosition.Length; i++)
            {
                Debug.Log(Old_DummytargetPoint);
                if (i == Old_DummytargetPoint)
                {
                    Debug.Log("OldPoint");
                    Old_DummytargetPoint = 0;
                    continue;
                }

                float Dummydis = Vector2.Distance(NodePosition[i].transform.position, this.transform.position);
                float DummytargetPointDis = Vector2.Distance(targetPoint.position, NodePosition[i].transform.position);
                float H = Dummydis + DummytargetPointDis;

                RaycastHit2D Nothit = Physics2D.Raycast(transform.position, NodePosition[i].transform.position - this.transform.position, Dummydis);
                
                if (Nothit.collider != null)
                {
                    if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Fire")
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
            if (targetPoint != Dummy)
            {
                ifNewtargetPoint = true;
                targetPoint = Dummy;
                Old_DummytargetPoint = DummyOldPoint;
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
        

        if (hit.collider != null&& ifNewtargetPoint == false) //
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire")
            {
                DummyTime += Time.deltaTime;
            }

        }
        else if (hit.collider != null && targetPoint.position == player.transform.position)
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire")
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
            Transform Dummy = NodePosition[1];
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
                if(I_hit_wall.collider != null)
                {
                    if (I_hit_wall.collider.tag == "Wall" || I_hit_wall.collider.tag == "Fire")
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
                    if (Nothit.collider.tag == "Wall" || Nothit.collider.tag == "Fire")
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
            if (targetPoint != Dummy)
            {
                ifNewtargetPoint = true;
                targetPoint = Dummy;
                Old_DummytargetPoint = DummyOldPoint;
            }
            DummyTime = 0f;
        }
    }

    void walk()
    {

        BackToTheOriginal(); // เดินกลับไปหาเส้นของมัน

        // rotate towards the target
        dir = targetPoint.position - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle - 90, transform.forward);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

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

    void walkToWard()
    {

        BackToTheOriginal(); // เดินกลับไปหาเส้นของมัน

        // rotate towards the target
        dir = targetPoint.position - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle - 90, transform.forward);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (transform.position == alertState_Obj.transform.position)
        {
            if (alertState_Obj.GetComponent<WardScript>().Get_isAlarm())
            {
                ifNewtargetPoint = true;
                isDetect = true;
                targetPoint = player.transform;
                alertState = false;
            }
            else
            {
                alertState = false;
                targetPoint = targetPin[curPathIndex].transform;
            }
            
        }
        else if (transform.position == targetPoint.position)
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

            if(alertState == false)
            {
                targetPoint = targetPin[curPathIndex].transform;
            }
            else
            {
                targetPoint = alertState_Obj.transform;
            }
            
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
            isDetect = false;

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
        StopMoveing = true;
        yield return new WaitForSeconds(2.0f);
        StopMoveing = false;
        isDetect = false;
        curPathIndex = 0;
        targetPoint = targetPin[curPathIndex].transform;
        transform.position = targetPin[curPathIndex].transform.position;
    }

    IEnumerator Fire()
    {
        isRecharge = true;

        var bullet = (GameObject)Instantiate(firePrefab, transform.position, transform.rotation);

        bullet.GetComponent<Rigidbody2D>().velocity = dir * fireSpeed;

        Destroy(bullet, 2.0f);

        yield return new WaitForSeconds(2.0f);

        isRecharge = false;
    }
}
