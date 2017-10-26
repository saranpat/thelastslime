using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_star_back_to_origin : MonoBehaviour {
	//still need to map tile yourself
	//Prevent overlap tile,didn't need to rescan all tile
	//
	 private char[,] map = new char[,] 
	{   { 'w', 'w', 'g', 'g', 'g', 'w', 'w', 'g', 'g', 'g', 'g', 'g' },
		{ 'w', 'g', 'g', 'g', 'g', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },
		{ 'w', 'g', 'g', 'g', 'g', 'w', 'w', 'g', 'w', 'g', 'w', 'g' },	
		{ 'w', 'g', 'w', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g' },
		{ 'w', 'g', 'g', 'g', 'g', 'w', 'w', 'g', 'w', 'g', 'w', 'g' },
		{ 'w', 'g', 'g', 'g', 'g', 'w', 'w', 'g', 'g', 'w', 'w', 'w' },
		{ 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'g', 'w', 'g', 'g', 'g' },
		{ 'w', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'w', 'w', 'g', 'g' },
		{ 'w', 'g', 'w', 'w', 'g', 'w', 'w', 'g', 'g', 'w', 'g', 'g' },
		{ 'w', 'g', 'w', 'g', 'g', 'g', 'w', 'g', 'g', 'w', 'g', 'g' },
		{ 'g', 'g', 'g', 'g', 'w', 'g', 'g', 'g', 'g', 'w', 'g', 'g' },
		{ 'w', 'g', 'w', 'g', 'g', 'g', 'w', 'g', 'w', 'w', 'g', 'g' },
		{ 'w', 'g', 'w', 'w', 'g', 'w', 'w', 'g', 'w', 'g', 'g', 'g' },
		{ 'w', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'w', 'g', 'g', 'g' },
		{ 'w', 'w', 'g', 'w', 'g', 'w', 'g', 'w', 'w', 'g', 'g', 'g' },
		{ 'w', 'w', 'g', 'w', 'g', 'w', 'g', 'w', 'w', 'g', 'g', 'g' },
	};
	//g : ground
	//w : wall
	//m : moveable ,use in recent_map



	//for destination offset ,
	private float offsetx=-6.0f;
	private float offsety=-8.0f;

/*	public GameObject[] moveable;
	public float Length { get { return moveable.Length; } }*/

	//orgigin destination
	//use real world destination,if use on map array data,plus offset
	private Vector2 origin_destination ;


	// Use this for initialization
	void Start () {
		origin_destination= new Vector2 (-2.0f,7.0f); 

		//for update moveable unit
		char[,] recent_map =(char[,]) map.Clone();
	}
	
	// Update is called once per frame
	void Update () {
		float posx = Mathf.Round(transform.position.x);
		float posy = Mathf.Round(transform.position.y);


		//not on origin position
		//if (origin_destination != new Vector2 (posx,posy) 
			
		//go to round pos first
			//add here later



		   
		}

}
			
