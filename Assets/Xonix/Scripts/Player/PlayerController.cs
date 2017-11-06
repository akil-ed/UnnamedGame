using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	#region Fields
	
	// The player's lives.
	private int m_playerLives = 1;
	public int PlayerLives {
		set { m_playerLives = value; }
		get { return m_playerLives; }
	}
	
	// The GUIController. 
	private GUIController m_guiController;
	
	// The Dumb Enemies Builder.
	private DumbEnemiesBuilder m_dumbEnemiesBuilder;
	
	// The Hunter Enemies Builder.
	private HunterEnemiesBuilder m_hunterEnemiesBuilder;
	
	// The time when an special effect has started.
	private float m_specialStartTime;
	
	// The special achieved. 
	private string m_specialPower;
	
	// The game object audio source component.
	private AudioSource m_audioSource;
	
	// Special soudns.
	[SerializeField]
	private AudioClip m_powerUpSound;
	
	[SerializeField]
	private AudioClip m_loseLifeSound;
	
	[SerializeField]
	private AudioClip m_explosionSound;
	
	// The player's animation component.
	private Animation m_animation;


	#endregion
	
	#region Methods
	
	void Awake ()
	{
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
		m_animation = this.gameObject.GetComponentInChildren<Animation> ();		
	}
	
	void Start ()
	{
		m_guiController = GameObject.Find("GUIController").GetComponent<GUIController> ();
		m_dumbEnemiesBuilder = GameObject.Find("EnemiesBuilder").GetComponent<DumbEnemiesBuilder> ();
		m_hunterEnemiesBuilder = GameObject.Find("EnemiesBuilder").GetComponent<HunterEnemiesBuilder> ();
	}
	
	private void OnTriggerEnter (Collider collider)
	{
		// Did I collid with an special item?
		if (collider.tag == "Item")
		{
			m_specialStartTime = Time.time;
			m_audioSource.clip = m_powerUpSound;
			m_audioSource.Play();
			
			if (collider.name.Contains("Time"))
			{
				m_specialPower = "Time";
				m_guiController.TotalTime += 30f;
				Debug.Log("30+sec");
			}
			else if (collider.name.Contains("Life"))
			{
				m_specialPower = "Life";
				PlayerLives += 1;
				Debug.Log("1+life");
			}
			else if (collider.name.Contains("Speed"))
			{
				m_specialPower = "Speed";
				m_dumbEnemiesBuilder.SlowDownEnemies ();
				m_hunterEnemiesBuilder.SlowDownEnemies ();
				m_guiController.TimeFactor = 0.5f;
				Debug.Log("+speed");
			}
			else if (collider.name.Contains("SurpriseBox"))
			{
				m_specialPower = "SurpriseBox";
				Debug.Log("SurpriseBox");
			}
			Destroy (collider.gameObject);
			StartCoroutine (ShutDownSpecial (m_specialPower));
		}
		
		// Did I collided with a hunter enemy?
		else if (collider.tag == "HunterEnemy")
		{
			Debug.Log("HunterEnemy");
			StartCoroutine (collider.GetComponent<HunterEnemyBehaviour> ().Kill ());
			
			LostLife ();
		}		
		
		// Did I collided with a dumb enemy?
		else if (collider.tag == "DumbEnemy")
		{
			LostLife ();
		}
	}
	
	public void LostLife ()
	{
		m_playerLives--;
		if (m_playerLives < 0)
			m_playerLives = 0;
		
		m_audioSource.clip = m_loseLifeSound;
		m_audioSource.Play ();
	}
	
	// Shut down the special in action.
	private IEnumerator ShutDownSpecial (string special)
	{
		float specialTotalTime = Time.time - m_specialStartTime;
	    while (specialTotalTime <= 3f) 
		{
    		specialTotalTime = Time.time - m_specialStartTime;
			yield return new WaitForEndOfFrame();
	    }
		if (special == "Speed")
		{
			m_guiController.TimeFactor = 1f;
			m_dumbEnemiesBuilder.NormalizeSpeed ();
			m_hunterEnemiesBuilder.NormalizeSpeed ();
		}
	}
	
	// Play the animation specific for when the player complete the game.
	public void PlayGameCompletedAnimation ()
	{		
		StartCoroutine (QueueAnim (m_animation["Bouncing"].clip, m_animation["Flying"].clip));
	}
	
	// Play a set of animations.
	IEnumerator QueueAnim (params AnimationClip[] anim){
		int index = 0;
	    while(index < anim.Length)
	    {
			if (index == 1)
				m_guiController.Restart ();
			m_animation.clip = anim[index];
	        m_animation.Play ();
	        yield return new WaitForSeconds (anim[index].length);
	        index++;
	    }
	}
	
	// Kill the player.
	public void Kill ()
	{
		m_audioSource.clip = m_explosionSound;
		m_audioSource.Play();
		
		this.gameObject.GetComponentInChildren<ParticleEmitter> ().Emit ();
		
		m_guiController.Restart ();
	}
	
	#endregion
}
