using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private float JumpTimeK=2;
	private float TurnTime=0;
	private int HurdleProb;
	private int HurdleNum;
	public GameObject c1;
	public GameObject light1;
	public int [] dir = new int[2];
	private float SpeedConst;
	public int PreviousWay;
	public int CurrentWay;
	public int NextWay;
	public int[] CurrentWayStartingPoint = new int[2];		// x and z component of the position
	private int SlowlyRotateCamera;							
	private bool Trigger=false;
	private bool GameOver=false;
	private float GameOverTime=-3.0F;
	public GameObject t1;
	public GameObject t2;
	public int BeforeDistance = 0;
	public Vector3 CameraDir;
	public int JumpConst=0;
	public float JumpTime=-2.0F;
//	public float TurnTime;
	public bool[] IsGapPresent = new bool[8];

	public int[] WaysLength = new int[8];					// length of ways in same order

	public GameObject[] Ways = new GameObject[8];			// array of all the ways
	public  int[] WaysDirection = new int[8];				// direction of the ways in same order. true for left, false for right

	public GameObject[] Hurdles = new GameObject[2];
	public GUITexture g1;

	public GameObject RightHand;
	public GameObject LeftHand;
	public GameObject Spine;
	public GameObject Hip;
	public GameObject Head;
	public GameObject LeftShoulder;
	public GameObject RightShoulder;
	private Vector3 HipSum;
	private Vector3 HipAvg;

	private bool LeftInst = false;
	private bool RightInst = false;
	private bool JumpGInst = false;
	private bool JumpHInst = false;
	private bool OverInst = false;

	public GUIText ScoreGUI;
	private int Score;
	public GameObject HighScoreDisplay;
	private int HighScore;
	private bool isHurdle = false;
	private float TimeAboveHurdle;

	public GameObject game1;
	public GameObject MainMenu;
	private float StartTime;
	private float sf1; 		// speed factor1

	private ArrayList RightHandRecord = new ArrayList();
	private Vector3 RightHandSum;
	private ArrayList RightHandVelRecord = new ArrayList();
	private Vector3 RightHandVelSum;
	private ArrayList LeftHandRecord = new ArrayList();
	private Vector3 LeftHandSum;
	private ArrayList LeftHandVelRecord = new ArrayList();
	private Vector3 LeftHandVelSum;
	public ArrayList HipRecord = new ArrayList();

	public GameObject Instructions;
//	public Color InsColor = Color.white;
	private float InstSetTime;

	// Use this for initialization
	void Start () {
		HighScore = PlayerPrefs.GetInt("HighScore");
		HighScoreDisplay.guiText.text = HighScore.ToString ();
		sf1 = 1.0F;
		StartTime = Time.time;
		BeforeDistance = 20;
		GameOver = false;
		dir [0] = 1;
		dir [1] = 0;
	//	SpeedConst = 6;
		CurrentWay = 1;
		NextWay = GetRandom();
		CurrentWayStartingPoint [0] = 0;
		CurrentWayStartingPoint [1] = 0;
		SlowlyRotateCamera = 0;
		Ways [1].SetActive (true);
		Ways [NextWay].SetActive (true);
		Ways [NextWay].transform.position = new Vector3 (CurrentWayStartingPoint[0]+dir[0]*WaysLength[CurrentWay],
		            Ways [NextWay].transform.position.y,CurrentWayStartingPoint[1]+2*WaysDirection[CurrentWay]);
		Ways [NextWay].transform.Rotate (0,Ways[CurrentWay].transform.rotation.y-90,0);
//		c1.transform.position = new Vector3 (transform.position.x - 8, c1.transform.position.y, transform.position.z);
		CameraDir = new Vector3 (1, 0, 0);
		Score = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("r")) {
			Application.LoadLevel(0);		
		}

		if (Time.time > InstSetTime + 3.0F && Instructions.activeSelf == true) {
			Instructions.guiText.fontSize = 1;
			Instructions.SetActive(false);
		}
		if (Instructions.activeSelf == true && Instructions.guiText.fontSize < 55) {
			Instructions.guiText.fontSize = Instructions.guiText.fontSize+5;		
		}
	
		if ((Time.time - StartTime) > 0.0F && (Time.time - StartTime) < 1.0F) {
			Instructions.SetActive(true);
			Instructions.guiText.text = "Stand Still";
			InstSetTime = Time.time;
		}

	
	//	Instructions.guiText.color = InsColor;


		SpeedConst = sf1 * (((Time.time-StartTime) / 120.0F) + 6.0F);
		/*
		HipSum = 0;
		for (int i =0; i<14; i++) {
			HipRecord[i] = HipRecord[i+1];
			HipSum = HipSum+HipRecord[i];
		}
		HipRecord [14] = Hip.transform.position.y * 100;
		HipSum = HipSum + HipRecord [14];
		HipAvg = HipSum / 15;
		*/

		if (HipRecord.Count != 15) {
			HipRecord.Add (Hip.transform.position);
			HipSum = HipSum + Hip.transform.position;
		} else {
			HipRecord.Add(Hip.transform.position);
			HipSum = HipSum + Hip.transform.position;
			HipSum = HipSum - (Vector3)HipRecord[0]; 
			HipRecord.RemoveAt(0);
		}
		HipAvg = HipSum / 15;

		if (RightHandRecord.Count != 10) {
			RightHandRecord.Add (RightHand.transform.position);
			RightHandVelRecord.Add ((RightHand.transform.position-(Vector3)RightHandRecord[RightHandRecord.Count-2])/Time.deltaTime);
			LeftHandRecord.Add (LeftHand.transform.position);
			LeftHandVelRecord.Add ((LeftHand.transform.position-(Vector3)LeftHandRecord[LeftHandRecord.Count-2])/Time.deltaTime);
			RightHandSum = RightHandSum + RightHand.transform.position;
			RightHandVelSum = RightHandVelSum + (Vector3)RightHandRecord[RightHandRecord.Count-1];
			LeftHandSum = LeftHandSum + LeftHand.transform.position;
			LeftHandVelSum = LeftHandVelSum + (Vector3)LeftHandRecord[LeftHandRecord.Count-1];

		} else {
			RightHandRecord.Add(RightHand.transform.position);
			RightHandVelRecord.Add ((RightHand.transform.position-(Vector3)RightHandRecord[RightHandRecord.Count-2])/Time.deltaTime);
			LeftHandRecord.Add(LeftHand.transform.position);
			LeftHandVelRecord.Add ((LeftHand.transform.position-(Vector3)LeftHandRecord[LeftHandRecord.Count-2])/Time.deltaTime);
			RightHandSum = RightHandSum + RightHand.transform.position;
			RightHandVelSum = RightHandVelSum + (Vector3)RightHandRecord[RightHandRecord.Count-1];
			LeftHandSum = LeftHandSum + LeftHand.transform.position;
			LeftHandVelSum = LeftHandVelSum + (Vector3)LeftHandRecord[LeftHandRecord.Count-1];
			RightHandSum = RightHandSum - (Vector3)RightHandRecord[0]; 
			RightHandVelSum = RightHandVelSum - (Vector3)RightHandVelRecord[0];
			LeftHandSum = LeftHandSum - (Vector3)LeftHandRecord[0]; 
			LeftHandVelSum = LeftHandVelSum - (Vector3)LeftHandVelRecord[0];
			RightHandRecord.RemoveAt(0);
			RightHandVelRecord.RemoveAt(0);
			LeftHandRecord.RemoveAt(0);
			LeftHandVelRecord.RemoveAt(0);
		}





	//	if (Time.time > GameOverTime + 3.0F) {
		transform.position = new Vector3 (transform.position.x + (dir [0] * SpeedConst * Time.deltaTime),
			                 transform.position.y, transform.position.z + (dir [1] * SpeedConst * Time.deltaTime));
	//			}
		if (dir [0] == 1 && GameOver == false ) {

			if(isHurdle == true && JumpHInst == false){
				if(transform.position.x >= Hurdles [HurdleNum].transform.position.x-10){
					JumpHInst=true;
					Instructions.SetActive(true);
					Instructions.guiText.text = "Jump now...";
					InstSetTime = Time.time;
				}
			}

			if(transform.position.x >= CurrentWayStartingPoint[0]+WaysLength[CurrentWay]-(BeforeDistance-5) 
			   && WaysDirection[CurrentWay]==1 && LeftInst == false){
				Instructions.SetActive(true);
				Instructions.guiText.text = "Swipe to left now";
				InstSetTime = Time.time;
			}

			if(transform.position.x >= CurrentWayStartingPoint[0]+WaysLength[CurrentWay]-(BeforeDistance-5) 
			   && WaysDirection[CurrentWay] == -1 && RightInst == false){
				Instructions.SetActive(true);
				Instructions.guiText.text = "Swipe to right now";
				InstSetTime = Time.time;
			}

			if (IsGapPresent [CurrentWay] == true && JumpGInst == false) {
				if(transform.position.x >= CurrentWayStartingPoint[0]+WaysLength[CurrentWay]-(BeforeDistance+26)){
					Instructions.SetActive(true);
					Instructions.guiText.text = "Jump now..";
					InstSetTime = Time.time;
					JumpGInst = true;
				}
			}

			if((DetectGesture() == 'l' || Input.GetKeyDown("left")) && transform.position.x >= CurrentWayStartingPoint[0]+WaysLength[CurrentWay]-BeforeDistance){
				if(WaysDirection[CurrentWay]==1){Trigger=true;}
				else{transform.Rotate (0,-90,0);	dir[0]=0;	dir[1]=1;
					GameOver=true;
					GameOverTime = Time.time;}
			}
			if((DetectGesture() == 'r' || Input.GetKeyDown("right")) && transform.position.x >= CurrentWayStartingPoint[0]+WaysLength[CurrentWay]-BeforeDistance){
				if(WaysDirection[CurrentWay]==-1){Trigger=true;}
				else{transform.Rotate (0,90,0);	dir[0]=0;	dir[1]=-1;
					GameOver=true;
					GameOverTime = Time.time;}
			}
			if(Trigger == true  && transform.position.x >CurrentWayStartingPoint[0]+WaysLength[CurrentWay]){
				TriggerRotationChanges(WaysDirection[CurrentWay]);
				Score=Score+50;
			}
	//		if((c1.transform.position.z-transform.position.z)==0){
	//			c1.transform.position = new Vector3(c1.transform.position.x+SpeedConst*Time.deltaTime,c1.transform.position.y,c1.transform.position.z);}		
		
			if(isHurdle){
				if(Hurdles[HurdleNum].transform.position.x + 5.0F < transform.position.x){
					if(transform.rotation.x < 5 && transform.rotation.x > -5 && 
					   transform.rotation.z < 5 && transform.rotation.z > -5){
						Score = Score + 25;
						isHurdle = false;
					}
				}
			}

		}
		if (dir [0] == -1 && GameOver == false) {

			if(isHurdle == true && JumpHInst == false){
				if(transform.position.x <= Hurdles [HurdleNum].transform.position.x + 10){
					JumpHInst=true;
					Instructions.SetActive(true);
					Instructions.guiText.text = "Jump now...";
					InstSetTime = Time.time;
				}
			}

			if(transform.position.x <= CurrentWayStartingPoint[0]-WaysLength[CurrentWay]+(BeforeDistance-5) 
			   && WaysDirection[CurrentWay]==-1 && RightInst == false){
				Instructions.SetActive(true);
				Instructions.guiText.text = "Swipe to right now";
				InstSetTime = Time.time;
			}

			if (IsGapPresent [CurrentWay] == true && JumpGInst == false) {
				if(transform.position.x <= CurrentWayStartingPoint[0]-WaysLength[CurrentWay]+(BeforeDistance+26)){
					Instructions.SetActive(true);
					Instructions.guiText.text = "Jump now..";
					InstSetTime = Time.time;
					JumpGInst = true;
				}
			}

			if((DetectGesture() == 'l' || Input.GetKeyDown("left")) && transform.position.x <= CurrentWayStartingPoint[0]-WaysLength[CurrentWay]+BeforeDistance){
				if(WaysDirection[CurrentWay]==1){Trigger=true;}
				else{transform.Rotate (0,-90,0);	dir[0]=0;	dir[1]=-1;
					GameOver=true;
					GameOverTime = Time.time;}
			}
			if((DetectGesture() == 'r' || Input.GetKeyDown("right")) && transform.position.x <= CurrentWayStartingPoint[0]-WaysLength[CurrentWay]+BeforeDistance){
				if(WaysDirection[CurrentWay]==-1){Trigger=true;}
				else{transform.Rotate (0,90,0);	dir[0]=0;	dir[1]=1;
					GameOver=true;
					GameOverTime = Time.time;}
			}
			if(Trigger == true && transform.position.x <CurrentWayStartingPoint[0]-WaysLength[CurrentWay]){
				TriggerRotationChanges(WaysDirection[CurrentWay]);
				Score=Score+50;
			}
	//		if(c1.transform.position.z==transform.position.z){
	//			c1.transform.position = new Vector3(c1.transform.position.x-SpeedConst*Time.deltaTime,c1.transform.position.y,c1.transform.position.z);	}	
		
			if(isHurdle){
				if(Hurdles[HurdleNum].transform.position.x - 5.0F > transform.position.x){
					if(transform.rotation.x < 5 && transform.rotation.x > -5 && 
					   transform.rotation.z < 5 && transform.rotation.z > -5){
						Score = Score + 25;
						isHurdle = false;
					}
				}
			}
		
		}

		if (dir [1] == 1 && GameOver == false) {

			if(isHurdle == true && JumpHInst == false){
				if(transform.position.z >= Hurdles [HurdleNum].transform.position.z-10){
					JumpHInst=true;
					Instructions.SetActive(true);
					Instructions.guiText.text = "Jump now...";
					InstSetTime = Time.time;
				}
			}

			if(transform.position.z >= CurrentWayStartingPoint[1]+WaysLength[CurrentWay]-(BeforeDistance-5) 
			   && WaysDirection[CurrentWay]==-1 && RightInst == false){
				Instructions.SetActive(true);
				Instructions.guiText.text = "Swipe to right now";
				InstSetTime = Time.time;
			}

			if (IsGapPresent [CurrentWay] == true && JumpGInst == false) {
				if(transform.position.z >= CurrentWayStartingPoint[1]+WaysLength[CurrentWay]-(BeforeDistance+26)){
					Instructions.SetActive(true);
					Instructions.guiText.text = "Jump now..";
					InstSetTime = Time.time;
					JumpGInst = true;
				}
			}


			if((DetectGesture() == 'l' || Input.GetKeyDown("left")) && transform.position.z >= CurrentWayStartingPoint[1]+WaysLength[CurrentWay]-BeforeDistance){
				if(WaysDirection[CurrentWay]==1){Trigger=true;}
				else{transform.Rotate (0,-90,0);	dir[0]=-1;	dir[1]=0;
					GameOver=true;
					GameOverTime = Time.time;}
			}
			if((DetectGesture() == 'r' || Input.GetKeyDown("right")) && transform.position.z >= CurrentWayStartingPoint[1]+WaysLength[CurrentWay]-BeforeDistance){
				if(WaysDirection[CurrentWay]==-1){Trigger=true;}
				else{transform.Rotate (0,90,0);	dir[0]=1;	dir[1]=0;
					GameOver=true;
					GameOverTime = Time.time;}
			}
			if(Trigger == true && transform.position.z > CurrentWayStartingPoint[1]+WaysLength[CurrentWay]){
				TriggerRotationChanges(WaysDirection[CurrentWay]);
				Score=Score+50;
			}
	//		if(c1.transform.position.x==transform.position.x){
	//			c1.transform.position = new Vector3(c1.transform.position.x,c1.transform.position.y,c1.transform.position.z+SpeedConst*Time.deltaTime);}		

			if(isHurdle){
				if(Hurdles[HurdleNum].transform.position.z + 5.0F < transform.position.z){
					if(transform.rotation.x < 5 && transform.rotation.x > -5 && 
					   transform.rotation.z < 5 && transform.rotation.z > -5){
						Score = Score + 25;
						isHurdle = false;
					}
				}
			}
		}
		if (dir [1] == -1 && GameOver == false) {

			if(isHurdle == true && JumpHInst == false){
				if(transform.position.z <= Hurdles [HurdleNum].transform.position.z+10){
					JumpHInst=true;
					Instructions.SetActive(true);
					Instructions.guiText.text = "Jump now...";
					InstSetTime = Time.time;
				}
			}

			if(transform.position.z <= CurrentWayStartingPoint[1]-WaysLength[CurrentWay]+(BeforeDistance-5) 
			   && WaysDirection[CurrentWay]==-1 && RightInst == false){
				Instructions.SetActive(true);
				Instructions.guiText.text = "Swipe to right now";
				InstSetTime = Time.time;
			}

			if (IsGapPresent [CurrentWay] == true && JumpGInst == false) {
				if(transform.position.z <= CurrentWayStartingPoint[1]-WaysLength[CurrentWay]+(BeforeDistance+26)){
					Instructions.SetActive(true);
					Instructions.guiText.text = "Jump now..";
					InstSetTime = Time.time;
					JumpGInst = true;
				}
			}

			if((DetectGesture() == 'l' || Input.GetKeyDown("left")) && transform.position.z <= CurrentWayStartingPoint[1]-WaysLength[CurrentWay]+BeforeDistance){
				if(WaysDirection[CurrentWay]==1){Trigger=true;}
				else{transform.Rotate (0,-90,0);	dir[0]=1;	dir[1]=0;
					GameOver=true;
					GameOverTime = Time.time;}
			}
			if((DetectGesture() == 'r' || Input.GetKeyDown("right")) && transform.position.z <= CurrentWayStartingPoint[1]-WaysLength[CurrentWay]+BeforeDistance){
				if(WaysDirection[CurrentWay]==-1){Trigger=true;}
				else{transform.Rotate (0,90,0);	dir[0]=-1;	dir[1]=0;
					GameOver=true;
					GameOverTime = Time.time;}
			}
			if(Trigger == true && transform.position.z < CurrentWayStartingPoint[1]-WaysLength[CurrentWay]){
				TriggerRotationChanges(WaysDirection[CurrentWay]);
				Score=Score+50;
			}
	//		if(c1.transform.position.x==transform.position.x){
		//		c1.transform.position = new Vector3(c1.transform.position.x,c1.transform.position.y,c1.transform.position.z-SpeedConst*Time.deltaTime);}		
		
			if(isHurdle){
				if(Hurdles[HurdleNum].transform.position.z - 5.0F > transform.position.z){
					if(transform.rotation.x < 5 && transform.rotation.x > -5 && 
					   transform.rotation.z < 5 && transform.rotation.z > -5){
						Score = Score + 25;
						isHurdle = false;
					}
				}
			}
		}
	
	
		if ((DetectGesture() == 'l' || Input.GetKeyDown("left")) && GameOver==false) {
			KeyPressed('l');		
		}
		if ((DetectGesture() == 'r' || Input.GetKeyDown("right")) && GameOver==false) {
			KeyPressed('r');		
		}
		if ((Input.GetKeyDown ("up") || DetectGesture()=='j') && GameOver == false) {
			if(Time.time>JumpTime+1.0F)
			{KeyPressed('u');}
		}

		if (JumpConst > 0 && JumpConst < 11 && JumpTime > 2.0F) {
			rigidbody.AddForce(0,330,0);
			sf1= 0.5F;
			JumpConst++;
			if(JumpConst==10){
				JumpConst=0;
				if(Time.time-StartTime<540.0F){
					sf1 = 1.0F + 0.5F*((540.0F-Time.time+StartTime)/540.0F);}
			}
		}

		if (Time.time > JumpTime + 1.0F) {
			sf1=1.0F;	
		}

		if (Time.time > GameOverTime+0.3 && GameOverTime != -3.0F) {
			sf1 = 0.3F;	
		}

		t1.guiText.text = (Hip.transform.position.y * 100).ToString ();
		t2.guiText.text = HipAvg.ToString ();
		ScoreGUI.guiText.text = Score.ToString ();

		if (SlowlyRotateCamera != 0) {

			if(SlowlyRotateCamera>0 && SlowlyRotateCamera<19){
				SlowlyRotateCamera++;
				c1.transform.Rotate(0,-5,0,Space.World);
			}
		if(SlowlyRotateCamera<0 && SlowlyRotateCamera>-19){
				SlowlyRotateCamera--;
				c1.transform.Rotate(0,5,0,Space.World);
			}
			if(SlowlyRotateCamera==19 || SlowlyRotateCamera==-19){
				SlowlyRotateCamera=0;
			}
		
		}
	
		if (dir [1] != 0 && CameraDir.x == 1) {
						if (c1.transform.position.x > transform.position.x) {
								CameraDir = new Vector3 (0, 0, dir [1]);
						}
				}		
		if (dir [1] != 0 && CameraDir.x == -1) {
			if(c1.transform.position.x < transform.position.x){
				CameraDir = new Vector3(0,0,dir[1]);
			}}		
		if (dir [0] != 0 && CameraDir.z == 1) {
			if(c1.transform.position.z > transform.position.z){
				CameraDir = new Vector3(dir[0],0,0);
					}}		
		if (dir [0] != 0 && CameraDir.z == -1) {
			if(c1.transform.position.z < transform.position.z){
				CameraDir = new Vector3(dir[0],0,0);
			}		
		}
		if (GameOver == false) {
			c1.transform.position = new Vector3 (c1.transform.position.x + CameraDir.x * SpeedConst * Time.deltaTime,
                                    c1.transform.position.y, c1.transform.position.z + CameraDir.z * SpeedConst * Time.deltaTime);
				}

		if ((transform.position - c1.transform.position).magnitude > 10 && GameOver == false) {
			GameOver=true;		
		}

		if (GameOver == false) {
			if(transform.rotation.x > 25 || transform.rotation.x < -25){GameOver=true;}	
			if(transform.rotation.z > 25 || transform.rotation.z < -25){GameOver=true;}
		}

		Debug.Log (100*RightHandVelSum.x);

		if (GameOver) {

			if(Score > HighScore){
				PlayerPrefs.SetInt("HighScore",Score);
			}
		
		}

		if (RightInst == true && LeftInst == true && JumpHInst == true && JumpGInst == true && OverInst == false) {
		//	JumpHInst=true;
			if(Time.time > InstSetTime+3.1F){
			Instructions.SetActive(true);
			Instructions.guiText.text = "Now you are good to go :)";
			InstSetTime = Time.time;
			OverInst = true;
			}

		}
	
	}

	int GetRandom(){
		int k;
		k = Random.Range (0, 8);
		if (k == CurrentWay || k == PreviousWay) {
						return GetRandom ();		
				} else {
						return k;
				}
	}

	void KeyPressed(char x){

		if (x == 'u' && JumpConst==0) {
			JumpConst=1;
			JumpTime=Time.time;
		}

		if (dir [0] == 1) {
			if (x == 'l' && transform.position.x < CurrentWayStartingPoint[0]+WaysLength[CurrentWay]-BeforeDistance) {
				transform.Rotate (0, -90, 0);
				dir[0]=0;dir[1]=1;
				GameOver = true;
				GameOverTime = Time.time;
			}
			if(x == 'r' && transform.position.x < CurrentWayStartingPoint[0]+WaysLength[CurrentWay]-BeforeDistance){
				transform.Rotate(0,90,0);
				dir[0]=0;dir[1]=-1;
				GameOver = true;
				GameOverTime = Time.time;
			}}
	    else	if (dir [0] == -1) {
			if (x == 'l' && transform.position.x > CurrentWayStartingPoint[0]-WaysLength[CurrentWay]+BeforeDistance) {
				transform.Rotate (0, -90, 0);		
				dir[0]=0;dir[1]=-1;
				GameOver = true;
				GameOverTime = Time.time;
			}
			if(x == 'r' && transform.position.x > CurrentWayStartingPoint[0]-WaysLength[CurrentWay]+BeforeDistance){
				transform.Rotate(0,90,0);
				dir[0]=0;dir[1]=1;
				GameOver = true;
				GameOverTime = Time.time;
			}}
		else    if (dir [1] == 1) {
			if (x == 'l' && transform.position.z < CurrentWayStartingPoint[1]+WaysLength[CurrentWay]-BeforeDistance) {
				transform.Rotate (0, -90, 0);		
				dir[0]=-1;dir[1]=0;
				GameOver = true;
				GameOverTime = Time.time;
			}
			if(x == 'r' && transform.position.z < CurrentWayStartingPoint[1]+WaysLength[CurrentWay]-BeforeDistance){
				transform.Rotate(0,90,0);
				dir[0]=1;dir[1]=0;
				GameOver = true;
				GameOverTime = Time.time;
			}}
		else 	if (dir [1] == -1) {
			if (x == 'l' && transform.position.z > CurrentWayStartingPoint[1]-WaysLength[CurrentWay]+BeforeDistance) {
				transform.Rotate (0, -90, 0);		
				dir[0]=1;dir[1]=0;
				GameOver = true;
				GameOverTime = Time.time;
			}
			if(x == 'r' && transform.position.z > CurrentWayStartingPoint[1]-WaysLength[CurrentWay]+BeforeDistance){
				transform.Rotate(0,90,0);
				dir[0]=-1;dir[1]=0;
				GameOver = true;
				GameOverTime = Time.time;
			}}

	}



	void TriggerRotationChanges(int x){
			
						Trigger = false;		
				if (x == 1) {

						if(LeftInst == false){LeftInst=true;
						Instructions.guiText.text = "Excellent";
						InstSetTime = Time.time;}
						transform.Rotate (0, -90, 0);
						TurnTime = Time.time;
						SlowlyRotateCamera = 1;
						if (dir [0] == 1) {
								CurrentWayStartingPoint [0] = CurrentWayStartingPoint [0] + WaysLength [CurrentWay];
								CurrentWayStartingPoint [1] = CurrentWayStartingPoint [1] + 2;
								dir [0] = 0;
								dir [1] = 1;
								Ways [PreviousWay].SetActive (false);
								PreviousWay = CurrentWay;
								CurrentWay = NextWay;
								NextWay = GetRandom ();
					
								Ways [NextWay].SetActive (true);
								Ways [NextWay].transform.position = new Vector3 (CurrentWayStartingPoint [0] - 2 * WaysDirection [CurrentWay],
		   		Ways [NextWay].transform.position.y, CurrentWayStartingPoint [1] + WaysLength [CurrentWay]);
								Ways [NextWay].transform.rotation = Ways [CurrentWay].transform.rotation;
								Ways [NextWay].transform.Rotate (0, -90 * WaysDirection [CurrentWay], 0);

								Hurdles [0].SetActive (false);
								Hurdles [1].SetActive (false);
								isHurdle = false;
								HurdleProb = Random.Range (0, 10);
								if (HurdleProb < 7 && IsGapPresent [CurrentWay] == false && (Time.time-StartTime)> 30.0F) {
										isHurdle = true;
										HurdleNum = Random.Range (0, 2);
										Hurdles [HurdleNum].SetActive (true);
										Hurdles [HurdleNum].transform.position = new Vector3 (Ways [CurrentWay].transform.position.x,
					                                                    Hurdles [HurdleNum].transform.position.y, Ways [CurrentWay].transform.position.z + Random.Range (0.4F, 0.7F) * WaysLength [CurrentWay]);
										Hurdles [HurdleNum].transform.rotation = Ways [CurrentWay].transform.rotation;
								}
								
								
						} else if (dir [0] == -1) {
								CurrentWayStartingPoint [0] = CurrentWayStartingPoint [0] - WaysLength [CurrentWay];
								CurrentWayStartingPoint [1] = CurrentWayStartingPoint [1] - 2;
								dir [0] = 0;
								dir [1] = -1;
								Ways [PreviousWay].SetActive (false);
								PreviousWay = CurrentWay;
								CurrentWay = NextWay;
								NextWay = GetRandom ();
			
								Ways [NextWay].SetActive (true);
								Ways [NextWay].transform.position = new Vector3 (CurrentWayStartingPoint [0] + 2 * WaysDirection [CurrentWay],
				                                               Ways [NextWay].transform.position.y, CurrentWayStartingPoint [1] - WaysLength [CurrentWay]);
								Ways [NextWay].transform.rotation = Ways [CurrentWay].transform.rotation;
								Ways [NextWay].transform.Rotate (0, -90 * WaysDirection [CurrentWay], 0);

								Hurdles [0].SetActive (false);
								Hurdles [1].SetActive (false);
								isHurdle = false;
								HurdleProb = Random.Range (0, 10);
				if (HurdleProb < 7 && IsGapPresent [CurrentWay] == false && (Time.time-StartTime)> 30.0F) {
										isHurdle = true;
										HurdleNum = Random.Range (0, 2);
										Hurdles [HurdleNum].SetActive (true);
										Hurdles [HurdleNum].transform.position = new Vector3 (Ways [CurrentWay].transform.position.x,
					                                                    Hurdles [HurdleNum].transform.position.y, Ways [CurrentWay].transform.position.z - Random.Range (0.4F, 0.7F) * WaysLength [CurrentWay]);
										Hurdles [HurdleNum].transform.rotation = Ways [CurrentWay].transform.rotation;
								}
			
						} else if (dir [1] == 1) {
								CurrentWayStartingPoint [1] = CurrentWayStartingPoint [1] + WaysLength [CurrentWay];
								CurrentWayStartingPoint [0] = CurrentWayStartingPoint [0] - 2;
								dir [0] = -1;
								dir [1] = 0;
								Ways [PreviousWay].SetActive (false);
								PreviousWay = CurrentWay;
								CurrentWay = NextWay;
								NextWay = GetRandom ();
				
								Ways [NextWay].SetActive (true);
								Ways [NextWay].transform.position = new Vector3 (CurrentWayStartingPoint [0] - WaysLength [CurrentWay],
				                                               Ways [NextWay].transform.position.y, CurrentWayStartingPoint [1] - 2 * WaysDirection [CurrentWay]);
								Ways [NextWay].transform.rotation = Ways [CurrentWay].transform.rotation;
								Ways [NextWay].transform.Rotate (0, -90 * WaysDirection [CurrentWay], 0);					
			
								Hurdles [0].SetActive (false);
								Hurdles [1].SetActive (false);
								isHurdle = false;
								HurdleProb = Random.Range (0, 10);
				if (HurdleProb < 7 && IsGapPresent [CurrentWay] == false && (Time.time-StartTime)> 30.0F) {
										isHurdle = true;
										HurdleNum = Random.Range (0, 2);
										Hurdles [HurdleNum].SetActive (true);
										Hurdles [HurdleNum].transform.position = new Vector3 (Ways [CurrentWay].transform.position.x - Random.Range (0.4F, 0.7F) * WaysLength [CurrentWay],
					                                                    Hurdles [HurdleNum].transform.position.y, Ways [CurrentWay].transform.position.z);
										Hurdles [HurdleNum].transform.rotation = Ways [CurrentWay].transform.rotation;
								}
						} else if (dir [1] == -1) {
								CurrentWayStartingPoint [1] = CurrentWayStartingPoint [1] - WaysLength [CurrentWay];
								CurrentWayStartingPoint [0] = CurrentWayStartingPoint [0] + 2;
								dir [0] = 1;
								dir [1] = 0;
								Ways [PreviousWay].SetActive (false);
								PreviousWay = CurrentWay;
								CurrentWay = NextWay;
								NextWay = GetRandom ();
				
								Ways [NextWay].SetActive (true);
								Ways [NextWay].transform.position = new Vector3 (CurrentWayStartingPoint [0] + WaysLength [CurrentWay],
				                                               Ways [NextWay].transform.position.y, CurrentWayStartingPoint [1] + 2 * WaysDirection [CurrentWay]);
								Ways [NextWay].transform.rotation = Ways [CurrentWay].transform.rotation;
								Ways [NextWay].transform.Rotate (0, -90 * WaysDirection [CurrentWay], 0);					
			
								Hurdles [0].SetActive (false);
								Hurdles [1].SetActive (false);
								isHurdle = false;
								HurdleProb = Random.Range (0, 10);
				if (HurdleProb < 7 && IsGapPresent [CurrentWay] == false && (Time.time-StartTime)> 30.0F) {
										isHurdle = true;
										HurdleNum = Random.Range (0, 2);
										Hurdles [HurdleNum].SetActive (true);
										Hurdles [HurdleNum].transform.position = new Vector3 (Ways [CurrentWay].transform.position.x + Random.Range (0.4F, 0.7F) * WaysLength [CurrentWay],
					                                                    Hurdles [HurdleNum].transform.position.y, Ways [CurrentWay].transform.position.z);
										Hurdles [HurdleNum].transform.rotation = Ways [CurrentWay].transform.rotation;
								}
						}
						
		
				}


				if (x == -1) {

			if(RightInst == false){RightInst=true;
				Instructions.guiText.text = "Good! You are a fast learner..";
				InstSetTime = Time.time;}

						transform.Rotate (0, 90, 0);
						SlowlyRotateCamera = -1;
						if (dir [0] == 1) {
								CurrentWayStartingPoint [0] = CurrentWayStartingPoint [0] + WaysLength [CurrentWay];
								CurrentWayStartingPoint [1] = CurrentWayStartingPoint [1] - 2;
								dir [0] = 0;
								dir [1] = -1;
								Ways [PreviousWay].SetActive (false);
								PreviousWay = CurrentWay;
								CurrentWay = NextWay;
								NextWay = GetRandom ();
				
								Ways [NextWay].SetActive (true);
								Ways [NextWay].transform.position = new Vector3 (CurrentWayStartingPoint [0] + 2 * WaysDirection [CurrentWay],
				                                               Ways [NextWay].transform.position.y, CurrentWayStartingPoint [1] - WaysLength [CurrentWay]);
								Ways [NextWay].transform.rotation = Ways [CurrentWay].transform.rotation;
								Ways [NextWay].transform.Rotate (0, -90 * WaysDirection [CurrentWay], 0);
								transform.rotation = new Quaternion (0, Ways [CurrentWay].transform.rotation.y + 90, 0, 0);

								Hurdles [0].SetActive (false);
								Hurdles [1].SetActive (false);
								isHurdle = false;
								HurdleProb = Random.Range (0, 10);
				if (HurdleProb < 7 && IsGapPresent [CurrentWay] == false && (Time.time-StartTime)> 30.0F) {
										isHurdle = true;
										HurdleNum = Random.Range (0, 2);
										Hurdles [HurdleNum].SetActive (true);
										Hurdles [HurdleNum].transform.position = new Vector3 (Ways [CurrentWay].transform.position.x,
					                                                    Hurdles [HurdleNum].transform.position.y, Ways [CurrentWay].transform.position.z - Random.Range (0.4F, 0.7F) * WaysLength [CurrentWay]);
										Hurdles [HurdleNum].transform.rotation = Ways [CurrentWay].transform.rotation;
								}
						} else if (dir [0] == -1) {
								CurrentWayStartingPoint [0] = CurrentWayStartingPoint [0] - WaysLength [CurrentWay];
								CurrentWayStartingPoint [1] = CurrentWayStartingPoint [1] + 2;
								dir [0] = 0;
								dir [1] = 1;
								Ways [PreviousWay].SetActive (false);
								PreviousWay = CurrentWay;
								CurrentWay = NextWay;
								NextWay = GetRandom ();
				
								Ways [NextWay].SetActive (true);
								Ways [NextWay].transform.position = new Vector3 (CurrentWayStartingPoint [0] - 2 * WaysDirection [CurrentWay],
				                                               Ways [NextWay].transform.position.y, CurrentWayStartingPoint [1] + WaysLength [CurrentWay]);
								Ways [NextWay].transform.rotation = Ways [CurrentWay].transform.rotation;
								Ways [NextWay].transform.Rotate (0, -90 * WaysDirection [CurrentWay], 0);
			
								Hurdles [0].SetActive (false);
								Hurdles [1].SetActive (false);
								isHurdle = false;
								HurdleProb = Random.Range (0, 10);
				if (HurdleProb < 7 && IsGapPresent [CurrentWay] == false && (Time.time-StartTime)> 30.0F) {
										isHurdle = true;
										HurdleNum = Random.Range (0, 2);
										Hurdles [HurdleNum].SetActive (true);
										Hurdles [HurdleNum].transform.position = new Vector3 (Ways [CurrentWay].transform.position.x,
					                                                    Hurdles [HurdleNum].transform.position.y, Ways [CurrentWay].transform.position.z + Random.Range (0.4F, 0.7F) * WaysLength [CurrentWay]);
										Hurdles [HurdleNum].transform.rotation = Ways [CurrentWay].transform.rotation;
								}
						} else if (dir [1] == 1) {
								CurrentWayStartingPoint [1] = CurrentWayStartingPoint [1] + WaysLength [CurrentWay];
								CurrentWayStartingPoint [0] = CurrentWayStartingPoint [0] + 2;
								dir [0] = 1;
								dir [1] = 0;
								Ways [PreviousWay].SetActive (false);
								PreviousWay = CurrentWay;
								CurrentWay = NextWay;
								NextWay = GetRandom ();
				
								Ways [NextWay].SetActive (true);
								Ways [NextWay].transform.position = new Vector3 (CurrentWayStartingPoint [0] + WaysLength [CurrentWay],
				                                               Ways [NextWay].transform.position.y, CurrentWayStartingPoint [1] + 2 * WaysDirection [CurrentWay]);
								Ways [NextWay].transform.rotation = Ways [CurrentWay].transform.rotation;
								Ways [NextWay].transform.Rotate (0, -90 * WaysDirection [CurrentWay], 0);					
			
								Hurdles [0].SetActive (false);
								Hurdles [1].SetActive (false);
								isHurdle = false;
								HurdleProb = Random.Range (0, 10);
				if (HurdleProb < 7 && IsGapPresent [CurrentWay] == false && (Time.time-StartTime)> 30.0F) {
										isHurdle = true;
										HurdleNum = Random.Range (0, 2);
										Hurdles [HurdleNum].SetActive (true);
										Hurdles [HurdleNum].transform.position = new Vector3 (Ways [CurrentWay].transform.position.x + Random.Range (0.4F, 0.7F) * WaysLength [CurrentWay],
					                                                    Hurdles [HurdleNum].transform.position.y, Ways [CurrentWay].transform.position.z);
										Hurdles [HurdleNum].transform.rotation = Ways [CurrentWay].transform.rotation;
								}
						} else if (dir [1] == -1) {	
								CurrentWayStartingPoint [1] = CurrentWayStartingPoint [1] - WaysLength [CurrentWay];
								CurrentWayStartingPoint [0] = CurrentWayStartingPoint [0] - 2;
								dir [0] = -1;
								dir [1] = 0;
								Ways [PreviousWay].SetActive (false);
								PreviousWay = CurrentWay;
								CurrentWay = NextWay;
								NextWay = GetRandom ();
				
								Ways [NextWay].SetActive (true);
								Ways [NextWay].transform.position = new Vector3 (CurrentWayStartingPoint [0] - WaysLength [CurrentWay],
				                                               Ways [NextWay].transform.position.y, CurrentWayStartingPoint [1] - 2 * WaysDirection [CurrentWay]);
								Ways [NextWay].transform.rotation = Ways [CurrentWay].transform.rotation;
								Ways [NextWay].transform.Rotate (0, -90 * WaysDirection [CurrentWay], 0);					

								Hurdles [0].SetActive (false);
								Hurdles [1].SetActive (false);
								isHurdle = false;
								HurdleProb = Random.Range (0, 10);
				if (HurdleProb < 7 && IsGapPresent [CurrentWay] == false && (Time.time-StartTime)> 30.0F) {
										isHurdle = true;
										HurdleNum = Random.Range (0, 2);
										Hurdles [HurdleNum].SetActive (true);
										Hurdles [HurdleNum].transform.position = new Vector3 (Ways [CurrentWay].transform.position.x - Random.Range (0.4F, 0.7F) * WaysLength [CurrentWay],
					                                                    Hurdles [HurdleNum].transform.position.y, Ways [CurrentWay].transform.position.z);
										Hurdles [HurdleNum].transform.rotation = Ways [CurrentWay].transform.rotation;
								}
						}
			
				}

	}


	void OnCollisionEnter(Collision col){
		if (col.gameObject.name == "hurdle1" || col.gameObject.name == "hurdle2") {
			GameOver = true;
			GameOverTime = Time.time;
			transform.Rotate(45,0,0);
			rigidbody.AddTorque(1000000,0,0);
			
		}
	}


	char DetectGesture(){
		if ((Hip.transform.position.y*100 > 100*HipAvg.y +12 ) && Time.time > JumpTimeK + 3.0F && (Time.time-StartTime)>1.0F) {
			return 'j'; JumpTimeK=Time.time;}
		if (RightHandSum.x/5 < (Spine.transform.position.x + 3*LeftShoulder.transform.position.x)/4 && Time.time > TurnTime + 2.0F && (Time.time-StartTime) > 1.0F ) {
			return 'l';	TurnTime=Time.time;}
		if (LeftHandSum.x/5 > (Spine.transform.position.x + 3*RightShoulder.transform.position.x)/4 &&  Time.time > TurnTime + 2.0F && (Time.time-StartTime) > 1.0F) {
			return 'r';	TurnTime=Time.time;}
		return 'n';
	}
	
}
