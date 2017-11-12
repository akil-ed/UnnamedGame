using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {
	public Transform target;
	public Vector3 velocity;
	public float smoothTime,maxSpeed;
	// Use this for initialization
	void Start () {
		transform.position = target.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//transform.position = Vector3.SmoothDamp (transform.position, target.position, ref velocity,smoothTime,maxSpeed);
		transform.position = Vector3.MoveTowards (transform.position, target.position, maxSpeed * Time.deltaTime);
	}
}
