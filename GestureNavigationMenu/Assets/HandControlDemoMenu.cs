using UnityEngine;
using System.Collections;

public class HandControlDemoMenu : MonoBehaviour {
	public bool buttonEnabled = false;
	public CursorController cursor;
	public void ButtonClicked(){
		print ("Button Clicked");
	}
	public void ButtonEnter(){
		if(buttonEnabled)cursor.BeginClicking ();
		print ("Button Enter");
	}
	public void ButtonExit(){
		if(buttonEnabled)cursor.CancelClicking ();
		print ("Button Exit");
	}
}
