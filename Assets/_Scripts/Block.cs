using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	public GameObject Trail,Block;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AssignTrail(){
		Block.SetActive (false);
		Trail.SetActive (true);
	}


	public void AssignComplete(){
		Block.SetActive (true);
		Trail.SetActive (false);
	}
}
