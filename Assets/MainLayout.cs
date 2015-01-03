using UnityEngine;
using System.Collections;

public class MainLayout : MonoBehaviour {

	public bool MainMenu; public GameObject MainMenuObject;
	public bool game1; public GameObject game1Object;
	public GameObject game2Object;
	public GUITexture g1;
	public GUITexture g2;
	public GUITexture cursor1;
	private int SelectCount1;
	private int SelectCount2;

	public GameObject HandLeft;
	public GameObject HandRight;
	public GameObject Head;
	public GameObject Hip;

	private float[] HandLeftAvg = new float[2];
	private float[] HandRightAvg = new float[2];
	private float[] HeadAvg = new float[2];
	private float[] HipAvg = new float[2];

	private float[,] HandRightArray = new float[10,2];
	private float[,] HandLeftArray = new float[10,2];
	private float[,] HeadArray = new float[10,2];


	private float GUIratio = 0.6F;

	// Use this for initialization
	void Start () {
		SelectCount1 = 0;
		SelectCount2 = 0;
		MainMenu = true;
		game1 = false;
		MainMenuObject.SetActive (true);
		game1Object.SetActive (false);
		game2Object.SetActive (false);
	
	}
	
	// Update is called once per frame
	void Update () {

		for (int i=0; i<9; i++) {
		
			HandRightArray[i+1,0] = HandRightArray[i,0];
			HandLeftArray[i+1,0] = HandLeftArray[i,0];
			HeadArray[i+1,0] = HeadArray[i,0];

			HandRightArray[i+1,1] = HandRightArray[i,1];
			HandLeftArray[i+1,1] = HandLeftArray[i,1];
			HeadArray[i+1,1] = HeadArray[i,1];

		}

		HandLeftArray [0,0] = HandLeft.transform.position.x;
		HandLeftArray [0,1] = HandLeft.transform.position.y;

		HandRightArray [0,0] = HandRight.transform.position.x;
		HandRightArray [0,1] = HandRight.transform.position.y;

		HeadArray [0,0] = Head.transform.position.x;
		HeadArray [0,1] = Head.transform.position.y;

		HeadAvg[0] = 0; HeadAvg [1] = 0; HandLeftAvg[0] = 0; HandLeftAvg [1] = 0;
		HandRightAvg[0] = 0; HandRightAvg [1] = 0;

		for (int i=0; i<9; i++) {
			for(int j=0;j<2;j++){
			HandLeftAvg[j] = HandLeftAvg[j]+HandLeftArray[i,j];
			HandRightAvg[j] = HandRightAvg[j]+HandRightArray[i,j];
		//	HipAvg[0] = HipAvg[0]+HipArray[i,0];
			HeadAvg[j] = HeadAvg[j]+HeadArray[i,j];
			}
		}
		for(int i =0;i<2;i++){

			HandLeftAvg[i] = HandLeftAvg[i]/10.0F;
			HandRightAvg[i] = HandRightAvg[i]/10.0F;
			HeadAvg[i] = HeadAvg[i]/10.0F;

		}
		/*
		cursor1.transform.position = Camera.main.ScreenToViewportPoint (Input.mousePosition);
		*/

	//	/*
		if (HandLeft.transform.position.y < Hip.transform.position.y && HandRight.transform.position.y < Hip.transform.position.y) {
			cursor1.transform.position = new Vector3(0.5F,0.5F,0.0F);		
		}
		else{
			if(HandLeftAvg[1] > HandRightAvg[1]){
				cursor1.transform.position= new Vector3((float)((HandLeftAvg[0]-HeadAvg[0])/GUIratio)+0.5F,
				          (float)((HandLeftAvg[1]-HeadAvg[1])/GUIratio)+0.5F,0.0F);}
			if(HandLeftAvg[1] < HandRightAvg[1]){
				cursor1.transform.position= new Vector3((float)((HandRightAvg[0]-HeadAvg[0])/GUIratio)+0.5F,
				          (float)((HandRightAvg[1]-HeadAvg[1])/GUIratio)+0.5F,0.0F);}
		}

	//	*/


	//	if (MainMenu == true) {
						Debug.Log ("chutiyapa");
						if (g1.HitTest (Camera.main.ViewportToScreenPoint (cursor1.transform.position))
		    ||
		    g1.HitTest(Input.mousePosition)) {
								SelectCount1++;
								if (SelectCount1 == 70) {
				//						MainMenu = false;
				//						game1 = true;
				//						game1Object.SetActive (true);
				//						game2Object.SetActive (false);
				//						MainMenuObject.SetActive (false);
				//						SelectCount1=0;
				//	cursor1.gameObject.SetActive(false);
				Application.LoadLevel(1);
				}
						} else {
								SelectCount1 = 0;
						}
			
			
			
						Debug.Log ("chutiyapa");
						if (g2.HitTest (Camera.main.ViewportToScreenPoint (cursor1.transform.position)) 
		    ||
		    g2.HitTest(Input.mousePosition)) {
										SelectCount2++;
										if (SelectCount2 == 70) {
				//							MainMenu = false;
				//							game1 = false;
				//							game2Object.SetActive(true);
				//							game1Object.SetActive (false);
				//							MainMenuObject.SetActive (false);
				//	SelectCount2=0;
				Application.LoadLevel(2);		
			}
						} else {
								SelectCount2 = 0;
						}

	

						
		//		}
	}
}
