using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;
using HedgehogTeam.EasyTouch;
// Define the character movement logic
public class GridMove : MonoBehaviour {
	
	#region Fields
	
	// The speed at which the character normally moves, in units per second.
	public float m_moveSpeed = 7f;
	public float MoveSpeed
	{
		set { m_moveSpeed = value; }
	}
	
	// The player location on the grid.
	private GridLocation m_characterLocation;
	public GridLocation CharacterLocation
	{
		get { return m_characterLocation; }
		set { m_characterLocation = value; }
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
    private Vector2 m_input;
	
	// The character is already moving?
    private bool m_isMoving = false;
	
	// The initial position of the player.
	private Vector2  m_initialPosition = Vector2.zero;
	
	//The character start position.
    private Vector3 m_startPosition;
	
	//The character end position.
    private Vector3 m_endPostion;
	
    private float m_movementTime;
	
	// How wide/tall each grid square is, in units.
    private float m_gridSize = 1f;
	
	// The game grid.
	private Grid<GridCell> m_gridMap;
	
	// The path controller.
	private PathController m_pathController;
	
	// The GUIController.
	private GUIController m_guiController;

	private GridRenderer m_gridrenderer;
	
	// For how long the player has't been moving.
	private float m_notMovingTime;

	public GameObject Character;
	#endregion
	
	#region Methods
	public static float rotationAngle;

	private void Start ()
	{
		m_gridMap = GameObject.Find ("GridBuilder").GetComponent<GridBuilder> ().GridMap;
		m_pathController = GameObject.Find("PathController").GetComponent<PathController> ();
		m_guiController = GameObject.Find("GUIController").GetComponent<GUIController> ();
		m_gridrenderer = GameObject.Find("GridRenderer").GetComponent<GridRenderer> ();
		ResetPosition ();
		
		m_notMovingTime = Time.time;


		m_input = Vector2.up;
		//StartCoroutine (move(transform));
	}

	bool isSwiped;
	void OnEnable(){
		EasyTouch.On_Swipe += On_Swipe;
		EasyTouch.On_SwipeEnd += On_SwipeEnd;
	}

	void OnDestroy(){
		EasyTouch.On_Swipe -= On_Swipe;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;
	}

	void On_SwipeEnd(Gesture gesture){
		isSwiped = false;


	}
	void On_Swipe (Gesture gesture){
		if (isSwiped)
			return;


		switch (gesture.swipe){

		case EasyTouch.SwipeDirection.Left:
			if (!Vector2.Equals (m_input,Vector2.right)) {
				m_input = Vector2.left;
				isSwiped = true;
			}
			break;

		case EasyTouch.SwipeDirection.Right:
			if (!Vector2.Equals (m_input,Vector2.left)) {
				m_input = Vector2.right;
				isSwiped = true;
			}
			break;
		case EasyTouch.SwipeDirection.Up:
			if (!Vector2.Equals (m_input,Vector2.down)) {
				m_input = Vector2.up;
				isSwiped = true;
			}
			break;
		case EasyTouch.SwipeDirection.Down:
			if (!Vector2.Equals (m_input,Vector2.up)) {
				m_input = Vector2.down;
				isSwiped = true;
			}
			break;

		}
	}
		
    void Update () 
	{	

		if(Input.GetKeyDown (KeyCode.LeftArrow))
			m_input = Vector2.left;
		if(Input.GetKeyDown (KeyCode.RightArrow))
			m_input = Vector2.right;
		if(Input.GetKeyDown (KeyCode.UpArrow))
			m_input = Vector2.up;
		if(Input.GetKeyDown (KeyCode.DownArrow))
			m_input = Vector2.down;




//		if (!m_guiController.GameCompleted && !m_isMoving) 
//		{
//			// Getting input values.
//	        m_input = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
//		
//
//
//			// Checking it it is a bound.
//			if (m_input.x > 0f &&  m_characterLocation.x == m_gridMap.Width-1) return;
//			else if (m_input.x < 0f &&  m_characterLocation.x == 0) return;
//			if (m_input.y > 0f &&  m_characterLocation.y == m_gridMap.Height-1) return;
//			else if (m_input.y < 0f &&  m_characterLocation.y == 0) return;
//					
//
//            if (Mathf.Abs (m_input.x) > Mathf.Abs (m_input.y))
//                m_input.y = 0;
//            else 
//            	m_input.x = 0;
//            
//
//			if (m_input.x < 0)
//				Character.transform.rotation = Quaternion.Euler (0, 270, 0);
//			else if (m_input.x > 0)
//				Character.transform.rotation = Quaternion.Euler (0, 90, 0);
//
//			if (m_input.y < 0)
//				Character.transform.rotation = Quaternion.Euler (0, 180, 0);
//			else if (m_input.y > 0)
//				Character.transform.rotation = Quaternion.Euler (0, 0, 0);
//
//
// 			// If the player moves the character.
//       //     if (m_input != Vector2.zero) 
//                StartCoroutine (move(transform));
//			
//			NotMovingAnimation ();
//        }
    }

	void LateUpdate(){
		Movement ();
	}

	public float m_rotateSpeed;
	public void Movement(){


		if (!m_guiController.GameCompleted && !m_isMoving) 
		{
			
			if (m_input.x > 0f && m_characterLocation.x == m_gridMap.Width - 1) {
				m_input = Vector3.zero;
				return;
			}
			else if (m_input.x < 0f &&  m_characterLocation.x == 0){
				m_input = Vector3.zero;
				return;
			}
			if (m_input.y > 0f &&  m_characterLocation.y == m_gridMap.Height-1){
				m_input = Vector3.zero;
				return;
			}
			else if (m_input.y < 0f &&  m_characterLocation.y == 0){
				m_input = Vector3.zero;
				return;
			}


			if (Mathf.Abs (m_input.x) > Mathf.Abs (m_input.y))
				m_input.y = 0;
			else 
				m_input.x = 0;


			if (m_input.x < 0)
				rotationAngle = 270;
			else if (m_input.x > 0)
				rotationAngle = 90;

			if (m_input.y < 0)
				rotationAngle = 180;
			else if (m_input.y > 0)
				rotationAngle = 0;

			Character.transform.DORotate (new Vector3 (0, rotationAngle, 0),m_rotateSpeed,RotateMode.Fast).SetEase (Ease.Linear);


			// If the player moves the character.
			//     if (m_input != Vector2.zero) 
			//StartCoroutine (move(transform));
			PlayerMove ();
		//	NotMovingAnimation ();
		}
	}




	// Reset the player position to 0,1,0.
	public void ResetPosition () 
	{
		//this.gameObject.transform.position = new Vector3 (m_initialPosition.x+m_gridMap.Width/2, 1f, m_initialPosition.y);
		this.gameObject.transform.position = new Vector3 (m_initialPosition.x, 1f, m_initialPosition.y);
		CharacterLocation = new GridLocation (m_initialPosition);
	}
	
	// Play an animation when the player has passed a long time doing nothing.
	private void NotMovingAnimation ()
	{
		if (!m_guiController.GameCompleted && Time.time - m_notMovingTime > 5f)
		{
			this.gameObject.GetComponentInChildren<Animation> ().GetComponent<Animation>().Play ("Bored");
			m_notMovingTime = Time.time;
		}
	}
 
	// Move the player on the grid.

	public void PlayerMove(){
		m_isMoving = true;
		m_startPosition = transform.position;
		m_movementTime = 0;

		// Check the grid orientation to move the character correctly.
		if(m_gridOrientation == Orientation.Horizontal)
		{
			m_endPostion = new Vector3 (m_startPosition.x + System.Math.Sign(m_input.x) * m_gridSize,
				m_startPosition.y, m_startPosition.z + System.Math.Sign(m_input.y) * m_gridSize);
			CharacterLocation = new GridLocation ((int) m_endPostion.x, (int) m_endPostion.z);
		}
		else 
		{
			m_endPostion = new Vector3 (m_startPosition.x + System.Math.Sign (m_input.x) * m_gridSize,
				m_startPosition.y + System.Math.Sign (m_input.y) * m_gridSize, m_startPosition.z);
			CharacterLocation = new GridLocation ((int) m_endPostion.x, (int) m_endPostion.y);
		}


		float m_moveTime = Vector3.Distance (m_startPosition,m_endPostion)/m_moveSpeed;
		transform.DOMove (m_endPostion, m_moveTime, false).SetEase (Ease.Linear).OnComplete (() => {
			//StartCoroutine (PostMovement());
			CheckFill();
		});
	}

	public void CheckFill(){
		if (!m_gridMap.GetCellAt (CharacterLocation).IsCovered)
		{
			m_pathController.AddPathCell (m_gridMap.GetCellAt (CharacterLocation));
		}
		// Close a path.
		else if (m_gridMap.GetCellAt (CharacterLocation).IsCovered)
		{
			GameObject[] GO = GameObject.FindGameObjectsWithTag ("DumbEnemy");
			foreach (GameObject go in GO)
			{
				GridLocation enemieLocation = new GridLocation((int) Math.Round(go.transform.position.x, MidpointRounding.ToEven), 
					(int) Math.Round(go.transform.position.z, MidpointRounding.ToEven));
				FloodFill.FillEnemiesArea (m_gridMap, m_gridMap.GetCellAt (enemieLocation));
			}
			FloodFill.FillPlayerCoveredArea (m_gridMap);
			FloodFill.ClearEnemiesArea (m_gridMap);	
			m_pathController.ClosePath ();

			m_gridrenderer.FillTrail ();

			//guiController.VerifyPercentageOfGridCovered ();
		}

		m_isMoving = false;
		m_notMovingTime = Time.time;
	}

    private IEnumerator move (Transform transform) 
	{
        m_isMoving = true;
        m_startPosition = transform.position;
        m_movementTime = 0;
 
		// Check the grid orientation to move the character correctly.
        if(m_gridOrientation == Orientation.Horizontal)
		{
            m_endPostion = new Vector3 (m_startPosition.x + System.Math.Sign(m_input.x) * m_gridSize,
                					  m_startPosition.y, m_startPosition.z + System.Math.Sign(m_input.y) * m_gridSize);
			CharacterLocation = new GridLocation ((int) m_endPostion.x, (int) m_endPostion.z);
		}
        else 
		{
            m_endPostion = new Vector3 (m_startPosition.x + System.Math.Sign (m_input.x) * m_gridSize,
               						  m_startPosition.y + System.Math.Sign (m_input.y) * m_gridSize, m_startPosition.z);
			CharacterLocation = new GridLocation ((int) m_endPostion.x, (int) m_endPostion.y);
		}
        
//        while (m_movementTime < 1f) {
//            m_movementTime += Time.deltaTime * (m_moveSpeed / m_gridSize);
//			transform.position = Vector3.Lerp (m_startPosition, m_endPostion, m_movementTime);
//			
//            yield return null;
//        }	

//		float m_moveTime = Vector3.Distance (m_startPosition,m_endPostion)/m_moveSpeed;
//		transform.DOMove (m_endPostion, m_moveTime, false).SetEase (Ease.Linear).OnComplete (() => {
//			StartCoroutine (PostMovement());
//		});

		transform.LookAt (m_endPostion);
		while (transform.position != m_endPostion) {
			transform.Translate (Vector3.forward * m_moveSpeed * Time.deltaTime);
			//transform.position = Vector3.MoveTowards (transform.position, m_endPostion, m_moveSpeed * Time.deltaTime);
			yield return new WaitForSeconds (Time.deltaTime);
		}


//		while (transform.position != m_endPostion) {
//			transform.position = Vector3.MoveTowards (transform.position, m_endPostion, m_moveSpeed * Time.deltaTime);
//			yield return new WaitForSeconds (Time.deltaTime);
//		}
		StartCoroutine (PostMovement());
		yield return 0;
	
	//	StartCoroutine (move(transform));
    }

	IEnumerator PostMovement(){
		// Add each new cell that the player has walk througt to the path.
		if (!m_gridMap.GetCellAt (CharacterLocation).IsCovered)
		{
			m_pathController.AddPathCell (m_gridMap.GetCellAt (CharacterLocation));
		}
		// Close a path.
		else if (m_gridMap.GetCellAt (CharacterLocation).IsCovered)
		{
			GameObject[] GO = GameObject.FindGameObjectsWithTag ("DumbEnemy");
			foreach (GameObject go in GO)
			{
				GridLocation enemieLocation = new GridLocation((int) Math.Round(go.transform.position.x, MidpointRounding.ToEven), 
					(int) Math.Round(go.transform.position.z, MidpointRounding.ToEven));
				FloodFill.FillEnemiesArea (m_gridMap, m_gridMap.GetCellAt (enemieLocation));
			}
			FloodFill.FillPlayerCoveredArea (m_gridMap);
			FloodFill.ClearEnemiesArea (m_gridMap);	
			m_pathController.ClosePath ();

			m_gridrenderer.FillTrail ();

			//guiController.VerifyPercentageOfGridCovered ();
		}

		m_isMoving = false;
		m_notMovingTime = Time.time;
		yield return 0;
	}
	
	#endregion
}
