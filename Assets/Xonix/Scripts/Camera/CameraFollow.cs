using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	// The distance that the camera will keep from the target.
	Vector3 distanceFromTarget = new Vector3 (0f, 13f, -10f);
	
	// The target.
	[SerializeField]
	Transform m_target;
	
	// A thereshold to define when the camera has to move.
	int m_threshold = 10;
	
	// The time to accomplish the movement.
	float m_journeyTime = 12.0F;
	
	// The time when the camera starts to follow the target.
	float m_startTime;
	
	// The camera is already following the target?
	bool m_following = false;
	
	void Start ()
	{ 
		// First let's start with our Camera centered on the player. 
		transform.position = m_target.position + distanceFromTarget;
	}
	
	void Update ()
	{
		float dist = Vector3.Distance (this.transform.position, m_target.transform.position);
		
		// If distance exceeds the treshold then lerp our focal point to new location.
		if (dist >= m_threshold && !m_following) {
			StartCoroutine (FollowPlayer ());
		}	
	}
	
	private IEnumerator FollowPlayer () 
	{
		m_following = true;
		m_startTime = Time.time;
		float fracComplete;
		float dist;
		do
		{
			fracComplete = (Time.time - m_startTime) / m_journeyTime;
			dist = Vector3.Distance (m_target.position, this.transform.position);
			this.transform.position = Vector3.Lerp (this.transform.position, m_target.position + distanceFromTarget, fracComplete);
			yield return new WaitForEndOfFrame();
		}
		while (dist > 0.1f);	
		m_following = false;
	}
}
