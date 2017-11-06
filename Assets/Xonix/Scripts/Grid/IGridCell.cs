using UnityEngine;
using System.Collections;

// Interface used by the Grid<T> generic collection type
public interface IGridCell
{
	#region Fields
	
	// The linear index of the cell.
	int Index { get; set; }
	
	// The location of the cell in the grid.
	GridLocation Location { get; set; }
	
	// The neighbouring cell to the north (+y).
	IGridCell North { get; set; }
	
	// The neighbouring cell to the south (-y).
	IGridCell South { get; set; }
	
	// The neighbouring cell to the east (+x).
	IGridCell East { get; set; }
	
	// The neighbouring cell to the west (-x).
	IGridCell West { get; set; }
	
	bool IsCovered { get; set; }
		
	bool CurrentPath { get; set; }
	
	bool IsEnemyArea { get; set; }
	
	bool IsDraw { get; set; }
	
	#endregion
}
