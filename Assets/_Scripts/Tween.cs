using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tween : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PerformScale(Vector3 size,float time,Ease EaseType){
		transform.DOScale (size, time).SetEase (EaseType);
	}
}
