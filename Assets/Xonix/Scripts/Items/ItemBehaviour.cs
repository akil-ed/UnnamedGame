using UnityEngine;
using System.Collections;
using System;

public class ItemBehaviour : MonoBehaviour {
	
	#region Fields
	
	// The life time of an item.
	[SerializeField]
	private float m_lifeTime = 8f;
	
	// The rotation speed.
	private float m_rotationSpeed = 30f;
	
	// The time when an item was created.
	private float m_startTime;
	
	// It is already time to destroy an item?
	private bool m_destroy = false;
	
	// Parameters to fade an item.
	Color m_colorStart;
	Color m_colorEnd;
	
	// The game grid.
	private Grid<GridCell> m_gridMap;
	
	private bool m_onFloor;
	public bool OnFloor {
		set { m_onFloor = value; }
		get {return m_onFloor; }
	}
	
	#endregion
	
	#region Methods
	
	private void Start ()
	{
		m_startTime = Time.time;
					
		m_colorStart = this.gameObject.GetComponentInChildren<MeshRenderer>().material.color;
  		m_colorEnd = new Color (m_colorStart.r, m_colorStart.g, m_colorStart.b, 0.0f);
		
		m_gridMap = GameObject.Find ("GridBuilder").GetComponent<GridBuilder> ().GridMap;
	}
	
	private void Update () 
	{
        transform.RotateAround(transform.position, Vector3.up, m_rotationSpeed * Time.deltaTime);
		
		if (Time.time - m_startTime > m_lifeTime && !m_destroy)
		{
			m_destroy = true;
			StartCoroutine (Kill ());
		}
		
		GridLocation ItemLocation = new GridLocation((int) Math.Round(this.transform.position.x, MidpointRounding.ToEven), 
													 (int) Math.Round(this.transform.position.z, MidpointRounding.ToEven));
		
		if (m_onFloor && m_gridMap.GetCellAt (ItemLocation).IsCovered)
			Destroy (this.gameObject);
		
    }
	
	public IEnumerator Kill ()
	{
		this.gameObject.GetComponentInChildren<ParticleEmitter> ().Emit ();
		this.gameObject.GetComponent<AudioSource> ().Play ();
		float wait = 0.0f;
		while (this.gameObject && wait < .5f)
		{
            wait += Time.deltaTime;
			this.gameObject.GetComponentInChildren<MeshRenderer> ().material.color = Color.Lerp (m_colorStart, m_colorEnd, wait);
			yield return new WaitForEndOfFrame ();
		}
		Destroy (this.gameObject);
	}
	
	void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "DumbEnemy")
			StartCoroutine(Kill ());
	}
	
	#endregion
}
