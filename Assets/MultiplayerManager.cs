using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {

	public Transform spawnpoint;

	// Use this for initialization
	void Start () {
		if(PhotonNetwork.isMasterClient)
			PhotonNetwork.Instantiate ("player 1",spawnpoint.position,spawnpoint.rotation,new byte());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
