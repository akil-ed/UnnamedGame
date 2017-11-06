using UnityEngine;
using System.Collections;

public class StartGUI : MonoBehaviour {
	
	// Just an auxiliar to positioning all gui elements.
	Vector3 position;
	
	// The GUI font.
	[SerializeField]
	Font font;
	
	// Space bettwen GUI items.
	int tabItem = 10;
	
	// The title GUIText.
	GUIText title;
	
	// The information GUIText.
	GUIText information;
	
	// A parent for all GUI elements.
	GameObject GUIparent;
	
	float StartAlpha = 1.0f;
 	float EndAlpha = 0.0f;
	
	bool isFading = false;
	
	private void Start ()
	{
		
		GUIStyle style = new GUIStyle();
		
		GUIparent = new GameObject ();
		GUIparent.name = "GUIElements";
		
		// Defining the title GUIText.
		GameObject titleGO = new GameObject();
		title = titleGO.transform.gameObject.AddComponent<GUIText> ();
		title.name = "Title";
		title.text = "Space Xonix";
		title.font = font;
		title.fontSize = 120;
		title.material.color = Color.red;
		title.transform.parent = GUIparent.transform;
		
		position = Camera.main.ScreenToViewportPoint (new Vector3 (4 * tabItem, Screen.height, 0f));
		position.z = 10f;
		title.transform.position = position;
		
		// Defining the information GUIText.
		GameObject informationGO = new GameObject();
		information = informationGO.transform.gameObject.AddComponent<GUIText> ();
		information.name = "Information";
		information.text = "Press space to start";
		information.font = font;
		information.fontSize = 70;
		information.material.color = Color.white;
		information.transform.parent = GUIparent.transform;
		
		style.font = information.font;
		style.fontSize = information.fontSize;
 		Vector2 size = style.CalcSize(new GUIContent(information.text));
		
		position = Camera.main.ScreenToViewportPoint (new Vector3 ((Screen.width - size.x)/2, Screen.height/2 + tabItem, 0f));
		position.z = 10f;
		information.transform.position = position;
	}
	
	void Update ()
	{
		if (!isFading)
		{
			if (information.color.a == StartAlpha)
				StartCoroutine (FadeAlpha (information, StartAlpha, EndAlpha));
			else
				StartCoroutine (FadeAlpha (information, EndAlpha, StartAlpha));
		}
		
		if (Input.GetKey(KeyCode.Space))
			Application.LoadLevel (1);
	}
	
	IEnumerator FadeAlpha (GUIText guiText, float startAlpha, float endAlpha)
    {
		isFading = true;
        float startTime = Time.time;
        while (guiText.color.a != endAlpha)
        {
			float r = guiText.color.r;
			float g = guiText.color.g;
			float b = guiText.color.b;
            float a = Mathf.Lerp(startAlpha, endAlpha, (Time.time - startTime) / 2f);
			guiText.color = new Color (r, g, b, a);
            yield return 0;
        }
		isFading = false;
    }
}
