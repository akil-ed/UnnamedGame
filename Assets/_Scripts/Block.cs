using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	public GameObject Trail,Block,Wall;
	bool Test;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AssignTrail(){
		Block.SetActive (false);
		Trail.SetActive (true);
		Wall.SetActive (false);
	}


	public void AssignComplete(){
		Block.SetActive (true);
		Trail.SetActive (false);
		Wall.SetActive (false);
	}

	public void AssignWall(){
		Wall.SetActive (true);
		Trail.SetActive (false);
		Block.SetActive (false);
	}
}
