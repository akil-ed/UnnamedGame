using UnityEngine;
using System.Collections;

// GridCell defines a single cell in a Grid
public class GridCell : IGridCell {

	#region Fields


	// The linear index of the cell.
	private int m_index;
	public int Index
	{
		get { return m_index; }
		set { m_index = value; }
	}
	
	// The location of the cell in the grid.
	private GridLocation m_gridLocation;
	public GridLocation Location
	{
		get { return m_gridLocation; }
		set { m_gridLocation = value; }
	}
	
	// The neighboring cell to the north (+y).
	private IGridCell m_north;
	public IGridCell North
	{
		get { return m_north; }
		set { m_north = value; }
	}
	
	// The neighboring cell to the south (-y).
	private IGridCell m_south;
	public IGridCell South
	{
		get { return m_south; }
		set { m_south = value; }
	}
	
	// The neighboring cell to the east (+x).
	private IGridCell m_east;
	public IGridCell East
	{
		get { return m_east; }
		set { m_east = value; }
	}
	
	// The neighboring cell to the west (-x).
	private IGridCell m_west;
	public IGridCell West
	{
		get { return m_west; }
		set { m_west = value; }
	}
	
	// The cell is already covered?
	private bool m_isCovered = false;
	public bool IsCovered
	{
		get { return m_isCovered; }
		set { m_isCovered = value; }
	}
	
	// The cell is part of a path?
	private bool  m_currentPath = false;
	public bool  CurrentPath
	{
		get { return  m_currentPath; }
		set {  m_currentPath = value; }
	}
	
	// The cell is part of the enemies' area?
	private bool m_isEnemyArea = false;
	public bool IsEnemyArea
	{
		get { return m_isEnemyArea; }
		set { m_isEnemyArea = value; }
	}
	
	// The cell cell has already been drawn?
	private bool m_isDrawn = false;
	public bool IsDraw
	{
		get { return m_isDrawn; }
		set { m_isDrawn = value; }
	}
	
	/*private bool m_Collided = false;
	public bool Collided
	{
		get { return m_Collided; }
		set { m_Collided = value; }
	}
	
	private bool m_Kill = false;
	public bool Kill
	{
		get { return m_Kill; }
		set { m_Kill = value; }
	}*/
	
	#endregion
}
