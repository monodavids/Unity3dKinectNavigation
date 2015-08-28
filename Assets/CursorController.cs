using UnityEngine;using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;
using Windows.Kinect;

public struct POINT
{
	public int X;
	public int Y;
	
	public POINT(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}
}


public class CursorController : MonoBehaviour  {
	public HandControlDemoMenu menu;
	public KinectManager kinectManager;
	public AvatarController avatar;
	public Transform rightHand, leftHand, rightShoulder, leftShoulder, hips;
	float distanceBetweenShoulders;
	float virtualHeight, virtualWidth;
	float totalScreenWidth, totalScreenHeight;
	float actualScreenWidth, actualScreenHeight;
	Vector3 virtualCenter;
	BoxCollider boundingBox;
	public Image handCursor;
	public Canvas myCanvas;
	public Button guestButton;


	void Awake(){
		//Screen.showCursor = false;
		distanceBetweenShoulders = rightShoulder.position.x - leftShoulder.position.x;
		virtualWidth = 2 * distanceBetweenShoulders;
		virtualHeight = rightShoulder.position.y-hips.position.y;
		virtualCenter = new Vector3(virtualWidth * 0.5f,virtualHeight,0f);

		totalScreenWidth = Screen.width;
		totalScreenHeight = Screen.height;
		print ("RESOLUTION: "+totalScreenWidth + " " + totalScreenHeight);
		//Debug.Break ();
		print ("Current "+Screen.currentResolution.width+" "+Screen.currentResolution.height);
		actualScreenWidth = Screen.currentResolution.width;
		actualScreenHeight = Screen.currentResolution.height;
		GameObject g = new GameObject ();
		g.name = "BoundingBox";
		boundingBox = g.AddComponent<BoxCollider> ();
		boundingBox.size = new Vector3 (virtualWidth, virtualHeight, virtualHeight);
		bottomLeftCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		topRightCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		handClampedCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		bottomLeftCube.renderer.enabled = topRightCube.renderer.enabled = handClampedCube.renderer.enabled = false;
		bottomLeftCube.transform.localScale = topRightCube.transform.localScale = handClampedCube.transform.localScale = Vector3.one * 0.05f;
		//handCursor.color = new Color(handCursor.color.r, handCursor.color.g, handCursor.color.b, 1f);
		//handCursor.CrossFadeAlpha (1f, 1f, true);
		LeanTween.rotateAroundLocal(handCursor.gameObject,Vector3.forward, -15f, 1f).setEase (LeanTweenType.easeInOutSine);
		//enabled = false;
		LeanTween.rotateZ (handCursor.gameObject, -15f, 1f).setLoopPingPong ().setDelay (1f).setEase(LeanTweenType.easeInOutSine);
		LeanTween.value (gameObject, UpdateCanvasGroupAlpha, 0f, 1f, 1f);
		enabled = false;
		print ("Set up correctly");
	}
	bool hasWaved = false;
	public void WaveRecognised(){
		if (!hasWaved) {
						LeanTween.cancel (handCursor.gameObject);
					//	genderUI.TransitionIn ();
			menu.buttonEnabled = true;
						enabled = true;
			hasWaved = true;
				}
	}
//	public GenderSelectionController genderUI;

	void UpdateCanvasGroupAlpha(float value){
		handCursor.color = new Color (handCursor.color.r, handCursor.color.g, handCursor.color.b, value);
	}
	GameObject bottomLeftCube, topRightCube, handClampedCube;
	[DllImport("user32.dll")]
	public static extern bool SetCursorPos(int X, int Y);
	[DllImport("user32.dll")]
	public static extern bool GetCursorPos(out POINT pos);
	// Update is called once per frame
	public LoadingCircle loadingCircle;
	float xPrevious=0f, yPrevious=0f;
	float moveThreshold = 0.001f;
	int count = 0;
	void Update(){
		boundingBox.transform.position = hips.position + virtualCenter;
		if (kinectManager.IsUserDetected ()) {
						/*
		 * We must find the position of the hand constrained to the bounding box
		 * translate it into screen coordinates
		 */
						//print ("Bounding box size: "+boundingBox.size.x+","+boundingBox.size.y);
						//print ("Bounding box position: " + boundingBox.transform.position);
						//print ("Right hand position: " + rightHand.transform.position);

						Vector2 bottomLeft = new Vector2 (boundingBox.transform.position.x - boundingBox.bounds.extents.x, boundingBox.transform.position.y - boundingBox.bounds.extents.y);
						Vector2 topRight = new Vector2 (boundingBox.transform.position.x + boundingBox.bounds.extents.x, boundingBox.transform.position.y + boundingBox.bounds.extents.y);
						Vector2 handClamped = new Vector2 (Mathf.Clamp (rightHand.transform.position.x, bottomLeft.x, topRight.x), Mathf.Clamp (rightHand.transform.position.y, bottomLeft.y, topRight.y));
		
						bottomLeftCube.transform.position = new Vector3 (bottomLeft.x, bottomLeft.y, boundingBox.transform.position.z);
						topRightCube.transform.position = new Vector3 (topRight.x, topRight.y, boundingBox.transform.position.z);
						handClampedCube.transform.position = new Vector3 (handClamped.x, handClamped.y, boundingBox.transform.position.z);

		
						// the hand has moved enough to update screen position (jitter control / smoothing)
						//if (Mathf.Abs (handClamped.x - xPrevious) > moveThreshold || Mathf.Abs (handClamped.y - yPrevious) > moveThreshold) {

						//handClamped += new Vector2 (boundingBox.bounds.extents.x, boundingBox.bounds.extents.y);
						//print ("Before:"+handClamped.x + " " + handClamped.y);
						float widthFactor = (Mathf.Abs (handClamped.x - bottomLeft.x) / (topRight.x - bottomLeft.x));
						float heightFactor = (Mathf.Abs (handClamped.y - bottomLeft.y) / (topRight.y - bottomLeft.y));
//						print ("Factored :"+widthFactor + " " + heightFactor);

						float xScaled = widthFactor * totalScreenWidth;// * (actualScreenWidth/totalScreenWidth);
						float yScaled = (1 - heightFactor) * totalScreenHeight;// * (actualScreenHeight/totalScreenHeight);
						//float xScaled = (rightHand.position.x - boundingBox.transform.position.x) / (boundingBox.size.x * 2) * totalScreenWidth;
						//float yScaled = (rightHand.position.y - boundingBox.transform.position.y) / (boundingBox.size.y * 2)* totalScreenHeight;
					//	print ("Mouse Position: "+xScaled + " " + yScaled);
						float rate = 2f;
						POINT current = new POINT ();
						GetCursorPos (out current);
					//	print ("Current Position:"+current.X+","+current.Y);
						//	xScaled = Mathf.Lerp(current.X,xScaled,Time.deltaTime*rate);
						yScaled = Mathf.Lerp (current.Y, yScaled, Time.deltaTime * rate);
						//MouseControl.MouseMove (new Vector3 (totalScreenWidth*0.5f,totalScreenHeight*0.5f,0), new GUIText ());
						SetCursorPos ((int)xScaled, (int)yScaled);
						xPrevious = handClamped.x;
						yPrevious = handClamped.y;
						Vector2 pos;
						RectTransformUtility.ScreenPointToLocalPointInRectangle (myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
						//transform.position = myCanvas.transform.TransformPoint(pos);
						transform.position = myCanvas.transform.TransformPoint (pos);
			if(debugText!=null){
				debugText.text = "Relative: "+widthFactor+","+heightFactor+'\n'+"Current: "+(int)xScaled+","+(int)yScaled;
			}
						//UnityEngine.EventSystems.PointerEventData update = new UnityEngine.EventSystems.PointerEventData (eventSystem);
						//update.position = handCursor.transform.position;
						//transform.position = myCanvas.transform.TransformPoint (pos);
						//		}
						if (Input.GetKeyDown (KeyCode.Space))
								MouseControl.MouseClick ();

						if (Input.GetKeyDown (KeyCode.L)) {
								BeginClicking ();
						}
						if (Input.GetKeyDown (KeyCode.K))
								CancelClicking ();


//		Rect rt = GUILayoutUtility.g (guestButton, GUIStyle.none);
//		if (rt.Contains(Event.current.mousePosition)){
//			print (count);
//			count++;
//		}
//			print ("moving cursor");
						/*Vector2 cursorPosition
		TrackHandMovement ();
		Vector2 handPosition = new Vector2 (RightHandX, RightHandY);
		print (handPosition);*/
				} else {
			//handCursor.transform.position = Vector3.zero;
			}
	}
	//bool guestButtonHighlighted;
	public Text debugText;
	public void ClickIfStillHighlighted(){
		//if (guestButtonHighlighted) {
			loadingCircle.Click();
			LeanTween.scale (handCursor.rectTransform, Vector3.one * 1f, loadingCircle.totalTime);//.setEase(LeanTweenType.easeInSine);
			//LeanTween.alpha(handCursor.rectTransform,0f,1f);
			MouseControl.MouseClick ();
			print ("Mouse Click");
		//}
	}
	public UnityEngine.EventSystems.EventSystem eventSystem;
	public void BeginClicking(){
		
		loadingCircle.Reset ();
		loadingCircle.BeginLoading ();
		//handCursor.transform.localScale = Vector3.one*0.75f;
		LeanTween.scale (handCursor.rectTransform, Vector3.one * 0.5f, loadingCircle.totalTime).setEase(LeanTweenType.easeInSine);
		//LeanTween.alpha(handCursor.rectTransform,0f,loadingCircle.totalTime*0.5f);
		Invoke ("ClickIfStillHighlighted", loadingCircle.totalTime+(loadingCircle.totalTime/8f));
	}
	public void CancelClicking(){
		CancelInvoke ();
		handCursor.transform.localScale = Vector3.one;
		LeanTween.cancel (handCursor.gameObject);
		handCursor.color = Color.white;
		loadingCircle.Reset ();
	}

}
