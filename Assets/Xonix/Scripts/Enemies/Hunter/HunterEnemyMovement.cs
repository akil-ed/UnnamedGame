using UnityEngine;
using System.Collections;
using System;

public class HunterEnemyMovement : MonoBehaviour {

#region Fields
	
	// The speed at which the character normally moves, in units per second.
	private float m_moveSpeed = 3f;
	public float MoveSpeed
	{
		set { m_moveSpeed = value; }
	}
	
	// The enemy location on the grid.
	private GridLocation m_enemyLocation;
	public GridLocation EnemyLocation
	{
		get { return m_enemyLocation; }
		set { m_enemyLocation = value; }
	}
	
	// Whether movement is on the horizontal (X/Z) plane or the vertical (X/Y) plane.
    private enum Orientation 
	{
        Horizontal,
        Vertical
    };
	
	// The grid orientation.
    private Orientation m_gridOrientation = Orientation.Horizontal;
	
	// The input controls values.
    private Vector2 input;
	
	// The character is already moving?
    private bool isMoving = false;
	
	//The character start position.
    private Vector3 startPosition;
	
	//The character end position.
    private Vector3 endPostion;
	
    private float time;
	
	// How wide/tall each grid square is, in units.
    private float gridSize = 1f;
	
	// The game grid.
	private Grid<GridCell> gridMap;
	
	GridCell cell;
	
	#endregion
	
	#region Methods
	
	private void Start ()
	{
		gridMap = GameObject.Find ("GridBuilder").GetComponent<GridBuilder> ().GridMap;
		
		EnemyLocation = new GridLocation((int) Math.Round(this.transform.position.x, MidpointRounding.ToEven), 
										 (int) Math.Round(this.transform.position.z, MidpointRounding.ToEven));
		
		GenerateNextPositon ();
	}
	
		
    private void Update () 
	{	
        if (!isMoving) 
		{
			//Debug.Log("CL: "+CharacterLocation.x+" "+ CharacterLocation.y+" | IP: "+input.x+" "+input.y);
			
			cell = gridMap.GetCellAt ((int) (EnemyLocation.x + input.x), (int) (EnemyLocation.y + input.y));
			
			// Checking it it is a bound.
			if (input.x > 0f && EnemyLocation.x == gridMap.Width-1) { GenerateNextPositon (); return; }
			else if (input.x < 0f &&  EnemyLocation.x == 0) { GenerateNextPositon (); return; }
			if (input.y > 0f &&  EnemyLocation.y == gridMap.Height-1) { GenerateNextPositon (); return; }
			else if (input.y < 0f &&  EnemyLocation.y == 0) { GenerateNextPositon (); return; }
			
			if (input == Vector2.zero) { GenerateNextPositon (); return; }
			
			if (!cell.IsCovered) { GenerateNextPositon (); return; }
            
            StartCoroutine (move(transform));
        }
    }
	
	private void GenerateNextPositon ()
	{
		input = new Vector2 (UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2));
	}
 
	// Move the player on the grid.
    private IEnumerator move (Transform transform) 
	{
        isMoving = true;
        startPosition = transform.position;
        time = 0;
 
		// Check the grid orientation to move the character correctly.
        if(m_gridOrientation == Orientation.Horizontal)
		{
            endPostion = new Vector3 (startPosition.x + System.Math.Sign(input.x) * gridSize,
                					  startPosition.y, startPosition.z + System.Math.Sign(input.y) * gridSize);
			m_enemyLocation = new GridLocation ((int) endPostion.x, (int) endPostion.z);
		}
        else 
		{
            endPostion = new Vector3 (startPosition.x + System.Math.Sign (input.x) * gridSize,
               						  startPosition.y + System.Math.Sign (input.y) * gridSize, startPosition.z);
			m_enemyLocation = new GridLocation ((int) endPostion.x, (int) endPostion.y);
		}
        
        while (time < 1f) {
            time += Time.deltaTime * (m_moveSpeed / gridSize);
            transform.position = Vector3.Lerp (startPosition, endPostion, time);
			
            yield return null;
        }	
 
        isMoving = false;
        yield return 0;
    }
	
	#endregion
}
