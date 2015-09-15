using UnityEngine;using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;
using Windows.Kinect;

//Struct needed to translate System screen cursor position
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

//Cursor controller initiates and controls a kinect hand controller which overrides the system cursor
public class CursorController : MonoBehaviour  {
	//The menu controller, which subscribes to cursor enter/exit events
	public HandControlDemoMenu menu;
	//Handle on the kinect manager in the scene
	public KinectManager kinectManager;
	//The Avatar controller whose hand transforms we subscribe to to control the cursor
	public AvatarController avatar;
	//Various transforms of the kinect controlled avatar we need
	public Transform rightHand, leftHand, rightShoulder, leftShoulder, hips;
	//Various necessary float variables
	float distanceBetweenShoulders;
	float virtualHeight, virtualWidth;
	float totalScreenWidth, totalScreenHeight;
	//The virtaul center of our bounding box
	Vector3 virtualCenter;
	//The bounding box, created only for visualisation
	//It defines the extent of the hand space which translates to screen space
	BoxCollider boundingBox;
	//The image of the cursor
	public Image handCursor;
	//The Canvas where the UI is drawn
	public Canvas myCanvas;
	//The demo button
	public Button guestButton;

	//First function called automatically by Unity to initialise everything
	void Awake(){
		//We first calculate the distance between the two shoulder transforms
		distanceBetweenShoulders = rightShoulder.position.x - leftShoulder.position.x;
		//Virtual width is half that
		virtualWidth = 2 * distanceBetweenShoulders;
		//Virtual height is the distance between the shoulder and the hips in Y
		virtualHeight = rightShoulder.position.y-hips.position.y;
		//The virtual centre is now found
		virtualCenter = new Vector3(virtualWidth * 0.5f,virtualHeight,0f);
		//get working resolution
		totalScreenWidth = Screen.width;
		totalScreenHeight = Screen.height;
		//debug
		print ("RESOLUTION: "+totalScreenWidth + " " + totalScreenHeight);
		print ("Current "+Screen.currentResolution.width+" "+Screen.currentResolution.height);
		//Instantiate the bounding (working area) box
		GameObject g = new GameObject ();
		g.name = "BoundingBox";
		boundingBox = g.AddComponent<BoxCollider> ();
		boundingBox.size = new Vector3 (virtualWidth, virtualHeight, virtualHeight);
		//Create primitives defining the upper most and lower most extents of the cursor area
		bottomLeftCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		topRightCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		handClampedCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		//Disable the renderer components of the primitives
		bottomLeftCube.renderer.enabled = topRightCube.renderer.enabled = handClampedCube.renderer.enabled = false;
		bottomLeftCube.transform.localScale = topRightCube.transform.localScale = handClampedCube.transform.localScale = Vector3.one * 0.05f;
		//Start idle animation of the cursor image
		LeanTween.rotateAroundLocal(handCursor.gameObject,Vector3.forward, -15f, 1f).setEase (LeanTweenType.easeInOutSine);
		LeanTween.rotateZ (handCursor.gameObject, -15f, 1f).setLoopPingPong ().setDelay (1f).setEase(LeanTweenType.easeInOutSine);
		//Tween the image alpha into view
		LeanTween.value (gameObject, UpdateCanvasGroupAlpha, 0f, 1f, 1f);
		//Disable component
		enabled = false;
		//debug
		print ("Set up correctly");
	}
	//Bool hasWaved set to false to begin with
	bool hasWaved = false;
	//function called when wave gesture recognised
	public void WaveRecognised(){
		//Once it hasn't already occured
		if (!hasWaved) {
			//Cancel the idle animation
						LeanTween.cancel (handCursor.gameObject);
			//Enable the menu button
			menu.buttonEnabled = true;
			//enable the component
						enabled = true;
			//Set the initial bool to true
			hasWaved = true;
				}
	}
	//This function is used by leantween to tween the alpha value of the cursor
	void UpdateCanvasGroupAlpha(float value){
		handCursor.color = new Color (handCursor.color.r, handCursor.color.g, handCursor.color.b, value);
	}
	//These game objects are used to define the working area of the avatar
	GameObject bottomLeftCube, topRightCube, handClampedCube;
	//The following functions override the System cursor functions
	[DllImport("user32.dll")]
	public static extern bool SetCursorPos(int X, int Y);
	[DllImport("user32.dll")]
	public static extern bool GetCursorPos(out POINT pos);
	// Update is called once per frame
	//Handle on the Loading circle
	public LoadingCircle loadingCircle;
	//Minimum move threshold value
	float moveThreshold = 0.001f;
	//This value decides whether we want to smoothly interpolate the value or jump straight to the new position
	public bool smoothMovement = false;
	//Called every frame
	void Update(){
		//Move the bounding box, centred above the hips
		boundingBox.transform.position = hips.position + virtualCenter;
		//Once a User is present
		if (kinectManager.IsUserDetected ()) {
						/*
		 * We must find the position of the hand constrained to the bounding box
		 * translate it into screen coordinates
		 */
			//Calculate the new positions of the extents
						Vector2 bottomLeft = new Vector2 (boundingBox.transform.position.x - boundingBox.bounds.extents.x, boundingBox.transform.position.y - boundingBox.bounds.extents.y);
						Vector2 topRight = new Vector2 (boundingBox.transform.position.x + boundingBox.bounds.extents.x, boundingBox.transform.position.y + boundingBox.bounds.extents.y);
			// Clamp the position of the hand within these extents			
			Vector2 handClamped = new Vector2 (Mathf.Clamp (rightHand.transform.position.x, bottomLeft.x, topRight.x), Mathf.Clamp (rightHand.transform.position.y, bottomLeft.y, topRight.y));
						//Reposition the visual primitives
						bottomLeftCube.transform.position = new Vector3 (bottomLeft.x, bottomLeft.y, boundingBox.transform.position.z);
						topRightCube.transform.position = new Vector3 (topRight.x, topRight.y, boundingBox.transform.position.z);
						handClampedCube.transform.position = new Vector3 (handClamped.x, handClamped.y, boundingBox.transform.position.z);
						//Calculate the position of the hand transform in terms of the extents
						float widthFactor = (Mathf.Abs (handClamped.x - bottomLeft.x) / (topRight.x - bottomLeft.x));
						float heightFactor = (Mathf.Abs (handClamped.y - bottomLeft.y) / (topRight.y - bottomLeft.y));
						//evaluate this position in terms of the screen space
						float xScaled = widthFactor * totalScreenWidth;
						float yScaled = (1 - heightFactor) * totalScreenHeight;
						//rate of movement, used only for smooth movement
						float rate = 2f;
						//extract the Systems cursor position into a new struct object
						POINT current = new POINT ();
						GetCursorPos (out current);
			//If smooth movement we lerp to the new position at the specified rate
			if(smoothMovement){
						xScaled = Mathf.Lerp(current.X,xScaled,Time.deltaTime*rate);
						yScaled = Mathf.Lerp (current.Y, yScaled, Time.deltaTime * rate);
			}
			//Set the cursor position
			SetCursorPos ((int)xScaled, (int)yScaled);
			//debug
//			print (xScaled.ToString()+","+yScaled.ToString());
			//Tranform the image to the new position on screen (above the cursor)
						Vector2 pos;
						RectTransformUtility.ScreenPointToLocalPointInRectangle (myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
						transform.position = myCanvas.transform.TransformPoint (pos);
			//debug
			if(debugText!=null){
				debugText.text = "Relative: "+widthFactor+","+heightFactor+'\n'+"Current: "+(int)xScaled+","+(int)yScaled;
			}
			//debug
						if (Input.GetKeyDown (KeyCode.Space))
								MouseControl.MouseClick ();

						if (Input.GetKeyDown (KeyCode.L)) {
								BeginClicking ();
						}
						if (Input.GetKeyDown (KeyCode.K))
								CancelClicking ();
				}
		else {
			//User is not detected so do nothing
			}
	}
	//Onscreen debug text
	public Text debugText;
	//This function performs a mouse click if the loading circle has completed and the cursor is still within the bounds of a butoon
	public void ClickIfStillHighlighted(){
		//tell the loading circle it has completed
			loadingCircle.Click();
			//tween the image out now a click has been performed
			LeanTween.scale (handCursor.rectTransform, Vector3.one * 1f, loadingCircle.totalTime);//.setEase(LeanTweenType.easeInSine);
		//perform mouse click
			MouseControl.MouseClick ();
		//Debug
			print ("Mouse Click");
	}
	//This function is called whenever the cursor enters the bounds of a button
	public void BeginClicking(){
		//Reset the loading circle back to the beginning
		loadingCircle.Reset ();
		//Tell it to start tweening
		loadingCircle.BeginLoading ();
		//Tween the cursor scale down as if it were pressing the button
		LeanTween.scale (handCursor.rectTransform, Vector3.one * 0.5f, loadingCircle.totalTime).setEase(LeanTweenType.easeInSine);
		//Invoke a method in the future to click the button if it is still highlighted (the total time the loading circle takes)
		Invoke ("ClickIfStillHighlighted", loadingCircle.totalTime+(loadingCircle.totalTime/8f));
	}
	//This function is called automatically when the cursor exits a button
	public void CancelClicking(){
		//Cancel all future invocations (i.e. Click if still highlighted)
		CancelInvoke ();
		//Reset everything
		handCursor.transform.localScale = Vector3.one;
		LeanTween.cancel (handCursor.gameObject);
		handCursor.color = Color.white;
		loadingCircle.Reset ();
	}

}
