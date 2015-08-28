using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GestureListener : MonoBehaviour,KinectGestures.GestureListenerInterface {
	public GestureNavigationMenu menu;
	
	// GUI Text to display the gesture messages.
	public Text GestureInfo;
	
	// private bool to track if progress message has been displayed
	private bool progressDisplayed;
	
	
	public void UserDetected(long userId, int userIndex)
	{
		// as an example - detect these user specific gestures
		KinectManager manager = KinectManager.Instance;
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeUp);
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeDown);
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
		
		//		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeUp);
		//		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeDown);
		print ("UserDetected");
		if(GestureInfo != null)
		{
			GestureInfo.text = "SwipeLeft, SwipeRight, Squat, Push or Pull.";
		}
	}
	
	public void UserLost(long userId, int userIndex)
	{
		if(GestureInfo != null)
		{
			GestureInfo.text = string.Empty;
		}
	}
	
	public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
	{
		/*if((gesture == KinectGestures.Gestures.ZoomOut || gesture == KinectGestures.Gestures.ZoomIn) && progress > 0.5f)
		{
			string sGestureText = string.Format ("{0} detected, zoom={1:F1}%", gesture, screenPos.z * 100);
			
			if(GestureInfo != null)
			{
				GestureInfo.text = sGestureText;
			}
			
			//Debug.Log(sGestureText);
			progressDisplayed = true;
		}
		else if(gesture == KinectGestures.Gestures.Wheel && progress > 0.5f)
		{
			string sGestureText = string.Format ("{0} detected, angle={1:F1} deg", gesture, screenPos.z);
			
			if(GestureInfo != null)
			{
				GestureInfo.text = sGestureText;
			}
			
			//Debug.Log(sGestureText);
			progressDisplayed = true;
		}*/
	}
	
	public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                             KinectInterop.JointType joint, Vector3 screenPos)
	{
		string sGestureText = gesture + " detected";
		
		if(GestureInfo != null)
		{
			GestureInfo.text = sGestureText;
		}

		switch (gesture) {
		case KinectGestures.Gestures.SwipeUp:
			menu.SwipeUp();
			break;
		case KinectGestures.Gestures.SwipeDown:
			menu.SwipeDown();
			break;
		case KinectGestures.Gestures.SwipeLeft:
			menu.SwipeLeft();
			break;
		case KinectGestures.Gestures.SwipeRight:
			menu.SwipeRight();
			break;
		default:
			break;
				}
		
		progressDisplayed = false;
		
		return true;
	}
	
	public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                             KinectInterop.JointType joint)
	{
		/*
		if(progressDisplayed)
		{
			// clear the progress info
			if(GestureInfo != null)
			{
				GestureInfo.guiText.text = string.Empty;
			}
			
			progressDisplayed = false;
		}
		*/
		return true;
	}

}
