using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// A grid map
public class Grid<T> where T : IGridCell, new()
{
	#region Constructors
	
	// Initializes a new instance of the GridMap class.
	public Grid (int width, int height)
	{
		Width = width;
		Height = height;
		m_cellArray = new T[width * height];
		InitializeGrid();
	}
	
	#endregion
	
	#region Fields
	
	// The width of the grid.
	private int m_width;
	public int Width
	{
		get { return m_width; }
		private set { m_width = value; }
	}
	
	// The height of the grid.
	private int m_height;
	public int Height
	{
		get { return m_height; }
		private set { m_height = value; }
	}
	
	// The array of grid cells.
	public T[] m_cellArray;
	
	#endregion
	
	#region Methods
	
	// Initializes the grid.
	private void InitializeGrid()
	{
		// Create cells
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				int index = GridToCellIndex (x, y);
				T cell = new T ();
				cell.Location = new GridLocation (x, y);
				cell.Index = index;
				m_cellArray[index] = cell;
			}
		}
		
		// Link cells.
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				T cell = m_cellArray[GridToCellIndex (x, y)];
				cell.North = GetCellAt (x, y + 1);
				cell.South = GetCellAt (x, y - 1);
				cell.East = GetCellAt (x + 1, y);
				cell.West = GetCellAt (x - 1, y);
			}
		}
	}
	
	// Gets the cell at grid position <x,y>.
	public T GetCellAt (int x, int y)
	{
		return m_cellArray[GridToCellIndex (x, y)];
	}
	
	// Gets the cell at the specified GridLocation.
	public T GetCellAt (GridLocation location)
	{
		return m_cellArray[GridToCellIndex (location.x, location.y)];
	}
	
	// Convert an <x,y> coordinate to a cell index.
	private int GridToCellIndex (int x, int y)
	{
		x = (x % Width + Width) % Width;
		y = (y % Height + Height) % Height;
		return x + y * Width;
	}
	
	// Wraps the specified coordinates to fit within the grid.
	public GridLocation WrapCoordinates (int x, int y)
	{
		x = x % Width;
		y = y % Height;
		return new GridLocation (x, y);
	}
	
	// Return all covered cells.
	public List<T> CoveredCells () 
	{
		List<T> coveredCells = new List<T>();
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				T cell = m_cellArray[GridToCellIndex (x, y)];
				if (cell.IsCovered)
					coveredCells.Add(cell);
			}
		}
		return coveredCells;
	}
	
	#endregion
}
