using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_generate : MonoBehaviour {

	public GameObject wallfab;
	public GameObject groundfab;
	public GameObject slimefab;
	//prefab for each tile
	private int Xtile = 5;
	private int Ytile = 5;
	public char[,]tile = new char[,]
	   {{'w','w','w','w','w'},
		{'w','g','g','g','w'},
		{'w','g','g','g','w'},
		{'w','g','g','g','w'},
		{'w','w','w','w','w'},} ;

	//tile data

	void Start() {
		for (int i = 0; i < Ytile; i++) {
			for (int j = 0; j < Xtile; j++) {
				Vector3 pos = new Vector3 (j, i, 0.0f);
				//check what tile in it
				if(tile[i,j]=='w')Instantiate(wallfab, pos, Quaternion.identity);
				else if(tile[i,j]=='g')Instantiate(groundfab, pos, Quaternion.identity);
			}
		}
		//put slime
		Instantiate(slimefab, new Vector3 (1.0f, 2.0f, 0.0f), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
