using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GestureNavigationMenu : MonoBehaviour {
	//List of all buttons, in vertical UI list
	public List<Button> buttons;
	//Dialog GameObject
	public GameObject dialog;
	//UnityEngine.UI.Text component, instructional text of the dialog
	public Text dialogText;
	//index of highlighted button (i.e. the button that will be selected when a right swipe is performed)
	int currentHighlightedButton = 0;
	// Use this for initialization
	void Start () {
		//start by highlighting one of the possible buttons (index at currentHighlightedButton)
		HighlightButton ();
	}
	//This function is called when a vertical swipe is performed, it changes the current highlighted button
	void HighlightButton(){
		//ensure the index is not less than 0 or greater than the amount of buttons in the list by clamping between those values
		currentHighlightedButton = Mathf.Clamp (currentHighlightedButton, 0, buttons.Count-1);
		//debug
		print (currentHighlightedButton);
		//UnityEngine.UI.Button function to "select" (highlight) but not click the button, it is akin to hovering over with a mouse
		buttons [currentHighlightedButton].Select ();
	}

	//Function to be called when a swipe up is performed
	public void SwipeUp(){
		//decrement index
		currentHighlightedButton--;
		//debug
		print ("Up "+ currentHighlightedButton);
		//Focus on the button at the updated index
		HighlightButton ();
	}
	
	//Function to be called when a swipe up is performed
	public void SwipeDown(){
		//increment index
		currentHighlightedButton++;
		//debug
		print ("Down "+ currentHighlightedButton);
		//Focus on the button at the updated index
		HighlightButton ();
	}
	
	//Function to be called when a swipe up is performed
	public void SwipeRight(){
		//debug
		print ("Right");
		//set dialog text
		dialogText.text = "You have chosen Button " + currentHighlightedButton + 1 + ".\n\n" + "Swipe RIGHT to confirm" + "\n\n" + "OR" + "\n\n" + "Swipe LEFT to go back";
		//enable the dialog
		dialog.SetActive (true);
	}
	
	//Function to be called when a swipe left is performed
	public void SwipeLeft(){
		//debug
		print ("Left");
		//cancel dialog
		dialog.SetActive (false);
	}
}
