using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleDown : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.DOScale (new Vector3 (0.4f, 0.4f, 0.4f), 0.6f).SetEase (Ease.OutBounce);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
