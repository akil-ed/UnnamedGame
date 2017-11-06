using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The hunter enenemies builder.
public class HunterEnemiesBuilder : MonoBehaviour {
	
	#region Fields.
	
	// The enemy prefab.
	[SerializeField]
	private Transform m_enemy;
	
	// The maximum number of enemies to build.
	[SerializeField]
	private int m_maxEnemiesNumber;
	
	// The game grid.
	private List<GridCell> m_gridMap;
	
	// The number of enemies already created and alive.
	private int m_enemiesAliveNumber = 0;
	public int EnemiesAliveNumber {
		set { m_enemiesAliveNumber = value; }
		get { return m_enemiesAliveNumber; }
	}
	
	// The enemies parent.
	private GameObject m_enemiesParent;
	
	// The time when the last enemy was created.
	private float m_lastCreationTime;
	
	// The interval between two enemies creation.
	[SerializeField]
	private float m_creationIntervalTime = 18f;
	
	// The GUIController.
	private GUIController m_guiController;
	
	#endregion
	
	#region Methods
	
	void Start () 
	{
		m_guiController = GameObject.Find("GUIController").GetComponent<GUIController> ();
		
		m_enemiesParent = new GameObject ();
		m_enemiesParent.name = "HunterEnemies";
		GenerateEnemies ();
	}
	
	private void Update () 
	{
		if (!m_guiController.GameCompleted && Time.time - m_lastCreationTime > m_creationIntervalTime)
		{
			GenerateEnemies ();
		}
    }
	
	// Generates an enemy.
	private void GenerateEnemies ()
	{
		if (EnemiesAliveNumber < m_maxEnemiesNumber)
		{
			m_gridMap = GameObject.Find("GridBuilder").GetComponent<GridBuilder> ().GridMap.CoveredCells ();
			
			int index = Random.Range(0, m_gridMap.Count-1);
			GridCell enemyLocation = m_gridMap[index];
			
			Transform e = Instantiate (m_enemy, new Vector3 (enemyLocation.Location.x, 5f, enemyLocation.Location.y), Quaternion.identity) as Transform;
			e.transform.parent = m_enemiesParent.transform;
			e.gameObject.AddComponent<HunterEnemyBehaviour> ();
			
			EnemiesAliveNumber++;
			
			StartCoroutine (MoveObject(e.gameObject, e.transform.position, new Vector3 (enemyLocation.Location.x, 1f, enemyLocation.Location.y)));
			
			m_lastCreationTime = Time.time;
		}
	}
	
	// Move the object from a point to another.
	IEnumerator MoveObject(GameObject go, Vector3 startPos, Vector3 finalPos)
    {
        float progress = 0.0f;
 
        while (go && progress < 2.0f)
        {
            go.transform.position = Vector3.Lerp(startPos, finalPos, progress);
            yield return new WaitForEndOfFrame();
            progress += Time.deltaTime;
        }
		
		if (go) 
			go.gameObject.AddComponent<HunterEnemyMovement> (); 
    }
	
	// Slow down all enemies.
	public void SlowDownEnemies () 
	{
		HunterEnemyMovement[] e = m_enemiesParent.GetComponentsInChildren<HunterEnemyMovement> ();
		for (int i = 0; i < e.Length; i ++)
			e[i].MoveSpeed = 1f;
	}
	
	// Normalize the enemies' speed.
	public void NormalizeSpeed ()
	{
		HunterEnemyMovement[] e = m_enemiesParent.GetComponentsInChildren<HunterEnemyMovement> ();
		for (int i = 0; i < e.Length; i ++)
			e[i].MoveSpeed = 3f;
	}
	
	#endregion
}
