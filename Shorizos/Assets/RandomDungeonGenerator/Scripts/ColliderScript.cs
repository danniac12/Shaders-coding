using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ColliderScript : MonoBehaviour {
	
	private GameObject DungeonGenerator;
	public int corridorPartCount;
	
	//types
	public string partType;
	public string doorType;
	public string finishingType;
	public bool TypeCastingDone = false;
	
	//buildingblocks within the part
	public bool buildFloor = false;
	public bool buildStairs = false;
	public bool buildCeiling = false;
	public bool FrontWall = false;
	public bool BackWall = false;
	public bool LeftWall = false;
	public bool RightWall = false;
	
	//models retreived through the main level script
	public GameObject CorridorFloorModel;
	public GameObject CorridorCeilingModel;
	
	public GameObject CorridorWallModelLeft;
	public GameObject CorridorWallModelRight;
	public GameObject CorridorWallModelFront;
	public GameObject CorridorWallModelBack;
	
	public GameObject doorModel;
	private GameObject nearbyFloorModel;
	private bool doorOnFront = false;
	private bool doorOnRight = false;
	private bool doorOnBack = false;
	private bool doorOnLeft = false;
		
	public GameObject RoomFloorModel;
	public GameObject RoomCeilingModel;
	public GameObject RoomWallModelLeft;
	public GameObject RoomWallModelRight;
	public GameObject RoomWallModelFront;
	public GameObject RoomWallModelBack;
	public GameObject RoomCornerWallModel;
	private int CornerWallChancePercentage;
	
	public GameObject RailingModel;
	public GameObject EndRailingModel;
	public GameObject PortalModel;
	public GameObject WindowModel;
	public GameObject endWallModel;
	
	public GameObject StairCaseModel;
	public int StairCaseNumber;
	public int direction;
	public string directionUpDown;
	
	//info for storing surrounding
	private GameObject roomOnFrontObject;
	private GameObject roomOnRightObject;
	private GameObject roomOnBackObject;
	private GameObject roomOnLeftObject;
	private GameObject roomOnTopObject;
	private GameObject roomBelowObject;
	
	//wall placement
	private Vector3 CorridormovePosition1 = new Vector3(0,0,2.5f);
	private Vector3 RoommovePosition1 = new Vector3(0,0,2.5f);
	private Vector3 rotatingPosition1 = new Vector3(0,-180,0);
			 		
	private Vector3 CorridormovePosition2 = new Vector3(-2.5f,0,0);
	private Vector3 RoommovePosition2 = new Vector3(-2.5f,0,0);
	private Vector3 rotatingPosition2 = new Vector3(0,90,0);
	
	private Vector3 CorridormovePosition3 = new Vector3(0,0,-2.5f);
	private Vector3 RoommovePosition3= new Vector3(0,0,-2.5f);
	private Vector3 rotatingPosition3 = new Vector3(0,0,0);
		
	private Vector3 CorridormovePosition4 = new Vector3(2.5f,0,0);
	private Vector3 RoommovePosition4 = new Vector3(2.5f,0,0);
	private Vector3 rotatingPosition4 = new Vector3(0,270,0);
	
	//railing placement
	private bool leftUpper = false;
	private bool rightUpper = false;
	private bool leftLower = false;
	private bool rightLower = false;
	
	//endwall positions
	private Vector3 leftUpperLocation = new Vector3(-2.25f,0,2.25f);
	private Vector3 rightUpperLocation = new Vector3(2.25f,0,2.25f);
	private Vector3 leftLowerLocation = new Vector3(-2.25f,0,-2.25f);
	private Vector3 rightLowerLocation = new Vector3(2.25f,0,-2.25f);
	
	//railing end positions
	private Vector3 leftUpperLocationRail = new Vector3(-2.3f,0,2.3f);
	private Vector3 rightUpperLocationRail = new Vector3(2.3f,0,2.3f);
	private Vector3 leftLowerLocationRail = new Vector3(-2.3f,0,-2.3f);
	private Vector3 rightLowerLocationRail = new Vector3(2.3f,0,-2.3f);
	
	//spots for lights or objects, 0 means unassigned, 1 means taken spot, 2 means there is room
	public int ceilingModelSpot =0;
	public int floorModelSpot =0;
	public int leftWallModelSpot =0;
	public int rightWallModelSpot =0;
	public int frontWallModelSpot =0;
	public int backWallModelSpot =0;
	
	//info for corridor primary object spacing
	public string primaryCorridorSpacingType;
	public int primarySpacing = 0;
	private bool primaryObjectSpot = false;
	
	private GameObject tempObject;
	//temp lists for object models
	public List<GameObject> corridorPrimaryLeftWallObjects;
	public List<GameObject> corridorPrimaryRightWallObjects;
	public List<GameObject> corridorPrimaryFrontWallObjects;
	public List<GameObject> corridorPrimaryBackWallObjects;
	public List<GameObject> corridorPrimaryCeilingObjects;
	public List<GameObject> corridorPrimaryFloorObjects;
	
	public List<GameObject> roomPrimaryLeftWallObjects;
	public List<GameObject> roomPrimaryRightWallObjects;
	public List<GameObject> roomPrimaryFrontWallObjects;
	public List<GameObject> roomPrimaryBackWallObjects;
	public List<GameObject> roomPrimaryCeilingObjects;
	public List<GameObject> roomPrimaryFloorObjects;
	
	public List<GameObject> corridorSecundaryLeftWallObjects;
	public List<GameObject> corridorSecundaryRightWallObjects;
	public List<GameObject> corridorSecundaryFrontWallObjects;
	public List<GameObject> corridorSecundaryBackWallObjects;
	public List<GameObject> corridorSecundaryCeilingObjects;
	public List<GameObject> corridorSecundaryFloorObjects;
	
	public List<GameObject> roomSecundaryLeftWallObjects;
	public List<GameObject> roomSecundaryRightWallObjects;
	public List<GameObject> roomSecundaryFrontWallObjects;
	public List<GameObject> roomSecundaryBackWallObjects;
	public List<GameObject> roomSecundaryCeilingObjects;
	public List<GameObject> roomSecundaryFloorObjects;
	
	//models for primary objects
	public GameObject corridorPrimaryWallObject;
	public GameObject corridorPrimaryCeilingObject;
	public GameObject corridorPrimaryFloorObject;
	public GameObject roomPrimaryWallObject;
	public GameObject roomPrimaryCeilingObject;
	public GameObject roomPrimaryFloorObject;
	
	//objects models
	public GameObject corridorWallObject;
	public GameObject corridorCeilingObject;
	public GameObject corridorFloorObject;
	public GameObject roomWallObject;
	public GameObject roomCeilingObject;
	public GameObject roomFloorObject;
	
	public int retryPrimaryCounter =0;
	public int retrySecundaryCounter =0;
	private int buildChance;
	
	private Vector3 tempRotation;
	
	//collider used for end or beginpoints
	public bool endPointer = false;
	public bool beginPointer = false;
	public GameObject endPointObject;
	public GameObject begingPointObject;
	
	
	//sphere used for buildup
	public GameObject colorSphere;
	
	
	//======================= start of functions =============//
	
	public void TypeCasting (){
		if (TypeCastingDone == false){
			TypeCastingDone = true;
			MapSurroundings ();
			//making sure that the staircases have all the correct floors
			if (this.transform.name == "StairCasePart" && buildFloor == false){
			//looking down
				if (roomBelowObject == null){
					buildFloor = true;
				} else {
					if( roomBelowObject.transform.name == "CorridorPart"){
						buildFloor = true;	
					}
				}			
			}
			//transferring type of part to touching parts and matching finishing types etc.
			if (this.transform.name == "CorridorPart"){
				if (primarySpacing == 0){
					primaryObjectSpot = true;
				}
				if (roomOnFrontObject != null){
					if (roomOnFrontObject.transform.name == "CorridorPart"){
						roomOnFrontObject.GetComponent<ColliderScript>().partType = partType;
						roomOnFrontObject.GetComponent<ColliderScript>().finishingType = finishingType;
						if (primaryCorridorSpacingType == "maximumObjects"){
							roomOnFrontObject.GetComponent<ColliderScript>().primarySpacing = 0;
						}
						if (primaryCorridorSpacingType == "objectNoObject"){
							if (primarySpacing == 1){
								roomOnFrontObject.GetComponent<ColliderScript>().primarySpacing = 0;
							}
							if (primarySpacing == 0){
								roomOnFrontObject.GetComponent<ColliderScript>().primarySpacing = 1;
							}
						}
						if (primaryCorridorSpacingType == "objectNoObjectNoObject"){
							if (primarySpacing == 2){
								roomOnFrontObject.GetComponent<ColliderScript>().primarySpacing = 1;
							}
							if (primarySpacing == 1){
								roomOnFrontObject.GetComponent<ColliderScript>().primarySpacing = 0;
							}
							if (primarySpacing == 0){
								roomOnFrontObject.GetComponent<ColliderScript>().primarySpacing = 2;
							}
						}
						roomOnFrontObject.GetComponent<ColliderScript>().primaryCorridorSpacingType = primaryCorridorSpacingType;
						roomOnFrontObject.GetComponent<ColliderScript>().TypeCasting ();
					}
				}
				if (roomOnRightObject != null){
					if (roomOnRightObject.transform.name == "CorridorPart"){
						roomOnRightObject.GetComponent<ColliderScript>().partType = partType;
						roomOnRightObject.GetComponent<ColliderScript>().finishingType = finishingType;
						if (primaryCorridorSpacingType == "maximumObjects"){
							roomOnRightObject.GetComponent<ColliderScript>().primarySpacing = 0;
						}
						if (primaryCorridorSpacingType == "objectNoObject"){
							if (primarySpacing == 1){
								roomOnRightObject.GetComponent<ColliderScript>().primarySpacing = 0;
							}
							if (primarySpacing == 0){
								roomOnRightObject.GetComponent<ColliderScript>().primarySpacing = 1;
							}
						}
						if (primaryCorridorSpacingType == "objectNoObjectNoObject"){
							if (primarySpacing == 2){
								roomOnRightObject.GetComponent<ColliderScript>().primarySpacing = 1;
							}
							if (primarySpacing == 1){
								roomOnRightObject.GetComponent<ColliderScript>().primarySpacing = 0;
							}
							if (primarySpacing == 0){
								roomOnRightObject.GetComponent<ColliderScript>().primarySpacing = 2;
							}
						}
						roomOnRightObject.GetComponent<ColliderScript>().primaryCorridorSpacingType = primaryCorridorSpacingType;
						roomOnRightObject.GetComponent<ColliderScript>().TypeCasting ();
					}
				}
				if (roomOnBackObject != null){
					if (roomOnBackObject.transform.name == "CorridorPart"){
						roomOnBackObject.GetComponent<ColliderScript>().partType = partType;
						roomOnBackObject.GetComponent<ColliderScript>().finishingType = finishingType;
						if (primaryCorridorSpacingType == "maximumObjects"){
							roomOnBackObject.GetComponent<ColliderScript>().primarySpacing = 0;
						}
						if (primaryCorridorSpacingType == "objectNoObject"){
							if (primarySpacing == 1){
								roomOnBackObject.GetComponent<ColliderScript>().primarySpacing = 0;
							}
							if (primarySpacing == 0){
								roomOnBackObject.GetComponent<ColliderScript>().primarySpacing = 1;
							}
						}
						if (primaryCorridorSpacingType == "objectNoObjectNoObject"){
							if (primarySpacing == 2){
								roomOnBackObject.GetComponent<ColliderScript>().primarySpacing = 1;
							}
							if (primarySpacing == 1){
								roomOnBackObject.GetComponent<ColliderScript>().primarySpacing = 0;
							}
							if (primarySpacing == 0){
								roomOnBackObject.GetComponent<ColliderScript>().primarySpacing = 2;
							}
						}
						roomOnBackObject.GetComponent<ColliderScript>().primaryCorridorSpacingType = primaryCorridorSpacingType;
						roomOnBackObject.GetComponent<ColliderScript>().TypeCasting ();
					}
				}
				if (roomOnLeftObject != null){
					if (roomOnLeftObject.transform.name == "CorridorPart"){
						roomOnLeftObject.GetComponent<ColliderScript>().partType = partType;
						roomOnLeftObject.GetComponent<ColliderScript>().finishingType = finishingType;
						if (primaryCorridorSpacingType == "maximumObjects"){
							roomOnLeftObject.GetComponent<ColliderScript>().primarySpacing = 0;
						}
						if (primaryCorridorSpacingType == "objectNoObject"){
							if (primarySpacing == 1){
								roomOnLeftObject.GetComponent<ColliderScript>().primarySpacing = 0;
							}
							if (primarySpacing == 0){
								roomOnLeftObject.GetComponent<ColliderScript>().primarySpacing = 1;
							}
						}
						if (primaryCorridorSpacingType == "objectNoObjectNoObject"){
							if (primarySpacing == 2){
								roomOnLeftObject.GetComponent<ColliderScript>().primarySpacing = 1;
							}
							if (primarySpacing == 1){
								roomOnLeftObject.GetComponent<ColliderScript>().primarySpacing = 0;
							}
							if (primarySpacing == 0){
								roomOnLeftObject.GetComponent<ColliderScript>().primarySpacing = 2;
							}
						}
						roomOnLeftObject.GetComponent<ColliderScript>().primaryCorridorSpacingType = primaryCorridorSpacingType;
						roomOnLeftObject.GetComponent<ColliderScript>().TypeCasting ();
					}
				}
			}
			if (this.transform.name == "RoomPart" || this.transform.name == "StairCasePart"){
				if (roomOnFrontObject != null){
					if (roomOnFrontObject.transform.name == "RoomPart" || roomOnFrontObject.transform.name == "StairCasePart"){
						roomOnFrontObject.GetComponent<ColliderScript>().partType = partType;
						roomOnFrontObject.GetComponent<ColliderScript>().finishingType = finishingType;
						roomOnFrontObject.GetComponent<ColliderScript>().TypeCasting ();
					}
				}
				if (roomOnRightObject != null){
					if (roomOnRightObject.transform.name == "RoomPart" || roomOnRightObject.transform.name == "StairCasePart"){
						roomOnRightObject.GetComponent<ColliderScript>().partType = partType;
						roomOnRightObject.GetComponent<ColliderScript>().finishingType = finishingType;
						roomOnRightObject.GetComponent<ColliderScript>().TypeCasting ();
					}
				}
				if (roomOnBackObject != null){
					if (roomOnBackObject.transform.name == "RoomPart" || roomOnBackObject.transform.name == "StairCasePart"){
						roomOnBackObject.GetComponent<ColliderScript>().partType = partType;
						roomOnBackObject.GetComponent<ColliderScript>().finishingType = finishingType;
						roomOnBackObject.GetComponent<ColliderScript>().TypeCasting ();
					}
				}
				if (roomOnLeftObject != null){
					if (roomOnLeftObject.transform.name == "RoomPart" || roomOnLeftObject.transform.name == "StairCasePart"){
						roomOnLeftObject.GetComponent<ColliderScript>().partType = partType;
						roomOnLeftObject.GetComponent<ColliderScript>().finishingType = finishingType;
						roomOnLeftObject.GetComponent<ColliderScript>().TypeCasting ();
					}
				}
				if (roomOnTopObject != null){
					if (roomOnTopObject.transform.name == "RoomPart" || roomOnTopObject.transform.name == "StairCasePart"){
						if (roomOnTopObject.transform.GetComponent<ColliderScript>().buildFloor == false){
							roomOnTopObject.GetComponent<ColliderScript>().partType = partType;
							roomOnTopObject.GetComponent<ColliderScript>().finishingType = finishingType;
							roomOnTopObject.GetComponent<ColliderScript>().TypeCasting ();
						}
					}
				}
				if (roomBelowObject != null){
					if (buildFloor == false){
						if (roomBelowObject.transform.name == "RoomPart" || roomBelowObject.transform.name == "StairCasePart"){
							roomBelowObject.GetComponent<ColliderScript>().partType = partType;
							roomBelowObject.GetComponent<ColliderScript>().finishingType = finishingType;
							roomBelowObject.GetComponent<ColliderScript>().TypeCasting ();
						}
					}
				}
			}
		}
	}
	void MapSurroundings (){
		var lookLocation = new Vector3(0,0,0);
		var lookAround = 1;	
		while (lookAround <= 6){
			if(lookAround == 1){
				lookLocation = new Vector3(0,0,5);
			}else if(lookAround == 2){
				lookLocation = new Vector3(-5,0,0);
			}else if(lookAround == 3){
				lookLocation = new Vector3(0,0,-5);
			}else if(lookAround == 4){
				lookLocation = new Vector3(5,0,0);
			}else if(lookAround == 5){
				lookLocation = new Vector3(0,4.5f,0);
			}else if(lookAround == 6){
				lookLocation = new Vector3(0,-4.5f,0);
			}
			
			var basicsInRange = Physics.OverlapSphere(transform.position+lookLocation, 0.5f);	
 			if(basicsInRange.Length == 0){	
 				if (lookAround==1){
 					roomOnFrontObject = null;
 				}else if (lookAround==2){
 					roomOnRightObject = null;
 				}else if (lookAround==3){
 					roomOnBackObject = null;
 				}else if (lookAround==4){
 					roomOnLeftObject = null;
 				}else if (lookAround==5){
 					roomOnTopObject = null;
				}else if (lookAround==6){
 					roomBelowObject = null;
				}
 			}else {
 				var countAll = 0;
	 			while (countAll < basicsInRange.Length){
					if (lookAround==1){
 						roomOnFrontObject = basicsInRange[countAll].gameObject;
					}else if (lookAround==2){
 						roomOnRightObject =basicsInRange[countAll].gameObject;
					}else if (lookAround==3){
 						roomOnBackObject =basicsInRange[countAll].gameObject;
					}else if (lookAround==4){
 						roomOnLeftObject =basicsInRange[countAll].gameObject;
					}else if (lookAround==5){
 						roomOnTopObject =basicsInRange[countAll].gameObject;
					}else if (lookAround==6){
 						roomBelowObject =basicsInRange[countAll].gameObject;
					}
 				countAll = countAll+1;
 				}
			}
 		lookAround = lookAround +1;
		}
	}

	public void Construct (){
		//getting models
		var LevelSettingsScript = GameObject.FindWithTag("DungeonBuilder").GetComponent<DungeonGenerator>();
		
		//temp model lists
		corridorPrimaryLeftWallObjects = new List<GameObject>();
		corridorPrimaryRightWallObjects = new List<GameObject>();
		corridorPrimaryFrontWallObjects = new List<GameObject>();
		corridorPrimaryBackWallObjects = new List<GameObject>();
		corridorPrimaryCeilingObjects = new List<GameObject>();
		corridorPrimaryFloorObjects = new List<GameObject>();
		
		roomPrimaryLeftWallObjects = new List<GameObject>();
		roomPrimaryRightWallObjects = new List<GameObject>();
		roomPrimaryFrontWallObjects = new List<GameObject>();
		roomPrimaryBackWallObjects = new List<GameObject>();
		roomPrimaryCeilingObjects = new List<GameObject>();
		roomPrimaryFloorObjects = new List<GameObject>();
		
		corridorSecundaryLeftWallObjects = new List<GameObject>();
		corridorSecundaryRightWallObjects = new List<GameObject>();
		corridorSecundaryFrontWallObjects = new List<GameObject>();
		corridorSecundaryBackWallObjects = new List<GameObject>();
		corridorSecundaryCeilingObjects = new List<GameObject>();
		corridorSecundaryFloorObjects = new List<GameObject>();
		
		roomSecundaryLeftWallObjects = new List<GameObject>();
		roomSecundaryRightWallObjects = new List<GameObject>();
		roomSecundaryFrontWallObjects = new List<GameObject>();
		roomSecundaryBackWallObjects = new List<GameObject>();
		roomSecundaryCeilingObjects = new List<GameObject>();
		roomSecundaryFloorObjects = new List<GameObject>();
				
		LevelSettingsScript.GettingModels(this.gameObject);
		//build staircase
		if (this.transform.name == "StairCasePart"){
			if (buildStairs == true){
				
				leftUpper = true;
				rightUpper = true;
				leftLower = true;
				rightLower = true;
				
				var newStairs = Instantiate(StairCaseModel, transform.position,Quaternion.identity) as GameObject;
				newStairs.transform.parent = this.transform;
				newStairs.transform.name = StairCaseModel.transform.name;
				newStairs.name = StairCaseModel.name;
				if (direction == 1 && directionUpDown == "Up"){
					newStairs.transform.Rotate(rotatingPosition1);
					leftUpper = false;
					rightUpper = false;
				}
				if (direction == 2 && directionUpDown == "Up"){
					newStairs.transform.Rotate(rotatingPosition4);
					leftUpper = false;
					leftLower = false;
				}
				if (direction == 3 && directionUpDown == "Up"){
					newStairs.transform.Rotate(rotatingPosition3);
					rightLower = false;
					leftLower = false;
				}
				if (direction == 4 && directionUpDown == "Up"){
					newStairs.transform.Rotate(rotatingPosition2);
					rightUpper = false;
					rightLower = false;
				}
				if (direction == 1 && directionUpDown == "Down"){
					newStairs.transform.Rotate(rotatingPosition3);
					rightLower = false;
					leftLower = false;					
				}
				if (direction == 2 && directionUpDown == "Down"){
					newStairs.transform.Rotate(rotatingPosition2);
					rightUpper = false;
					rightLower = false;
				}
				if (direction == 3 && directionUpDown == "Down"){
					newStairs.transform.Rotate(rotatingPosition1);
					leftUpper = false;
					rightUpper = false;
				}
				if (direction == 4 && directionUpDown == "Down"){
					newStairs.transform.Rotate(rotatingPosition4);
					leftUpper = false;
					leftLower = false;
				}
			}
		}
		//build door 
		if (this.transform.name == "CorridorPart"){
			if (roomOnFrontObject != null && roomOnRightObject == null && roomOnLeftObject == null){
				if (roomOnFrontObject.transform.name == "RoomPart" && roomOnFrontObject.GetComponent<ColliderScript>().buildFloor == true){
					doorOnFront = true;
					
					doorType = roomOnFrontObject.GetComponent<ColliderScript>().partType;
					LevelSettingsScript.GettingDoorModel(this.gameObject);
										
					var theDoor = Instantiate(doorModel, transform.position + CorridormovePosition1,Quaternion.identity) as GameObject;
					theDoor.transform.parent = this.transform;
					theDoor.transform.Rotate(rotatingPosition1);
					theDoor.name = doorModel.name;
					floorModelSpot = 1;
					ceilingModelSpot = 2;
					leftWallModelSpot = 1;
					rightWallModelSpot = 1;
					frontWallModelSpot = 1;
					backWallModelSpot = 1;
					
					var roomScript = roomOnFrontObject.GetComponent<ColliderScript>();
					roomScript.floorModelSpot =1;
				}
			}
			if (roomOnBackObject != null && roomOnRightObject == null && roomOnLeftObject == null){
				if (roomOnBackObject.transform.name == "RoomPart" && roomOnBackObject.GetComponent<ColliderScript>().buildFloor == true){
					doorOnBack = true;
					
					doorType = roomOnBackObject.GetComponent<ColliderScript>().partType;
					LevelSettingsScript.GettingDoorModel(this.gameObject);
										
					var theDoor = Instantiate(doorModel, transform.position + CorridormovePosition3,Quaternion.identity) as GameObject;
					theDoor.transform.parent = this.transform;
					theDoor.transform.Rotate(rotatingPosition3);
					theDoor.name = doorModel.name;
					floorModelSpot = 1;
					ceilingModelSpot = 2;
					leftWallModelSpot = 1;
					rightWallModelSpot = 1;
					frontWallModelSpot = 1;
					backWallModelSpot = 1;
					
					var roomScript = roomOnBackObject.GetComponent<ColliderScript>();
					roomScript.floorModelSpot =1;				
				}
			}
			if (roomOnRightObject != null && roomOnBackObject == null && roomOnFrontObject == null){
				if (roomOnRightObject.transform.name == "RoomPart" && roomOnRightObject.GetComponent<ColliderScript>().buildFloor == true){
					doorOnRight = true;
					
					doorType = roomOnRightObject.GetComponent<ColliderScript>().partType;
					LevelSettingsScript.GettingDoorModel(this.gameObject);
					
					var theDoor = Instantiate(doorModel, transform.position + CorridormovePosition2,Quaternion.identity) as GameObject;
					theDoor.transform.parent = this.transform;
					theDoor.transform.Rotate(rotatingPosition2);
					theDoor.name = doorModel.name;
					floorModelSpot = 1;
					ceilingModelSpot = 2;
					leftWallModelSpot = 1;
					rightWallModelSpot = 1;
					frontWallModelSpot = 1;
					backWallModelSpot = 1;
					
					var roomScript = roomOnRightObject.GetComponent<ColliderScript>();
					roomScript.floorModelSpot =1;
				}
			}
			if (roomOnLeftObject != null && roomOnBackObject == null && roomOnFrontObject == null){
				if (roomOnLeftObject.transform.name == "RoomPart" && roomOnLeftObject.GetComponent<ColliderScript>().buildFloor == true){
					doorOnLeft = true;
					
					doorType = roomOnLeftObject.GetComponent<ColliderScript>().partType;
					LevelSettingsScript.GettingDoorModel(this.gameObject);
					
					var theDoor = Instantiate(doorModel, transform.position + CorridormovePosition4,Quaternion.identity) as GameObject;
					theDoor.transform.parent = this.transform;
					theDoor.transform.Rotate(rotatingPosition4);
					theDoor.name = doorModel.name;
					floorModelSpot = 1;
					ceilingModelSpot = 2;
					leftWallModelSpot = 1;
					rightWallModelSpot = 1;
					frontWallModelSpot = 1;
					backWallModelSpot = 1;
					
					var roomScript = roomOnLeftObject.GetComponent<ColliderScript>();
					roomScript.floorModelSpot =1;
				}
			}
		}
		//build floors if needed
		if (this.transform.name == "CorridorPart"){
			var newFloor = Instantiate(CorridorFloorModel, transform.position,Quaternion.identity) as GameObject;
			newFloor.transform.parent = this.transform;
			newFloor.name = CorridorFloorModel.name;
			if (floorModelSpot == 0){
				floorModelSpot = 2;
			}
		}else if (this.transform.name == "RoomPart" && buildFloor == true){
			var newFloor = Instantiate(RoomFloorModel, transform.position,Quaternion.identity) as GameObject;
			newFloor.transform.parent = this.transform;
			newFloor.name = RoomFloorModel.name;
			if (floorModelSpot == 0){
				floorModelSpot = 2;
			}
		}else if (this.transform.name == "StairCasePart" && buildFloor == true){
			var newFloor = Instantiate(RoomFloorModel, transform.position,Quaternion.identity) as GameObject;
			newFloor.transform.parent = this.transform;
			newFloor.name = RoomFloorModel.name;
			if (floorModelSpot == 0){
				floorModelSpot = 2;
			}
		}
		//build ceilings if needed
		if (this.transform.name == "CorridorPart"){
			if (roomOnTopObject == null){
				var newCeiling = Instantiate(CorridorCeilingModel, transform.position,Quaternion.identity) as GameObject;
				newCeiling.transform.parent = this.transform;
				newCeiling.name = CorridorCeilingModel.name;
				buildCeiling = true;
				if (ceilingModelSpot == 0){
					ceilingModelSpot = 2;
				}
			} else {
				if (roomOnTopObject.transform.name == "CorridorPart"){
					var newCeiling = Instantiate(CorridorCeilingModel, transform.position,Quaternion.identity) as GameObject;
					newCeiling.transform.parent = this.transform;
					newCeiling.name = CorridorCeilingModel.name;
					buildCeiling = true;
					if (ceilingModelSpot == 0){
						ceilingModelSpot = 2;
					}
				}
				if (roomOnTopObject.transform.name == "RoomPart" || roomOnTopObject.transform.name == "StairCasePart"){
					if (roomOnTopObject.transform.GetComponent<ColliderScript>().buildFloor == true){
						var newCeiling = Instantiate(CorridorCeilingModel, transform.position,Quaternion.identity) as GameObject;
						newCeiling.transform.parent = this.transform;
						newCeiling.name = CorridorCeilingModel.name;
						buildCeiling = true;
						if (ceilingModelSpot == 0){
							ceilingModelSpot = 2;
						}
					}
				}
			}
		}else if (this.transform.name == "RoomPart" || this.transform.name == "StairCasePart"){
			if (roomOnTopObject == null){
				var newCeiling = Instantiate(RoomCeilingModel, transform.position,Quaternion.identity) as GameObject;
				newCeiling.transform.parent = this.transform;
				newCeiling.name = RoomCeilingModel.name;
				buildCeiling = true;
				if (ceilingModelSpot == 0 && this.transform.name == "RoomPart"){
					ceilingModelSpot = 2;
				}
				if (ceilingModelSpot == 0 && this.transform.name == "StairCasePart"){
					ceilingModelSpot = 1;
				}
			} else {
				if (roomOnTopObject.transform.name == "CorridorPart"){
					var newCeiling = Instantiate(RoomCeilingModel, transform.position,Quaternion.identity) as GameObject;
					newCeiling.gameObject.isStatic = true;	
					newCeiling.transform.parent = this.transform;
					newCeiling.name = RoomCeilingModel.name;
					buildCeiling = true;
					if (ceilingModelSpot == 0 && this.transform.name == "RoomPart"){
						ceilingModelSpot = 2;
					}
					if (ceilingModelSpot == 0 && this.transform.name == "StairCasePart"){
						ceilingModelSpot = 1;
					}
				}
				if (roomOnTopObject.transform.name == "RoomPart" || roomOnTopObject.transform.name == "StairCasePart"){
					if (roomOnTopObject.transform.GetComponent<ColliderScript>().buildFloor == true){
						var newCeiling = Instantiate(RoomCeilingModel, transform.position,Quaternion.identity) as GameObject;
						newCeiling.transform.parent = this.transform;
						newCeiling.name = RoomCeilingModel.name;
						buildCeiling = true;
						if (ceilingModelSpot == 0 && this.transform.name == "RoomPart"){
							ceilingModelSpot = 2;
						}
						if (ceilingModelSpot == 0 && this.transform.name == "StairCasePart"){
							ceilingModelSpot = 1;
						}
					}
				}
			}
		}
		//generate walls
		if (this.transform.name == "CorridorPart"){
			if (roomOnFrontObject == null){
				var theWall = Instantiate(CorridorWallModelFront, transform.position + CorridormovePosition1,Quaternion.identity) as GameObject;
				theWall.transform.parent = this.transform;
				theWall.transform.Rotate(rotatingPosition1);
				theWall.name = CorridorWallModelFront.name;
				if (frontWallModelSpot == 0){
					frontWallModelSpot = 2;
				}
				FrontWall = true;
			}
			if (roomOnRightObject == null){
				var theWall = Instantiate(CorridorWallModelRight, transform.position + CorridormovePosition2,Quaternion.identity) as GameObject;
				theWall.transform.parent = this.transform;
				theWall.transform.Rotate(rotatingPosition2);
				theWall.name = CorridorWallModelRight.name;
				if (rightWallModelSpot == 0){
					rightWallModelSpot = 2;
				}
				RightWall = true;
			}
			if (roomOnBackObject == null){
				var theWall = Instantiate(CorridorWallModelBack, transform.position + CorridormovePosition3,Quaternion.identity) as GameObject;
				theWall.transform.parent = this.transform;
				theWall.transform.Rotate(rotatingPosition3);
				theWall.name = CorridorWallModelBack.name;
				if (backWallModelSpot == 0){
					backWallModelSpot = 2;
				}
				BackWall = true;
			}
			if (roomOnLeftObject == null){
				var theWall = Instantiate(CorridorWallModelLeft, transform.position + CorridormovePosition4,Quaternion.identity) as GameObject;
				theWall.transform.parent = this.transform;
				theWall.transform.Rotate(rotatingPosition4);
				theWall.name = CorridorWallModelLeft.name;
				if (leftWallModelSpot == 0){
					leftWallModelSpot = 2;
				}
				LeftWall = true;
			}
		}else if (this.transform.name == "RoomPart" || this.transform.name == "StairCasePart"){
			if (roomOnFrontObject == null){
				var theWall = Instantiate(RoomWallModelFront, transform.position + RoommovePosition1,Quaternion.identity) as GameObject;
				theWall.transform.parent = this.transform;
				theWall.transform.Rotate(rotatingPosition1);
				theWall.name = RoomWallModelFront.name;
				if (frontWallModelSpot == 0 && this.transform.name == "RoomPart"){
					frontWallModelSpot = 2;
				}
				FrontWall = true;
			}
			if (roomOnRightObject == null){
				var theWall = Instantiate(RoomWallModelRight, transform.position + RoommovePosition2,Quaternion.identity) as GameObject;
				theWall.transform.parent = this.transform;
				theWall.transform.Rotate(rotatingPosition2);
				theWall.name = RoomWallModelRight.name;
				if (rightWallModelSpot == 0 && this.transform.name == "RoomPart"){
					rightWallModelSpot = 2;
				}
				RightWall = true;
			}
			if (roomOnBackObject == null){
				var theWall = Instantiate(RoomWallModelBack, transform.position + RoommovePosition3,Quaternion.identity) as GameObject;
				theWall.transform.parent = this.transform;
				theWall.transform.Rotate(rotatingPosition3);
				theWall.name = RoomWallModelBack.name;
				if (backWallModelSpot == 0 && this.transform.name == "RoomPart"){
					backWallModelSpot = 2;
				}
				BackWall = true;
			}
			if (roomOnLeftObject == null){
				var theWall = Instantiate(RoomWallModelLeft, transform.position + RoommovePosition4,Quaternion.identity) as GameObject;
				theWall.transform.parent = this.transform;
				theWall.transform.Rotate(rotatingPosition4);
				theWall.name = RoomWallModelLeft.name;
				if (leftWallModelSpot == 0 && this.transform.name == "RoomPart"){
					leftWallModelSpot = 2;
				}
				LeftWall = true;
			}
		}
	}	
	public void ConstructFinishing (){	
		//railings, windows or portals of corridorPart
		if (this.transform.name == "RoomPart" || this.transform.name == "CorridorPart"){	
			if (buildFloor == true){
				if (roomOnFrontObject != null){
					if (roomOnFrontObject.transform.name == "RoomPart" || roomOnFrontObject.transform.name == "StairCasePart"){
						//portal
						if (roomOnFrontObject.transform.name != this.transform.name){
							if (roomOnFrontObject.transform.GetComponent<ColliderScript>().buildFloor == true && buildCeiling == true && doorOnFront == false){
								var thePortalMiddel = Instantiate(PortalModel, transform.position + CorridormovePosition1,Quaternion.identity) as GameObject;
								thePortalMiddel.transform.parent = this.transform;
								thePortalMiddel.transform.Rotate(rotatingPosition1);
								thePortalMiddel.name = PortalModel.name;
								//checken corners
								if (leftUpper == false){
									leftUpper = true;
									var thePortalEnd = Instantiate(endWallModel, transform.position + leftUpperLocation,Quaternion.identity) as GameObject;
									thePortalEnd.transform.parent = this.transform;
									thePortalEnd.name = endWallModel.name;
								}
								if (rightUpper == false){
									rightUpper = true;
									var thePortalEnd = Instantiate(endWallModel, transform.position + rightUpperLocation,Quaternion.identity) as GameObject;
									thePortalEnd.transform.parent = this.transform;
									thePortalEnd.name = endWallModel.name;
								}
							}
						}
						if (roomOnFrontObject.transform.GetComponent<ColliderScript>().buildFloor == false){
							//window
							if (buildCeiling == true){
								var theRailingMiddel = Instantiate(WindowModel, transform.position + CorridormovePosition1,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition1);
								theRailingMiddel.name = WindowModel.name;
								//checken corners
								if (leftUpper == false){
									leftUpper = true;
									var theRailingEnd = Instantiate(endWallModel, transform.position + leftUpperLocation,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = endWallModel.name;
								}
								if (rightUpper == false){
									rightUpper = true;
									var theRailingEnd = Instantiate(endWallModel, transform.position + rightUpperLocation,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = endWallModel.name;
								}
							}else{
								//railing
								var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition1,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition1);
								theRailingMiddel.name = RailingModel.name;
								//checken corners
								if (leftUpper == false){
									leftUpper = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftUpperLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
								if (rightUpper == false){
									rightUpper = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightUpperLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
							}
						}
					}
				}
				if (roomOnRightObject != null){
					if (roomOnRightObject.transform.name == "RoomPart" || roomOnRightObject.transform.name == "StairCasePart"){
						//portal
						if (roomOnRightObject.transform.name != this.transform.name){
							if (roomOnRightObject.transform.GetComponent<ColliderScript>().buildFloor == true && buildCeiling == true && doorOnRight == false){
								var thePortalMiddel = Instantiate(PortalModel, transform.position + CorridormovePosition2,Quaternion.identity) as GameObject;
								thePortalMiddel.transform.parent = this.transform;
								thePortalMiddel.transform.Rotate(rotatingPosition2);
								thePortalMiddel.name = PortalModel.name;
								//checken corners
								if (leftUpper == false){
									leftUpper = true;
									var thePortalEnd = Instantiate(endWallModel, transform.position + leftUpperLocation,Quaternion.identity) as GameObject;
									thePortalEnd.transform.parent = this.transform;
									thePortalEnd.name = endWallModel.name;
								}
								if (leftLower == false){
									leftLower = true;
									var thePortalEnd = Instantiate(endWallModel, transform.position + leftLowerLocation,Quaternion.identity) as GameObject;
									thePortalEnd.transform.parent = this.transform;
									thePortalEnd.name = endWallModel.name;
								}
							}
						}
						if (roomOnRightObject.transform.GetComponent<ColliderScript>().buildFloor == false){
							//window
							if (buildCeiling == true){
								var theRailingMiddel = Instantiate(WindowModel, transform.position + CorridormovePosition2,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition2);
								theRailingMiddel.name = WindowModel.name;
								//checken corners
								if (leftUpper == false){
									leftUpper = true;
									var theRailingEnd = Instantiate(endWallModel, transform.position + leftUpperLocation,Quaternion.identity) as GameObject;
									theRailingEnd.gameObject.isStatic = true;			
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = endWallModel.name;
								}
								if (leftLower == false){
									leftLower = true;
									var theRailingEnd = Instantiate(endWallModel, transform.position + leftLowerLocation,Quaternion.identity) as GameObject;
									theRailingEnd.gameObject.isStatic = true;		
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = endWallModel.name;
								}
							}
							//railing
							if (buildCeiling == false){
								var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition2,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition2);
								theRailingMiddel.name = RailingModel.name;
								//checken corners
								if (leftUpper == false){
									leftUpper = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftUpperLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
								if (leftLower == false){
									leftLower = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftLowerLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
							}
						}	
					}
				}
				if (roomOnBackObject != null){
					if (roomOnBackObject.transform.name == "RoomPart" || roomOnBackObject.transform.name == "StairCasePart"){
						//portal
						if (roomOnBackObject.transform.name != this.transform.name){
							if (roomOnBackObject.transform.GetComponent<ColliderScript>().buildFloor == true && buildCeiling == true && doorOnBack == false){
								var thePortalMiddel = Instantiate(PortalModel, transform.position + CorridormovePosition3,Quaternion.identity) as GameObject;
								thePortalMiddel.transform.parent = this.transform;
								thePortalMiddel.transform.Rotate(rotatingPosition3);
								thePortalMiddel.name = PortalModel.name;
								//checken corners
								if (rightLower == false){
									rightLower = true;
									var thePortalEnd = Instantiate(endWallModel, transform.position + rightLowerLocation,Quaternion.identity) as GameObject;
									thePortalEnd.transform.parent = this.transform;
									thePortalEnd.name = endWallModel.name;
								}
								if (leftLower == false){
									leftLower = true;
									var thePortalEnd = Instantiate(endWallModel, transform.position + leftLowerLocation,Quaternion.identity) as GameObject;
									thePortalEnd.transform.parent = this.transform;
									thePortalEnd.name = endWallModel.name;
								}
							}
						}
						if (roomOnBackObject.transform.GetComponent<ColliderScript>().buildFloor == false){
							//window
							if (buildCeiling == true){
								var theRailingMiddel = Instantiate(WindowModel, transform.position + CorridormovePosition3,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition3);
								theRailingMiddel.name = WindowModel.name;
								//checken corners
								if (rightLower == false){
									rightLower = true;
									var theRailingEnd = Instantiate(endWallModel, transform.position + rightLowerLocation,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = endWallModel.name;
								}
								if (leftLower == false){
									leftLower = true;
									var theRailingEnd = Instantiate(endWallModel, transform.position + leftLowerLocation,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = endWallModel.name;
								}
							}else{
								//railing
								var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition3,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition3);
								theRailingMiddel.name = RailingModel.name;
								//checken corners
								if (rightLower == false){
									rightLower = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightLowerLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
								if (leftLower == false){
									leftLower = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftLowerLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
							}
						}
					}
				}
				if (roomOnLeftObject != null){
					if (roomOnLeftObject.transform.name == "RoomPart" || roomOnLeftObject.transform.name == "StairCasePart"){
						//portal
						if (roomOnLeftObject.transform.name != this.transform.name){
							if (roomOnLeftObject.transform.GetComponent<ColliderScript>().buildFloor == true && buildCeiling == true && doorOnLeft == false){
								var thePortalMiddel = Instantiate(PortalModel, transform.position + CorridormovePosition4,Quaternion.identity) as GameObject;
								thePortalMiddel.transform.parent = this.transform;
								thePortalMiddel.transform.Rotate(rotatingPosition4);
								thePortalMiddel.name = PortalModel.name;
								//checken corners
								if (rightLower == false){
									rightLower = true;
									var thePortalEnd = Instantiate(endWallModel, transform.position + rightLowerLocation,Quaternion.identity) as GameObject;
									thePortalEnd.transform.parent = this.transform;
									thePortalEnd.name = endWallModel.name;
								}
								if (rightUpper == false){
									rightUpper = true;
									var thePortalEnd = Instantiate(endWallModel, transform.position + rightUpperLocation,Quaternion.identity) as GameObject;
									thePortalEnd.transform.parent = this.transform;
									thePortalEnd.name = endWallModel.name;
								}
							}
						}
						if (roomOnLeftObject.transform.GetComponent<ColliderScript>().buildFloor == false){
							//window
							if (buildCeiling == true){
								var theRailingMiddel = Instantiate(WindowModel, transform.position + CorridormovePosition4,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition4);
								theRailingMiddel.name = WindowModel.name;
								//checken corners
								if (rightUpper == false){
									rightUpper = true;
									var theRailingEnd = Instantiate(endWallModel, transform.position + rightUpperLocation,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = endWallModel.name;
								}
								if (rightLower == false){
									rightLower = true;
									var theRailingEnd = Instantiate(endWallModel, transform.position + rightLowerLocation,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = endWallModel.name;
								}
							}else{
								//railing
								var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition4,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition4);
								theRailingMiddel.name = RailingModel.name;
								//checken corners
								if (rightUpper == false){
									rightUpper = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightUpperLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
								if (rightLower == false){
									rightLower = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightLowerLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
							}
						}
					}
				}
			}
		}
		//railings staircases
		if (this.transform.name == "StairCasePart"){	
			if (buildFloor == true){
				if (roomOnFrontObject != null){
					if (roomOnFrontObject.transform.name == "RoomPart"){
						if (roomOnFrontObject.transform.GetComponent<ColliderScript>().buildFloor == false){
							var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition1,Quaternion.identity) as GameObject;
							theRailingMiddel.transform.parent = this.transform;
							theRailingMiddel.transform.Rotate(rotatingPosition1);
							theRailingMiddel.name = RailingModel.name;
							if (leftUpper == false){
								leftUpper = true;
								var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftUpperLocationRail,Quaternion.identity) as GameObject;
								theRailingEnd.transform.parent = this.transform;
								theRailingEnd.name = EndRailingModel.name;
							}
							if (rightUpper == false){
								rightUpper = true;
								var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightUpperLocationRail,Quaternion.identity) as GameObject;
								theRailingEnd.transform.parent = this.transform;
								theRailingEnd.name = EndRailingModel.name;
							}
						}
					}
					if (roomOnFrontObject.transform.name == "StairCasePart"){
						if (StairCaseNumber != roomOnFrontObject.transform.GetComponent<ColliderScript>().StairCaseNumber){
							if (roomOnFrontObject.transform.GetComponent<ColliderScript>().buildFloor == false){
								var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition1,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition1);
								theRailingMiddel.name = RailingModel.name;
								
								if (leftUpper == false){
									leftUpper = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftUpperLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
								if (rightUpper == false){
									rightUpper = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightUpperLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
							}
						}
					}
				}
				if (roomOnRightObject != null){
					if (roomOnRightObject.transform.name == "RoomPart"){
						if (roomOnRightObject.transform.GetComponent<ColliderScript>().buildFloor == false){
							var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition2,Quaternion.identity) as GameObject;
							theRailingMiddel.transform.parent = this.transform;
							theRailingMiddel.transform.Rotate(rotatingPosition2);
							theRailingMiddel.name = RailingModel.name;
							if (leftUpper == false){
								leftUpper = true;
								var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftUpperLocationRail,Quaternion.identity) as GameObject;
								theRailingEnd.transform.parent = this.transform;
								theRailingEnd.name = EndRailingModel.name;
							}
							if (leftLower == false){
								leftLower = true;
								var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftLowerLocationRail,Quaternion.identity) as GameObject;
								theRailingEnd.transform.parent = this.transform;
								theRailingEnd.name = EndRailingModel.name;
							}	
						}
					}
					if (roomOnRightObject.transform.name == "StairCasePart"){
						if (StairCaseNumber != roomOnRightObject.transform.GetComponent<ColliderScript>().StairCaseNumber){
							if (roomOnRightObject.transform.GetComponent<ColliderScript>().buildFloor == false){
								var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition2,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition2);
								theRailingMiddel.name = RailingModel.name;
								if (leftUpper == false){
									leftUpper = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftUpperLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
								if (leftLower == false){
									leftLower = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftLowerLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}	
							}
						}
					}
				}
				if (roomOnBackObject != null){
					if (roomOnBackObject.transform.name == "RoomPart"){
						if (roomOnBackObject.transform.GetComponent<ColliderScript>().buildFloor == false){
							var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition3,Quaternion.identity) as GameObject;
							theRailingMiddel.transform.parent = this.transform;
							theRailingMiddel.transform.Rotate(rotatingPosition3);
							theRailingMiddel.name = RailingModel.name;
							if (rightLower == false){
								rightLower = true;
								var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightLowerLocationRail,Quaternion.identity) as GameObject;
								theRailingEnd.transform.parent = this.transform;
								theRailingEnd.name = EndRailingModel.name;
							}
							if (leftLower == false){
								leftLower = true;
								var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftLowerLocationRail,Quaternion.identity) as GameObject;
								theRailingEnd.transform.parent = this.transform;
								theRailingEnd.name = EndRailingModel.name;
							}
						}
					}
					if (roomOnBackObject.transform.name == "StairCasePart"){
						if (StairCaseNumber != roomOnBackObject.transform.GetComponent<ColliderScript>().StairCaseNumber){
							if (roomOnBackObject.transform.GetComponent<ColliderScript>().buildFloor == false){
								var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition3,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition3);
								theRailingMiddel.name = RailingModel.name;
								if (rightLower == false){
									rightLower = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightLowerLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
								if (leftLower == false){
									leftLower = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + leftLowerLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
							}
						}
					}
				}
				if (roomOnLeftObject != null){
					if (roomOnLeftObject.transform.name == "RoomPart"){
						if (roomOnLeftObject.transform.GetComponent<ColliderScript>().buildFloor == false){
							var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition4,Quaternion.identity) as GameObject;
							theRailingMiddel.transform.parent = this.transform;
							theRailingMiddel.transform.Rotate(rotatingPosition4);
							theRailingMiddel.name = RailingModel.name;
							if (rightUpper == false){
								rightUpper = true;
								var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightUpperLocationRail,Quaternion.identity) as GameObject;
								theRailingEnd.transform.parent = this.transform;
								theRailingEnd.name = EndRailingModel.name;
							}
							if (rightLower == false){
								rightLower = true;
								var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightLowerLocationRail,Quaternion.identity) as GameObject;
								theRailingEnd.transform.parent = this.transform;
								theRailingEnd.name = EndRailingModel.name;
							}
						}
					}
					if (roomOnLeftObject.transform.name == "StairCasePart"){
						if (StairCaseNumber != roomOnLeftObject.transform.GetComponent<ColliderScript>().StairCaseNumber){
							if (roomOnLeftObject.transform.GetComponent<ColliderScript>().buildFloor == false){
								var theRailingMiddel = Instantiate(RailingModel, transform.position + CorridormovePosition4,Quaternion.identity) as GameObject;
								theRailingMiddel.transform.parent = this.transform;
								theRailingMiddel.transform.Rotate(rotatingPosition4);
								theRailingMiddel.name = RailingModel.name;
								if (rightUpper == false){
									rightUpper = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightUpperLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
								if (rightLower == false){
									rightLower = true;
									var theRailingEnd = Instantiate(EndRailingModel, transform.position + rightLowerLocationRail,Quaternion.identity) as GameObject;
									theRailingEnd.transform.parent = this.transform;
									theRailingEnd.name = EndRailingModel.name;
								}
							}
						}
					}
				}
			}		
		}
	}
	public void StartBuildCornerWalls (){
		if (this.transform.name == "RoomPart" && buildCeiling == true){
			var randomEdges = Random.Range (0,101);
			var dungeonGeneratorScript = GameObject.FindWithTag("DungeonBuilder").GetComponent<DungeonGenerator>();
			CornerWallChancePercentage = dungeonGeneratorScript.CornerWallChancePercentage;
			if (randomEdges < CornerWallChancePercentage){
				if (roomOnFrontObject == null && roomOnRightObject == null && roomOnBackObject != null && roomOnLeftObject != null){
					if (roomOnBackObject.transform.name != "CorridorPart" && roomOnLeftObject.transform.name != "CorridorPart"){
						var theCorner = Instantiate(RoomCornerWallModel, transform.position+new Vector3(0,0.01f,0),Quaternion.identity) as GameObject;
						theCorner.transform.Rotate(rotatingPosition2);
						theCorner.transform.name = RoomCornerWallModel.name;
						theCorner.transform.parent = this.transform;
						
						ceilingModelSpot = 1;
						floorModelSpot = 1;
						leftWallModelSpot = 1;
						rightWallModelSpot = 1;
						frontWallModelSpot = 1;
						backWallModelSpot = 1;
						
						if (roomBelowObject != null){
							if (roomBelowObject.transform.name == "RoomPart" ){
							roomBelowObject.GetComponent<ColliderScript>().RoomCornerWallModel = RoomCornerWallModel;
							roomBelowObject.GetComponent<ColliderScript>().BuildCornerWallsContinued ();
							}
						}
					}
				}else if (roomOnFrontObject == null && roomOnLeftObject == null & roomOnRightObject != null && roomOnBackObject != null){
					if (roomOnRightObject.transform.name != "CorridorPart" && roomOnBackObject.transform.name != "CorridorPart"){
						var theCorner = Instantiate(RoomCornerWallModel, transform.position+new Vector3(0,0.01f,0),Quaternion.identity) as GameObject;
						theCorner.transform.Rotate(rotatingPosition1);
						theCorner.transform.name = RoomCornerWallModel.name;
						theCorner.transform.parent = this.transform;
						
						ceilingModelSpot = 1;
						floorModelSpot = 1;
						leftWallModelSpot = 1;
						rightWallModelSpot = 1;
						frontWallModelSpot = 1;
						backWallModelSpot = 1;
						
						if (roomBelowObject != null){
							if (roomBelowObject.transform.name == "RoomPart" ){
								roomBelowObject.GetComponent<ColliderScript>().RoomCornerWallModel = RoomCornerWallModel;
								roomBelowObject.GetComponent<ColliderScript>().BuildCornerWallsContinued ();
							}
						}
					}
				}else if (roomOnBackObject == null && roomOnRightObject == null && roomOnFrontObject != null && roomOnLeftObject != null){
					if (roomOnFrontObject.transform.name != "CorridorPart" && roomOnLeftObject.transform.name != "CorridorPart"){
						var theCorner = Instantiate(RoomCornerWallModel, transform.position+new Vector3(0,0.01f,0),Quaternion.identity) as GameObject;
						theCorner.transform.Rotate(rotatingPosition3);
						theCorner.transform.name = RoomCornerWallModel.name;
						theCorner.transform.parent = this.transform;
						
						ceilingModelSpot = 1;
						floorModelSpot = 1;
						leftWallModelSpot = 1;
						rightWallModelSpot = 1;
						frontWallModelSpot = 1;
						backWallModelSpot = 1;
						
						if (roomBelowObject != null){
							if (roomBelowObject.transform.name == "RoomPart" ){
								roomBelowObject.GetComponent<ColliderScript>().RoomCornerWallModel = RoomCornerWallModel;
								roomBelowObject.GetComponent<ColliderScript>().BuildCornerWallsContinued ();
							}
						}
					}
				}else if (roomOnBackObject == null && roomOnLeftObject == null && roomOnRightObject != null && roomOnFrontObject != null){
					if (roomOnRightObject.transform.name != "CorridorPart" && roomOnFrontObject.transform.name != "CorridorPart"){
						var theCorner = Instantiate(RoomCornerWallModel, transform.position+new Vector3(0,0.01f,0),Quaternion.identity) as GameObject;
						theCorner.transform.Rotate(rotatingPosition4);
						theCorner.transform.name = RoomCornerWallModel.name;
						theCorner.transform.parent = this.transform;
						
						ceilingModelSpot = 1;
						floorModelSpot = 1;
						leftWallModelSpot = 1;
						rightWallModelSpot = 1;
						frontWallModelSpot = 1;
						backWallModelSpot = 1;
						
						if (roomBelowObject != null){
							if (roomBelowObject.transform.name == "RoomPart" ){
								roomBelowObject.GetComponent<ColliderScript>().RoomCornerWallModel = RoomCornerWallModel;
								roomBelowObject.GetComponent<ColliderScript>().BuildCornerWallsContinued ();
							}
						}
					}
				}
			}
		}
	}
	public void BuildCornerWallsContinued (){
		if (this.transform.name == "RoomPart" && beginPointer == false && endPointer == false){
			if (roomOnFrontObject == null && roomOnRightObject == null && roomOnBackObject != null && roomOnLeftObject != null){
				if (roomOnBackObject.transform.name != "CorridorPart" && roomOnLeftObject.transform.name != "CorridorPart"){
					var theCorner = Instantiate(RoomCornerWallModel, transform.position+new Vector3(0,0.01f,0),Quaternion.identity) as GameObject;
					theCorner.transform.Rotate(rotatingPosition2);
					theCorner.transform.name = RoomCornerWallModel.name;
					theCorner.transform.parent = this.transform;
					
					ceilingModelSpot = 1;
					floorModelSpot = 1;
					leftWallModelSpot = 1;
					rightWallModelSpot = 1;
					frontWallModelSpot = 1;
					backWallModelSpot = 1;
					
					if (roomBelowObject != null){
						if (roomBelowObject.transform.name == "RoomPart" ){
							roomBelowObject.GetComponent<ColliderScript>().RoomCornerWallModel = RoomCornerWallModel;
							roomBelowObject.GetComponent<ColliderScript>().BuildCornerWallsContinued ();
						}
					}
				}
			}else if (roomOnFrontObject == null && roomOnLeftObject == null & roomOnRightObject != null && roomOnBackObject != null){
				if (roomOnRightObject.transform.name != "CorridorPart" && roomOnBackObject.transform.name != "CorridorPart"){
					var theCorner = Instantiate(RoomCornerWallModel, transform.position+new Vector3(0,0.01f,0),Quaternion.identity) as GameObject;
					theCorner.transform.Rotate(rotatingPosition1);
					theCorner.transform.name = RoomCornerWallModel.name;
					theCorner.transform.parent = this.transform;
					
					ceilingModelSpot = 1;
					floorModelSpot = 1;
					leftWallModelSpot = 1;
					rightWallModelSpot = 1;
					frontWallModelSpot = 1;
					backWallModelSpot = 1;
					
					if (roomBelowObject != null){
						if (roomBelowObject.transform.name == "RoomPart" ){
							roomBelowObject.GetComponent<ColliderScript>().RoomCornerWallModel = RoomCornerWallModel;
							roomBelowObject.GetComponent<ColliderScript>().BuildCornerWallsContinued ();
						}
					}
				}
			}else if (roomOnBackObject == null && roomOnRightObject == null && roomOnFrontObject != null && roomOnLeftObject != null){
				if (roomOnFrontObject.transform.name != "CorridorPart" && roomOnLeftObject.transform.name != "CorridorPart"){
					var theCorner = Instantiate(RoomCornerWallModel, transform.position+new Vector3(0,0.01f,0),Quaternion.identity) as GameObject;
					theCorner.transform.Rotate(rotatingPosition3);						
					theCorner.transform.name = RoomCornerWallModel.name;
					theCorner.transform.parent = this.transform;
					
					ceilingModelSpot = 1;
					floorModelSpot = 1;
					leftWallModelSpot = 1;
					rightWallModelSpot = 1;
					frontWallModelSpot = 1;
					backWallModelSpot = 1;
					
					if (roomBelowObject != null){
						if (roomBelowObject.transform.name == "RoomPart" ){
							roomBelowObject.GetComponent<ColliderScript>().RoomCornerWallModel = RoomCornerWallModel;
							roomBelowObject.GetComponent<ColliderScript>().BuildCornerWallsContinued ();
						}
					}
				}
			}else if (roomOnBackObject == null && roomOnLeftObject == null && roomOnRightObject != null && roomOnFrontObject != null){
				if (roomOnRightObject.transform.name != "CorridorPart" && roomOnFrontObject.transform.name != "CorridorPart"){
					var theCorner = Instantiate(RoomCornerWallModel, transform.position+new Vector3(0,0.01f,0),Quaternion.identity) as GameObject;
					theCorner.transform.Rotate(rotatingPosition4);
					theCorner.transform.name = RoomCornerWallModel.name;
					theCorner.transform.parent = this.transform;
				
					ceilingModelSpot = 1;
					floorModelSpot = 1;
					leftWallModelSpot = 1;
					rightWallModelSpot = 1;
					frontWallModelSpot = 1;
					backWallModelSpot = 1;
					
					if (roomBelowObject != null){
						if (roomBelowObject.transform.name == "RoomPart" ){
							roomBelowObject.GetComponent<ColliderScript>().RoomCornerWallModel = RoomCornerWallModel;
							roomBelowObject.GetComponent<ColliderScript>().BuildCornerWallsContinued ();
						}
					}
				}
			}
		}	
	}
	public void GeneratePrimaryObject(){
		var LevelSettingsScript = GameObject.FindWithTag("DungeonBuilder").GetComponent<DungeonGenerator>();
		
			if (rightWallModelSpot == 2){
				tempObject = null;
				if (corridorPrimaryRightWallObjects.Count>0 && this.transform.name == "CorridorPart"){
					if (primaryObjectSpot == true){	
						GettingNewPrimaryWallObject(corridorPrimaryRightWallObjects);
						tempObject = corridorPrimaryWallObject;
					}
				}
				if (roomPrimaryRightWallObjects.Count > 0 && this.transform.name == "RoomPart"){
					var random = Random.Range (0,101);
					if (random < LevelSettingsScript.wallObjectPercentage){
						GettingNewPrimaryWallObject(roomPrimaryRightWallObjects);
						tempObject = roomPrimaryWallObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position + CorridormovePosition2, Quaternion.identity) as GameObject;
					TheObject.transform.Rotate(rotatingPosition2);
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "rightWallModelSpot";
					ObjectScript.PlacementRotation = 90;
					ObjectScript.CheckingPrimary = true;
					ObjectScript.ExecuteObject();
				}
			}
			if (leftWallModelSpot == 2){
				tempObject = null;
				if (corridorPrimaryLeftWallObjects.Count>0 && this.transform.name == "CorridorPart"){
					if (primaryObjectSpot == true){	
						GettingNewPrimaryWallObject(corridorPrimaryLeftWallObjects);
						tempObject = corridorPrimaryWallObject;
					}
				}
				if (roomPrimaryLeftWallObjects.Count > 0 && this.transform.name == "RoomPart"){
					var random = Random.Range (0,101);
					if (random < LevelSettingsScript.wallObjectPercentage){
						GettingNewPrimaryWallObject(roomPrimaryLeftWallObjects);
						tempObject = roomPrimaryWallObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position + CorridormovePosition4, Quaternion.identity) as GameObject;
					TheObject.transform.Rotate(rotatingPosition4);
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "leftWallModelSpot";
					ObjectScript.PlacementRotation = 270;
					ObjectScript.CheckingPrimary = true;
					ObjectScript.ExecuteObject();
				}
			}
			if (frontWallModelSpot == 2){
				tempObject = null;
				if (corridorPrimaryFrontWallObjects.Count>0 && this.transform.name == "CorridorPart"){
					if (primaryObjectSpot == true){	
						GettingNewPrimaryWallObject(corridorPrimaryFrontWallObjects);
						tempObject = corridorPrimaryWallObject;
					}
				}
				if (roomPrimaryFrontWallObjects.Count > 0 && this.transform.name == "RoomPart"){
					var random = Random.Range (0,101);
					if (random < LevelSettingsScript.wallObjectPercentage){
						GettingNewPrimaryWallObject(roomPrimaryFrontWallObjects);
						tempObject = roomPrimaryWallObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position + CorridormovePosition1, Quaternion.identity) as GameObject;
					TheObject.transform.Rotate(rotatingPosition1);
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "frontWallModelSpot";
					ObjectScript.PlacementRotation = -180;
					ObjectScript.CheckingPrimary = true;	
					ObjectScript.ExecuteObject();
				}
			}
			if (backWallModelSpot == 2){
				tempObject = null;
				if (corridorPrimaryBackWallObjects.Count>0 && this.transform.name == "CorridorPart"){
					if (primaryObjectSpot == true){	
						GettingNewPrimaryWallObject(corridorPrimaryBackWallObjects);
						tempObject = corridorPrimaryWallObject;
					}
				}
				if (roomPrimaryBackWallObjects.Count > 0 && this.transform.name == "RoomPart"){
					var random = Random.Range (0,101);
					if (random < LevelSettingsScript.wallObjectPercentage){	
						GettingNewPrimaryWallObject(roomPrimaryBackWallObjects);
						tempObject = roomPrimaryWallObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position + CorridormovePosition3, Quaternion.identity) as GameObject;
					TheObject.transform.Rotate(rotatingPosition3);
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "backWallModelSpot";
					ObjectScript.PlacementRotation = 0;
					ObjectScript.CheckingPrimary = true;	
					ObjectScript.ExecuteObject();
				}
			}
			if (ceilingModelSpot == 2){
				tempObject = null;
				if (corridorPrimaryCeilingObjects.Count > 0 && this.transform.name == "CorridorPart"){
					if (primaryObjectSpot == true){	
						GettingNewPrimaryCeilingObject(corridorPrimaryCeilingObjects);
						tempObject = corridorPrimaryCeilingObject;
					}
				}
				if (roomPrimaryCeilingObjects.Count > 0 && this.transform.name == "RoomPart"){
					var random = Random.Range (0,101);
					if (random < LevelSettingsScript.ceilingObjectPercentage){	
						GettingNewPrimaryCeilingObject(roomPrimaryCeilingObjects);
						tempObject = roomPrimaryCeilingObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position, Quaternion.identity) as GameObject;
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "ceilingModelSpot";
					if (ObjectScript.TouchingWall != "corner" && ObjectScript.TouchingWall != "cornerOne" && ObjectScript.TouchingWall != "cornerTwo"){
						ObjectScript.PlacementRotation = RandomRotate();
						TheObject.transform.Rotate(tempRotation);
					}
					ObjectScript.CheckingPrimary = true;
					ObjectScript.ExecuteObject();
				}
			}
			if (floorModelSpot == 2){
				tempObject = null;
				if (corridorPrimaryFloorObjects.Count > 0 && this.transform.name == "CorridorPart"){
					if (primaryObjectSpot == true){	
						GettingNewPrimaryFloorObject(corridorPrimaryFloorObjects);
						tempObject = corridorPrimaryFloorObject;
					}
				}
				if (roomPrimaryFloorObjects.Count > 0 && this.transform.name == "RoomPart"){
					var random = Random.Range (0,101);
					if (random < LevelSettingsScript.floorObjectPercentage){	
						GettingNewPrimaryFloorObject(roomPrimaryFloorObjects);
						tempObject = roomPrimaryFloorObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position, Quaternion.identity) as GameObject;
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "floorModelSpot";
					if (ObjectScript.TouchingWall != "corner" && ObjectScript.TouchingWall != "cornerOne" && ObjectScript.TouchingWall != "cornerTwo"){
						ObjectScript.PlacementRotation = RandomRotate();
						TheObject.transform.Rotate(tempRotation);
					}
					ObjectScript.CheckingPrimary = true;
					ObjectScript.ExecuteObject();
				}
			}	
		//}	
	}
	public void GenerateSecundaryObject(){
			var LevelSettingsScript = GameObject.FindWithTag("DungeonBuilder").GetComponent<DungeonGenerator>();
		
			
		
		//if (retrySecundaryCounter <15){
			if (rightWallModelSpot == 2){
				tempObject = null;
				if (corridorSecundaryRightWallObjects.Count>0 && this.transform.name == "CorridorPart"){
					if (corridorSecundaryRightWallObjects.Count == LevelSettingsScript.corridorSecundaryWallObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.wallObjectPercentage){
							GettingNewSecundaryWallObject(corridorSecundaryRightWallObjects);
							tempObject = corridorWallObject;
						}
					} else {
						GettingNewSecundaryWallObject(corridorSecundaryRightWallObjects);
						tempObject = corridorWallObject;
					}
				}
				if (roomSecundaryRightWallObjects.Count > 0 && this.transform.name == "RoomPart"){
					if (roomSecundaryRightWallObjects.Count == LevelSettingsScript.roomSecundaryWallObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.wallObjectPercentage){
							GettingNewSecundaryWallObject(roomSecundaryRightWallObjects);
							tempObject = roomWallObject;
						}
					} else {
						GettingNewSecundaryWallObject(roomSecundaryRightWallObjects);
						tempObject = roomWallObject;
					}
				}
				
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position + CorridormovePosition2, Quaternion.identity) as GameObject;
					TheObject.transform.Rotate(rotatingPosition2);
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "rightWallModelSpot";
					ObjectScript.PlacementRotation = 90;
					ObjectScript.CheckingPrimary = false;
					ObjectScript.ExecuteObject();
				}
			}
			
			if (leftWallModelSpot == 2){
				tempObject = null;
				if (corridorSecundaryLeftWallObjects.Count>0 && this.transform.name == "CorridorPart"){
					if (corridorSecundaryLeftWallObjects.Count == LevelSettingsScript.corridorSecundaryWallObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.wallObjectPercentage){
							GettingNewSecundaryWallObject(corridorSecundaryLeftWallObjects);
							tempObject = corridorWallObject;
						}
					} else {
						GettingNewSecundaryWallObject(corridorSecundaryLeftWallObjects);
						tempObject = corridorWallObject;
					}
				}
				if (roomSecundaryLeftWallObjects.Count > 0 && this.transform.name == "RoomPart"){
					if (roomSecundaryLeftWallObjects.Count == LevelSettingsScript.roomSecundaryWallObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.wallObjectPercentage){
							GettingNewSecundaryWallObject(roomSecundaryLeftWallObjects);
							tempObject = roomWallObject;
						}
					} else {
						GettingNewSecundaryWallObject(roomSecundaryLeftWallObjects);
						tempObject = roomWallObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position + CorridormovePosition4, Quaternion.identity) as GameObject;
					TheObject.transform.Rotate(rotatingPosition4);
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "leftWallModelSpot";
					ObjectScript.PlacementRotation = 270;
					ObjectScript.CheckingPrimary = false;	
					ObjectScript.ExecuteObject();
				}
			}
			if (frontWallModelSpot == 2){
				tempObject = null;
				if (corridorSecundaryFrontWallObjects.Count>0 && this.transform.name == "CorridorPart"){
					if (corridorSecundaryFrontWallObjects.Count == LevelSettingsScript.corridorSecundaryWallObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.wallObjectPercentage){
							GettingNewSecundaryWallObject(corridorSecundaryFrontWallObjects);
							tempObject = corridorWallObject;
						}
					} else {
						GettingNewSecundaryWallObject(corridorSecundaryFrontWallObjects);
						tempObject = corridorWallObject;
					}
				}
				if (roomSecundaryFrontWallObjects.Count > 0 && this.transform.name == "RoomPart"){
					if (roomSecundaryFrontWallObjects.Count == LevelSettingsScript.roomSecundaryWallObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.wallObjectPercentage){
							GettingNewSecundaryWallObject(roomSecundaryFrontWallObjects);
							tempObject = roomWallObject;
						}
					} else {
						GettingNewSecundaryWallObject(roomSecundaryFrontWallObjects);
						tempObject = roomWallObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position + CorridormovePosition1, Quaternion.identity) as GameObject;
					TheObject.transform.Rotate(rotatingPosition1);
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "frontWallModelSpot";
					ObjectScript.PlacementRotation = -180;
					ObjectScript.CheckingPrimary = false;	
					ObjectScript.ExecuteObject();
				}
			}
			if (backWallModelSpot == 2){
				tempObject = null;
				if (corridorSecundaryBackWallObjects.Count>0 && this.transform.name == "CorridorPart"){
					if (corridorSecundaryBackWallObjects.Count == LevelSettingsScript.corridorSecundaryWallObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.wallObjectPercentage){
							GettingNewSecundaryWallObject(corridorSecundaryBackWallObjects);
							tempObject = corridorWallObject;
						}
					} else {
						GettingNewSecundaryWallObject(corridorSecundaryBackWallObjects);
						tempObject = corridorWallObject;
					}
				}
				if (roomSecundaryBackWallObjects.Count > 0 && this.transform.name == "RoomPart"){
					if (roomSecundaryBackWallObjects.Count == LevelSettingsScript.roomSecundaryWallObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.wallObjectPercentage){
							GettingNewSecundaryWallObject(roomSecundaryBackWallObjects);
							tempObject = roomWallObject;
						}
					} else {
						GettingNewSecundaryWallObject(roomSecundaryBackWallObjects);
						tempObject = roomWallObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position + CorridormovePosition3, Quaternion.identity) as GameObject;
					TheObject.transform.Rotate(rotatingPosition3);
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "backWallModelSpot";
					ObjectScript.PlacementRotation = 0;
					ObjectScript.CheckingPrimary = false;	
					ObjectScript.ExecuteObject();
				}
			}
			if (ceilingModelSpot == 2){
				tempObject = null;
				if (corridorSecundaryCeilingObjects.Count>0 && this.transform.name == "CorridorPart"){
					if (corridorSecundaryCeilingObjects.Count == LevelSettingsScript.corridorSecundaryCeilingObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.ceilingObjectPercentage){
							GettingNewSecundaryCeilingObject(corridorSecundaryCeilingObjects);
							tempObject = corridorCeilingObject;
						}
					} else {
						GettingNewSecundaryCeilingObject(corridorSecundaryCeilingObjects);
						tempObject = corridorCeilingObject;
					}
				}
				if (roomSecundaryCeilingObjects.Count > 0 && this.transform.name == "RoomPart"){
					if (roomSecundaryCeilingObjects.Count == LevelSettingsScript.roomSecundaryCeilingObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.ceilingObjectPercentage){
							GettingNewSecundaryCeilingObject(roomSecundaryCeilingObjects);
							tempObject = roomCeilingObject;
						}
					} else {
						GettingNewSecundaryCeilingObject(roomSecundaryCeilingObjects);
						tempObject = roomCeilingObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position, Quaternion.identity) as GameObject;
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "ceilingModelSpot";
					if (ObjectScript.TouchingWall != "corner" && ObjectScript.TouchingWall != "cornerOne" && ObjectScript.TouchingWall != "cornerTwo"){
						ObjectScript.PlacementRotation = RandomRotate();
						TheObject.transform.Rotate(tempRotation);
					}
					ObjectScript.CheckingPrimary = false;
					ObjectScript.ExecuteObject();
				}
			}
			if (floorModelSpot == 2){
				tempObject = null;
				//print (corridorSecundaryFloorObjects.Count);
				if (corridorSecundaryFloorObjects.Count >0 && this.transform.name == "CorridorPart"){
					if (corridorSecundaryFloorObjects.Count == LevelSettingsScript.corridorSecundaryFloorObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.floorObjectPercentage){
							GettingNewSecundaryFloorObject(corridorSecundaryFloorObjects);
							tempObject = corridorFloorObject;
						}
					} else {
						GettingNewSecundaryFloorObject(corridorSecundaryFloorObjects);
						tempObject = corridorFloorObject;
					}
				}
				if (roomSecundaryFloorObjects.Count > 0 && this.transform.name == "RoomPart"){
					if (roomSecundaryFloorObjects.Count == LevelSettingsScript.roomSecundaryFloorObjects.Count){
						var random = Random.Range (0,101);
						if (random < LevelSettingsScript.floorObjectPercentage){
							GettingNewSecundaryFloorObject(roomSecundaryFloorObjects);
							tempObject = roomFloorObject;
						}
					} else {
						GettingNewSecundaryFloorObject(roomSecundaryFloorObjects);
						tempObject = roomFloorObject;
					}
				}
				if (tempObject != null){
					var TheObject = Instantiate(tempObject, this.transform.position, Quaternion.identity) as GameObject;
					TheObject.transform.parent = this.transform;
					TheObject.transform.name = tempObject.transform.name;
					var ObjectScript = TheObject.GetComponent<ObjectScript>();
					ObjectScript.ParentCollider = this.transform.gameObject;
					ObjectScript.ObjectOrientation = "floorModelSpot";
					if (ObjectScript.TouchingWall != "corner" && ObjectScript.TouchingWall != "cornerOne" && ObjectScript.TouchingWall != "cornerTwo"){
						ObjectScript.PlacementRotation = RandomRotate();
						TheObject.transform.Rotate(tempRotation);
					}
					ObjectScript.CheckingPrimary = false;
					ObjectScript.ExecuteObject();
				}
			}	
		//}	
	}
	int RandomRotate(){
		var tempNumber = Random.Range(0,4);
		if (tempNumber == 0){
			tempRotation = new Vector3(0,-180,0);
			return -180;	
		}
		if (tempNumber == 1){
			tempRotation = new Vector3(0,90,0);
			return 90;	
		}
		if (tempNumber == 2){
			tempRotation = new Vector3(0,0,0);
			return 0;	
		}
		if (tempNumber == 3){
			tempRotation = new Vector3(0,270,0);
			return 270;	
		}
		return 0;
	}
	
	public void DestroyAllColorSpheres(){
		DestroyImmediate(colorSphere);	
	}
	
	public void DestroyAllTraces(){
		
		if (beginPointer == true){
			var begin = Instantiate(begingPointObject, this.transform.position, Quaternion.identity) as GameObject;
			begin.transform.parent = this.transform;
			begin.transform.name = begingPointObject.transform.name;
			var LevelSettingsScript = GameObject.FindWithTag("DungeonBuilder").GetComponent<DungeonGenerator>();
			//set the start location 
			LevelSettingsScript.playerSpawnLocation = this.transform.position;
			
		}
		if (endPointer == true){
			var end = Instantiate(endPointObject, this.transform.position, Quaternion.identity) as GameObject;
			end.transform.parent = this.transform;
			end.transform.name = endPointObject.transform.name;
		}
		
		
		DestroyImmediate(this);
	}
	public void GettingNewPrimaryWallObject(List<GameObject> requestingList){
		//tempPartRequestingModel = requestingCollider;
		//var attachedColliderScript = requestingCollider.GetComponent<ColliderScript>();
		if (this.transform.name == "RoomPart"){
			var tempRoomWallModel = RandomModel(requestingList);
			roomPrimaryWallObject = tempRoomWallModel;
		}
		if (this.transform.name == "CorridorPart"){
			var tempCorridorWallModel = RandomModel(requestingList);
			corridorPrimaryWallObject = tempCorridorWallModel;
		}
	}
	public void GettingNewPrimaryCeilingObject(List<GameObject> requestingList){
		//tempPartRequestingModel = requestingCollider;
		//var attachedColliderScript = requestingCollider.GetComponent<ColliderScript>();
		if (this.transform.name == "RoomPart"){
			var tempRoomCeilingLightModel = RandomModel(requestingList);
			roomPrimaryCeilingObject = tempRoomCeilingLightModel;
		}
		if (this.transform.name == "CorridorPart"){
			var tempCorridorCeilingLightModel = RandomModel(requestingList);
			corridorPrimaryCeilingObject = tempCorridorCeilingLightModel;
		}
	}
	public void GettingNewPrimaryFloorObject(List<GameObject> requestingList){
		//tempPartRequestingModel = requestingCollider;
		//var attachedColliderScript = requestingCollider.GetComponent<ColliderScript>();
		if (this.transform.name == "RoomPart"){
			var tempRoomFloorLightModel = RandomModel(requestingList);
			roomPrimaryFloorObject = tempRoomFloorLightModel;
		}
		if (this.transform.name == "CorridorPart"){
			var tempCorridorFloorLightModel = RandomModel(requestingList);
			corridorPrimaryFloorObject = tempCorridorFloorLightModel;
		}
	}
	public void GettingNewSecundaryWallObject(List<GameObject> requestingList){
		//tempPartRequestingModel = requestingCollider;
		//var attachedColliderScript = requestingCollider.GetComponent<ColliderScript>();
		if (this.transform.name == "RoomPart"){
			var tempRoomWallObjectModel = RandomModel(requestingList);
			roomWallObject = tempRoomWallObjectModel;
		}
		if (this.transform.name == "CorridorPart"){
			var tempCorridorWallObjectModel = RandomModel(requestingList);
			corridorWallObject = tempCorridorWallObjectModel;
		}
	}
	public void GettingNewSecundaryCeilingObject(List<GameObject> requestingList){
		//tempPartRequestingModel = requestingCollider;
		//var attachedColliderScript = requestingCollider.GetComponent<ColliderScript>();
		if (this.transform.name == "RoomPart"){
			var tempRoomCeilingObjectModel = RandomModel(requestingList);
			roomCeilingObject = tempRoomCeilingObjectModel;
		}
		if (this.transform.name == "CorridorPart"){
			var tempCorridorCeilingObjectModel = RandomModel(requestingList);
			corridorCeilingObject = tempCorridorCeilingObjectModel;
		}
	}
	public void GettingNewSecundaryFloorObject(List<GameObject> requestingList){
		//tempPartRequestingModel = requestingCollider;
		//var attachedColliderScript = requestingCollider.GetComponent<ColliderScript>();
		if (this.transform.name == "RoomPart"){
			var tempRoomFloorObjectModel = RandomModel(requestingList);
			roomFloorObject = tempRoomFloorObjectModel;
		}
		if (this.transform.name == "CorridorPart"){
			var tempCorridorFloorObjectModel = RandomModel(requestingList);
			corridorFloorObject = tempCorridorFloorObjectModel;
		}
	}
	GameObject RandomModel(List<GameObject> tempList){
		var tempTypeList =  new List<GameObject>();
		if (tempList.Count > 0){
			foreach (GameObject model in tempList){
				var modelShortName = model.transform.name.Split("_"[0]);
				if (partType == modelShortName[0] ){
					tempTypeList.Add (model);
				}
			}
		}
		if (tempTypeList.Count > 0){		
			var tempTypeModelNumber = Random.Range(0,tempTypeList.Count);
			if (tempTypeList[tempTypeModelNumber] != null){
				var tempModel = tempTypeList[tempTypeModelNumber];
				//removing the model from the original list
				for(int i = tempList.Count -1; i > -1 ; i--){
					if (tempModel == tempList[i]){
						tempList.RemoveAt(i);
						break;
					}
				}
				return tempModel;
			} else {
				return null;
			}
		} else {
			return null;
		}
	}
}