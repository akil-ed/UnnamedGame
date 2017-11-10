using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;
public class Test : MonoBehaviour {

	void OnEnable(){
		EasyTouch.On_SwipeStart += On_Swipe;
		//EasyTouch.On_SwipeEnd += On_SwipeEnd;
	}

	void OnDestroy(){
		EasyTouch.On_SwipeStart -= On_Swipe;
	//	EasyTouch.On_SwipeEnd -= On_SwipeEnd;
	}


	void On_Swipe (Gesture gesture){

		print ("swiping");
		// Getting input values.
		switch (gesture.swipe){

		case EasyTouch.SwipeDirection.Left:
			//m_input = Vector2.left;
			break;

		case EasyTouch.SwipeDirection.Right:
			//m_input = Vector2.right;
			break;
		case EasyTouch.SwipeDirection.Up:
		//	m_input = Vector2.up;
			break;
		case EasyTouch.SwipeDirection.Down:
			//m_input = Vector2.down;
			break;

		default:
			print ("dddddf");
			break;

		}

	}
}
