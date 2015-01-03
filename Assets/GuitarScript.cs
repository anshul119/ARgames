using UnityEngine;
using System.Collections;

public class GuitarScript : MonoBehaviour {

	public GameObject spine;
	public GameObject HandRight;
	public GameObject HandLeft;
	public GameObject t1;

	private float angle1;
	private float difx;
	private float dify;

	public GameObject[] StrAnim = new GameObject[6];
//	public GameObject StrAnim2;
	int flag1;

	public GameObject[] StrEnda = new GameObject[6];
	public GameObject[] StrEndb = new GameObject[6];
	private float[] deltaY = new float[6];
	private float[] deltaX = new float[6];
	private float const1;
	private Vector3 PrevDotPosition;
	private Vector3 PrevMousePosition;

	public GameObject[] AudioSources = new GameObject[6]; 

	//public GameObject AudioSource1;

	private float[] TimeVibrate = new float[6];
	private Vector3 VibrateNormal;
	private bool[,] StrNormal = new bool[6,2];

	public GameObject cursor1;
	public GameObject dot1;
	public GameObject Shoulders;
	private Vector3 DotPos;

	private ArrayList HandRecord = new ArrayList();
	private ArrayList SpineRecord = new ArrayList();
	private Vector3 SpineSum;
	private ArrayList ShouldersRecord = new ArrayList();
	private Vector3 ShouldersSum;
	private Vector3 HandSum;
	private Vector3 HandAvg;

	// Use this for initialization
	void Start () {
	//	cursor1.SetActive (false);
	//	Camera.main.orthographic = true;
		for (int i=0; i<6; i++) {
			StrNormal[i,0] = true;
			StrNormal[i,1] = true;
			TimeVibrate[i] = -10.0F;
		}
		flag1 = 0;
			
	}
	
	// Update is called once per frame
	void Update () {

		if (HandRecord.Count != 10) {
						HandRecord.Add (HandRight.transform.position);
			HandSum = HandSum + HandRight.transform.position;
				} else {
			HandRecord.Add(HandRight.transform.position);
			HandSum = HandSum + HandRight.transform.position;
			HandSum = HandSum - (Vector3)HandRecord[0]; 
			HandRecord.RemoveAt(0);
		}

		if (SpineRecord.Count != 5) {
			SpineRecord.Add (spine.transform.position);
			ShouldersRecord.Add (Shoulders.transform.position);
			SpineSum = SpineSum + spine.transform.position;
			ShouldersSum = ShouldersSum + Shoulders.transform.position;
		} else {
			SpineRecord.Add(spine.transform.position);
			ShouldersRecord.Add(Shoulders.transform.position);
			SpineSum = SpineSum + spine.transform.position;
			ShouldersSum = ShouldersSum + Shoulders.transform.position;
			SpineSum = SpineSum - (Vector3)SpineRecord[0]; 
			ShouldersSum = ShouldersSum - (Vector3)ShouldersRecord[0]; 
			SpineRecord.RemoveAt(0);
			ShouldersRecord.RemoveAt(0);
		}

		HandAvg = HandSum / 10;


	//	dot1.transform.position = new Vector2(0.6F + 0.6F*(HandRight.transform.position.x-spine.transform.position.x),
	//	                                      0.6F + 0.6F*( HandRight.transform.position.y - Shoulders.transform.position.y));

		dot1.transform.position = new Vector2(0.6F + 0.6F*(HandAvg.x-(SpineSum.x/5)),
		                                      0.6F + 0.6F*(HandAvg.y - (ShouldersSum.y/5)));


		DotPos = Camera.main.ViewportToScreenPoint (dot1.transform.position);
	//	DotPos = Input.mousePosition;

		for (int i =0; i<6; i++) {
						deltaX [i] = Camera.main.WorldToScreenPoint (StrEnda [i].transform.position).x - 
								Camera.main.WorldToScreenPoint (StrEndb [i].transform.position).x;
						deltaY [i] = Camera.main.WorldToScreenPoint (StrEnda [i].transform.position).y - 
								Camera.main.WorldToScreenPoint (StrEndb [i].transform.position).y;

						const1 = deltaX [i] * (Camera.main.WorldToScreenPoint (StrEnda [i].transform.position).y) - 
								deltaY [i] * (Camera.main.WorldToScreenPoint (StrEnda [i].transform.position).x);
			if(PrevDotPosition.y > DotPos.y || PrevDotPosition.y <= DotPos.y){
						if ((((deltaX [i] * PrevDotPosition.y) - (deltaY [i] * PrevDotPosition.x) - const1) *
								((deltaX [i] * DotPos.y) - (deltaY [i] * DotPos.x) - const1)) < 0 ||
								(((deltaX [i] * PrevDotPosition.y) - (deltaY [i] * PrevDotPosition.x) - const1) *
								((deltaX [i] * DotPos.y) - (deltaY [i] * DotPos.x) - const1)) == 0) {
				
								if ((DotPos.x < Camera.main.WorldToScreenPoint (StrEnda [i].transform.position).x 
										&& DotPos.x > Camera.main.WorldToScreenPoint (StrEndb [i].transform.position).x)
			 																	  ||
										(DotPos.x > Camera.main.WorldToScreenPoint (StrEnda [i].transform.position).x 
										&& DotPos.x < Camera.main.WorldToScreenPoint (StrEndb [i].transform.position).x)) {
										
										AudioSources [i].audio.Play ();
										TimeVibrate [i] = Time.time;
								}
						}
				}
				}


		flag1++;
		difx = (100.0F)*(spine.transform.position.x - HandLeft.transform.position.x);
		dify =   (100.0F)*(HandLeft.transform.position.y - spine.transform.position.y);
	
		angle1 = Mathf.Atan (dify / difx);
	//	angle1 = ((1.0F * angle1) / (Mathf.PI));

		t1.guiText.text = (100.0F * HandLeft.transform.position.y).ToString ();

	//	transform.rotation = new Quaternion(-Mathf.Sin(angle1)/6.0F,0,0,1);

	//	if (StrAnim1.activeSelf) {
		/*
		if (flag1 == 2) {
			StrAnim[0].transform.position = new Vector3(StrAnim[0].transform.position.x ,StrAnim[0].transform.position.y+0.015F,
			                                          StrAnim[0].transform.position.z);
		}

		if(flag1==4){
	//	StrAnim1.SetActive(!StrAnim1.activeSelf);
	//	StrAnim2.SetActive(!StrAnim2.activeSelf);
			StrAnim[0].transform.position = new Vector3(StrAnim[0].transform.position.x ,StrAnim[0].transform.position.y-0.015F,
			                                          StrAnim[0].transform.position.z);
			//	}
			flag1=0;
		}
	*/
	//	Camera.main.WorldToScreenPoint(Input.mousePosition) == Camera.main.
		Vibrate ();

		PrevDotPosition = DotPos;
		PrevMousePosition = Input.mousePosition;
	}


	void Vibrate(){

		VibrateNormal = (StrEnda [0].transform.position - StrEnda[5].transform.position);
		VibrateNormal.Normalize ();

		for (int i =0; i<6; i++) {

			if(Time.time < TimeVibrate[i]+0.3F || StrNormal[i,0] == false || StrNormal[i,1] == false){
				if(StrNormal[i,0] == true && StrNormal[i,1] == true){
					StrAnim[i].transform.position = StrAnim[i].transform.position + 
						0.015F*VibrateNormal;
					StrNormal[i,0] = false;
					continue;
				}
				else if(StrNormal[i,0] == false && StrNormal[i,1] == true){
					StrAnim[i].transform.position = StrAnim[i].transform.position + 
						0.015F*VibrateNormal;
					StrNormal[i,1] = false;
					continue;
				}
				else if(StrNormal[i,0] == false && StrNormal[i,1] == false){
					StrAnim[i].transform.position = StrAnim[i].transform.position - 
						0.015F*VibrateNormal;
					StrNormal[i,0] = true;
					continue;
				}
				else if(StrNormal[i,0] == true && StrNormal[i,1] == false){
					StrAnim[i].transform.position = StrAnim[i].transform.position - 
						0.015F*VibrateNormal;
					StrNormal[i,1] = true;
					continue;
				}
			}
		}

	}

}
