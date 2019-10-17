using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor {
		
		private bool pickStyleOnce = false;
		private GameObject Level;
		
		private float corridorParts;
	 	private float sideCorridorParts;
		private float roomParts;
		private float stairCaseParts;		
		private float minSize;
		private float maxSize;
		private int LastClicked = 3;
		private bool DeleteButton = false;
	
		private int BuildStage =0;
	
	public override void OnInspectorGUI() {
		
		Level = GameObject.FindWithTag("DGRandomLevel");
		GUI.backgroundColor = Color.grey;
		
		//DungeonGenerator currentComponent = target as DungeonGenerator;
		var currentComponent = (DungeonGenerator) target; 
		
		GUI.changed = false;
		
		//Design for descriptiontext
		GUI.backgroundColor = Color.gray;
		
		var _descriptionText = new GUIStyle(GUI.skin.box);
		_descriptionText.wordWrap = true;
		_descriptionText.alignment = TextAnchor.UpperLeft;
		Color descriptionColor = Color.white;
		descriptionColor.a = 1.0f;
		_descriptionText.normal.textColor = descriptionColor;
		
		//Design for helptext
		var _helpText = new GUIStyle(GUI.skin.box);
		_helpText.wordWrap = true;
		_helpText.alignment = TextAnchor.UpperLeft;
		Color helpColor = Color.white;
		helpColor.a = 1.00f;
		
		_helpText.normal.textColor = helpColor;
		
		//Design for items
		var _itemText = new GUIStyle(GUI.skin.window);
		_itemText.wordWrap = true;
		_itemText.alignment = TextAnchor.UpperLeft;
	
		
		//Design for headerText
		var _headerText = new GUIStyle(GUI.skin.label);
		_headerText.wordWrap = true;
		_headerText.alignment = TextAnchor.UpperLeft;
		Color _headerTextitemColor = Color.white;
		_headerTextitemColor.a = 1.0f;
		_headerText.normal.textColor = _headerTextitemColor;
		
		GUILayout.Label("Welcome to the Dungeon Generator! Simply use the sliders to adjust the settings and click on Build New Dungeon!", _descriptionText, GUILayout.ExpandWidth(true));
		
		
		//style picker
		GUILayout.BeginVertical("---| STYLES |---",_itemText);
				
		GUILayout.Label("The design of your dungeon is build up out of 3 elements: Corridors, rooms and detailing. These three elements can have a certain style (for instance roman or gothic, but also diningroom or weaponroom). If you only want specific styles click on the checkbox below", _helpText, GUILayout.ExpandWidth(true));
		
		if(currentComponent.pickStyle == false){
			EditorGUILayout.HelpBox(string.Format("All styles will be used"),MessageType.Info);
				pickStyleOnce = true;
		}
		currentComponent.pickStyle = GUILayout.Toggle(currentComponent.pickStyle,"Only specific styles");
		
		if(currentComponent.pickStyle == true){
		
			if (pickStyleOnce == true){
				pickStyleOnce = false;
				//currentComponent.CleanUpLevel();
				currentComponent.CollectResources();
				currentComponent.userCorridorTypes = new List<string>();
				for (int corridorIndex = 0; corridorIndex < currentComponent.corridorTypes.Count;corridorIndex++){
					var corridorType = currentComponent.corridorTypes[corridorIndex];
					currentComponent.userCorridorTypes.Add(corridorType);
				};
				currentComponent.userRoomTypes = new List<string>();
				for (int roomIndex = 0; roomIndex < currentComponent.roomTypes.Count;roomIndex++){
					var roomType = currentComponent.roomTypes[roomIndex];
					currentComponent.userRoomTypes.Add(roomType);
				};
				currentComponent.userFinishingModelTypes = new List<string>();
				for (int finishingModelIndex = 0; finishingModelIndex < currentComponent.finishingModelTypes.Count;finishingModelIndex++){
					var finishingModelType = currentComponent.finishingModelTypes[finishingModelIndex];	
					currentComponent.userFinishingModelTypes.Add(finishingModelType);
				};
			};
			GUILayout.Label("Corridor styles:",_headerText);
			for (int boolCorIndex = 0; boolCorIndex < currentComponent.userCorridorTypes.Count;boolCorIndex++){
				EditorGUILayout.BeginHorizontal();
				if(currentComponent.userCorridorTypes.Count > 1){
					GUILayout.Label(""+currentComponent.userCorridorTypes[boolCorIndex]+"");
					if (GUILayout.Button("Remove this Style")) {
						currentComponent.userCorridorTypes.RemoveAt(boolCorIndex);
					};
				}else{
					GUILayout.Label(""+currentComponent.userCorridorTypes[boolCorIndex]+" is the only style you will be using");		
				};
				EditorGUILayout.EndHorizontal();
			};
			
			GUILayout.Label("");
			GUILayout.Label("Room styles:",_headerText);
			for (int boolRoomIndex = 0; boolRoomIndex < currentComponent.userRoomTypes.Count;boolRoomIndex++){
				EditorGUILayout.BeginHorizontal();
				if(currentComponent.userRoomTypes.Count > 1){
					 GUILayout.Label(""+currentComponent.userRoomTypes[boolRoomIndex]+"");
					if (GUILayout.Button("Remove this Style")) {
						currentComponent.userRoomTypes.RemoveAt(boolRoomIndex);
					};
				}else{;	
					GUILayout.Label(""+currentComponent.userRoomTypes[boolRoomIndex]+" is the only style you will be using");
				};
				EditorGUILayout.EndHorizontal();
			};
			GUILayout.Label("");
			GUILayout.Label("Detailing styles:",_headerText);
			for (int boolFinishedIndex = 0; boolFinishedIndex < currentComponent.userFinishingModelTypes.Count;boolFinishedIndex++){
				EditorGUILayout.BeginHorizontal();
				if(currentComponent.userFinishingModelTypes.Count > 1){
					GUILayout.Label(""+currentComponent.userFinishingModelTypes[boolFinishedIndex]+"");
					if (GUILayout.Button("Remove this Style")) {
						currentComponent.userFinishingModelTypes.RemoveAt(boolFinishedIndex);
					};
				}else{
					GUILayout.Label(""+currentComponent.userFinishingModelTypes[boolFinishedIndex]+" is the only style you will be using");
				};
				EditorGUILayout.EndHorizontal();
			};
			if (GUILayout.Button("Reset")) {
				//currentComponent.CleanUpLevel();	
				pickStyleOnce = true;
			};
		};	
		GUILayout.EndVertical();
		GUILayout.BeginVertical("---| CORRIDORS |---",_itemText);
		GUILayout.Label("The corridors are the main route of the dungeon. It checks if there is a path from beginning to end, and creates: Side-corridors, Rooms and Staircases ", _helpText, GUILayout.ExpandWidth(true));
		////
		//corridor setup
		GUILayout.BeginVertical("Amount of Corridors",_itemText);
		currentComponent.maxAmountOfCorridor = EditorGUILayout.IntSlider(currentComponent.maxAmountOfCorridor,2,30);
		GUILayout.EndVertical();
		GUILayout.BeginVertical("Amount of parts per Corridor",_itemText);
			currentComponent.minCorridorLength = EditorGUILayout.IntSlider("min",currentComponent.minCorridorLength,1,10);
			currentComponent.maxCorridorLength = EditorGUILayout.IntSlider("max",currentComponent.maxCorridorLength,currentComponent.minCorridorLength,10);
		GUILayout.EndVertical();
		
		//sidecorridor setup
		GUILayout.Label("Side-corridors grow from the main Corridor, creating a more maze like dungeon. They will also create rooms but no additional staircases", _helpText, GUILayout.ExpandWidth(true));
		GUILayout.BeginVertical("Side-corridors chance",_itemText);
			currentComponent.sideCorridorChancePercentage = EditorGUILayout.IntSlider("",currentComponent.sideCorridorChancePercentage,0,25);
		GUILayout.EndVertical();
	
		if (currentComponent.sideCorridorChancePercentage > 0){
			GUILayout.BeginVertical("Amount of Side-corridors",_itemText);
				currentComponent.maxAmountOfSideCorridor = EditorGUILayout.IntSlider(currentComponent.maxAmountOfSideCorridor,1,10);
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical("Amount of parts per Side-corridor",_itemText);
				currentComponent.minSideCorridorLength = EditorGUILayout.IntSlider("min",currentComponent.minSideCorridorLength,1,10);
				currentComponent.maxSideCorridorLength = EditorGUILayout.IntSlider("max",currentComponent.maxSideCorridorLength,currentComponent.minSideCorridorLength,10);
			GUILayout.EndVertical();
		}	
		GUILayout.Label("Corridors only go straight, left or right and they cant make a 180 degree turn. Can result in longer dungeons", _helpText, GUILayout.ExpandWidth(true));
		currentComponent.lessRandomCorridors = GUILayout.Toggle(currentComponent.lessRandomCorridors,"Only straight, left or right");
		

		//corridor object placem
		GUILayout.Label("Objects inside Corridors and Side-corridors can be placed in certain patterns. Place Objects that you want in a pattern inside the Recourses/Corridors/PaternObjects folder", _helpText, GUILayout.ExpandWidth(true));
	
		currentComponent.maximumObjects = GUILayout.Toggle(currentComponent.maximumObjects,"Object in every corridorpart");
		currentComponent.objectNoObject = GUILayout.Toggle(currentComponent.objectNoObject,"Object in every other corridor part");
		currentComponent.objectNoObjectNoObject = GUILayout.Toggle(currentComponent.objectNoObjectNoObject,"Object in every second corridor part");
		if(currentComponent.maximumObjects == false && currentComponent.objectNoObject == false && currentComponent.objectNoObjectNoObject == false){
			EditorGUILayout.HelpBox(string.Format("No pattern is chosen. This setting will use a random pattern for each connected corridor"),MessageType.Warning);
		}
		GUILayout.EndVertical();
		
		//room setup
		GUILayout.BeginVertical("---| ROOMS |---",_itemText);
		GUILayout.Label("Rooms are created from corridors and grow in random directions. When rooms grow over other rooms or corridors they become 1 big room with the same style", _helpText, GUILayout.ExpandWidth(true));
		GUILayout.BeginVertical("Chance of a room per corridorpart",_itemText);
			currentComponent.roomChancePercentage = EditorGUILayout.IntSlider(currentComponent.roomChancePercentage,0,100);
		GUILayout.EndVertical();
		
		if (currentComponent.roomChancePercentage > 0){
			GUILayout.BeginVertical("Room width",_itemText);
				currentComponent.minRoomWidth = EditorGUILayout.IntSlider("min",currentComponent.minRoomWidth,1,5);
				currentComponent.maxRoomWidth = EditorGUILayout.IntSlider("max",currentComponent.maxRoomWidth,currentComponent.minRoomWidth,5);
			GUILayout.EndVertical();
			GUILayout.BeginVertical("Room depth",_itemText);
				currentComponent.minRoomLength = EditorGUILayout.IntSlider("min",currentComponent.minRoomLength,1,5);
				currentComponent.maxRoomLength = EditorGUILayout.IntSlider("max",currentComponent.maxRoomLength,currentComponent.minRoomLength,5);
			GUILayout.EndVertical();
			GUILayout.BeginVertical("Room height",_itemText);
				currentComponent.minRoomHeigth = EditorGUILayout.IntSlider("min",currentComponent.minRoomHeigth,1,5);
				currentComponent.maxRoomHeigth = EditorGUILayout.IntSlider("max",currentComponent.maxRoomHeigth,currentComponent.minRoomHeigth,5);
			GUILayout.EndVertical();
			GUILayout.BeginVertical("Rounded corner chance",_itemText);
				currentComponent.CornerWallChancePercentage = EditorGUILayout.IntSlider(currentComponent.CornerWallChancePercentage,0,100);
				currentComponent.deadSpots = GUILayout.Toggle(currentComponent.deadSpots,"Prefent unreachable areas");
			GUILayout.EndVertical();
			
		}	
		GUILayout.EndVertical();
		//staircase setup
		GUILayout.BeginVertical("---| STAIRCASES |---",_itemText);
		GUILayout.Label("You can choose to add staircases, adding additional floors to your dungeon. This will create overhangs and balconies, giving a new level of complexity to the dungeon.", _helpText, GUILayout.ExpandWidth(true));
		GUILayout.BeginVertical("Chance for a staircase after each Corridor",_itemText);
			currentComponent.staircaseChancePercentage = EditorGUILayout.IntSlider(currentComponent.staircaseChancePercentage,0,100);
		GUILayout.EndVertical();
		if (currentComponent.staircaseChancePercentage > 0){
		
			
			GUILayout.Label("A staircase always goes 1 level up or 1 level down", _helpText, GUILayout.ExpandWidth(true));
		
			currentComponent.stairCaseUp = GUILayout.Toggle(currentComponent.stairCaseUp,"Only stairCases going Up");
			if (currentComponent.stairCaseUp == true){
				currentComponent.stairCaseDirectionSetup = "Up";
				currentComponent.stairCaseDown = false;
				currentComponent.stairCaseRandom = false;
				LastClicked = 1;
			}
			currentComponent.stairCaseDown = GUILayout.Toggle(currentComponent.stairCaseDown,"Only stairCases going Down");
			if (currentComponent.stairCaseDown == true){
				currentComponent.stairCaseDirectionSetup = "Down";
				currentComponent.stairCaseUp = false;
				currentComponent.stairCaseRandom = false;
				LastClicked = 2;
			}
			currentComponent.stairCaseRandom = GUILayout.Toggle(currentComponent.stairCaseRandom,"StairCases going a random direction");
			if (currentComponent.stairCaseRandom == true){
				currentComponent.stairCaseDirectionSetup = "Random";
				currentComponent.stairCaseDown = false;
				currentComponent.stairCaseUp = false;
				LastClicked = 3;
			}
			if(currentComponent.stairCaseUp == false && currentComponent.stairCaseDown == false && currentComponent.stairCaseRandom == false){
				if(LastClicked == 1){
					currentComponent.stairCaseUp = true;
				}else if(LastClicked == 2){
					currentComponent.stairCaseDown = true;
				}else{
					currentComponent.stairCaseRandom = true;
				}
			}
			//spiraling staircases
			GUILayout.Label("Spiraling staircases", _helpText, GUILayout.ExpandWidth(true));
			
			currentComponent.SpiralLeft = GUILayout.Toggle(currentComponent.SpiralLeft,"StairCases spiraling left");
			if (currentComponent.SpiralLeft == true){
				currentComponent.SpiralRight = false;
			}
			currentComponent.SpiralRight = GUILayout.Toggle(currentComponent.SpiralRight,"StairCases spiraling right");
			if (currentComponent.SpiralRight == true){
				currentComponent.SpiralLeft = false;
			}
		}
		GUILayout.EndVertical();
		
		//object chance
		GUILayout.BeginVertical("---| OBJECTS |---",_itemText);
		GUILayout.Label("You can set the chance that objects will appear in your dungeon.  If you want to prioritise an object put it in Recources/Rooms/PrimaryCheckedObjects.", _helpText, GUILayout.ExpandWidth(true));
		
		GUILayout.BeginVertical("percentage of ceiling objects",_itemText);
			currentComponent.ceilingObjectPercentage = EditorGUILayout.IntSlider("",currentComponent.ceilingObjectPercentage,0,100);
		GUILayout.EndVertical();
		GUILayout.BeginVertical("percentage of wall objects",_itemText);
			currentComponent.wallObjectPercentage = EditorGUILayout.IntSlider("",currentComponent.wallObjectPercentage,0,100);
		GUILayout.EndVertical();
		GUILayout.BeginVertical("percentage of floor objects",_itemText);
			currentComponent.floorObjectPercentage = EditorGUILayout.IntSlider("",currentComponent.floorObjectPercentage,0,100);
		GUILayout.EndVertical();
		GUILayout.EndVertical();
		GUILayout.BeginVertical(_itemText);
		
		
		
			corridorParts = currentComponent.maxAmountOfCorridor*currentComponent.minCorridorLength;
			sideCorridorParts = corridorParts*(currentComponent.sideCorridorChancePercentage/100.0f)*currentComponent.maxAmountOfSideCorridor*currentComponent.minSideCorridorLength;
			roomParts = (corridorParts+sideCorridorParts)*(currentComponent.roomChancePercentage/100.0f)*currentComponent.minRoomWidth*currentComponent.minRoomLength*currentComponent.minRoomHeigth;
			stairCaseParts = corridorParts*(currentComponent.staircaseChancePercentage/100.0f)*5;
			
			minSize = corridorParts+sideCorridorParts+roomParts+stairCaseParts;
			
			corridorParts = currentComponent.maxAmountOfCorridor*currentComponent.maxCorridorLength;
			sideCorridorParts = corridorParts*(currentComponent.sideCorridorChancePercentage/100.0f)*currentComponent.maxAmountOfSideCorridor*currentComponent.maxSideCorridorLength;
			roomParts = (corridorParts+sideCorridorParts)*(currentComponent.roomChancePercentage/100.0f)*currentComponent.maxRoomWidth*currentComponent.maxRoomLength*currentComponent.maxRoomHeigth;
			stairCaseParts = corridorParts*(currentComponent.staircaseChancePercentage/100.0f)*5;
			
			maxSize = corridorParts+sideCorridorParts+roomParts+stairCaseParts;
				
			
			if(maxSize - minSize < 500){
				EditorGUILayout.HelpBox(string.Format("Expected generation time SHORT"),MessageType.Warning);
			}else if(maxSize - minSize < 1500){
				EditorGUILayout.HelpBox(string.Format("Generation time shouldn't take long"),MessageType.Warning);
			}else if(maxSize - minSize < 3500){
				EditorGUILayout.HelpBox(string.Format("Generation time could take a while"),MessageType.Warning);
			}else if(maxSize - minSize < 5000){
				EditorGUILayout.HelpBox(string.Format("There is a chance of a LONG generation time"),MessageType.Warning);
			}else if(maxSize - minSize >= 5000){
				EditorGUILayout.HelpBox(string.Format("There is a chance of an EXTREMELY LONG generation time"),MessageType.Warning);
			}
		//comments about generated dungeon
		
		if (currentComponent.generating == -1){	
			if (GUILayout.Button("GENERATE DUNGEON with full feedback")) {
				currentComponent.generating = -2;
				BuildStage =0;
				EditorApplication.update += BuildingDungeon;
					
			}
		}
		if (currentComponent.generating == -2){
			if (GUILayout.Button("STOP DUNGEON GENERATION")) {
				EditorApplication.update -= BuildingDungeon;
				currentComponent.CleanUpLevel();
				currentComponent.generating = -1;
			}
		}
		
		if(currentComponent.generating > 0){
			EditorGUILayout.HelpBox(string.Format("GENERATING DUNGEON! Please wait untill this message disappears"),MessageType.Warning);	
			currentComponent.generating = currentComponent.generating-1;
			if(currentComponent.generating == 1){
				//BuildStage =0;
				EditorApplication.update -= BuildingDungeon;
				currentComponent.Build();
				currentComponent.generating = -1;
			}
		}
		if (currentComponent.generating == -1){
			if (GUILayout.Button("GENERATE DUNGEON (faster)")) {
				currentComponent.generating = 10;
			}
		}
		if(currentComponent.generating == -1 && Level.transform.childCount > 0){
			
			var partCount = (currentComponent.corridorPartList.Count+currentComponent.roomPartList.Count+currentComponent.stairCasePartList.Count);
			var objectCount = currentComponent.objectsBuildCounter;
			
			EditorGUILayout.HelpBox(string.Format("Your dungeon currently contains "+partCount+" Parts, and "+objectCount+" Objects. Press play to test it!"),MessageType.Info);
		}
		
		
		
		
		//clean up entire dungeon
		if (currentComponent.generating == -1 && Level.transform.childCount > 0){	
			if (GUILayout.Button("Delete previous Dungeon")) {
				
				DeleteButton = true;
			}
			
			if(DeleteButton == true){
				GUILayout.BeginVertical("Are you sure?",_itemText);
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Delete")) {
					EditorApplication.update -= BuildingDungeon;
					currentComponent.CleanUpLevel();
					DeleteButton = false;
				}
				if (GUILayout.Button("Cancel")) {
					DeleteButton = false;
				}
				EditorGUILayout.EndHorizontal();
				GUILayout.EndVertical();
			}
		}
		
		GUILayout.EndVertical();
		
		
		if (GUI.changed)EditorUtility.SetDirty(currentComponent);
	}
	
	private void BuildingDungeon(){
		
		var currentComponent = (DungeonGenerator) target; 
		
		if (BuildStage == 0){
			currentComponent.BuildStage1();
			
			Debug.Log("finished with stage 1: gather resources");
			BuildStage +=1;
		}
		if (BuildStage == 1){
			if(currentComponent.currentCorridorCounter < currentComponent.maxAmountOfCorridor){
				currentComponent.BuildStage2();
			} else {
				Debug.Log("finished with stage 2: main corridors and staircases");
				BuildStage +=1;
			}
		}
		if (BuildStage == 2){
			
			if (currentComponent.splitterListCounter < currentComponent.splitterBuildPointsList.Count){
				currentComponent.BuildStage3();
				currentComponent.splitterListCounter +=1;
			} else {
				Debug.Log("finished with stage 3: side corridors");
				BuildStage +=1;
			}
		}
		if (BuildStage == 3){
			if (currentComponent.roomBuilderListCounter < currentComponent.roomBuildPointsList.Count){
				currentComponent.BuildStage4();
				//Debug.Log("finished with room: "+currentComponent.roomBuilderListCounter);
				currentComponent.roomBuilderListCounter += 1;
			} else {
				Debug.Log("finished with stage 4: rooms");
				BuildStage +=1;
			}
		}
		if (BuildStage == 4){
			currentComponent.BuildStage5A();
			Debug.Log("starting with typecasting, this can take a while");
			BuildStage +=1;
		}
		if (BuildStage == 5){	
			
			if (currentComponent.corridorPartListCounter < currentComponent.corridorPartList.Count){
				currentComponent.BuildStage5B();
				currentComponent.corridorPartListCounter += 1;
				//Debug.Log("finished with typecasting corridorpart: "+currentComponent.corridorPartListCounter+" out of "+currentComponent.corridorPartList.Count+" parts");
				
			} else if(currentComponent.roomPartListCounter < currentComponent.roomPartList.Count){
				currentComponent.BuildStage5C();
				currentComponent.roomPartListCounter += 1;
				//Debug.Log("finished with typecasting roompart: "+currentComponent.roomPartListCounter+" out of "+currentComponent.roomPartList.Count+" parts");
				
			} else if(currentComponent.stairCasePartListCounter < currentComponent.stairCasePartList.Count){
				currentComponent.BuildStage5D();
				currentComponent.stairCasePartListCounter += 1;
				//Debug.Log("finished with typecasting staircasepart: "+currentComponent.stairCasePartListCounter+" out of "+currentComponent.stairCasePartList.Count+" parts");
				
			} else {
				Debug.Log("finished with stage 5: typecasting");
				BuildStage +=1;
			}
		}
		if (BuildStage == 6){
			currentComponent.BuildStage6Reset();
			Debug.Log("starting with closing dungeon, building walls, ceilings and floors");
			BuildStage +=1;
		}
		if (BuildStage == 7){
			if (currentComponent.corridorPartListCounter < currentComponent.corridorPartList.Count){
				currentComponent.BuildStage6A();
				currentComponent.corridorPartListCounter += 1;
				//Debug.Log("finished with closing corridorpart: "+currentComponent.corridorPartListCounter+" out of "+currentComponent.corridorPartList.Count+" parts");
				
			} else if(currentComponent.roomPartListCounter < currentComponent.roomPartList.Count){
				currentComponent.BuildStage6B();
				currentComponent.roomPartListCounter += 1;
				//Debug.Log("finished with closing roompart: "+currentComponent.roomPartListCounter+" out of "+currentComponent.roomPartList.Count+" parts");
				
			} else if(currentComponent.stairCasePartListCounter < currentComponent.stairCasePartList.Count){
				currentComponent.BuildStage6C();
				currentComponent.stairCasePartListCounter += 1;
				//Debug.Log("finished with closing staircasepart: "+currentComponent.stairCasePartListCounter+" out of "+currentComponent.stairCasePartList.Count+" parts");
				
			} else {
				Debug.Log("finished with stage 6: closing dungeon");
				BuildStage +=1;
			}
		}
		
		if (BuildStage == 8){
			currentComponent.BuildStage6Reset();
			Debug.Log("starting with detailing the dungeons, building railings and portals");
			BuildStage +=1;
		}
		
		if (BuildStage == 9){
			if (currentComponent.corridorPartListCounter < currentComponent.corridorPartList.Count){
				currentComponent.BuildStage6D();
				currentComponent.corridorPartListCounter += 1;
				//Debug.Log("finished with detailing corridorpart: "+currentComponent.corridorPartListCounter+" out of "+currentComponent.corridorPartList.Count+" parts");
				
			} else if(currentComponent.roomPartListCounter < currentComponent.roomPartList.Count){
				currentComponent.BuildStage6E();
				currentComponent.roomPartListCounter += 1;
				//Debug.Log("finished with detailing roompart: "+currentComponent.roomPartListCounter+" out of "+currentComponent.roomPartList.Count+" parts");
				
			} else if(currentComponent.stairCasePartListCounter < currentComponent.stairCasePartList.Count){
				currentComponent.BuildStage6F();
				currentComponent.stairCasePartListCounter += 1;
				//Debug.Log("finished with detailing staircasepart: "+currentComponent.stairCasePartListCounter+" out of "+currentComponent.stairCasePartList.Count+" parts");
				
			} else {
				currentComponent.BuildStage6G();
				Debug.Log("finished with stage 7: detailing dungeon");
				BuildStage +=1;
			}
		}
		
		if (BuildStage == 10){
			
			Debug.Log("starting stage 8: main objects");
			currentComponent.BuildStage7A();
			currentComponent.BuildStage7B();
			BuildStage +=1;
		}
		if (BuildStage == 11){
			if (currentComponent.corridorPartListCounter < currentComponent.corridorPartList.Count){
				currentComponent.BuildStage7C();
				//Debug.Log("finished with main corridorobject: "+currentComponent.corridorPartListCounter+" out of "+currentComponent.corridorPartList+" parts");
				currentComponent.corridorPartListCounter +=1;
			} else {
				//randomizing roomlists
				currentComponent.BuildStage7D();
				BuildStage +=1;
			}
		}
		if (BuildStage == 12){
			if (currentComponent.tempCounter < currentComponent.tempNumber){
				//Debug.Log("finished with main roomobject: "+currentComponent.tempCounter+" out of "+currentComponent.tempNumber+" parts");
				currentComponent.BuildStage7E();
			} else {
				Debug.Log("finished with stage 8: main objects");
				BuildStage +=1;
			}
		}
		if (BuildStage == 13){
			Debug.Log("starting with stage 9: secundairy objects");
			currentComponent.BuildStage8Shuffle();
			BuildStage +=1;
		}
		if (BuildStage == 14){
			if (currentComponent.tempCounter < currentComponent.tempNumber){
				currentComponent.BuildStage8A();
				//Debug.Log("finished with positioning secundairy object: "+currentComponent.tempCounter+" out of "+currentComponent.tempNumber+" parts");
				currentComponent.tempCounter +=1;
			} else {
				Debug.Log("finished with stage 9: secundairy objects");
				BuildStage +=1;
			}
		}
		if (BuildStage == 15){
			currentComponent.BuildStage9();
			
			Debug.Log("finished with building the dungeon");
			currentComponent.generating = -1;
			EditorApplication.update -= BuildingDungeon;
		}
	}
}