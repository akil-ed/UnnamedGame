using UnityEngine;
using System.Collections;

public class HunterEnemyBehaviour : MonoBehaviour {
	
	//
	Color colorStart;
	Color colorEnd;
	
	private void Start ()
	{
		colorStart = this.gameObject.GetComponentInChildren<MeshRenderer>().material.color;
  		colorEnd = new Color (colorStart.r, colorStart.g, colorStart.b, 0.0f);
	}
	
	private void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "Item")
		{
			Destroy(collider.gameObject);
		}
	}
	
	public IEnumerator Kill ()
	{
		this.gameObject.GetComponent<AudioSource> ().Play ();
		float wait = 0.0f;
		while (this.gameObject && wait < .2f)
		{
            wait += Time.deltaTime;
			this.gameObject.GetComponentInChildren<MeshRenderer> ().material.color = Color.Lerp (colorStart, colorEnd, wait);
			yield return new WaitForEndOfFrame ();
		}
		GameObject.Find("EnemiesBuilder").GetComponent<HunterEnemiesBuilder> ().EnemiesAliveNumber--;
		Destroy (this.gameObject);
	}
}
