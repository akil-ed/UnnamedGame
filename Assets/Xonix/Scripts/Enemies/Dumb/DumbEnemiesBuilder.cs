using UnityEngine;
using System.Collections;

// The dumb enemies builder.
public class DumbEnemiesBuilder : MonoBehaviour {
	
	#region Fields
	
	// The enemy prefab.
	[SerializeField]
	private Transform m_enemy;
	
	// The number of enemies to build.
	[SerializeField]
	private int m_enemiesNumber;
	
	// The game grid.
	private Grid<GridCell> m_gridMap;
	
	// The enemies parent.
	private GameObject m_enemiesParent;
	
	#endregion
	
	#region Methods
	
	void Start () 
	{
		m_gridMap = GameObject.Find("GridBuilder").GetComponent<GridBuilder>().GridMap;
		m_enemiesParent = new GameObject ();
		m_enemiesParent.name = "DumbEnemies";
		GenerateEnemies ();
	}
	
	// Generates a defined number of enemies.
	private void GenerateEnemies ()
	{
		for (int i = 0; i < m_enemiesNumber; i ++)
		{
			GridLocation enemyLocation = new GridLocation (Random.Range(1, m_gridMap.Width-1), Random.Range(1, m_gridMap.Height-1));
			
			Transform e = Instantiate (m_enemy, new Vector3 (enemyLocation.x, 0f, enemyLocation.y), Quaternion.identity) as Transform;
			e.transform.parent = m_enemiesParent.transform;
			e.gameObject.AddComponent<DumbEnemyBehaviour>(); 
		}
	}
	
	// Slow down all enemies.
	public void SlowDownEnemies () 
	{
		DumbEnemyBehaviour[] e = m_enemiesParent.GetComponentsInChildren<DumbEnemyBehaviour> ();
		for (int i = 0; i < e.Length; i ++)
			e[i].Speed = 1f;
	}
	
	// Normalize the enemies' speed.
	public void NormalizeSpeed ()
	{
		DumbEnemyBehaviour[] e = m_enemiesParent.GetComponentsInChildren<DumbEnemyBehaviour> ();
		for (int i = 0; i < e.Length; i ++)
			e[i].Speed = 4f;
	}
	
	#endregion
	
}
