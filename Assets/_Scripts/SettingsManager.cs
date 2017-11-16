using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
	public PlanarRealtimeReflection Reflection;
	public BloomPro Glow;
	public static SettingsManager instance;
	public bool multiplayer;
	// Use this for initialization
	void Start () {
		instance = this;
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
