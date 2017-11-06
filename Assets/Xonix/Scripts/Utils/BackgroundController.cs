using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour
{
	[SerializeField]
	private GUITexture backgroundGUITexture;

	private void Start()
	{
		if( backgroundGUITexture == null )
		{
			this.enabled = false;
		}

		backgroundGUITexture.enabled = true;
	}

	private void Update()
	{
		Rect r = new Rect( 0, 0, Screen.width, Screen.height );
		backgroundGUITexture.pixelInset = r;
	}
}
