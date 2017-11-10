using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
	public PlanarRealtimeReflection Reflection;
	public BloomPro Glow;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToggleGlow(){
		if (Glow.isActiveAndEnabled)
			Glow.enabled = false;
		else
			Glow.enabled = true;
	}

	public void ToggleReflections(){
		if (Reflection.isActiveAndEnabled)
			Reflection.enabled = false;
		else
			Reflection.enabled = true;
	}
}
