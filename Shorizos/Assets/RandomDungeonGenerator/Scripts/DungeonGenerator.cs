using UnityEngine;

//using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DungeonGenerator : MonoBehaviour {
	
	//values for coridor primary object placement
	public bool maximumObjects = false;
 	public bool objectNoObject = true;
 	public bool objectNoObjectNoObject = false;
	
	//dungeon inspector editor item
	public int generating = 0;
	public bool pickStyle = false;
	
	//corridor setup
	public int maxAmountOfCorridor;
	public int currentCorridorCounter;
	public int corridorPartCount;
	public int minCorridorLength;
	public int maxCorridorLength;
	
	//corridor variables during build
	private int actualCorridorLength;
	private int currentCorridorLength;
	private bool corridorBuilt =false;
	private int OldCorridorDirection;
	public bool lessRandomCorridors = false;
	
	//room setup
	public int roomChancePercentage;
	public int minRoomWidth;
	public int maxRoomWidth;
	public int minRoomLength;
	public int maxRoomLength;
	public int minRoomHeigth;
	public int maxRoomHeigth;
	public int CornerWallChancePercentage;
	
	//staircase setup
	public int staircaseChancePercentage;
	public string stairCaseDirectionSetup;
	private string stairCaseDirection;
	private int StairCaseNumber;
	//user chosen staircase directions
	public bool stairCaseUp;
	public bool stairCaseDown;
	public bool stairCaseRandom;
	
	//side corridor setup
	public int sideCorridorChancePercentage;
	public int maxAmountOfSideCorridor;
	private int currentSideCorridorCounter;
	public int minSideCorridorLength;
	public int maxSideCorridorLength;
	
	//percentages for secondary object chance
	public int wallObjectPercentage;
	public int ceilingObjectPercentage;
	public int floorObjectPercentage;
	
	//resources for building the dungeon
	public GameObject buildPointObject;
	public GameObject corridorPart;
	public GameObject stairCasePart;
	public GameObject splitterBuildPoint;
	public GameObject roomBuildPoint;
	public GameObject roomPart;
	
	public GameObject playerCharacter;
	public Vector3 playerSpawnLocation;
	private GameObject buildPointer;
	public GameObject level;
	
	//lists to keep track of generated parts
	public List<GameObject> corridorPartList;
	public List<GameObject> stairCasePartList;
	public List<GameObject> splitterBuildPointsList;
	public List<GameObject> roomBuildPointsList;
	public List<GameObject> roomPartList;
	
	//model lists corridors
	public List<GameObject> CorridorFloorModels;
	public List<GameObject> CorridorCeilingModels;
	public List<GameObject> CorridorWallModels;
		
	//model lists rooms
	public List<GameObject> RoomFloorModels;
	public List<GameObject> RoomCeilingModels;
	public List<GameObject> RoomWallModels;
	public List<GameObject> RoomCornerWallModels;
	public List<GameObject> RoomDoorModels;
	
	//object modellists
	public List<GameObject> corridorPrimaryWallObjects;
	public List<GameObject> corridorPrimaryCeilingObjects;
	public List<GameObject> corridorPrimaryFloorObjects;
	
	public List<GameObject> corridorSecundaryWallObjects;
	public List<GameObject> corridorSecundaryCeilingObjects;
	public List<GameObject> corridorSecundaryFloorObjects;
	
	public List<GameObject> roomPrimaryWallObjects;
	public List<GameObject> roomPrimaryCeilingObjects;
	public List<GameObject> roomPrimaryFloorObjects;
	
	public List<GameObject> roomSecundaryWallObjects;
	public List<GameObject> roomSecundaryCeilingObjects;
	public List<GameObject> roomSecundaryFloorObjects;
	
	public int objectsBuildCounter;
	
	//finishing modelstyle
	public List<GameObject> finishingPortalModels;
	public List<GameObject> finishingWindowModels;
	public List<GameObject> finishingEndWallModels;
	public List<GameObject> finishingRailingModels;
	public List<GameObject> finishingEndRailingModels;
	public List<GameObject> finishingStairCaseModels;
	
	//storing the different Types of the corridors, rooms and finishing styles
	public List<string> corridorTypes;
	public List<string> corridorPrimarySpacingTypes;
	public List<string> roomTypes;
	public List<string> finishingModelTypes;
	
	//storing the user chosen Types of the corridors, rooms and finishing styles	
	public List<string> userCorridorTypes;
	public List<string> userRoomTypes;
	public List<string> userFinishingModelTypes;
	
	//current types
	private string currentCorridorType;
	private string currentPrimarySpacingType;
	private string currentRoomType;
	private string currentFinishingType;
	
	//temp model used for collecting the correct models for the correct parts
	private GameObject tempPartRequestingModel;
	
	//list counters for making sure all parts have generated all models
	public int corridorPartListCounter;
	public int splitterListCounter;
	public int roomBuilderListCounter;
	public int roomPartListCounter;
	public int stairCasePartListCounter;
		
	//buildingsteps
	private string buildingChoise;
	
	//counters for buildtries to prevent the editor to get stuck
	private int buildTry =0;
	
	//direction resources
	private Vector3 directionVector;
	private Vector3 heightVector = new Vector3(0,4.5f,0);
	private string stairCaseUpDownDirection;
	private int direction =0;
	
	//staircases spirals
	private int OldStaircaseDirection;
	public bool SpiralLeft = false;
	public bool SpiralRight = false;
	
	//deadspots or higher dungeons
	public bool deadSpots = false;
	
	//shuffle list for better object placement
	private List<GameObject> TempColliderList;
	public int tempCounter =0;
	public int tempNumber =0;
	
		
	//======================= start of functions =============//
	
	
	public void BuildStage1(){
		level = GameObject.FindWithTag("DGRandomLevel");
				
		//cleaning up any existing levels
		if(level.transform.childCount > 0){
			CleanUpLevel();
		}
		//collecting all needed objects/types etc
		CollectResources();
		
		//buildpointer
		buildPointer = Instantiate(buildPointObject, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		buildPointer.gameObject.isStatic = true;
		buildPointer.transform.parent = level.transform;
		buildPointer.transform.name = buildPointObject.transform.name;
		
		currentCorridorCounter = 0;
		splitterListCounter = 0;
		roomBuilderListCounter = 0;
	}
	public void BuildStage2(){
		//building main corridors and staircases
		if (currentCorridorCounter == 0){
			buildCorridor(buildPointer, false);
		} else {
			if (buildTry < 20){
				var buildingChoise = BuildingChoise();
				if (buildingChoise == "corridor"){
					
					buildTry += 1;
					buildCorridor(buildPointer, false);
			
					}else{
					
					buildTry += 1;
					buildStairCase();
					if (SpiralLeft == false && SpiralRight == false){
						buildCorridor(buildPointer, false);
					} else {
						buildCorridor(buildPointer, true);
					}
				}
			}
		}
		
	}
	public void BuildStage3(){
		//stage 3
		
		//now we built all side corridors using their starting positions generated during the corridor build fase
		currentSideCorridorCounter = 0;
		while(currentSideCorridorCounter < maxAmountOfSideCorridor){
			buildCorridor(splitterBuildPointsList[splitterListCounter], false);
		}
		DestroyImmediate(splitterBuildPointsList[splitterListCounter]);
	}
	public void BuildStage4(){
		//stage 4
				
		//now we expand all rooms using their starting positions generated during the corridor build fase
		roomBuildPointsList[roomBuilderListCounter].SendMessage("Construct");
			
	}
	public void BuildStage5A(){
		//stage 5
		corridorPartListCounter = 0;
		roomPartListCounter = 0;
		stairCasePartListCounter = 0;
		
	}
	public void BuildStage5B(){
		//now we do a typecasting to make sure all connecting corridors and rooms have the same type to make sure a conformity in texture and object styles
		corridorPartList[corridorPartListCounter].SendMessage("TypeCasting");
	}
	public void BuildStage5C(){	
		roomPartList[roomPartListCounter].SendMessage("TypeCasting");
	}
	public void BuildStage5D(){
		stairCasePartList[stairCasePartListCounter].SendMessage("TypeCasting");
	}
	public void BuildStage6Reset(){
		corridorPartListCounter = 0;
		roomPartListCounter = 0;
		stairCasePartListCounter = 0;
	}
	
	//building walls, ceilings and floors
	
	public void BuildStage6A(){
		corridorPartList[corridorPartListCounter].SendMessage("Construct");
	}
	public void BuildStage6B(){	
		roomPartList[roomPartListCounter].SendMessage("Construct");
	}
	public void BuildStage6C(){
		stairCasePartList[stairCasePartListCounter].SendMessage("Construct");
	}
	
	//building detailing (reset first)
	
	public void BuildStage6D(){
		corridorPartList[corridorPartListCounter].SendMessage("ConstructFinishing");
	}
	public void BuildStage6E(){
		roomPartList[roomPartListCounter].SendMessage("ConstructFinishing");
	}
	public void BuildStage6F(){
		stairCasePartList[stairCasePartListCounter].SendMessage("ConstructFinishing");
	}
	
	public void BuildStage6G(){
		//now we determine the endPoint
		var basicsInRange = Physics.OverlapSphere(buildPointer.transform.position, 1);
		if (basicsInRange.Length > 0){
			var countAll = 0;
			while (countAll < basicsInRange.Length){
				if (basicsInRange[countAll] != null){
					if(basicsInRange[countAll].transform.name == "RoomPart" || basicsInRange[countAll].transform.name == "CorridorPart" || basicsInRange[countAll].transform.name == "StairCasePart" ){
						var basicsInRangeScript = basicsInRange[countAll].GetComponent<ColliderScript>();
						
						basicsInRangeScript.endPointer = true;
						basicsInRangeScript.floorModelSpot =1;
						basicsInRangeScript.leftWallModelSpot =1;
						basicsInRangeScript.rightWallModelSpot =1;
						basicsInRangeScript.frontWallModelSpot =1;
						basicsInRangeScript.backWallModelSpot =1;
					}
				}
				countAll +=1;
			}
		}
	}
	public void BuildStage7A(){
		//stage 7
		
		level.BroadcastMessage("DestroyAllColorSpheres");
				
		corridorPartListCounter = 0;
		roomPartListCounter = 0;
		TempColliderList = new List<GameObject>();
		
	}
	public void BuildStage7B(){	
		//rounding off any room corners
		level.BroadcastMessage("StartBuildCornerWalls");
		
	}
	public void BuildStage7C(){	
		//primary objects corridor
		corridorPartList[corridorPartListCounter].SendMessage("GeneratePrimaryObject");
	}
	public void BuildStage7D(){
		//primary objects rooms shuffle list
		while (roomPartListCounter < roomPartList.Count){
			TempColliderList.Add (roomPartList[roomPartListCounter]);	
			roomPartListCounter = roomPartListCounter +1;
		}
		tempNumber = TempColliderList.Count;
		tempCounter =0;
	}
	
	public void BuildStage7E(){
		//going through the shuffled list
		if(TempColliderList.Count > 0){
			var random = Random.Range (0,TempColliderList.Count);
			TempColliderList[random].SendMessage("GeneratePrimaryObject");
			TempColliderList.RemoveAt(random);
			tempCounter +=1;
		}
		
	}
	public void BuildStage8Shuffle(){
		//stage 8
		
		//secundary objects, all parts in one big list
		corridorPartListCounter = 0;
		TempColliderList = new List<GameObject>();
		while (corridorPartListCounter < corridorPartList.Count){
			TempColliderList.Add (corridorPartList[corridorPartListCounter]);	
			corridorPartListCounter = corridorPartListCounter +1;
		}
		roomPartListCounter = 0;
		while (roomPartListCounter < roomPartList.Count){
				
			TempColliderList.Add (roomPartList[roomPartListCounter]);	
			roomPartListCounter = roomPartListCounter +1;
		}
		tempNumber = TempColliderList.Count;
		tempCounter = 0;
	}
	public void BuildStage8A(){
		//going through the shuffled list
		if(TempColliderList.Count > 0){
			var random = Random.Range (0,TempColliderList.Count);
			TempColliderList[random].SendMessage("GenerateSecundaryObject");
			TempColliderList.RemoveAt(random);
		}
	}
	public void BuildStage9(){
		//stage 9
		
		//removing scripts from objects and randomizing certain objects (like our dining chairs)
		DestroyImmediate(buildPointer.transform.gameObject);
		
		level.BroadcastMessage("DestroyAllTraces");
		//cleaning up any potential left over destroyed items from the carbage collector
		System.GC.Collect();
		// Instantiate player
		var thePlayer = Instantiate(playerCharacter, playerSpawnLocation, Quaternion.identity) as GameObject;
		thePlayer.transform.name = "A Player";
		thePlayer.transform.parent = level.transform;
		//done
		//print ("all done");
		generating = 0;	
	}
	
	
	public void CollectResources(){
		
		ClearingLists();
		
		//collecting all resources and important building blocks
		level = GameObject.FindWithTag("DGRandomLevel");
		
		//loading all resource folders into a list
		var allPrimaryListCorridor = Resources.LoadAll("CorridorModels/PatternObjects");
		var allSecundaryListCorridor = Resources.LoadAll("CorridorModels/Objects");
		var allPrimaryListRoom = Resources.LoadAll("RoomModels/PrimaryCheckedObjects");
		var allSecundaryListRoom = Resources.LoadAll("RoomModels/Objects");
		//temp counters for ordering lists
		var listAddModifier = 1;
		var currentListAddModifier = 1;
		//filling modellists, checking them against how many times a user wants the model to appear (using = plus a number behind the modelname)
		//Corridor Models
		var loadAllArray = Resources.LoadAll("CorridorModels/Floors");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			//var longname = Model.transform.name;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				CorridorFloorModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("CorridorModels/Ceilings");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			//var longname = Model.transform.name;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				CorridorCeilingModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("CorridorModels/Walls");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			//var longname = Model.transform.name;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				CorridorWallModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		//sorting corridor models into shorter lists corresponding to their location (ceiling, walls or floors)
		corridorPrimaryWallObjects = new List<GameObject>();
		corridorPrimaryFloorObjects = new List<GameObject>();
		corridorPrimaryCeilingObjects = new List<GameObject>();
		
		if (allPrimaryListCorridor.Length > 0){
			foreach(GameObject Model in allPrimaryListCorridor){
				if (Model.GetComponent<ObjectScript>().CorridorWallObject == true){
					corridorPrimaryWallObjects.Add(Model);
				}
				if (Model.GetComponent<ObjectScript>().CorridorFloorObject == true){
					corridorPrimaryFloorObjects.Add(Model);
				}
				if (Model.GetComponent<ObjectScript>().CorridorCeilingObject == true){
					corridorPrimaryCeilingObjects.Add(Model);
				}
			}
		}
		corridorSecundaryWallObjects = new List<GameObject>();
		corridorSecundaryFloorObjects = new List<GameObject>();
		corridorSecundaryCeilingObjects = new List<GameObject>();
				
		if (allSecundaryListCorridor.Length > 0){
			foreach(GameObject Model in allSecundaryListCorridor){
				if (Model.GetComponent<ObjectScript>().CorridorWallObject == true){
					corridorSecundaryWallObjects.Add(Model);
				}
				if (Model.GetComponent<ObjectScript>().CorridorFloorObject == true){
					corridorSecundaryFloorObjects.Add(Model);
				}
				if (Model.GetComponent<ObjectScript>().CorridorCeilingObject == true){
					corridorSecundaryCeilingObjects.Add(Model);
				}
			}
		}
		//room models	
		loadAllArray = Resources.LoadAll("RoomModels/Doors");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				RoomDoorModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("RoomModels/Ceilings");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				RoomCeilingModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("RoomModels/Floors");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				RoomFloorModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("RoomModels/Walls");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				RoomWallModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("RoomModels/Corners");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				RoomCornerWallModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		//sorting corridor models into shorter lists corresponding to their location (ceiling, walls or floors)
		roomPrimaryWallObjects = new List<GameObject>();
		roomPrimaryFloorObjects = new List<GameObject>();
		roomPrimaryCeilingObjects = new List<GameObject>();
		
		if (allPrimaryListRoom.Length > 0){
			foreach(GameObject Model in allPrimaryListRoom){
				if (Model.GetComponent<ObjectScript>().RoomWallObject == true){
					roomPrimaryWallObjects.Add(Model);
				}
				if (Model.GetComponent<ObjectScript>().RoomFloorObject == true){
					roomPrimaryFloorObjects.Add(Model);
				}
				if (Model.GetComponent<ObjectScript>().RoomCeilingObject == true){
					roomPrimaryCeilingObjects.Add(Model);
				}
			}
		}
		roomSecundaryWallObjects = new List<GameObject>();
		roomSecundaryFloorObjects = new List<GameObject>();
		roomSecundaryCeilingObjects = new List<GameObject>();
		if (allSecundaryListRoom.Length > 0){
			foreach(GameObject Model in allSecundaryListRoom){
				if (Model.GetComponent<ObjectScript>().RoomWallObject == true){
					roomSecundaryWallObjects.Add(Model);
				}
				if (Model.GetComponent<ObjectScript>().RoomFloorObject == true){
					roomSecundaryFloorObjects.Add(Model);
				}
				if (Model.GetComponent<ObjectScript>().RoomCeilingObject == true){
					roomSecundaryCeilingObjects.Add(Model);
				}
			}
		}
		//finishing models
		loadAllArray = Resources.LoadAll("DetailingModels/Portals");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				finishingPortalModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("DetailingModels/Windows");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				finishingWindowModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("DetailingModels/Corners");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				finishingEndWallModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("DetailingModels/Railings");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				finishingRailingModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("DetailingModels/EndRailings");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				finishingEndRailingModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		loadAllArray = Resources.LoadAll("DetailingModels/Staircases");
		foreach(GameObject Model in loadAllArray){
			listAddModifier = 1;
			currentListAddModifier = 1;
			var shortname = Model.transform.name.Split("="[0]);
			if (shortname.Length > 1){
				listAddModifier = int.Parse(shortname[1]);
			}
			while (currentListAddModifier <= listAddModifier){
				finishingStairCaseModels.Add(Model);
				currentListAddModifier += 1;
			}
		}
		//corridor primary spacing type
		if (maximumObjects == true){
			corridorPrimarySpacingTypes.Add("maximumObjects");
		}
		if (objectNoObject == true){
			corridorPrimarySpacingTypes.Add("objectNoObject");
		}
		if (objectNoObjectNoObject == true){
			corridorPrimarySpacingTypes.Add("objectNoObjectNoObject");
		}
		if (maximumObjects == false && objectNoObject == false && objectNoObjectNoObject == false){
			corridorPrimarySpacingTypes.Add("maximumObjects");
			corridorPrimarySpacingTypes.Add("objectNoObject");
			corridorPrimarySpacingTypes.Add("objectNoObjectNoObject");
		}
		//selecting corridor types using the corridorFloorModels !
		var tempCorridorList = 	new List<string>();
		foreach (GameObject Model in CorridorFloorModels){
			var shortname = Model.transform.name.Split("_"[0]);
			tempCorridorList.Add (shortname[0]);
			if (corridorTypes.Count ==0){
				corridorTypes.Add (shortname[0]);
			}
		}
		foreach (string TempModelName in tempCorridorList){
			if (corridorTypes.IndexOf(TempModelName) == -1){
				corridorTypes.Add (TempModelName);
			}
		}
		//selecting room types using the roomFloorModels !	
		var tempRoomList = 	new List<string>();
		foreach (GameObject Model in RoomFloorModels){
			var shortname = Model.transform.name.Split("_"[0]);
			tempRoomList.Add (shortname[0]);
			if (roomTypes.Count ==0){
				roomTypes.Add (shortname[0]);
			}
		}
		foreach (string TempModelName in tempRoomList){
			if (roomTypes.IndexOf(TempModelName) == -1){
				roomTypes.Add (TempModelName);
			}
		}	
		//selecting finishing types using the staircase models !
		var tempFinishingList = 	new List<string>();
		foreach (GameObject Model in finishingStairCaseModels){
			var shortname = Model.transform.name.Split("_"[0]);
			tempFinishingList.Add (shortname[0]);
			if (finishingModelTypes.Count ==0){
				finishingModelTypes.Add (shortname[0]);
			}
		}
		foreach (string TempModelName in tempFinishingList){
			if (finishingModelTypes.IndexOf(TempModelName) == -1){
				finishingModelTypes.Add (TempModelName);
			}
		}
	}
	public void ClearingLists(){
		//clearing any existing lists of parts
		corridorPartList = new List<GameObject>();
		stairCasePartList = new List<GameObject>();
		splitterBuildPointsList = new List<GameObject>();
		roomBuildPointsList = new List<GameObject>();
		roomPartList = new List<GameObject>();
		
		
		//clearing modellists
		CorridorFloorModels = new List<GameObject>();
		CorridorCeilingModels = new List<GameObject>();
		CorridorWallModels = new List<GameObject>();
		
		RoomDoorModels = new List<GameObject>();
		RoomFloorModels = new List<GameObject>();
		RoomCeilingModels = new List<GameObject>();
		RoomWallModels = new List<GameObject>();
		RoomCornerWallModels = new List<GameObject>();
	
		finishingPortalModels = new List<GameObject>();
		finishingWindowModels = new List<GameObject>();
		finishingEndWallModels = new List<GameObject>();
		finishingRailingModels = new List<GameObject>();
		finishingEndRailingModels = new List<GameObject>();
		finishingStairCaseModels = new List<GameObject>();
		
		//resetting type lists
		corridorTypes = new List<string>();
		corridorPrimarySpacingTypes = new List<string>();
		roomTypes = new List<string>();
		finishingModelTypes = new List<string>();
		
		//resetting counters
		StairCaseNumber =0;
		corridorPartCount =0;
		objectsBuildCounter = 0;
	}
	
	public void CleanUpLevel (){
		level = GameObject.FindWithTag("DGRandomLevel");
		
		//clearing an existing level
		while(level.transform.childCount > 0){
			foreach(Transform child in level.transform){
				
				DestroyImmediate(child.transform.gameObject);
			}
		}
	}
	
	private string BuildingChoise(){
		//choise between staircase or corridor
		var buildChance = Random.Range (1,101);
		if (buildChance <= staircaseChancePercentage){
			return "stairCase";
		} else {
			return "corridor";
		}
	}
	private void buildCorridor(GameObject CurrentBuildPoint, bool skipDirection){
		//lenght corridor, choosing between main corridor or side corridors
		if (CurrentBuildPoint.transform.name == "BuildPoint"){
			actualCorridorLength = Random.Range( minCorridorLength,maxCorridorLength+1);
		} else {
			actualCorridorLength = Random.Range( minSideCorridorLength,maxSideCorridorLength+1);
		}
		currentCorridorLength = 0;
		corridorBuilt = false;
		//collecting details part
		if (skipDirection == false){
			if (lessRandomCorridors){
				GettingLessRandomDirection();
			} else {
				GettingDirection();
			}
		}
		GettingCorridorPartType ();
		GettingCorridorPrimarySpacingType ();
		GettingRoomType ();
		GettingFinishingType ();
		while (currentCorridorLength < actualCorridorLength){
			CurrentBuildPoint.transform.position = CurrentBuildPoint.transform.position + directionVector;
			var basicsInRange = Physics.OverlapSphere(CurrentBuildPoint.transform.position, 1);
						
			if(basicsInRange.Length == 0){
				corridorBuilt = true;
				corridorPartCount = corridorPartCount +1;
				var theCollider = Instantiate(corridorPart, CurrentBuildPoint.transform.position,Quaternion.identity) as GameObject;
				theCollider.transform.parent = level.transform;
				theCollider.transform.name =  corridorPart.transform.name;
				var theColliderScript = theCollider.GetComponent<ColliderScript>();
				theColliderScript.partType = currentCorridorType;
				theColliderScript.primaryCorridorSpacingType = currentPrimarySpacingType;
				theColliderScript.finishingType = currentFinishingType;
				theColliderScript.direction = direction;
				theColliderScript.corridorPartCount = corridorPartCount;
				theColliderScript.buildFloor = true;
				
				if (currentCorridorLength == 0 && currentCorridorCounter == 0){
					theColliderScript.beginPointer = true;
					theColliderScript.floorModelSpot =1;
					theColliderScript.leftWallModelSpot =1;
					theColliderScript.rightWallModelSpot =1;
					theColliderScript.frontWallModelSpot =1;
					theColliderScript.backWallModelSpot =1;
				}
								
				currentCorridorLength = currentCorridorLength +1;
				corridorPartList.Add(theCollider);
				
				if (CurrentBuildPoint.transform.name == "BuildPoint"){
					BuildSplitter();	
				}
				BuildRoomBuilder(CurrentBuildPoint);
			} else {
				if (basicsInRange[0].transform.name == "StairCasePart"){
					CurrentBuildPoint.transform.position = CurrentBuildPoint.transform.position - directionVector;
						if (corridorBuilt == true){
							if (CurrentBuildPoint.transform.name == "BuildPoint"){
								currentCorridorCounter += 1;
								buildTry = 0;
							} else {
								currentSideCorridorCounter += 1;
							}
						} 
					return;
					
				} else {
					currentCorridorLength = currentCorridorLength +1;
				}
			}
		}
		if (corridorBuilt == true){
			if (CurrentBuildPoint.transform.name == "BuildPoint"){
				currentCorridorCounter += 1;
				buildTry = 0;
			} else {
				currentSideCorridorCounter += 1;
			}
		}
	}
	private void buildStairCase(){
		var buildStairCaseTry = 0;
		while (buildStairCaseTry < 10){
			GettingStairCaseDirection();
			var spaceForStairCase = StairCaseRoom();
		
			if (spaceForStairCase == true){
				OldStaircaseDirection = direction;
				InstantiateStairCase();
				currentCorridorCounter +=1;
				buildTry = 0;
				buildStairCaseTry = 100;
			} else {
				buildStairCaseTry +=1;
				if (SpiralLeft){
					OldStaircaseDirection += 1;
					if(OldStaircaseDirection > 4){
						OldStaircaseDirection = 1;
					}
				}
				if (SpiralRight){
					OldStaircaseDirection -= 1;
					if(OldStaircaseDirection < 1){
						OldStaircaseDirection = 4;
					}
				}
			}
		}
	}
	bool StairCaseRoom(){
		var tempBool = true;
		var tempPosition = new Vector3 (0,0,0);
		if (stairCaseDirection == "Down"){
			//step 1
			tempPosition = buildPointer.transform.position + directionVector;
			var basicsInRange1 = Physics.OverlapSphere(tempPosition, 1);
				if(basicsInRange1.Length != 0){
						tempBool = false;
						return tempBool;
				}
			//step 2
			tempPosition = buildPointer.transform.position + directionVector + directionVector;
			var basicsInRange2 = Physics.OverlapSphere(tempPosition, 1);
				if(basicsInRange2.Length != 0){
						tempBool = false;
						return tempBool;
				}
			//step 3
			tempPosition = buildPointer.transform.position + directionVector + directionVector + directionVector;
			var basicsInRange3 = Physics.OverlapSphere(tempPosition, 1);
				if(basicsInRange3.Length != 0){
						tempBool = false;
						return tempBool;
				}
			//step 4
			tempPosition = buildPointer.transform.position + directionVector + directionVector - heightVector;
			var basicsInRange4 = Physics.OverlapSphere(tempPosition, 1);
				if(basicsInRange4.Length != 0){
						tempBool = false;
						return tempBool;
				}
			//step 5
			tempPosition = buildPointer.transform.position + directionVector + directionVector + directionVector - heightVector;
			var basicsInRange5 = Physics.OverlapSphere(tempPosition, 1);
				if(basicsInRange5.Length != 0){
						tempBool = false;
						return tempBool;
				}
		}
		if (stairCaseDirection == "Up"){
			//step 1
			tempPosition = buildPointer.transform.position + directionVector;
			var basicsInRangeA = Physics.OverlapSphere(tempPosition, 1);
				if(basicsInRangeA.Length != 0){
						tempBool = false;
						return tempBool;
				}
			//step 2
			tempPosition = buildPointer.transform.position + directionVector + directionVector;
			var basicsInRangeB = Physics.OverlapSphere(tempPosition, 1);
				if(basicsInRangeB.Length != 0){
						tempBool = false;
						return tempBool;
				}
			//step 3
			tempPosition = buildPointer.transform.position + directionVector + heightVector;
			var basicsInRangeC = Physics.OverlapSphere(tempPosition, 1);
				if(basicsInRangeC.Length != 0){
						tempBool = false;
						return tempBool;
				}
			//step 4
			tempPosition = buildPointer.transform.position + directionVector + directionVector + heightVector;
			var basicsInRangeD = Physics.OverlapSphere(tempPosition, 1);
				if(basicsInRangeD.Length != 0){
						tempBool = false;
						return tempBool;
				}
			//step 5
			tempPosition = buildPointer.transform.position + directionVector + directionVector + directionVector + heightVector;
			var basicsInRangeE = Physics.OverlapSphere(tempPosition, 1);
				if(basicsInRangeE.Length != 0){
						tempBool = false;
						return tempBool;
				}
		}
		return tempBool;	
	}
	void InstantiateStairCase(){
		GettingRoomType ();
		GettingFinishingType ();
		var tempPosition = new Vector3 (0,0,0);
		if (stairCaseDirection == "Down"){
			//step 1
			tempPosition = buildPointer.transform.position + directionVector;
			var theCollider1 = Instantiate(stairCasePart, tempPosition,Quaternion.identity) as GameObject;
			theCollider1.transform.parent = level.transform;
			theCollider1.transform.name = stairCasePart.name;
			//type kamertextures
			var theColliderScript1 = theCollider1.GetComponent<ColliderScript>();
			theColliderScript1.direction = direction;
			theColliderScript1.partType = currentRoomType;	
			theColliderScript1.buildFloor = true;
			theColliderScript1.finishingType = currentFinishingType;
			theColliderScript1.StairCaseNumber = StairCaseNumber;
			stairCasePartList.Add(theCollider1);
			//step 2
			tempPosition = buildPointer.transform.position + directionVector + directionVector;
			var theCollider2 = Instantiate(stairCasePart, tempPosition,Quaternion.identity) as GameObject;
			theCollider2.transform.parent = level.transform;
			theCollider2.transform.name = stairCasePart.name;
			//type kamertextures
			var theColliderScript2 = theCollider2.GetComponent<ColliderScript>();
			theColliderScript2.direction = direction;	
			theColliderScript2.partType = currentRoomType;
			theColliderScript2.finishingType = currentFinishingType;
			theColliderScript2.StairCaseNumber = StairCaseNumber;
			stairCasePartList.Add(theCollider2);
			//step 3
			tempPosition = buildPointer.transform.position + directionVector + directionVector + directionVector;
			var theCollider3 = Instantiate(stairCasePart, tempPosition,Quaternion.identity) as GameObject;
			theCollider3.transform.parent = level.transform;
			theCollider3.transform.name = stairCasePart.name;
			//type kamertextures
			var theColliderScript3 = theCollider3.GetComponent<ColliderScript>();
			theColliderScript3.direction = direction;
			theColliderScript3.partType = currentRoomType;
			theColliderScript3.finishingType = currentFinishingType;
			theColliderScript3.StairCaseNumber = StairCaseNumber;
			stairCasePartList.Add(theCollider3);
			//step 4
			tempPosition = buildPointer.transform.position + directionVector + directionVector - heightVector;
			var theCollider4 = Instantiate(stairCasePart, tempPosition,Quaternion.identity) as GameObject;
			theCollider4.transform.parent = level.transform;
			theCollider4.transform.name = stairCasePart.name;
			//type kamertextures
			var theColliderScript4 = theCollider4.GetComponent<ColliderScript>();
			theColliderScript4.direction = direction;
			theColliderScript4.directionUpDown = stairCaseDirection;
			theColliderScript4.partType = currentRoomType;	
			theColliderScript4.finishingType = currentFinishingType;
			theColliderScript4.buildStairs = true;
			theColliderScript4.StairCaseNumber = StairCaseNumber;
			stairCasePartList.Add(theCollider4);
			//step 5
			tempPosition = buildPointer.transform.position + directionVector + directionVector + directionVector - heightVector;
			var theCollider5 = Instantiate(stairCasePart, tempPosition,Quaternion.identity) as GameObject;
			theCollider5.transform.parent = level.transform;
			theCollider5.transform.name = stairCasePart.name;
			//type kamertextures
			var theColliderScript5 = theCollider5.GetComponent<ColliderScript>();
			theColliderScript5.direction = direction;
			theColliderScript5.partType = currentRoomType;
			theColliderScript5.finishingType = currentFinishingType;
			theColliderScript5.buildFloor = true;
			theColliderScript5.StairCaseNumber = StairCaseNumber;
			stairCasePartList.Add(theCollider5);
			//placing builderpoint
			buildPointer.transform.position = tempPosition;
		}
		if (stairCaseDirection == "Up"){
			//step 1
			tempPosition = buildPointer.transform.position + directionVector;
			var theCollider1 = Instantiate(stairCasePart, tempPosition,Quaternion.identity) as GameObject;
			theCollider1.transform.parent = level.transform;
			theCollider1.transform.name = stairCasePart.name;
			//type kamertextures
			var theColliderScript1 = theCollider1.GetComponent<ColliderScript>();
			theColliderScript1.direction = direction;
			theColliderScript1.partType = currentRoomType;	
			theColliderScript1.finishingType = currentFinishingType;
			theColliderScript1.buildFloor = true;
			theColliderScript1.StairCaseNumber = StairCaseNumber;
			stairCasePartList.Add(theCollider1);
			//step 2
			tempPosition = buildPointer.transform.position + directionVector + directionVector;
			var theCollider2 = Instantiate(stairCasePart, tempPosition,Quaternion.identity) as GameObject;
			theCollider2.transform.parent = level.transform;
			theCollider2.transform.name = stairCasePart.name;
			//type kamertextures
			var theColliderScript2 = theCollider2.GetComponent<ColliderScript>();
			theColliderScript2.direction = direction;
			theColliderScript2.directionUpDown = stairCaseDirection;
			theColliderScript2.partType = currentRoomType;	
			theColliderScript2.finishingType = currentFinishingType;
			//theColliderScript2.railingType = currentRailingType;
			theColliderScript2.buildStairs = true;
			theColliderScript2.StairCaseNumber = StairCaseNumber;
			stairCasePartList.Add(theCollider2);
			//step 3
			tempPosition = buildPointer.transform.position + directionVector + heightVector;
			var theCollider3 = Instantiate(stairCasePart, tempPosition,Quaternion.identity) as GameObject;
			theCollider3.transform.parent = level.transform;
			theCollider3.transform.name = stairCasePart.name;
			//type kamertextures
			var theColliderScript3 = theCollider3.GetComponent<ColliderScript>();
			theColliderScript3.direction = direction;
			theColliderScript3.partType = currentRoomType;
			theColliderScript3.finishingType = currentFinishingType;
			theColliderScript3.StairCaseNumber = StairCaseNumber;
			stairCasePartList.Add(theCollider3);
			//step 4
			tempPosition = buildPointer.transform.position + directionVector + directionVector + heightVector;
			var theCollider4 = Instantiate(stairCasePart, tempPosition,Quaternion.identity) as GameObject;
			theCollider4.transform.parent = level.transform;
			theCollider4.transform.name = stairCasePart.name;
			//type kamertextures
			var theColliderScript4 = theCollider4.GetComponent<ColliderScript>();
			theColliderScript4.direction = direction;
			theColliderScript4.partType = currentRoomType;
			theColliderScript4.finishingType = currentFinishingType;
			theColliderScript4.StairCaseNumber = StairCaseNumber;
			stairCasePartList.Add(theCollider4);
			//step 5
			tempPosition = buildPointer.transform.position + directionVector + directionVector + directionVector + heightVector;
			var theCollider5 = Instantiate(stairCasePart, tempPosition,Quaternion.identity) as GameObject;
			theCollider5.transform.parent = level.transform;
			theCollider5.transform.name = stairCasePart.name;
			//type kamertextures
			var theColliderScript5 = theCollider5.GetComponent<ColliderScript>();
			theColliderScript5.direction = direction;
			theColliderScript5.partType = currentRoomType;	
			theColliderScript5.finishingType = currentFinishingType;
			theColliderScript5.buildFloor = true;
			theColliderScript5.StairCaseNumber = StairCaseNumber;
			stairCasePartList.Add(theCollider5);
			//placing builderpoint
			buildPointer.transform.position = tempPosition;
		}
		StairCaseNumber +=1;
	}
	//==============| generic functions |===============
	void GettingDirection (){
		direction = Random.Range(1,5);
		if(direction == 1){
			directionVector = new Vector3(0,0,5);
		}else if(direction == 2){
			directionVector = new Vector3(5,0,0);
		}else if(direction == 3){
			directionVector = new Vector3(0,0,-5);
		}else if(direction == 4){
			directionVector = new Vector3(-5,0,0);
		}		
	}
	
	void GettingLessRandomDirection (){
		
		if (OldCorridorDirection == 1){
			direction = Random.Range(1,4);
			if(direction == 1){
				directionVector = new Vector3(0,0,5);
			}else if(direction == 2){
				directionVector = new Vector3(5,0,0);
			}else if(direction == 3){
				directionVector = new Vector3(-5,0,0);
			}
		} else if (OldCorridorDirection == 2){
			direction = Random.Range(1,4);
			if(direction == 1){
				directionVector = new Vector3(0,0,5);
			}else if(direction == 2){
				directionVector = new Vector3(5,0,0);
			}else if(direction == 3){
				directionVector = new Vector3(5,0,0);
			}
		} else if (OldCorridorDirection == 3){
			direction = Random.Range(1,4);
			if(direction == 1){
				directionVector = new Vector3(0,0,-5);
			}else if(direction == 2){
				directionVector = new Vector3(5,0,0);
			}else if(direction == 3){
				directionVector = new Vector3(-5,0,0);
			}
		} else if (OldCorridorDirection == 4){
			direction = Random.Range(1,4);
			if(direction == 1){
				directionVector = new Vector3(0,0,5);
			}else if(direction == 2){
				directionVector = new Vector3(0,0,-5);
			}else if(direction == 3){
				directionVector = new Vector3(-5,0,0);
			}
		}
	
		OldCorridorDirection = direction;
	}
	
	
	void GettingStairCaseDirection(){
		
		if(!SpiralLeft && !SpiralRight){
				direction = Random.Range(1,5);
			if(direction == 1){
				directionVector = new Vector3(0,0,5);
			}else if(direction == 2){
				directionVector = new Vector3(5,0,0);
			}else if(direction == 3){
				directionVector = new Vector3(0,0,-5);
			}else if(direction == 4){
				directionVector = new Vector3(-5,0,0);
			}
		}else if(SpiralLeft){
			
			if(OldStaircaseDirection == 1){
				direction = 4;
				directionVector = new Vector3(-5,0,0);
			}else if(OldStaircaseDirection == 2){
				direction = 1;
				directionVector = new Vector3(0,0,5);
			}else if(OldStaircaseDirection == 3){
				direction = 2;
				directionVector = new Vector3(5,0,0);
			}else if(OldStaircaseDirection == 4){
				direction = 3;
				directionVector = new Vector3(0,0,-5);
			}
		}else if(SpiralRight){
			if(OldStaircaseDirection == 1){
				direction = 2;
				directionVector = new Vector3(5,0,0);
			}else if(OldStaircaseDirection == 2){
				direction = 3;
				directionVector = new Vector3(0,0,-5);
			}else if(OldStaircaseDirection == 3){
				direction = 4;
				directionVector = new Vector3(-5,0,0);
			}else if(OldStaircaseDirection == 4){
				direction = 1;
				directionVector = new Vector3(0,0,5);
			}
		}
		
		var randomNumber = Random.Range(0,2);
		if (stairCaseDirectionSetup == "Up"){
			randomNumber = 1;
		}
		if (stairCaseDirectionSetup == "Down"){
			randomNumber = 0;
		}
		if (randomNumber == 0){
			stairCaseDirection = "Down";
		}
		if (randomNumber == 1){
			stairCaseDirection = "Up";
		}
	}
	void BuildRoomBuilder (GameObject CurrentBuildPoint){
		var buildChance = Random.Range (1,101);
		if (buildChance <= roomChancePercentage){
			var theRoomBuilder = Instantiate(roomBuildPoint, CurrentBuildPoint.transform.position,Quaternion.identity) as GameObject;
			theRoomBuilder.transform.parent = level.transform;
			theRoomBuilder.name =  "RoomBuilder";
			var theRoomBuilderScript = theRoomBuilder.GetComponent<ConstructRoomScript>();
			theRoomBuilderScript.roomPart = roomPart;
			theRoomBuilderScript.roomLenghtNow = Random.Range (minRoomLength,maxRoomLength+1);
			theRoomBuilderScript.roomWidthNow = Random.Range (minRoomWidth,maxRoomWidth+1);
			theRoomBuilderScript.roomHeightNow = Random.Range (minRoomHeigth,maxRoomHeigth+1);		
			theRoomBuilderScript.roomType = currentRoomType;
			theRoomBuilderScript.finishingType = currentFinishingType;
			roomBuildPointsList.Add(theRoomBuilder);
		}
	}
	void BuildSplitter (){
		var buildChance = Random.Range (1,101);
		if (buildChance <= sideCorridorChancePercentage){
			var theSplitter = Instantiate(splitterBuildPoint, buildPointer.transform.position,Quaternion.identity) as GameObject;
			theSplitter.transform.parent = level.transform;
			theSplitter.name =  "Splitter";
			splitterBuildPointsList.Add(theSplitter);
		}
	}
	public void GettingDoorModel (GameObject requestingCollider){
		tempPartRequestingModel = requestingCollider;
		var attachedColliderScript = tempPartRequestingModel.GetComponent<ColliderScript>();
		var tempDoorModel = RandomDoorModel(RoomDoorModels);
		attachedColliderScript.doorModel = tempDoorModel;
	}
	public void GettingModels (GameObject requestingCollider){
		tempPartRequestingModel = requestingCollider;
		var attachedColliderScript = tempPartRequestingModel.GetComponent<ColliderScript>();
				
		//finishingmodels (all parts need these)
		var tempRailingModel = RandomFinishingModel(finishingRailingModels);
		attachedColliderScript.RailingModel = tempRailingModel;
		var tempRailingEndModel = RandomFinishingModel(finishingEndRailingModels);
		attachedColliderScript.EndRailingModel = tempRailingEndModel;
		var tempPortalModel = RandomFinishingModel(finishingPortalModels);
		attachedColliderScript.PortalModel = tempPortalModel;
		var tempWindowModel = RandomFinishingModel(finishingWindowModels);
		attachedColliderScript.WindowModel = tempWindowModel;
		var tempEndWallModel = RandomFinishingModel(finishingEndWallModels);
		attachedColliderScript.endWallModel = tempEndWallModel;
		//corridor part models
		if (requestingCollider.transform.name == "CorridorPart"){
			//floormodel
			var tempFloorModel = RandomModel(CorridorFloorModels);
			attachedColliderScript.CorridorFloorModel = tempFloorModel;
			//ceilingmodel
			var tempCeilingModel = RandomModel(CorridorCeilingModels);
			attachedColliderScript.CorridorCeilingModel = tempCeilingModel;
			//wallmodel
			var tempWallModel = RandomModel(CorridorWallModels);
			attachedColliderScript.CorridorWallModelLeft = tempWallModel;
			tempWallModel = RandomModel(CorridorWallModels);
			attachedColliderScript.CorridorWallModelRight = tempWallModel;
			tempWallModel = RandomModel(CorridorWallModels);
			attachedColliderScript.CorridorWallModelFront = tempWallModel;
			tempWallModel = RandomModel(CorridorWallModels);
			attachedColliderScript.CorridorWallModelBack = tempWallModel;
			//transfer all primary corridor objects to collider
			if (corridorPrimaryWallObjects.Count > 0){
				foreach (GameObject model in corridorPrimaryWallObjects){
					attachedColliderScript.corridorPrimaryLeftWallObjects.Add (model);
					attachedColliderScript.corridorPrimaryRightWallObjects.Add (model);
					attachedColliderScript.corridorPrimaryFrontWallObjects.Add (model);
					attachedColliderScript.corridorPrimaryBackWallObjects.Add (model);
				}
			}
			if (corridorPrimaryCeilingObjects.Count > 0){
				foreach (GameObject model in corridorPrimaryCeilingObjects){
					attachedColliderScript.corridorPrimaryCeilingObjects.Add (model);
				}
			}
			if (corridorPrimaryFloorObjects.Count > 0){
				foreach (GameObject model in corridorPrimaryFloorObjects){
					attachedColliderScript.corridorPrimaryFloorObjects.Add (model);
				}
			}
			
			
		/*	var tempCorridorWallLightModel = RandomModel(corridorPrimaryWallObjects);
			attachedColliderScript.corridorPrimaryWallObject = tempCorridorWallLightModel;
			var tempCorridorCeilingLightModel = RandomModel(corridorPrimaryCeilingObjects);
			attachedColliderScript.corridorPrimaryCeilingObject = tempCorridorCeilingLightModel;
			var tempCorridorFloorLightModel = RandomModel(corridorPrimaryFloorObjects);
			attachedColliderScript.corridorPrimaryFloorObject = tempCorridorFloorLightModel;*/
			
			//transfer all secundary corridor objects to collider
			if (corridorSecundaryWallObjects.Count > 0){
				foreach (GameObject model in corridorSecundaryWallObjects){
					attachedColliderScript.corridorSecundaryLeftWallObjects.Add (model);
					attachedColliderScript.corridorSecundaryRightWallObjects.Add (model);
					attachedColliderScript.corridorSecundaryFrontWallObjects.Add (model);
					attachedColliderScript.corridorSecundaryBackWallObjects.Add (model);
				}
			}
			if (corridorSecundaryCeilingObjects.Count > 0){
				foreach (GameObject model in corridorSecundaryCeilingObjects){
					attachedColliderScript.corridorSecundaryCeilingObjects.Add (model);
				}
			}
			if (corridorSecundaryFloorObjects.Count > 0){
				foreach (GameObject model in corridorSecundaryFloorObjects){
					attachedColliderScript.corridorSecundaryFloorObjects.Add (model);
				}
			}
			
			
			/*
			var tempCorridorWallObjectModel = RandomModel(corridorSecundaryWallObjects);
			attachedColliderScript.corridorWallObject = tempCorridorWallObjectModel;
			var tempCorridorFloorObjectModel = RandomModel(corridorSecundaryFloorObjects);
			attachedColliderScript.corridorFloorObject = tempCorridorFloorObjectModel;
			var tempCorridorCeilingObjectModel = RandomModel(corridorSecundaryCeilingObjects);
			attachedColliderScript.corridorCeilingObject = tempCorridorCeilingObjectModel;*/
		}
		//room and staircase part models
		if (requestingCollider.transform.name == "RoomPart" || requestingCollider.transform.name == "StairCasePart"){
			//floormodel
			var tempFloorModel = RandomModel(RoomFloorModels);
			attachedColliderScript.RoomFloorModel = tempFloorModel;
			//ceilingmodel
			var tempCeilingModel = RandomModel(RoomCeilingModels);
			attachedColliderScript.RoomCeilingModel = tempCeilingModel;
			//wallmodel
			var tempWallModel = RandomModel(RoomWallModels);
			attachedColliderScript.RoomWallModelLeft = tempWallModel;
			tempWallModel = RandomModel(RoomWallModels);
			attachedColliderScript.RoomWallModelRight = tempWallModel;
			tempWallModel = RandomModel(RoomWallModels);
			attachedColliderScript.RoomWallModelFront = tempWallModel;
			tempWallModel = RandomModel(RoomWallModels);
			attachedColliderScript.RoomWallModelBack = tempWallModel;
			//cornermodel
			var tempCornerWallModel = RandomModel(RoomCornerWallModels);
			if (tempCornerWallModel != null){
				attachedColliderScript.RoomCornerWallModel = tempCornerWallModel;
			}
			if (requestingCollider.transform.name == "RoomPart"){
				//transfer all primary corridor objects to collider
				if (roomPrimaryWallObjects.Count > 0){
					foreach (GameObject model in roomPrimaryWallObjects){
						attachedColliderScript.roomPrimaryLeftWallObjects.Add (model);
						attachedColliderScript.roomPrimaryRightWallObjects.Add (model);
						attachedColliderScript.roomPrimaryFrontWallObjects.Add (model);
						attachedColliderScript.roomPrimaryBackWallObjects.Add (model);
					}
				}
				if (roomPrimaryCeilingObjects.Count > 0){
					foreach (GameObject model in roomPrimaryCeilingObjects){
						attachedColliderScript.roomPrimaryCeilingObjects.Add (model);
					}
				}
				if (roomPrimaryFloorObjects.Count > 0){
					foreach (GameObject model in roomPrimaryFloorObjects){
						attachedColliderScript.roomPrimaryFloorObjects.Add (model);
					}
				}
				
				/*
				var tempRoomWallLightModel = RandomModel(roomPrimaryWallObjects);
				attachedColliderScript.roomPrimaryWallObject = tempRoomWallLightModel;
				var tempRoomCeilingLightModel = RandomModel(roomPrimaryCeilingObjects);
				attachedColliderScript.roomPrimaryCeilingObject = tempRoomCeilingLightModel;
				var tempRoomFloorLightModel = RandomModel(roomPrimaryFloorObjects);
				attachedColliderScript.roomPrimaryFloorObject = tempRoomFloorLightModel;*/
				
				//transfer all secundary corridor objects to collider
				if (roomSecundaryWallObjects.Count > 0){
					foreach (GameObject model in roomSecundaryWallObjects){
						attachedColliderScript.roomSecundaryLeftWallObjects.Add (model);
						attachedColliderScript.roomSecundaryRightWallObjects.Add (model);
						attachedColliderScript.roomSecundaryFrontWallObjects.Add (model);
						attachedColliderScript.roomSecundaryBackWallObjects.Add (model);
					}
				}
				if (roomSecundaryCeilingObjects.Count > 0){
					foreach (GameObject model in roomSecundaryCeilingObjects){
						attachedColliderScript.roomSecundaryCeilingObjects.Add (model);
					}
				}
				if (roomSecundaryFloorObjects.Count > 0){
					foreach (GameObject model in roomSecundaryFloorObjects){
						attachedColliderScript.roomSecundaryFloorObjects.Add (model);
					}
				}
				
				
				/*
				var tempRoomWallObjectModel = RandomModel(roomSecundaryWallObjects);
				attachedColliderScript.roomWallObject = tempRoomWallObjectModel;
				var tempRoomFloorObjectModel = RandomModel(roomSecundaryFloorObjects);
				attachedColliderScript.roomFloorObject = tempRoomFloorObjectModel;
				var tempRoomCeilingObjectModel = RandomModel(roomSecundaryCeilingObjects);
				attachedColliderScript.roomCeilingObject = tempRoomCeilingObjectModel;*/
			}
			if (requestingCollider.transform.name == "StairCasePart" && attachedColliderScript.buildStairs == true){
				//staircase model
				var tempStairCaseModel = RandomFinishingModel(finishingStairCaseModels);
				attachedColliderScript.StairCaseModel = tempStairCaseModel;
			}
		}
	}
	
	GameObject RandomDoorModel(List<GameObject> tempList){
		var attachedColliderScript = tempPartRequestingModel.GetComponent<ColliderScript>();
		var tempTypeList =  new List<GameObject>();
		if (tempList.Count > 0){
			foreach (GameObject model in tempList){
				var modelShortName = model.transform.name.Split("_"[0]);
				if (attachedColliderScript.doorType == modelShortName[0] ){
					tempTypeList.Add (model);
				}
			}
		}
		if (tempTypeList.Count > 0){		
			var tempTypeModelNumber = Random.Range(0,tempTypeList.Count);
			if (tempTypeList[tempTypeModelNumber] != null){
				var tempModel = tempTypeList[tempTypeModelNumber];
				return tempModel;
			} else {
				return null;
			}
		} else {
			return null;
		}
	}
	GameObject RandomModel(List<GameObject> tempList){
		var attachedColliderScript = tempPartRequestingModel.GetComponent<ColliderScript>();
		var tempTypeList =  new List<GameObject>();
		if (tempList.Count > 0){
			foreach (GameObject model in tempList){
				var modelShortName = model.transform.name.Split("_"[0]);
				if (attachedColliderScript.partType == modelShortName[0] ){
					tempTypeList.Add (model);
				}
			}
		}
		if (tempTypeList.Count > 0){		
			var tempTypeModelNumber = Random.Range(0,tempTypeList.Count);
			if (tempTypeList[tempTypeModelNumber] != null){
				var tempModel = tempTypeList[tempTypeModelNumber];
				return tempModel;
			} else {
				return null;
			}
		} else {
			return null;
		}
	}
	GameObject RandomFinishingModel(List<GameObject> tempList){
		var attachedColliderScript = tempPartRequestingModel.GetComponent<ColliderScript>();
		var tempTypeList =  new List<GameObject>();
		foreach (GameObject model in tempList){
			var modelShortName = model.transform.name.Split("_"[0]);
			if (attachedColliderScript.finishingType == modelShortName[0] ){
				tempTypeList.Add (model);
			}
		}
		var tempTypeModelNumber = Random.Range(0,tempTypeList.Count);
		var tempModel = tempTypeList[tempTypeModelNumber];
		return tempModel;
	}
	void GettingCorridorPartType (){
		if (pickStyle == false){
			var tempTypeNumber = Random.Range(0,corridorTypes.Count);
			currentCorridorType = corridorTypes[tempTypeNumber];
		}
		if (pickStyle == true){
			var tempTypeNumber = Random.Range(0,userCorridorTypes.Count);
			currentCorridorType = userCorridorTypes[tempTypeNumber];
		}
	}
	void GettingCorridorPrimarySpacingType (){
		var tempTypeNumber = Random.Range(0,corridorPrimarySpacingTypes.Count);
		currentPrimarySpacingType = corridorPrimarySpacingTypes[tempTypeNumber];
	}
	public void GettingRoomType (){
		if (pickStyle == false){
			var tempTypeNumber = Random.Range(0,roomTypes.Count);
			currentRoomType = roomTypes[tempTypeNumber];
		}
		if (pickStyle == true){
			var tempTypeNumber = Random.Range(0,userRoomTypes.Count);
			currentRoomType = userRoomTypes[tempTypeNumber];
		}
	}
	void GettingFinishingType (){
		if (pickStyle == false){
			var tempTypeNumber = Random.Range(0,finishingModelTypes.Count);
			currentFinishingType = finishingModelTypes[tempTypeNumber];
		}
		if (pickStyle == true){
			var tempTypeNumber = Random.Range(0,userFinishingModelTypes.Count);
			currentFinishingType = userFinishingModelTypes[tempTypeNumber];
		}
	}
	
	public void PrintIt (){
	
		Build();
	}
	public void Build(){
		
		//for instant generation without feedback during inspector build and for continuing with a new dungeon during runtime
		
		level = GameObject.FindWithTag("DGRandomLevel");
		//ClearingLists();
		
		//cleaning up any existing levels
		if(level.transform.childCount > 0){
			CleanUpLevel();
		}
		//collecting all needed objects/types etc
		CollectResources();
		
		//buildpointer
		buildPointer = Instantiate(buildPointObject, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		buildPointer.gameObject.isStatic = true;
		buildPointer.transform.parent = level.transform;
		buildPointer.transform.name = buildPointObject.transform.name;
		
		//building main corridors and staircases
		currentCorridorCounter = 0;
		while(currentCorridorCounter < maxAmountOfCorridor){
			if (currentCorridorCounter == 0){
				buildCorridor(buildPointer, false);
			} else {
				if (buildTry < 20){
					var buildingChoise = BuildingChoise();
					if (buildingChoise == "corridor"){
						
						buildTry += 1;
						buildCorridor(buildPointer, false);
				
						}else{
						
						buildTry += 1;
						buildStairCase();
						if (SpiralLeft == false && SpiralRight == false){
							buildCorridor(buildPointer, false);
						} else {
							buildCorridor(buildPointer, true);
						}
					}
				}
			}
		}
		
		//now we built all side corridors using their starting positions generated during the corridor build fase
		splitterListCounter =0;
		while (splitterListCounter < splitterBuildPointsList.Count){
			currentSideCorridorCounter = 0;
			while(currentSideCorridorCounter < maxAmountOfSideCorridor){
				buildCorridor(splitterBuildPointsList[splitterListCounter], false);
			}
			DestroyImmediate(splitterBuildPointsList[splitterListCounter]);
			splitterListCounter = splitterListCounter +1;
		}
		//now we expand all rooms using their starting positions generated during the corridor build fase
		roomBuilderListCounter = 0;
		while (roomBuilderListCounter < roomBuildPointsList.Count){
			roomBuildPointsList[roomBuilderListCounter].SendMessage("Construct");
			roomBuilderListCounter += 1;
		}
		//now we do a typecasting to make sure all connecting corridors and rooms have the same type to make sure a conformity in texture and object styles
		corridorPartListCounter = 0;
		while (corridorPartListCounter < corridorPartList.Count){
				corridorPartList[corridorPartListCounter].SendMessage("TypeCasting");
				corridorPartListCounter = corridorPartListCounter +1;
		}
		roomPartListCounter = 0;
		while (roomPartListCounter < roomPartList.Count){
				roomPartList[roomPartListCounter].SendMessage("TypeCasting");
				roomPartListCounter = roomPartListCounter +1;
		}
		stairCasePartListCounter = 0;
		while (stairCasePartListCounter < stairCasePartList.Count){
				stairCasePartList[stairCasePartListCounter].SendMessage("TypeCasting");
				stairCasePartListCounter = stairCasePartListCounter +1;
		}
		//building walls and detailing
		corridorPartListCounter = 0;
		while (corridorPartListCounter < corridorPartList.Count){
				corridorPartList[corridorPartListCounter].SendMessage("Construct");
				corridorPartListCounter = corridorPartListCounter +1;
		}
		roomPartListCounter = 0;
		while (roomPartListCounter < roomPartList.Count){
				roomPartList[roomPartListCounter].SendMessage("Construct");
				roomPartListCounter = roomPartListCounter +1;
		}
		stairCasePartListCounter = 0;
		while (stairCasePartListCounter < stairCasePartList.Count){
				stairCasePartList[stairCasePartListCounter].SendMessage("Construct");
				stairCasePartListCounter = stairCasePartListCounter +1;
		}
		corridorPartListCounter = 0;
		while (corridorPartListCounter < corridorPartList.Count){
				corridorPartList[corridorPartListCounter].SendMessage("ConstructFinishing");
				corridorPartListCounter = corridorPartListCounter +1;
		}
		roomPartListCounter = 0;
		while (roomPartListCounter < roomPartList.Count){
				roomPartList[roomPartListCounter].SendMessage("ConstructFinishing");
				roomPartListCounter = roomPartListCounter +1;
		}
		stairCasePartListCounter = 0;
		while (stairCasePartListCounter < stairCasePartList.Count){
				stairCasePartList[stairCasePartListCounter].SendMessage("ConstructFinishing");
				stairCasePartListCounter = stairCasePartListCounter +1;
		}
		//now we determine the endPoint
		var basicsInRange = Physics.OverlapSphere(buildPointer.transform.position, 1);
		if (basicsInRange.Length > 0){
			var countAll = 0;
			while (countAll < basicsInRange.Length){
				
				if(basicsInRange[countAll].transform.name == "RoomPart" || basicsInRange[countAll].transform.name == "CorridorPart" || basicsInRange[countAll].transform.name == "StairCasePart" ){
					var basicsInRangeScript = basicsInRange[countAll].GetComponent<ColliderScript>();
					
					basicsInRangeScript.endPointer = true;
					basicsInRangeScript.floorModelSpot =1;
					basicsInRangeScript.leftWallModelSpot =1;
					basicsInRangeScript.rightWallModelSpot =1;
					basicsInRangeScript.frontWallModelSpot =1;
					basicsInRangeScript.backWallModelSpot =1;
				}
				countAll +=1;
			}
		}
		//rounding off any room corners
		level.BroadcastMessage("StartBuildCornerWalls");
		//primary objects corridor
		corridorPartListCounter = 0;
		while (corridorPartListCounter < corridorPartList.Count){
			corridorPartList[corridorPartListCounter].SendMessage("GeneratePrimaryObject");
			corridorPartListCounter = corridorPartListCounter +1;
		}
		//primary objects rooms
		roomPartListCounter = 0;
		var TempColliderList = new List<GameObject>();
		while (roomPartListCounter < roomPartList.Count){
			TempColliderList.Add (roomPartList[roomPartListCounter]);	
			roomPartListCounter = roomPartListCounter +1;
		}
		var tempNumber = TempColliderList.Count;
		var tempCounter =0;
		while (tempCounter < tempNumber){
			if(TempColliderList.Count > 0){
				var random = Random.Range (0,TempColliderList.Count);
				TempColliderList[random].SendMessage("GeneratePrimaryObject");
				TempColliderList.RemoveAt(random);
				tempCounter +=1;
			}
		}
		//secundary objects, all parts in one big list
		corridorPartListCounter = 0;
		TempColliderList = new List<GameObject>();
		while (corridorPartListCounter < corridorPartList.Count){
			TempColliderList.Add (corridorPartList[corridorPartListCounter]);	
			corridorPartListCounter = corridorPartListCounter +1;
		}
		roomPartListCounter = 0;
		while (roomPartListCounter < roomPartList.Count){
				
			TempColliderList.Add (roomPartList[roomPartListCounter]);	
			roomPartListCounter = roomPartListCounter +1;
		}
		tempNumber = TempColliderList.Count;
		tempCounter =0;
		while (tempCounter < tempNumber){
			if(TempColliderList.Count > 0){
				var random = Random.Range (0,TempColliderList.Count);
				TempColliderList[random].SendMessage("GenerateSecundaryObject");
				TempColliderList.RemoveAt(random);
				tempCounter +=1;
			}
		}
		//removing scripts from objects and randomizing certain objects (like our dining chairs)
		DestroyImmediate(buildPointer.transform.gameObject);
		level.BroadcastMessage("DestroyAllColorSpheres");
		level.BroadcastMessage("DestroyAllTraces");
		//cleaning up any potential left over destroyed items from the carbage collector
		System.GC.Collect();
		// Instantiate player
		var thePlayer = Instantiate(playerCharacter, playerSpawnLocation, Quaternion.identity) as GameObject;
		thePlayer.transform.name = "A Player";
		thePlayer.transform.parent = level.transform;
		//done
		print ("all done");
		generating = 0;	
	}
	
}