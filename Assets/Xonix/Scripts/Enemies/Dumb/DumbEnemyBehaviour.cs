using UnityEngine;
using System.Collections;

public class DumbEnemyBehaviour : MonoBehaviour {
	
	#region Fields
	
	AudioSource m_audioSource;
	
	// The speed to add.
	private float m_speed = 4f;
	public float Speed 
	{
		get { return m_speed; }
		set { m_speed = value; }
	}	
	
	#endregion
	
	#region Methods
	
	void Awake ()
	{
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
	}
	
	void Start () 
	{
		// Chosing a direction randomly
		float x = Random.Range(-1f, 1f);
		float z = Random.Range(-1f, 1f);
		Vector3 direction = new Vector3(x, 0f, z);
			
		this.gameObject.GetComponent<Rigidbody>().velocity = direction * m_speed;
	}
	
	void Update () 
	{
		this.gameObject.GetComponent<Rigidbody>().velocity = m_speed * this.gameObject.GetComponent<Rigidbody>().velocity.normalized; 
	}
	
	private void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Wall")
		{
			m_audioSource.Play ();
		}
	}
	
	#endregion
	
}
