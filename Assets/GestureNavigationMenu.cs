using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GestureNavigationMenu : MonoBehaviour {
	public List<Button> buttons;
	public GameObject dialog;
	public Text dialogText;
	int currentHighlightedButton = 0;
	// Use this for initialization
	void Start () {
		HighlightButton ();
	}

	void HighlightButton(){
		currentHighlightedButton = Mathf.Clamp (currentHighlightedButton, 0, buttons.Count-1);
		print (currentHighlightedButton);
		buttons [currentHighlightedButton].Select ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SwipeUp(){
		currentHighlightedButton--;
		print ("Up "+ currentHighlightedButton);
		HighlightButton ();
	}

	public void SwipeDown(){
		currentHighlightedButton++;
		print ("Down "+ currentHighlightedButton);
		HighlightButton ();
	}

	public void SwipeRight(){
		print ("Right");
		dialogText.text = "You have chosen Button " + currentHighlightedButton + 1 + ".\n\n" + "Swipe RIGHT to confirm" + "\n\n" + "OR" + "\n\n" + "Swipe LEFT to go back";
		dialog.SetActive (true);
	}

	public void SwipeLeft(){
		print ("Left");
		dialog.SetActive (false);
	}
}
