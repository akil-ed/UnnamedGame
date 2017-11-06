using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemsController : MonoBehaviour {
	
	#region Fields
	
	// The game grid.
	private Grid<GridCell> m_gridMap;
	
	// The GUIController.
	private GUIController m_guiController;
	
	// A list of all special items.
	[SerializeField]
	private List<Transform> m_specialItems;
	
	// The time when the last enemy was created.
	private float m_lastCreationTime;
	
	// The interval between two enemies creation.
	[SerializeField]
	private float m_creationIntervalTime = 7f;
	
	#endregion
	
	#region Methods
	
	void Start () 
	{
		m_gridMap = GameObject.Find ("GridBuilder").GetComponent<GridBuilder> ().GridMap;
		m_guiController = GameObject.Find("GUIController").GetComponent<GUIController> ();
		
		GenerateSpecialItems ();
	}
	
	private void Update () 
	{
		// Checking if it is time to create another item.
		if (!m_guiController.GameCompleted && Time.time - m_lastCreationTime > m_creationIntervalTime)
			GenerateSpecialItems ();
    }
	
	// Generate an special item.
	private void GenerateSpecialItems () 
	{
		int index = Random.Range(0, m_specialItems.Count);
		
		int cell_z = Random.Range(0, m_gridMap.Height);
		int cell_x = Random.Range(0, m_gridMap.Width);
		
		Vector3 centroid = new Vector3 (cell_x, 5f, cell_z);
		Transform i = Instantiate (m_specialItems[index], centroid, Quaternion.identity) as Transform;
		ItemBehaviour iB = i.gameObject.AddComponent<ItemBehaviour>();
		
		float i_height = i.GetComponent<Collider>().bounds.center.y - i.GetComponent<Collider>().bounds.min.y;
		
		if (m_gridMap.GetCellAt (cell_x, cell_z).IsCovered)
		{
			iB.OnFloor = false;
			StartCoroutine (MoveObject(i.gameObject, i.transform.position, new Vector3 (cell_x, 0.5f+i_height, cell_z)));
		}
		else
		{
			iB.OnFloor = true;
			StartCoroutine (MoveObject(i.gameObject, i.transform.position, new Vector3 (cell_x, -0.5f+i_height, cell_z)));
		}
		m_lastCreationTime = Time.time;
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
    }
	
	#endregion
}
