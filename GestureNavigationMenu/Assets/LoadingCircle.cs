using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadingCircle : MonoBehaviour {
	public Image emptyCircle;
	public List<Image> images;
	public Color baseColour;
	public Color baseEmptyColour;
	public float totalTime;
	// Use this for initialization
	void Awake () {
		baseEmptyColour = baseColour * 0.1f;
		Reset ();
	}
	int currentIndex = 0;
	public void BeginLoading(){
		currentIndex = 0;
		LeanTween.alpha (emptyCircle.rectTransform, 1f, totalTime / 8);
		TweenInNextImage ();
	}

	void TweenInNextImage(){
		if(currentIndex%2!=0)AudioSource.PlayClipAtPoint (sfx [sfx.Count-1], Camera.main.transform.position);//sfx [(currentIndex-1)/2], Camera.main.transform.position);
		LeanTween.alpha (images [currentIndex].rectTransform, 1f, totalTime / 8);
		if(currentIndex<7)Invoke ("TweenInNextImage", totalTime / 8);
		currentIndex++;
	}
	// Update is called once per frame
	public void Reset () {
		currentIndex = 0;
		CancelInvoke ();
		foreach (Image i in images) {
			LeanTween.cancel(i.gameObject);
						i.color = new Color(baseColour.r,baseColour.g,baseColour.b,0f);
//						i.color.a = 0f;
		}
		LeanTween.cancel(emptyCircle.gameObject);
		emptyCircle.color = new Color(baseEmptyColour.r,baseEmptyColour.g,baseEmptyColour.b,0f);
	}
	public List<AudioClip>sfx;
	public void Click(){
		foreach(Image i in images)
			LeanTween.alpha (i.rectTransform, 0f, 1f);
		
		LeanTween.alpha (emptyCircle.rectTransform, 0f, 1f);
		AudioSource.PlayClipAtPoint (sfx [0], Camera.main.transform.position);
	}
}
