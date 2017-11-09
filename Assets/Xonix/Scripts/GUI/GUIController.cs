using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	
	#region Fields.
	
	// The Grid Renderer.
	private GridRenderer m_gridRenderer;
	
	// The Player Controller.
	private PlayerController m_playerController;
	
	// Just an auxiliar to positioning all gui elements.
	private Vector3 m_position;
	
	// The time the player has to complete the level.
	private float totalTime = 60f;
	private float m_startTime;
	private float m_restSeconds;
	private float m_roundedRestSeconds;
    private float m_displaySeconds;
    private float m_displayMinutes;
	private float m_countDownSeconds = 60f;
	public float TotalTime {
		get { return m_countDownSeconds; }
		set { m_countDownSeconds = value; }
	}
	
	// The time speed factor.
	private float m_timeFactor = 1f;
	public float TimeFactor {
		get { return m_timeFactor; }
		set { m_timeFactor = value; }
	}
	
	// The has been completed?
	private bool m_gameCompleted = false;
	public bool GameCompleted {
		get { return m_gameCompleted; }
		set { m_gameCompleted = value; }
	}
	
	// A parent for all GUI elements.
	private GameObject m_GUIparent;
	
	// The timer GUIText.
	private GUIText m_timer;
	
	// The life Texture.
	[SerializeField]
	private Texture m_lifeTexture;
	
	// The lives GUIText.
	private GUIText m_livesNumber;
	
	// The life GUITexture.
	private GUITexture m_lifeIcon;
	
	// Life icon size.
	private int m_lifeWidth = 110;
	private int m_lifeHeight = 100;
	
	// The GUI font.
	[SerializeField]
	private Font m_font;
	
	// Space bettwen GUI items.
	private int m_tabItem = 10;
	
	// The screen is been faded?
	private bool m_fadeScreen = false;
	
	// The fade texture.
	[SerializeField]
	private Texture m_fadeTexture;
	
	// Fade parameters.
	private float m_startAlpha = 0.0f;
	private float m_endAlpha = 1.0f;
	private float m_fadeSpeed = 0.25f;
	private int m_drawDepth = -1000;
	private int m_fadeDir = 1;
	
	// Background musics.
	[SerializeField]
	private AudioClip m_music1;
	
	[SerializeField]
	private AudioClip m_music2;
	
	[SerializeField]
	private AudioClip m_countdown10;
	
	// It is time to countdown?
	private bool m_countdown = false;
	
	// The game object audio source.
	private AudioSource m_audioSource;
	
	#endregion
	
	#region Methos
	
	void Awake ()
	{
		m_audioSource = this.gameObject.GetComponent<AudioSource> ();
	}
	
	void Start () 
	{	
		m_GUIparent = new GameObject ();
		m_GUIparent.name = "GUIElements";
		
		initializeGame();
		m_startTime = Time.time;
	}
	
	void initializeGame ()
	{
		m_gridRenderer = GameObject.Find("GridRenderer").GetComponent<GridRenderer> ();
		m_playerController = GameObject.Find("Player").GetComponent<PlayerController> ();
		
		// Defining the timer GUIText.
		m_position = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width - m_tabItem - 120, Screen.height - m_tabItem, 0f));
		m_position.z = 10f;
		GameObject timerGO = new GameObject();
		m_timer = timerGO.transform.gameObject.AddComponent<GUIText>();
		m_timer.name = "Timer";
		m_timer.transform.position = m_position;
		m_timer.font = m_font;
		m_timer.fontSize = 50;
		m_timer.material.color = Color.red;	
		m_timer.transform.parent = m_GUIparent.transform;
			
		// Defining the lives GUIText.
		m_position = Camera.main.ScreenToViewportPoint (new Vector3 (m_tabItem + m_lifeWidth - m_lifeWidth/3, Screen.height - m_tabItem, 0f));
		m_position.z = 10f;
		GameObject livesGO = new GameObject();
		m_livesNumber = livesGO.transform.gameObject.AddComponent<GUIText> ();
		m_livesNumber.name = "Lives";
		m_livesNumber.transform.position = m_position;
		m_livesNumber.font = m_font;
		m_livesNumber.fontSize = 65;
		m_livesNumber.material.color = Color.red;	
		m_livesNumber.transform.parent = m_GUIparent.transform;
		
		// Defining the life GUITexture.
		m_position = Camera.main.ScreenToViewportPoint (new Vector3 (m_tabItem, Screen.height - m_tabItem - m_lifeHeight, 0f));
		m_position.z = 0f;
		GameObject lifeGO = new GameObject();
		lifeGO.transform.localScale = Vector3.zero;
		m_lifeIcon = lifeGO.gameObject.AddComponent<GUITexture> ();
		m_lifeIcon.name = "LifesIcon";
		m_lifeIcon.texture = m_lifeTexture;
		m_lifeIcon.transform.position = m_position;
		m_lifeIcon.pixelInset = new Rect (0, 0, m_lifeWidth, m_lifeHeight);
		m_lifeIcon.transform.parent = m_GUIparent.transform;
		
		// Chosing the background music.
		int music = Random.Range(1, 3);
		if (music == 1)
		{
			m_audioSource.clip = m_music1;
			m_audioSource.Play ();
		}
		else
		{
			m_audioSource.clip = m_music2;
			m_audioSource.Play ();
		}
	}
	
	void Update ()
	{
		Timer ();
		Lives ();
	}
	
	void Lives ()
	{
		if (!GameCompleted)
		{
			if (m_playerController.PlayerLives <= 0)
				KillPlayer ();
			m_livesNumber.text = m_playerController.PlayerLives.ToString ();
		}
	}
	
	// Calculating the remaining time.
	void Timer ()
	{
		if (!GameCompleted)
		{
			float guiTime = 0;
			
	    	guiTime = Time.time * m_timeFactor - m_startTime;
		   	m_restSeconds = m_countDownSeconds - (guiTime);
		    // Game over?
			if (guiTime >= totalTime) 
			{
		        KillPlayer ();
		    }
			// Time to countdown?
			if (!m_countdown && guiTime >= totalTime - 10f) 
			{
				m_countdown = true;
				AudioSource aS = this.gameObject.AddComponent<AudioSource> ();
				aS.clip = m_countdown10;
				aS.Play ();
			}
	    	m_roundedRestSeconds = Mathf.CeilToInt(m_restSeconds);
	    	m_displaySeconds = m_roundedRestSeconds % totalTime;
	    	m_displayMinutes = Mathf.FloorToInt(m_roundedRestSeconds / totalTime); 
	    	string text = string.Format ("{0:00}:{1:00}", m_displayMinutes, m_displaySeconds); 
			m_timer.text = text;
		}
	}
	
	// Kill the player.
	private void KillPlayer ()
	{
		m_playerController.Kill ();
		GameCompleted = true;
	}
	
	// Verify wheter the player has complete the game.
	public void VerifyPercentageOfGridCovered () 
	{
		if (m_gridRenderer.PercentageOfRenderedCells() >= 0.85f)
		{
			m_playerController.PlayGameCompletedAnimation ();
			GameCompleted = true;
			
			GameObject[] itemsGO = GameObject.FindGameObjectsWithTag("Item");
			for (int i = 0; i < itemsGO.Length; i++)
				StartCoroutine (itemsGO[i].GetComponent<ItemBehaviour> ().Kill ());
			
			GameObject[] hunterEnemiesGO = GameObject.FindGameObjectsWithTag("HunterEnemy");
			for (int i = 0; i < hunterEnemiesGO.Length; i++)
				StartCoroutine (hunterEnemiesGO[i].GetComponent<HunterEnemyBehaviour> ().Kill ());
		}	
	}
	
	public void Restart ()
	{
		m_fadeScreen = true;
	}
	
	private void OnGUI ()
    {
		return;
		if (m_fadeScreen)
		{
			float r = GUI.color.r;
			float g = GUI.color.g;
			float b = GUI.color.b;
			
			GUI.color = new Color (r, g, b, m_startAlpha);
			GUI.depth = m_drawDepth;
			GUI.DrawTexture(new Rect (0f, 0f, Screen.width, Screen.height), m_fadeTexture);
			
	        if (m_startAlpha != m_endAlpha)
			{
				m_startAlpha += m_fadeDir * m_fadeSpeed * Time.deltaTime;  
    			m_startAlpha = Mathf.Clamp01(m_startAlpha);
			}
			else 
			{
				Application.LoadLevel (0);
			}
		}
    }
	
	#endregion
}


