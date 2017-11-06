using UnityEngine;
using System.Collections;

// Define a cell of the path.
public class PathCollision : MonoBehaviour
{
	
	#region Fields
	
	// Is this cell part of a path?
	private bool m_isCurrentPathCell = true;
	public bool IsCurrentPathCell {
		get { return m_isCurrentPathCell; }
		set { m_isCurrentPathCell = value; }
	}
	
	#endregion
	
	#region Methods
		
	// Called when a collision happens.
	private void OnCollisionEnter (Collision collision)
	{
		if (collision.transform.tag == "DumbEnemy" && this.m_isCurrentPathCell) {
			GameObject.Find ("PathController").GetComponent<PathController> ().DeletePath (this.gameObject);
		}
	}
	
	// Destroy this game object.
	public void Kill ()
	{
		Destroy (this.gameObject);
	}
	
	#endregion
}
