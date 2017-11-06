using UnityEngine;
using System.Collections;

// GridLocation structure
public struct GridLocation
{
	#region Constructors
	
	// Initializes a new instance of the GridLocation struct
	public GridLocation (int px, int py)
	{
		x = px;
		y = py;
	}
	
	public GridLocation (Vector2 p)
	{
		x = (int) p.x;
		y = (int) p.y;
	}
	
	#endregion
	
	#region Fields
	
	// The x-position.
	public int x;
	
	// The y-position.
	public int y;
	
	#endregion
}
