using UnityEngine;
using System.Collections;using UnityEngine.UI;

public class UserDetector : MonoBehaviour, KinectGestures.GestureListenerInterface {
	public CursorController cursor;
	public Image personDetectedIcon, noPersonDetectedIcon,emptyCircleIcon;
	//public TurretAim turretAim;
	void Awake(){
		//if (personDetectedIcon == null)
		//				Debug.Break ();
		personDetectedIcon.enabled = false;
		noPersonDetectedIcon.enabled = emptyCircleIcon.enabled = true;
	}

	public void UserDetected(long userId, int userIndex)
	{
		//if (personDetectedIcon == null)
		//	Debug.Break ();
	// as an example - detect these user specific gestures
	KinectManager manager = KinectManager.Instance;
	manager.DetectGesture(userId, KinectGestures.Gestures.Wave);
		//Debug.Break ();
		//print ("User Detected! Id: " + userId);
		personDetectedIcon.enabled = emptyCircleIcon.enabled = true; 
		noPersonDetectedIcon.enabled = false;
	//		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeUp);
	//		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeDown);
//	
//	if(GestureInfo != null)
//	{
//		GestureInfo.guiText.text = "SwipeLeft, SwipeRight, Squat, Push or Pull.";
//	}
}

public void UserLost(long userId, int userIndex)
{
		print ("UserLost");
		//Debug.Break ();
		personDetectedIcon.enabled = false; 
		noPersonDetectedIcon.enabled = emptyCircleIcon.enabled =true;//false;
		if(cursor!=null)cursor.CancelClicking ();
}

public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, 
                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
{
	if((gesture == KinectGestures.Gestures.ZoomOut || gesture == KinectGestures.Gestures.ZoomIn) && progress > 0.5f)
	{
		string sGestureText = string.Format ("{0} detected, zoom={1:F1}%", gesture, screenPos.z * 100);
		
//		if(GestureInfo != null)
//		{
//			GestureInfo.guiText.text = sGestureText;
//		}
//		
//		//Debug.Log(sGestureText);
//		progressDisplayed = true;
	}
	else if(gesture == KinectGestures.Gestures.Wheel && progress > 0.5f)
	{
		string sGestureText = string.Format ("{0} detected, angle={1:F1} deg", gesture, screenPos.z);
		
//		if(GestureInfo != null)
//		{
//			GestureInfo.guiText.text = sGestureText;
//		}
//		
//		//Debug.Log(sGestureText);
//		progressDisplayed = true;
	}
	}
	//public CollectableApplesController applesController;
	//public WorkOutController workOutController;
public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, 
                             KinectInterop.JointType joint, Vector3 screenPos)
{
	string sGestureText = gesture + " detected";
		if(cursor!=null)cursor.WaveRecognised();
		/*if (applesController != null)
			applesController.WaveRecognised ();
		if (workOutController != null)
			workOutController.WaveRecognised ();
		if (turretAim != null)
			turretAim.WaveRecognised ();*/
//	if(GestureInfo != null)
//	{
//		turretGame.FireTurret();
//		GestureInfo.guiText.text = sGestureText;
//	}
//	
//	progressDisplayed = false;
//	
	return true;
}

public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, 
                             KinectInterop.JointType joint)
{
//	if(progressDisplayed)
//	{
//		// clear the progress info
//		if(GestureInfo != null)
//		{
//			GestureInfo.guiText.text = gesture.ToString();
//		}
//		
//		progressDisplayed = false;
//	}
	
	return true;
}

}