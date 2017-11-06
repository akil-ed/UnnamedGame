using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Implements an adapted Flood-Fill algorithm
public class FloodFill 
{
	/*Flood-fill (node, target-color, replacement-color):
 	*	1. If the color of node is not equal to target-color, return.
 	*	2. Set the color of node to replacement-color.
 	*	3. Perform Flood-fill (one step to the west of node, target-color, replacement-color).
    *	   Perform Flood-fill (one step to the east of node, target-color, replacement-color).
    *	   Perform Flood-fill (one step to the north of node, target-color, replacement-color).
    *	   Perform Flood-fill (one step to the south of node, target-color, replacement-color).
 	*	4. Return.
 	*/
	
	#region Methods
	
	// Implements the Flood-Fill with a seed as a enemy cell
    public static void FillEnemiesArea (Grid<GridCell> GridMap, GridCell node)
	{
		if (node.IsCovered || node.CurrentPath || node.IsEnemyArea)
			return;
		
		node.IsEnemyArea = true;
		FillEnemiesArea (GridMap, GridMap.GetCellAt(node.West.Location));
		FillEnemiesArea (GridMap, GridMap.GetCellAt(node.East.Location));
		FillEnemiesArea (GridMap, GridMap.GetCellAt(node.North.Location));
		FillEnemiesArea (GridMap, GridMap.GetCellAt(node.South.Location));
		
		return;	
	}
	
	// Based on the previous method, fill all area covered by the player
	public static void FillPlayerCoveredArea (Grid<GridCell> GridMap)
	{
		for (int x = 0; x < GridMap.Width; x++)
		{
			for (int y = 0; y < GridMap.Height; y++)
			{
				GridCell node = GridMap.GetCellAt (x, y);
				if (!node.IsEnemyArea)
				{
					node.IsCovered = true;
					node.CurrentPath = false;
				}
			}
		}
	}
	
	// Clear all cell set as enemy are
	public static void ClearEnemiesArea (Grid<GridCell> GridMap)
	{
		for (int x = 0; x < GridMap.Width; x++)
		{
			for (int y = 0; y < GridMap.Height; y++)
			{
				GridCell node = GridMap.GetCellAt (x, y);
				if (node.IsEnemyArea) node.IsEnemyArea = false;
			}
		}
	}
	
	#endregion

}
