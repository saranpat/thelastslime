using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Joystick : MonoBehaviour,IDragHandler,IPointerUpHandler,IPointerDownHandler {

	private Image bgImg;
	private Image joyimg;
	private Vector2 inputvec;
	private void Start()
	{
		bgImg = GetComponent<Image> ();
		joyimg = transform.GetChild (0).GetComponent<Image> ();
	}
	public virtual void OnDrag (PointerEventData ped)
	{
		Vector2 pos;
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform,ped.position,ped.pressEventCamera,out pos))
			{
			pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
			pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

			inputvec = new Vector2 (pos.x * 2 + 1, pos.y * 2 - 1);
			inputvec = (inputvec.magnitude > 1.0f) ? inputvec.normalized : inputvec;

			//move joystick img 
			joyimg.rectTransform.anchoredPosition = new Vector2 (inputvec.x * (bgImg.rectTransform.sizeDelta.x/3),inputvec.y*(bgImg.rectTransform.sizeDelta.y/3));
			Debug.Log (inputvec);
			}
	}
	public virtual void OnPointerDown (PointerEventData ped)
	{
		OnDrag (ped);
	}
	public virtual void OnPointerUp (PointerEventData ped)
	{
		inputvec = Vector2.zero;
		joyimg.rectTransform.anchoredPosition = Vector2.zero;
	}
	public float Horizontal()
	{
		if (inputvec.x != 0)
			return inputvec.x;
		else
			return Input.GetAxis ("Horizontal");
	}
	public float Vertical()
	{
		if (inputvec.y != 0)
			return inputvec.y;
		else
			return Input.GetAxis ("Vertical");
	}
}

/*
target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
target.z = transform.position.z;
target2d = new Vector2(target.x, target.y);

var dir = target - transform.position;
var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

transform.rotation = Quaternion.AngleAxis(angle - 90, transform.forward); //-90 for face toward mouse
transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);*/