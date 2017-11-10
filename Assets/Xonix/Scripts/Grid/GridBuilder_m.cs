using UnityEngine;
using System.Collections;

// Build the grid
public class GridBuilder_m : MonoBehaviour {
	
	#region Fields
	
	// The width of the grid
	public int m_width = 32;
	
	// The height of the grid
	public int m_height = 32;
	
	// The grid
	private Grid<GridCell> m_gameGrid;
	public Grid<GridCell> GridMap
	{
		get { return m_gameGrid; }
	}
	
	#endregion
	
	#region Methods
	
	// Local initialization.
	private void Awake ()
	{
		m_gameGrid = GenerateGrid ();
		
		SetBounds ();
			
		GenerateDebug ();
	}
	
	// Generates a grid to use as a basis for the map.
	private Grid<GridCell> GenerateGrid ()
	{	
		// Initialize variables
		Grid<GridCell> grid = new Grid<GridCell> (m_width, m_height);
		return grid;
	}
	
	// Set the bouds of the grid.
	private void SetBounds ()
	{
		foreach (GridCell cell in m_gameGrid.m_cellArray)
			if (cell.Location.x == 0 || cell.Location.x == m_width - 1 || cell.Location.y == 0 || cell.Location.y == m_height - 1) {
				cell.IsCovered = true;
				//cell.
			}
	}
	
	// Generates the debug render.
	private void GenerateDebug()
	{
		GridRenderer_m debug = GameObject.Find("GridRenderer").GetComponent<GridRenderer_m>();
		foreach (GridCell cell in m_gameGrid.m_cellArray)
			debug.AddCell (cell);
	}
	
	#endregion
}
