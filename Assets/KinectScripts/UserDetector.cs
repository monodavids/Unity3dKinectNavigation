using UnityEngine;
using System.Collections;using UnityEngine.UI;

//This class controls the on-screen user detected/not detected UI
//It also implements the KinectGestures.GestureListenerInterface needed for the wave recognition
public class UserDetector : MonoBehaviour, KinectGestures.GestureListenerInterface {
	//A handle on the CursorController Object
	public CursorController cursor;
	//The three images which make up the user detection UI
	public Image personDetectedIcon, noPersonDetectedIcon,emptyCircleIcon;
	//Function called first automatically by Unity
	void Awake(){
		//Disable the person icon
		personDetectedIcon.enabled = false;
		//Enable the empty circle and the cross icon, indicating no person is detected
		noPersonDetectedIcon.enabled = emptyCircleIcon.enabled = true;
	}
	//KinectGestures.GestureListenerInterface virtual implementation
	public void UserDetected(long userId, int userIndex)
	{
		//Get a handle on the KinectManager via static Instance
	KinectManager manager = KinectManager.Instance;
		//Subscribe to the Wave Gesture
	manager.DetectGesture(userId, KinectGestures.Gestures.Wave);
		//Since person detected, enable the icons
		personDetectedIcon.enabled = emptyCircleIcon.enabled = true; 
		//disable the crossed circle
		noPersonDetectedIcon.enabled = false;
}
	
	//KinectGestures.GestureListenerInterface virtual implementation
public void UserLost(long userId, int userIndex)
{
		//debug
		print ("UserLost");
		//User lost so disable icon
		personDetectedIcon.enabled = false; 
		//enable the crossed circle
		noPersonDetectedIcon.enabled = emptyCircleIcon.enabled =true;//false;
		//Cancel the cursor clicking phase because user was lost
		if(cursor!=null)cursor.CancelClicking ();
}
	
	//KinectGestures.GestureListenerInterface virtual implementation
public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, 
                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
{
}
	
	//KinectGestures.GestureListenerInterface virtual implementation
public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, 
                             KinectInterop.JointType joint, Vector3 screenPos)
{
	string sGestureText = gesture + " detected";
		//Propogate the recognised gesture to the cursor controller
		if(cursor!=null)cursor.WaveRecognised();
	return true;
}

	//KinectGestures.GestureListenerInterface virtual implementation
public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, 
                             KinectInterop.JointType joint)
{
	return true;
}

}