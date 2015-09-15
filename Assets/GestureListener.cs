using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//This class is needed to intercept Kinects in built Gestures
//It does so by extending from the KinectGestures.GestureListenerInterface
//You must implement all of the virtual functions to avoid compilation errors
//for our use we only really need to flesh out the UserDetected, UserLost & GestureCompleted methods
//And propogate the info to the GestureNavigationMenu
public class GestureListener : MonoBehaviour,KinectGestures.GestureListenerInterface {
	//Handle on our GestureNavigationMenu script
	public GestureNavigationMenu menu;
	
	// GUI Text to display the gesture messages.
	public Text GestureInfo;
	
	// private bool to track if progress message has been displayed
	private bool progressDisplayed;
	
	//Virtual override of KinectGestures.GestureListenerInterface.UserDetected
	public void UserDetected(long userId, int userIndex)
	{
		// as an example - detect these user specific gestures
		//Subscribing to the following gestures
		KinectManager manager = KinectManager.Instance;
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeUp);
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeDown);
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);

		print ("UserDetected");
		if(GestureInfo != null)
		{
			GestureInfo.text = "SwipeLeft, SwipeRight, Squat, Push or Pull.";
		}
	}
	
	//Virtual override of KinectGestures.GestureListenerInterface.UserLost
	public void UserLost(long userId, int userIndex)
	{
		if(GestureInfo != null)
		{
			GestureInfo.text = string.Empty;
		}
	}
	
	//Virtual override of KinectGestures.GestureListenerInterface.GestureInProgress
	public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
	{

	}
	
	//Virtual override of KinectGestures.GestureListenerInterface.GestureCompleted
	//Once completed a gesture propogate the gesture to the GestureNavigationMenu
	public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                             KinectInterop.JointType joint, Vector3 screenPos)
	{
		string sGestureText = gesture + " detected";
		
		if(GestureInfo != null)
		{
			GestureInfo.text = sGestureText;
		}
		//depending on which gesture is performed, call the relevant function in the GestureNavigationMenu
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
	
	//Virtual override of KinectGestures.GestureListenerInterface.GestureCancelled
	public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                             KinectInterop.JointType joint)
	{
		return true;
	}

}
