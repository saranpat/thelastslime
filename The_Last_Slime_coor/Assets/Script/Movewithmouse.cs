using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Movewithmouse : MonoBehaviour {
	public float speed = 1.5f;
    [HideInInspector] public float timer = 15.0f;

    public static bool cantDetect;
	public static bool isDead;
	public static bool isWin;
    public static bool OnUI; //check if on any ui

    public bool bulkUp; //check state of slime (small or big)
	public bool theRealOne;
    public bool GetKey;
	//public static bool Real;
    public bool isControl;

    private Vector3 target;
	private Vector2 target2d;
    private Vector2 startPos;

	private Rigidbody2D rb2d;

    private bool isLeavingWater; // check if out of water
    private bool CheckAgainIfInWater;

    public Animator _Animator;
    public GameObject KeyEff;
    private string Ani_Move = "Move";
    private string Ani_IsCamouflage = "IsCamouflage";
    private string Ani_IsBig = "IsBig";
    private string Ani_Dead = "Dead";

    private Vector2 S_Big = new Vector2(1.4f, 1.4f);
    private Vector2 S_Normal = new Vector2(1.0f, 1.0f);
    
    LayerMask targetMask;

    public Collider2D ColliderInChildren;
    private Collider2D ColliderInThis;

    void Start () {
		target = transform.position;
        startPos = target;

		rb2d = GetComponent<Rigidbody2D> ();
        ColliderInThis = GetComponent<Collider2D>();
        isDead = false;
		isWin = false;
        cantDetect = false;
        GetKey = false;
        bulkUp = false;

        isLeavingWater = false;

        targetMask = 11; // layer 11 PlayerInWater

        if (PlayerPrefs.HasKey("Level"))
        {
            string s = SceneManager.GetActiveScene().name;
            string[] split = new string[s.Length];
           
            for (int j = 0; j < s.Length; j++)
            {
                split[j] = s[j].ToString();
            }

            if (split.Length > 6)
            {
                split[5] = split[5] + split[6];
            }
            
            int i = int.Parse(split[5]);
            print(split[5]);
            if (i > PlayerPrefs.GetInt("Level"))
                PlayerPrefs.SetInt("Level", i);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                OnUI = true;
            else
                OnUI = false;
        }

        if (Input.GetMouseButton(0) && !isDead && isControl && !OnUI)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
            target2d = new Vector2(target.x, target.y);

            var dir = target - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle - 90, transform.forward); //-90 for face toward mouse
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            //rb2d.velocity =target2d.normalized * speed;

            if (_Animator != null)
                _Animator.SetBool(Ani_Move, true);

        }
        else
        {
            if (_Animator != null)
                _Animator.SetBool(Ani_Move, false);
        }
    }

    void Update () {
        /* Dandy: ย้ายไป FixedUpdate เพราะเป็นการ Update position & physic ความเร็ว
         * 
		if (Input.GetMouseButton(0) && !isDead && isControl) {
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target.z = transform.position.z;
			target2d = new Vector2 (target.x, target.y);

            var dir = target - transform.position;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.AngleAxis(angle-90, transform.forward); //-90 for face toward mouse
			transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
			//rb2d.velocity =target2d.normalized * speed;
		}*/
        KeyEff.SetActive(GetKey);

        if (Input.GetMouseButtonDown(1))
        {
            ColliderInThis.enabled = !ColliderInThis.enabled;
            ColliderInChildren.enabled = !ColliderInChildren.enabled;
            Debug.Log("Debug!!! Collider : " + ColliderInThis.enabled);
        }

        if (isDead && theRealOne)
        {
            if(!isinRespawn)
            StartCoroutine(Respawn());
        }

        if (isDead && !theRealOne)
        {
            /*if (_Animator != null)
                _Animator.SetTrigger(Ani_Dead);*/

            //Destroy(this.gameObject,1.5f);
        }



        if (isLeavingWater && theRealOne)
            StartCoroutine(LeavingWater());
        else if (isLeavingWater && !theRealOne)
        {
            if (_Animator != null)
            {
                _Animator.SetTrigger(Ani_IsBig); //กลับร่างเดิมสำหรับตัวปลอม
            }
        }

	}

    public void NotReal_DeadOrTimeUP()
    {
        if (_Animator != null)
            _Animator.SetTrigger(Ani_Dead);
        ColliderInThis.enabled = false;
        ColliderInChildren.enabled = false;
        Destroy(this.gameObject, 0.5f);
    }


    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Fire")
        {
            isDead = true;
        }

        if (other.tag == "Key")
        {
            if (theRealOne && GetKey == false)
            {
                GetKey = true;
                other.gameObject.SetActive(false);
                SoundManager.UnlockedRea = true;
            }
        }

        if (other.tag == "Water")
        {
            cantDetect = true;
            this.gameObject.layer = 11; // layer 11 PlayerInWater
            isLeavingWater = false;
            CheckAgainIfInWater = true;
            StopAllCoroutines();

            GoToNormalSize();
        }

        if (other.tag == "Exit") //Win level door
        {
			isWin = true;
            ColliderInThis.enabled = false;
            ColliderInChildren.enabled = false;
			//use scenecontrolscript
           // Application.LoadLevel(1);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
               CheckAgainIfInWater = false;
               StartCoroutine(DelayGetOffTheWater());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            cantDetect = true;
            this.gameObject.layer = 11; // layer 11 PlayerInWater
            isLeavingWater = false;
            CheckAgainIfInWater = true;

            if (_Animator != null)
                _Animator.SetBool(Ani_IsCamouflage, true);
        }
    }

    bool isinRespawn;

    IEnumerator Respawn()
    {
        
        ColliderInThis.enabled = false;
        ColliderInChildren.enabled = false;


        isinRespawn = true;
        SoundManager.DeadRea = true;
        if (_Animator != null)
            _Animator.SetTrigger(Ani_Dead);
        yield return new WaitForSeconds(5f);

        transform.position = startPos;
        isDead = false;
        isinRespawn = false;
		//use here to execute lose scene

      //  Application.LoadLevel("Lose_UI");
    }

    IEnumerator DelayGetOffTheWater()
    {

        yield return new WaitForSeconds(0.1f);
        if(CheckAgainIfInWater == false)
        {
            isLeavingWater = true;
            cantDetect = false;
            this.gameObject.layer = 10; // layer 10 Player

            if (_Animator != null)
                _Animator.SetBool(Ani_IsCamouflage, false);
        }

    }

    public void GoToNormalSize()
    {
        if (_Animator != null)
        {
            transform.localScale = S_Normal;
        }
    }

    IEnumerator LeavingWater()
    {
        if (isLeavingWater)
        {
            if (_Animator != null)
            {
                _Animator.SetTrigger(Ani_IsBig);
                transform.localScale = S_Big;
            }

            bulkUp = true;
            isLeavingWater = false;

            yield return new WaitForSeconds(5.0f);
            GoToNormalSize();
            bulkUp = false;

        }
    }

}
