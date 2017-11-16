using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {

	public Transform spawnpoint;
	public string playerName;

	// Use this for initialization
	void Start () {
		if(PhotonNetwork.isMasterClient)
			PhotonNetwork.Instantiate (playerName,spawnpoint.position,spawnpoint.rotation,new byte());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
