using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardScript : MonoBehaviour {

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

    private bool isDetect_ButWall;
    private float TimeDetect;

    // Use this for initialization
    void Start()
    {
        _AI_GetNode = GameObject.FindGameObjectWithTag("Node").GetComponent<AI_GetNode>();
        curPathIndex = 0;

        isDetect = false;
        plus = true;
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

        DetectPlayer();

        if (isDetect)
        {
            //// เพิ่ม หลบ กำแพง
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 0.5f);
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Fire")
                {
                    isDetect_ButWall = true;
                }
                else
                {
                    isDetect_ButWall = false;
                }

            }
            else
            {
                isDetect_ButWall = false;
            }

            if (isDetect_ButWall)
            {
                Debug.Log("Wall");
                dir = player.transform.position - transform.position;
                //angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                //angle += Time.deltaTime * speed;
                Quaternion dummyQ = Quaternion.AngleAxis(angle, transform.forward);


                transform.rotation = Quaternion.Lerp(transform.rotation, dummyQ, 0.05f);
                transform.Translate(0, speed * Time.deltaTime, 0);
                Debug.DrawRay(transform.position, transform.up, Color.white);

            } ////////////////////////////////////////////////////////////////////////////////////////////////////// End หลบ กำแพง
            else if (Vector2.Distance(transform.position, player.transform.position) <= dist
                && Vector2.Distance(transform.position, player.transform.position) > 0.7f)
            {
                dir = player.transform.position - transform.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                //แก้ไข transform.rotation = Quaternion.AngleAxis(angle - 90, transform.forward); 
                Quaternion dummyQ = Quaternion.AngleAxis(angle - 90, transform.forward); //เป็น Lerp แทนจ
                transform.rotation = Quaternion.Lerp(transform.rotation, dummyQ, 0.05f);
                //

                if (ifNewtargetPoint)
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                }


            }
            else
            {
                StartCoroutine(ReturnToPatrol());
            }

            TimeDetect += Time.deltaTime;
            if (TimeDetect > 10 || Vector2.Distance(transform.position, player.transform.position) > dist + 1)
            {
                isDetect = false;
                isDetect_ButWall = false;
                TimeDetect = 0;
            }

        }
    }

    void BackToTheOriginal()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPoint.position - transform.position, 0.5f); //ชนกำแพงมากว่ากี่วิ
        Debug.DrawRay(transform.position, targetPoint.position - transform.position, Color.white);

        if (hit.collider != null && ifNewtargetPoint == false)
        {
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
            disNearest = Vector2.Distance(NodePosition[1].transform.position, this.transform.position)
                       + Vector2.Distance(targetPoint.position, this.transform.position);
            Transform Dummy = NodePosition[1];
            for (int i = 1; i < NodePosition.Length; i++)
            {
                float Dummydis = Vector2.Distance(NodePosition[i].transform.position, this.transform.position);
                float DummytargetPointDis = Vector2.Distance(targetPoint.position, this.transform.position);
                float H = Dummydis + DummytargetPointDis;

                RaycastHit2D Nothit = Physics2D.Raycast(transform.position, NodePosition[i].transform.position - this.transform.position, Dummydis);
                if (i == Old_DummytargetPoint)
                {
                    Old_DummytargetPoint = 0;
                    continue;
                }
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
                            disNearest = Dummydis;
                            Dummy = NodePosition[i];
                            Old_DummytargetPoint = i;
                        }
                    }
                }
                else
                {
                    if (H < disNearest)
                    {
                        disNearest = Dummydis;
                        Dummy = NodePosition[i];
                        Old_DummytargetPoint = i;
                    }
                }
            }
            if (targetPoint != Dummy)
            {
                ifNewtargetPoint = true;
                targetPoint = Dummy;
            }
            DummyTime = 0;
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
                isDetect = true;
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
        if (hit2.collider != null)
        {
            if (hit2.collider.tag == "Player" && !Movewithmouse.cantDetect)
            {
                isDetect = true;
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
        if (hit3.collider != null)
        {
            if (hit3.collider.tag == "Player" && !Movewithmouse.cantDetect)
            {
                isDetect = true;
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
    }

    IEnumerator ReturnToPatrol()
    {
        Movewithmouse.isDead = true;

        yield return new WaitForSeconds(2.0f);

        isDetect = false;
        curPathIndex = 0;
        targetPoint = targetPin[curPathIndex].transform;
        transform.position = targetPin[curPathIndex].transform.position;
    }
}
