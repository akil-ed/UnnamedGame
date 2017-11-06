using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonHelper : MonoBehaviour 
{
	public static PhotonHelper instance;

	void Start()
	{
		instance = this;
		PhotonNetwork.ConnectUsingSettings (Application.version);
	}


	void OnConnectedToMaster()
	{
		print ("Connected to server");
		CreateOrJoinAGame ();
	}

	public void CreateOrJoinAGame()
	{
		PhotonNetwork.JoinRandomRoom ();
	}

	void OnPhotonRandomJoinFailed()
	{
		print ("Join random room failed!");
		PhotonNetwork.CreateRoom (null,new RoomOptions(){MaxPlayers = 2},null);
	}

	void OnCreatedRoom()
	{
		print ("room created!");
	}

	void OnJoinedRoom()
	{
		print ("room joined!");
		StartCoroutine (WaitForPlayers());
		//PhotonNetwork.Instantiate ("player",Vector3.zero,Quaternion.identity,new byte());
	}

	void OnConnectionFail(DisconnectCause cause)
	{
		print (cause.ToString());
	}

	IEnumerator WaitForPlayers()
	{
		while (PhotonNetwork.playerList.Length != PhotonNetwork.room.MaxPlayers)
			yield return null;
		SceneManager.LoadScene ("Main_2");
	}

}
