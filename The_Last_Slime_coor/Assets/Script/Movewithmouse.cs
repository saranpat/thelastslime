using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movewithmouse : MonoBehaviour {
	public float speed = 1.5f;
    public Sprite normalSlime, camouflage, bigSlime;

    public static bool cantDetect;
    public static bool isDead;
    public static bool OnUI; //check if on any ui

    public bool bulkUp; //check state of slime (small or big)
    public bool theRealOne;
    public bool isControl;

    private Vector3 target;
	private Vector2 target2d;
    private Vector2 startPos;

	private Rigidbody2D rb2d;

    private bool isLeavingWater; // check if out of water
    private bool CheckAgainIfInWater;

    private SpriteRenderer spriteRenderer;
    private Color camouflageAlpha, normalColor;

    LayerMask targetMask;

    void Start () {
		target = transform.position;
        startPos = target;

		rb2d = GetComponent<Rigidbody2D> ();

        isDead = false;
        cantDetect = false;

        bulkUp = false;

        isLeavingWater = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
        camouflageAlpha = normalColor;
        camouflageAlpha.a = 0.6f;

        targetMask = 11; // layer 11 PlayerInWater
    }

    private void FixedUpdate()
    {
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

        if (isDead && theRealOne)
            StartCoroutine(Respawn());

        if (isDead && !theRealOne)
            Destroy(this.gameObject);

        if (isLeavingWater && theRealOne)
            StartCoroutine(LeavingWater());

        if (!bulkUp && !cantDetect) //Return to normal size while not swimming
        {
            //gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Slime");
            spriteRenderer.sprite = normalSlime;
            //transform.localScale = new Vector2(0.25f, 0.25f);
            //transform.localScale = new Vector2(0.4f, 0.4f);
        }
	}

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Fire")
        {
            isDead = true;
        }

        if (other.tag == "Key")
        {
            other.gameObject.SetActive(false);
        }

        if (other.tag == "Water")
        {
            cantDetect = true;
            this.gameObject.layer = 11; // layer 11 PlayerInWater
            isLeavingWater = false;
            CheckAgainIfInWater = true;
            StopAllCoroutines();
            spriteRenderer.sprite = camouflage;
            spriteRenderer.color = camouflageAlpha;
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
            spriteRenderer.sprite = camouflage;
            spriteRenderer.color = camouflageAlpha;
        }
    }

    public void IsOverUI()
    {
        OnUI = true;
    }

    public void OutOfUI()
    {
        OnUI = false;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        
        transform.position = startPos;
        isDead = false;
        //Application.LoadLevel();
    }

    IEnumerator DelayGetOffTheWater()
    {

        yield return new WaitForSeconds(0.05f);
        if(CheckAgainIfInWater == false)
        {
            isLeavingWater = true;
            cantDetect = false;
            this.gameObject.layer = 10; // layer 10 Player
            spriteRenderer.color = normalColor;
        }

    }

    IEnumerator LeavingWater()
    {
        //gameObject.GetComponent<CircleCollider2D>().isTrigger = false;

        //yield return new WaitForSeconds(0.1f);

        if (isLeavingWater)
        {
            //cantDetect = false;
            bulkUp = true;
            //gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/big_slime");
            spriteRenderer.sprite = bigSlime;
            //transform.localScale = new Vector2(0.55f, 0.55f);
            isLeavingWater = false;

            yield return new WaitForSeconds(5.0f);

            bulkUp = false;
        }
    }

}
