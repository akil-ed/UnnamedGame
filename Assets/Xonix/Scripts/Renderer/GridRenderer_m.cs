using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;
//Render the objects in Debug MOde
public class GridRenderer_m : MonoBehaviour {

	#region Fields
	
	// The wall prefab.
	[SerializeField]
	private Transform m_wall;
	
	// The floor prefab.
	[SerializeField]
	private Transform m_floor;
	
	// The cell centroid for each cell added to the debug render.
	public List<GridCell> m_cells = new List<GridCell>();
	
	// An empty game object to store all game objects representing cells.
	private GameObject m_gridComponents;
	
	// The game grid.
	private Grid<GridCell> m_gridMap;
	
	// The number of rendered cells.
	private int m_numberOfRenderedCells = 0;
	
	// The total number of cells.
	private int m_numberOfGridCells = 0;

	public List<Transform> TrailList,CompletedList;
		
	#endregion
	
	#region Methods
	
	// Adds the specified cell to debug drawing.
	public void AddCell (GridCell cell)
	{
		m_cells.Add (cell);
	}
	public static GridRenderer_m instance;
	void OnEnable(){
		instance = this;
	}

	void OnDisable(){
		instance = null;
	}
	
	private void Start () 
	{
		m_gridMap = GameObject.Find ("GridBuilder").GetComponent<GridBuilder_m> ().GridMap;
		m_numberOfGridCells = m_gridMap.Height * m_gridMap.Width;
		
		m_gridComponents = new GameObject ();
		m_gridComponents.name = "GridComponents";
		
		// ****************************** TO DO!!! ******************************
		// Calculate the y position dinamically
		Transform f = PhotonNetwork.Instantiate("floor", new Vector3(m_gridMap.Width/2-0.5f, -.65f, m_gridMap.Height/2-0.5f), Quaternion.identity,new byte()).transform;
		f.transform.localScale = new Vector3 (m_gridMap.Width, .3f, m_gridMap.Height);
		f.parent = m_gridComponents.transform;
		f.name = "Floor";
		
		RefreshGrid();
		PathController_m.instance.InitPlayer (this);
	}
	
	// Instantiate a new path cell.
	public Transform CreatePathCell (GridCell cell)
	{
		Vector3 centroid = new Vector3 (cell.Location.x, 0f, cell.Location.y);
		Transform w = PhotonNetwork.Instantiate ("wall", centroid, Quaternion.identity,new byte()).transform;
		w.name = "Wall"+cell.Location.x.ToString()+"-"+cell.Location.y.ToString();
		w.parent = m_gridComponents.transform;

		//w.localScale = Vector3.zero;
		w.DOScale (new Vector3 (0.4f, 0.4f, 0.4f), 0.6f).SetEase (Ease.OutBounce);
		TrailList.Add (w);

		w.gameObject.AddComponent<PathCollision>();
		cell.IsDraw = true;
		m_numberOfRenderedCells++;
		return w;
	}
	
	// Refresh the rendered grid.
	bool isFirst=true;
	public void RefreshGrid () 
	{
		if (PhotonNetwork.isMasterClient) 
		{
			foreach (GridCell cell in m_cells)
			{
				Vector3 centroid = new Vector3 (cell.Location.x, 0f, cell.Location.y);

				if (cell.IsCovered && !cell.IsDraw)
				{
					Transform w = PhotonNetwork.Instantiate ("wall", centroid, Quaternion.identity,new byte()).transform;
					w.name = "Wall"+cell.Location.x.ToString()+"-"+cell.Location.y.ToString();
					w.parent = m_gridComponents.transform;
					//w.localScale = Vector3.zero;
					w.DOScale (new Vector3 (1, 1, 1), 0.5f);
					CompletedList.Add (w);
					foreach (Transform T in TrailList) {
						T.DOScale (new Vector3 (1, 1, 1), 0.5f).SetEase (Ease.OutElastic);
						CompletedList.Add (T);
					}
					TrailList.Clear ();

					if (isFirst) {
						w.GetComponent <MeshRenderer> ().enabled = false;
					}
					m_numberOfRenderedCells++;
					cell.IsDraw = true;
				}	
			}


			isFirst = false;
		}
	}
	
	// Return the percentage of covered cells.
	public float PercentageOfRenderedCells ()
	{
		return (float) m_numberOfRenderedCells/ (float) m_numberOfGridCells;
	}


	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		string trailString = "";
		string completedString = "";
		string gridmapString = "";
		if(stream.isWriting)
		{
			trailString = JsonUtility.ToJson (TrailList);
			completedString = JsonUtility.ToJson (CompletedList);
			gridmapString = JsonUtility.ToJson (m_gridMap);
			stream.Serialize(ref trailString);
			stream.Serialize(ref completedString);
			stream.Serialize(ref gridmapString);

		}
		else
		{
			stream.Serialize (ref trailString);
			stream.Serialize(ref completedString);
			stream.Serialize(ref gridmapString);
			TrailList = JsonUtility.FromJson <List<Transform>>(trailString);
			CompletedList = JsonUtility.FromJson <List<Transform>>(completedString);
			m_gridMap = JsonUtility.FromJson <Grid<GridCell>>(gridmapString);


		}
	}
	
	// Draw the debug line
	/*private void OnDrawGizmos ()
	{
		Vector3 idScale = new Vector3(0.7f, 0.1f, 0.7f);
		
		foreach (GridCell cell in Cells)
		{
			Vector3 centroid = new Vector3 (cell.Location.x, 1f, cell.Location.y);
			Vector3 topLeft = new Vector3 (centroid.x - 0.5f, 1f, centroid.z - 0.5f);
			Vector3 topRight = new Vector3 (centroid.x + 0.5f, 1f, centroid.z - 0.5f);
			Vector3 bottomLeft = new Vector3 (centroid.x - 0.5f, 1f, centroid.z + 0.5f);
			Vector3 bottomRight = new Vector3 (centroid.x + 0.5f, 1f, centroid.z + 0.5f);
				
			// Draw color coded cell properties
			if (cell.Kill)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawWireCube (centroid, idScale);
			}
			if (cell.Collided)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube (centroid, idScale);
			}
				
			// Draw edge lines
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine (topLeft, topRight);
			Gizmos.DrawLine (bottomLeft, bottomRight);
			Gizmos.DrawLine (topRight, bottomRight);
			Gizmos.DrawLine (topLeft, bottomLeft);
		}
	}*/
	
	#endregion
}
