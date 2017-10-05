using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour {

    //Floor Type Check
    public static bool onPressurePlate = false;
    public static bool onFireTrap = false;

    //Test door sprite
    public Sprite doorImg;

    //Input map size
    public int xSize;
    public int ySize;

    //Move Speed
    public float speed;

    //Store Position
    private GameObject[] floor;
    private GameObject[] keys;
    private int curPos;
    private int StartPos;
    private string targetPos;
    private string[] splitString;

    //Store Direction
    private float heading;

    //moving Check
    private bool isMove = false;

    //Check lost status
    private bool isLost = false;

    //Pick key check
    private int keyCnt = 0;

	// Use this for initialization
	void Start () {
        //Collect the tile
        floor = new GameObject[xSize * ySize];

        int i = 0;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                floor[i] = GameObject.Find("GroundBlock " + x + " " + y);

                if (transform.position == floor[i].transform.position)
                {
                    curPos = i;
                    StartPos = curPos;
                }

                i++;
            }
        }

        if (keys == null)
        {
            keys = GameObject.FindGameObjectsWithTag("Key");
        }

        keyCnt = keys.Length;

        heading = Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
    }
	
	// Update is called once per frame
	void Update () {
        //Check key to move
        if (!isMove && !isLost)
        {
            splitString = floor[curPos].name.Split(' ');

            if (Input.GetKeyDown(KeyCode.W))
            {//Check tile Position
                if (System.Convert.ToInt32(splitString[2]) < ySize - 1)
                {
                    int i = System.Convert.ToInt32(splitString[2]);
                    i++;
                    splitString[2] = i.ToString();
                }
                //Rotate to
                transform.eulerAngles = new Vector3(0, 0, 0);
                isMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (System.Convert.ToInt32(splitString[2]) > 0)
                {
                    int i = System.Convert.ToInt32(splitString[2]);
                    i--;
                    splitString[2] = i.ToString();
                }

                transform.eulerAngles = new Vector3(0, 0, 180);
                isMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.A)) {
                if (System.Convert.ToInt32(splitString[1]) > 0)
                {
                    int i = System.Convert.ToInt32(splitString[1]);
                    i--;
                    splitString[1] = i.ToString();
                }

                transform.eulerAngles = new Vector3(0, 0, 90);
                isMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (System.Convert.ToInt32(splitString[1]) < xSize - 1)
                {
                    int i = System.Convert.ToInt32(splitString[1]);
                    i++;
                    splitString[1] = i.ToString();
                }

                transform.eulerAngles = new Vector3(0, 0, 270);
                isMove = true;
            }
        }
        else if (isMove)//Move State
        {
            targetPos = splitString[0] + " " + splitString[1] + " " + splitString[2];

            for (int i = 0; i < floor.Length; i++)
            {//Check Floor Type
                if (floor[i].name == targetPos)
                {
                    if (floor[i].tag != "Wall")
                    {
                        curPos = i;
                    }

                    if (floor[i].tag == "PressurePlate")
                    {
                        onPressurePlate = true;
                    }

                    if (floor[i].tag == "FireTrap")
                    {
                        onFireTrap = true;
                        print("You Lost !!!");
                    }

                    if (floor[i].tag == "Door" && keyCnt == 0)
                    {
                        print("You Win !!!");
                    }

                    break;
                }
            }
            //Check collected keys
            for (int j = 0; j < keys.Length; j++)
            {
                if (floor[curPos].transform.position == keys[j].transform.position)
                {
                    if (keys[j].activeSelf)
                    {
                        keyCnt--;

                        if(keyCnt == 0)
                        {
                            GameObject d = GameObject.FindGameObjectWithTag("Door");
                            d.GetComponent<SpriteRenderer>().sprite = doorImg;
                        }
                    }
                    keys[j].SetActive(false);
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, floor[curPos].transform.position,
                                                        speed * Time.deltaTime);
            //Check if finish movement
            if (transform.position == floor[curPos].transform.position)
            {
                isMove = false;
            }
        }
        //Check if step on Fire traps
        if (onFireTrap)
        {
            isLost = true;
            onFireTrap = false;
            StartCoroutine(Respawn());
        }
    }
    //Wait to respawn
    private IEnumerator Respawn ()
    {
        yield return new WaitForSeconds(2.0f);

        transform.position = floor[StartPos].transform.position;
        transform.eulerAngles = new Vector3(0, 0, heading);
        curPos = StartPos;
        isLost = false;
    }
}
