using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Movewithmouse : MonoBehaviour {
	public float speed = 1.5f;
    public float timer;
	private GameObject joyobj;
	private Joystick joy;
    public static float staticTimer;
    public static bool cantDetect;
	public static bool isDead;
	public static bool isGrilled;
	public static bool isWin;
    public static bool OnUI; //check if on any ui

    public bool bulkUp; //check state of slime (small or big)
	public bool theRealOne;
    public bool GetKey;
	//public static bool Real;
    public bool isControl;

    private Vector3 target;
    //private Vector2 target2d; Dandy: never used
    private Vector2 startPos;

    //private Rigidbody2D rb2d; Dandy: never used

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
    
    //LayerMask targetMask; Dandy: never used

    public Collider2D ColliderInChildren;
    private Collider2D ColliderInThis;

    private GameObject fade;

    private float walkTiming; //walk Sound
    private AudioSource audio;

    void Start () {
        staticTimer = timer;

		target = transform.position;
        startPos = target;

        //rb2d = GetComponent<Rigidbody2D> (); Dandy: never used
        ColliderInThis = GetComponent<Collider2D>();
        isDead = false;
		isWin = false;
        cantDetect = false;
        GetKey = false;
        bulkUp = false;
		isGrilled = false;
        isLeavingWater = false;

        //targetMask = 11; // layer 11 PlayerInWater Dandy: never used
		joyobj = GameObject.Find("JoystickBG");
		joy = joyobj.GetComponent<Joystick> ();
        fade = GameObject.Find("Fade");

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
            
            if (i > PlayerPrefs.GetInt("Level"))
                PlayerPrefs.SetInt("Level", i);
        }

        walkTiming = Time.time;

        audio = gameObject.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            /*if (EventSystem.current.IsPointerOverGameObject())
                OnUI = true;
            else
                OnUI = false;*/
			OnUI = false;
        }

		if (joy.Horizontal() !=0.0f && joy.Vertical() !=0.0f && !isDead && isControl && !OnUI)//Input.GetMouseButton(0) &&
        {
			Vector2 dir = Vector2.zero;
			float angle;
			dir.x = joy.Horizontal ();
			Debug.Log (dir.x);
			dir.y = joy.Vertical ();
			Vector3 dirformove = new Vector3 (transform.position.x + (dir.x*10.0f), transform.position.y + (dir.y*10.0f), transform.position.z);
			//Debug.Log (dirformove);
			angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			//transform.Translate (dir * speed * Time.deltaTime);
			if (Time.timeScale != 0)transform.rotation = Quaternion.AngleAxis(angle-90, transform.forward);

			transform.position = Vector3.MoveTowards(transform.position, dirformove, speed * Time.deltaTime);
			//transform.rotation = rot;

            if (_Animator != null)
                _Animator.SetBool(Ani_Move, true);

        }
        else
        {
            if (_Animator != null)
                _Animator.SetBool(Ani_Move, false);
        }

        if (_Animator.GetBool(Ani_Move))
        {
            if (walkTiming < Time.time)
            {
                if (!audio.isPlaying)
                    audio.Play();
                walkTiming = Time.time + 1.0f;
            }
        }
        else
        {
            audio.Stop();
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
			isGrilled = true;
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

            string s = SceneManager.GetActiveScene().name;
            string[] split = new string[s.Length];

            for (int j = 0; j < s.Length; j++)
            {
                split[j] = s[j].ToString();
            }

            int i;
            
            if (split.Length == 6)
            {
                i = int.Parse(split[5]);
                i++;
                s = split[0] + split[1] + split[2] + split[3] + split[4] + i.ToString();
            }
            else
            {
                i = int.Parse(split[6]);
                i++;

				if (s == "Scene13")
                    s = "Win_UI";
                else
                    s = split[0] + split[1] + split[2] + split[3] + split[4] + split[5] + i.ToString();
            }

            StartCoroutine(loadLevel(s));
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

    IEnumerator loadLevel(string s)
    {
        fade.GetComponent<Animator>().SetBool("Fade", true);
        yield return new WaitUntil(() => fade.GetComponent<Image>().color.a == 1);

        SceneManager.LoadScene(s);
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
        yield return new WaitForSeconds(10f);

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
