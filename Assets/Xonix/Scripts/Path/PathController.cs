using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Define all paths logics
public class PathController : MonoBehaviour
{
	
	#region Fields
	
	// There is an open path?
	private bool m_openPath = false;

	// A dictionary to store <path number, <path cells, path game object>>.
	private List<KeyValuePair<GridCell, GameObject>> m_cellsOfPath;
	
	// The game grid.
	private GridRenderer m_gridRenderer;
	
	// The player movement controller.
	private GridMove m_playerGridMove;
	
	// The player controller.
	private PlayerController m_playerController;
	
	// The GUIController. 
	GUIController m_guiController;
	
	#endregion
	
	#region Methods
	
	void Awake ()
	{
		m_cellsOfPath = new List<KeyValuePair<GridCell, GameObject>> ();
	}
	
	void Start ()
	{
		m_guiController = GameObject.Find ("GUIController").GetComponent<GUIController> ();
		m_gridRenderer = GameObject.Find ("GridRenderer").GetComponent<GridRenderer> ();
		m_playerGridMove = GameObject.Find ("Player").GetComponent<GridMove> ();
		m_playerController = GameObject.Find ("Player").GetComponent<PlayerController> ();
	}
	
	// Add a cell to the current path.
	public void AddPathCell (GridCell cell)
	{
		bool repeatedCell = false;
		int i;
		
		// Verify it the cell is already in the path.
		for (i = 0; i < m_cellsOfPath.Count; i++) {
			KeyValuePair<GridCell, GameObject> item = m_cellsOfPath [i];
			if (item.Key == cell) {
				repeatedCell = true;
				break;
			}	
		}
		// If it is, it's necessary to remove it from the path and put it again, so it will be at the rigth order.
		if (repeatedCell) {
			m_cellsOfPath [i].Value.GetComponent<PathCollision> ().Kill ();
			m_cellsOfPath.RemoveAt (i);
		}
	
		m_openPath = true;
		cell.CurrentPath = true;
		Transform go = m_gridRenderer.CreatePathCell (cell);
		go.name = "Cell"+(i+1);
		KeyValuePair<GridCell, GameObject> goCell = new KeyValuePair<GridCell, GameObject> (cell, go.gameObject);
		m_cellsOfPath.Add (goCell);	
	}
	
	// Close a path, setting it as already covered.
	public void ClosePath ()
	{
		m_openPath = false;
		for (int i = 0; i < m_cellsOfPath.Count; i++) {
			KeyValuePair<GridCell, GameObject> cell = m_cellsOfPath [i];
			cell.Value.GetComponent<PathCollision> ().IsCurrentPathCell = false;
		}
		m_cellsOfPath.Clear ();
		m_gridRenderer.RefreshGrid ();
		
		m_guiController.VerifyPercentageOfGridCovered ();
	}
	
	// Delete the current path.
	public void DeletePath (GameObject collisionPoint)
	{
		/* Sometimes an enemy can collide with two path cells at the same time. In this case,
		 * when calling this method with the second cell, this may not be at the path anymore (it
		 * has already been destroyed). So, this has to be take into accounting, in order to the
		 * method work properly and provide the expected result (removing all cells until the cell
		 * at the collision point, and just these cells). Then, its necessary to verify if 
		 * the cell that we want to destroy is still in the path. If we don't do that, all cells will
		 * be destroyed, 'cause we will not find this cell.
		 */ 	
		bool cellAlreadyDestroyed = true;
		for (int i = 0; i < m_cellsOfPath.Count; i++) {
			KeyValuePair<GridCell, GameObject> item = m_cellsOfPath [i];
			if (item.Value == collisionPoint) {
				cellAlreadyDestroyed = false;
				break;
			}	
		}
		
		// If the cell is already in the path, we can delete all cells until it.
		if (!cellAlreadyDestroyed)
		{
			if (m_openPath && m_cellsOfPath.Count > 0) {
				int i = 0;
				KeyValuePair<GridCell, GameObject> cell = m_cellsOfPath [i];
				// Remove each cell until we get at the collision point.
				do {
					if (cell.Value != null) {
						cell.Value.GetComponent<PathCollision> ().Kill ();
						cell.Key.CurrentPath = false;
						cell.Key.IsDraw = false;
						if (cell.Key.Location.x == m_playerGridMove.CharacterLocation.x &&
							cell.Key.Location.y == m_playerGridMove.CharacterLocation.y) {
							m_playerController.LostLife ();
							//m_playerGridMove.ResetPosition ();
						}
						//cell.Key.Kill = true;
					}
					cell = m_cellsOfPath [i++];
				} 
				while (cell.Value != collisionPoint && i < m_cellsOfPath.Count);
				//Debug.Log(collisionPoint.transform.name+" "+cell.Value.gameObject.name);
				//cell.Key.Collided = true;
				m_cellsOfPath.RemoveRange (0, i - 1);
				StartCoroutine (Fade ());
			}
		}
		//StartCoroutine(ForceDestroy ());
		/*else
		{
			Debug.Log("Not here anymore");
		}*/
	}
		
	// Fade the remanescents cells.
	IEnumerator Fade ()
	{
		while (m_cellsOfPath.Count > 0) {
			KeyValuePair<GridCell, GameObject> cell = m_cellsOfPath [0];
			if (cell.Value != null) {
				if (cell.Key.Location.x == m_playerGridMove.CharacterLocation.x &&
					cell.Key.Location.y == m_playerGridMove.CharacterLocation.y) {
					m_playerController.LostLife ();
					//m_playerGridMove.ResetPosition ();
				}
				cell.Value.GetComponent<PathCollision> ().Kill ();
				cell.Key.CurrentPath = false;
				cell.Key.IsDraw = false;
			}
			m_cellsOfPath.RemoveAt (0);
			yield return new WaitForSeconds(.05f);
		}

	//	if (m_cellsOfPath.Count == 0) {

	//	}
	}

	IEnumerator ForceDestroy(){
		foreach (Transform T in GridRenderer.instance.TrailList) {
			if(T)
				T.GetComponent<PathCollision> ().Kill ();
			print ("inside destriy");
			yield return new WaitForSeconds(.02f);
		}
		GridRenderer.instance.TrailList.Clear ();
		m_playerController.LostLife ();
	}
	
	#endregion
}
