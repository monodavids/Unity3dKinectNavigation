using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//This class controls the cursors loading UI, when it is hovering over a button
public class LoadingCircle : MonoBehaviour {
	//The UI image as the clicking begins
	public Image emptyCircle;
	//The overlayed circles which fill up as time progresses
	public List<Image> images;
	//The base color the circles turn into
	public Color baseColour;
	//The base color the circles begin at
	public Color baseEmptyColour;
	//The total time it takes to click the button upon entering its bound
	public float totalTime;
	// Use this for initialization
	void Awake () {
		//Set the base empty colour to be 10% of its final colour
		baseEmptyColour = baseColour * 0.1f;
		//Reset the process to the beginning for good measure
		Reset ();
	}
	//There are 8 circles to be filled before the click event is fired, start index = 0
	int currentIndex = 0;
	//This function is called once the cursor enters a buttons bounds
	public void BeginLoading(){
		//start with the first image
		currentIndex = 0;
		//LeanTween is the Lerping library used to blend the images colours
		//Blend in the circle of 8 empty circles
		LeanTween.alpha (emptyCircle.rectTransform, 1f, totalTime / 8);
		//Begin blending in the first circle
		TweenInNextImage ();
	}

	void TweenInNextImage(){
		//Every second circle emit the beep sfx
		if(currentIndex%2!=0)AudioSource.PlayClipAtPoint (sfx [sfx.Count-1], Camera.main.transform.position);//sfx [(currentIndex-1)/2], Camera.main.transform.position);
		//start blending the circle at the currentIndex
		LeanTween.alpha (images [currentIndex].rectTransform, 1f, totalTime / 8);
		//If we aren't at the end of the list of circle images, Invoke this function to begin belnding the next
		if(currentIndex<7)Invoke ("TweenInNextImage", totalTime / 8);
		//Increment the index
		currentIndex++;
	}
	//Reset
	public void Reset () {
		//Reset index
		currentIndex = 0;
		//Cancel any left over Invocations
		CancelInvoke ();
		//for every child circle image
		foreach (Image i in images) {
			//cancel any LeanTween operations on it
			LeanTween.cancel(i.gameObject);
			//Reset the color to the start color
						i.color = new Color(baseColour.r,baseColour.g,baseColour.b,0f);
		}
		//cancel any LeanTween operations on the base circle image
		LeanTween.cancel(emptyCircle.gameObject);
		//Reset the color to the start color
		emptyCircle.color = new Color(baseEmptyColour.r,baseEmptyColour.g,baseEmptyColour.b,0f);
	}
	//List of sfx, comprises of a single loading beep and a completion beep
	public List<AudioClip>sfx;
	//This function is called from CursorController
	public void Click(){
		//Blend out all overlayed child circles
		foreach(Image i in images)
			LeanTween.alpha (i.rectTransform, 0f, 1f);
		
		//Blend out base circle image
		LeanTween.alpha (emptyCircle.rectTransform, 0f, 1f);
		//Play the click sfx
		AudioSource.PlayClipAtPoint (sfx [0], Camera.main.transform.position);
	}
}
