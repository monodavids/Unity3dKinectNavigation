using UnityEngine;
using System.Collections;

//This class subscribes the button events when it is entered or exited by the cursor
public class HandControlDemoMenu : MonoBehaviour {
	//bool controlling whether the button is enabled or not
	public bool buttonEnabled = false;
	//The CursorController object
	public CursorController cursor;
	public CursorControllerDirectKinect cursorDirect;
	//The function which is called when a button is clicked
	public void ButtonClicked(){
		print ("Button Clicked");
	}
	//When the cursor enters a buttons bounds...
	public void ButtonEnter(){
		//If the button is enabled make the cursor start it's countdown click phase
		if (buttonEnabled) {
			if(cursor!=null)cursor.BeginClicking ();
			if(cursorDirect!=null)cursorDirect.BeginClicking ();

		}
		//debug
		print ("Button Enter");
	}
	//When the cursor exits a buttons bounds...
	public void ButtonExit(){
		//If the button is enabled make the cursor exit it's countdown click phase
		if (buttonEnabled) {
			if(cursor!=null)cursor.CancelClicking ();
			if(cursorDirect!=null)cursorDirect.CancelClicking ();
		}
		//debug
		print ("Button Exit");
	}
}
