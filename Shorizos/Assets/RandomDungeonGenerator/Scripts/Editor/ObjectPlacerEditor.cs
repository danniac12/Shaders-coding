using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(ObjectScript))]
public class ObjectPlacerEditor : Editor {
	
	//private int ObjectLocation = 3;
	
	public override void OnInspectorGUI() {
		
		var currentComponent = (ObjectScript) target; 
		
		GUI.backgroundColor = Color.grey;	
		
		GUI.changed = false;
		
	
		//Design for descriptiontext
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
		
		GUILayout.Label("Welcome to the Object Editor. Use the sliders to adjust the size and position of this object", _descriptionText, GUILayout.ExpandWidth(true));
		GUILayout.BeginVertical(_itemText);
		GUILayout.Label("Objects are either located in a Corridor or a Room. Choose if the object will be placed on the ceiling, walls or floor", _helpText, GUILayout.ExpandWidth(true));
		GUILayout.EndVertical();
		
		
		/*
		var tempObject = Resources.Load("CorridorModels/PrimaryObjects/"+currentComponent.transform.name) as GameObject;
		if (tempObject != null){
			currentComponent.CorridorObject = true;
			currentComponent.RoomObject = false;
			currentComponent.Primary = true;
			currentComponent.Secundary = false;
		}
		tempObject = null;
		tempObject = Resources.Load("CorridorModels/SecundaryObjects/"+currentComponent.transform.name) as GameObject;
		if (tempObject != null){
			currentComponent.CorridorObject = true;
			currentComponent.RoomObject = false;
			currentComponent.Primary = false;
			currentComponent.Secundary = true;
		}
		tempObject = null;
		tempObject = Resources.Load("RoomModels/PrimaryObjects/"+currentComponent.transform.name) as GameObject;
		if (tempObject != null){
			currentComponent.RoomObject = true;
			currentComponent.CorridorObject = false;
			currentComponent.Primary = true;
			currentComponent.Secundary = false;
		}
		tempObject = null;
		tempObject = Resources.Load("RoomModels/SecundaryObjects/"+currentComponent.transform.name) as GameObject;
		if (tempObject != null){
			currentComponent.RoomObject = true;
			currentComponent.CorridorObject = false;
			currentComponent.Primary = false;
			currentComponent.Secundary = true;
		}
		*/	
		GUILayout.BeginVertical("the Prefab Model",_itemText);
		currentComponent.prefabModel = EditorGUILayout.ObjectField(currentComponent.prefabModel, typeof(Object), true);
			
		currentComponent.CorridorObject = GUILayout.Toggle(currentComponent.CorridorObject,"Corridor Object");
		if (currentComponent.CorridorObject == true){
			currentComponent.RoomObject = false;
		}
		currentComponent.RoomObject = GUILayout.Toggle(currentComponent.RoomObject,"Room Object");
		if (currentComponent.RoomObject == true){
			currentComponent.CorridorObject = false;
		}
		GUILayout.EndVertical();
		
		if (currentComponent.prefabModel == null){
			EditorGUILayout.HelpBox(string.Format("Please select a Prefab Model"),MessageType.Warning);
				
		} else {	
			
			/*
			if (currentComponent.CorridorObject == true && currentComponent.Primary == true){
				EditorGUILayout.HelpBox(string.Format("This object is a Corridor Pattern Object"),MessageType.Info);
			}
			if (currentComponent.CorridorObject == true && currentComponent.Secundary == true){
				EditorGUILayout.HelpBox(string.Format("This object is a Corridor Object"),MessageType.Info);
			}
			if (currentComponent.RoomObject == true && currentComponent.Primary == true){
				EditorGUILayout.HelpBox(string.Format("This object is a Primary Checked Room Object"),MessageType.Info);
			}
			if (currentComponent.RoomObject == true && currentComponent.Secundary == true){
				EditorGUILayout.HelpBox(string.Format("This object is a Room Object"),MessageType.Info);
			}
			
			*/
			GUILayout.BeginVertical(_itemText);
			if (currentComponent.CorridorObject == true){
				
				currentComponent.RoomCeilingObject = false;
				currentComponent.RoomWallObject = false;
				currentComponent.RoomFloorObject = false;
				
				GUILayout.Label("Corridor Object Location",_headerText);
				currentComponent.CorridorCeilingObject = GUILayout.Toggle(currentComponent.CorridorCeilingObject,"Ceiling Object");
				if (currentComponent.CorridorCeilingObject == true){
					currentComponent.CorridorWallObject = false;
					currentComponent.CorridorFloorObject = false;
					currentComponent.ObjectLocation = 1;
				}
				currentComponent.CorridorWallObject = GUILayout.Toggle(currentComponent.CorridorWallObject,"Wall Object");
				if (currentComponent.CorridorWallObject == true){
					currentComponent.CorridorCeilingObject = false;
					currentComponent.CorridorFloorObject = false;
					currentComponent.ObjectLocation = 2;
				}
				currentComponent.CorridorFloorObject = GUILayout.Toggle(currentComponent.CorridorFloorObject,"Floor Object");
				if (currentComponent.CorridorFloorObject == true){
					currentComponent.CorridorWallObject = false;
					currentComponent.CorridorCeilingObject = false;
					currentComponent.ObjectLocation = 3;
				}
				if (currentComponent.CorridorCeilingObject == false && currentComponent.CorridorWallObject == false && currentComponent.CorridorFloorObject == false){
					if (currentComponent.ObjectLocation == 1){
						currentComponent.CorridorCeilingObject = true;
					} else if(currentComponent.ObjectLocation == 2){
						currentComponent.CorridorWallObject = true;
					} else {
						currentComponent.CorridorFloorObject = true;
					}
					
				}
			}
			
			if (currentComponent.RoomObject == true){
				currentComponent.CorridorCeilingObject = false;
				currentComponent.CorridorWallObject = false;
				currentComponent.CorridorFloorObject = false;
				
				GUILayout.Label("Room Object Location",_headerText);
				currentComponent.RoomCeilingObject = GUILayout.Toggle(currentComponent.RoomCeilingObject,"Ceiling Object");
				if (currentComponent.RoomCeilingObject == true){
					currentComponent.RoomWallObject = false;
					currentComponent.RoomFloorObject = false;
					if(currentComponent.ObjectLocation != 1){
						ResetPositionValues();
						currentComponent.ObjectLocation = 1;
					}
				}
				currentComponent.RoomWallObject = GUILayout.Toggle(currentComponent.RoomWallObject,"Wall Object");
				if (currentComponent.RoomWallObject == true){
					currentComponent.RoomCeilingObject = false;
					currentComponent.RoomFloorObject = false;
					if(currentComponent.ObjectLocation != 2){
						ResetPositionValues();
						currentComponent.ObjectLocation = 2;
					}
				}
				currentComponent.RoomFloorObject = GUILayout.Toggle(currentComponent.RoomFloorObject,"Floor Object");
				if (currentComponent.RoomFloorObject == true){
					currentComponent.RoomWallObject = false;
					currentComponent.RoomCeilingObject = false;
					if(currentComponent.ObjectLocation != 3){
						ResetPositionValues();
						currentComponent.ObjectLocation = 3;
					}
				}
				if (currentComponent.RoomCeilingObject == false && currentComponent.RoomWallObject == false && currentComponent.RoomFloorObject == false){
					if (currentComponent.ObjectLocation == 1){
						currentComponent.RoomCeilingObject = true;
					} else if(currentComponent.ObjectLocation == 2){
						currentComponent.RoomWallObject = true;
					} else {
						currentComponent.RoomFloorObject = true;
					}
					
				}
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical(_itemText);
			GUILayout.Label("Objects can be small flat objects not sticking out from the place they are located , but they can also be big objects taking more spots then the location they are originally placed.", _helpText, GUILayout.ExpandWidth(true));
			
			currentComponent.smallObject = GUILayout.Toggle(currentComponent.smallObject,"Small Object");
			GUILayout.EndVertical();
			if (currentComponent.smallObject == true){
				currentComponent.LeftSize =0;
				currentComponent.RightSize =0;
				currentComponent.FrontSize =0;
				currentComponent.BackSize =0;
				currentComponent.AboveSize =0;
				currentComponent.BelowSize =0;
			}
			if (currentComponent.smallObject == false){
				GUILayout.Label("The object will take all the space of this grid", _helpText, GUILayout.ExpandWidth(true));
				GUILayout.Label("If the object needs to be bigger than the grid it is located on, here you can set the values",_helpText, GUILayout.ExpandWidth(true));
				
				//GUILayout.BeginVertical(_itemText);
				
				//GUILayout.Label("room needed left of the object", _headerText, GUILayout.ExpandWidth(true));
			//	currentComponent.LeftSize = EditorGUILayout.IntSlider("",currentComponent.LeftSize,0,2);
					GUILayout.BeginVertical("room needed right of the object",_itemText, GUILayout.ExpandWidth(true));
						currentComponent.RightSize = EditorGUILayout.IntSlider("",currentComponent.RightSize,0,2);
					GUILayout.EndVertical();
					
					GUILayout.BeginVertical("room needed in front of the object",_itemText, GUILayout.ExpandWidth(true));
						currentComponent.FrontSize = EditorGUILayout.IntSlider("",currentComponent.FrontSize,0,2);
					GUILayout.EndVertical();
				
					if (currentComponent.CorridorWallObject == true || currentComponent.RoomWallObject == true){
						//GUILayout.Label("room behind the object is 0, because its a Wall Object", _helpText, GUILayout.ExpandWidth(true));
						currentComponent.BackSize = 0;
					} else {
						//GUILayout.Label("room needed behind the object", _headerText, GUILayout.ExpandWidth(true));
						//currentComponent.BackSize = EditorGUILayout.IntSlider("",currentComponent.BackSize,0,2);
					}
					if (currentComponent.CorridorCeilingObject == false && currentComponent.RoomCeilingObject == false){
						GUILayout.BeginVertical("room needed above the object",_itemText, GUILayout.ExpandWidth(true));
							currentComponent.AboveSize = EditorGUILayout.IntSlider("",currentComponent.AboveSize,0,2);
						GUILayout.EndVertical();
					} else {
						//GUILayout.Label("room above the object is 0, because its a Ceiling Object", _helpText, GUILayout.ExpandWidth(true));
						currentComponent.AboveSize = 0;
						if (currentComponent.CorridorFloorObject == false && currentComponent.RoomFloorObject == false){
						GUILayout.BeginVertical("room needed below the object",_itemText, GUILayout.ExpandWidth(true));
							currentComponent.BelowSize = EditorGUILayout.IntSlider("",currentComponent.BelowSize,0,2);
						GUILayout.EndVertical();
						}else {
							//GUILayout.Label("room below the object is 0, because its a Floor Object", _helpText, GUILayout.ExpandWidth(true));
							currentComponent.BelowSize = 0;
						}
					}
					
				//GUILayout.EndVertical();
			}
			GUILayout.BeginVertical(_itemText);
			GUILayout.Label("Here you position the object further in relation to walls, ceiling or floors. This will determine how often an object fits in the dungeon and is a good way to steer the look and feel of your dungeon by how often and were an object appears.", _helpText, GUILayout.ExpandWidth(true));
				
			if (currentComponent.CorridorCeilingObject == true || currentComponent.CorridorFloorObject == true || currentComponent.RoomFloorObject == true || currentComponent.RoomCeilingObject == true  ){
				
				GUILayout.BeginVertical("Object location in relation to Walls",_itemText, GUILayout.ExpandWidth(true));
				currentComponent.WallTouching = GUILayout.Toggle(currentComponent.WallTouching,"Touching Wall");
				if (currentComponent.WallTouching == true){
					
					currentComponent.TouchingWall = "touching";
					
					currentComponent.WallTouchingOne = false;
					currentComponent.WallTouchingTwo = false;
					currentComponent.CornerTouching = false;
					currentComponent.CornerTouchingOne = false;
					currentComponent.CornerTouchingTwo = false;
					currentComponent.WallNotTouching = false;
					currentComponent.WallNoMatter = false;
				}
				currentComponent.WallTouchingOne = GUILayout.Toggle(currentComponent.WallTouchingOne,"One distance from a Wall");
				if (currentComponent.WallTouchingOne == true){
					
					currentComponent.TouchingWall = "one";
					
					currentComponent.WallTouching = false;
					currentComponent.WallTouchingTwo = false;
					currentComponent.CornerTouching = false;
					currentComponent.CornerTouchingOne = false;
					currentComponent.CornerTouchingTwo = false;
					currentComponent.WallNotTouching = false;
					currentComponent.WallNoMatter = false;
				}
				currentComponent.WallTouchingTwo = GUILayout.Toggle(currentComponent.WallTouchingTwo,"Two distance from a Wall");
				if (currentComponent.WallTouchingTwo == true){
					
					currentComponent.TouchingWall = "two";
					
					currentComponent.WallTouching = false;
					currentComponent.WallTouchingOne = false;
					currentComponent.CornerTouching = false;
					currentComponent.CornerTouchingOne = false;
					currentComponent.CornerTouchingTwo = false;
					currentComponent.WallNotTouching = false;
					currentComponent.WallNoMatter = false;
				}
				currentComponent.CornerTouching = GUILayout.Toggle(currentComponent.CornerTouching,"Object in a corner");
				if (currentComponent.CornerTouching == true){
					
					currentComponent.TouchingWall = "corner";
					
					currentComponent.WallTouching = false;
					currentComponent.WallTouchingOne = false;
					currentComponent.WallTouchingTwo = false;
					currentComponent.CornerTouchingOne = false;
					currentComponent.CornerTouchingTwo = false;
					currentComponent.WallNotTouching = false;
					currentComponent.WallNoMatter = false;
				}
				currentComponent.CornerTouchingOne = GUILayout.Toggle(currentComponent.CornerTouchingOne,"Object one distance from a corner");
				if (currentComponent.CornerTouchingOne == true){
					
					currentComponent.TouchingWall = "cornerOne";
					
					currentComponent.WallTouching = false;
					currentComponent.WallTouchingOne = false;
					currentComponent.WallTouchingTwo = false;
					currentComponent.CornerTouching = false;
					currentComponent.CornerTouchingTwo = false;
					currentComponent.WallNotTouching = false;
					currentComponent.WallNoMatter = false;
				}
				currentComponent.CornerTouchingTwo = GUILayout.Toggle(currentComponent.CornerTouchingTwo,"Object two distance from a corner");
				if (currentComponent.CornerTouchingTwo == true){
					
					currentComponent.TouchingWall = "cornerTwo";
					
					currentComponent.WallTouching = false;
					currentComponent.WallTouchingOne = false;
					currentComponent.WallTouchingTwo = false;
					currentComponent.CornerTouching = false;
					currentComponent.CornerTouchingOne = false;
					currentComponent.WallNotTouching = false;
					currentComponent.WallNoMatter = false;
				}
				currentComponent.WallNotTouching = GUILayout.Toggle(currentComponent.WallNotTouching,"Not touching a Wall");
				if (currentComponent.WallNotTouching == true){
					
					currentComponent.TouchingWall = "notTouching";
					
					currentComponent.WallTouching = false;
					currentComponent.WallTouchingOne = false;
					currentComponent.WallTouchingTwo = false;
					currentComponent.CornerTouching = false;
					currentComponent.CornerTouchingOne = false;
					currentComponent.CornerTouchingTwo = false;
					currentComponent.WallNoMatter = false;
				}
				currentComponent.WallNoMatter = GUILayout.Toggle(currentComponent.WallNoMatter,"Wall location not important");
				if (currentComponent.WallNoMatter == true){
					
					currentComponent.TouchingWall = "notMatter";
					
					currentComponent.WallTouching = false;
					currentComponent.WallTouchingOne = false;
					currentComponent.WallTouchingTwo = false;
					currentComponent.CornerTouching = false;
					currentComponent.CornerTouchingOne = false;
					currentComponent.CornerTouchingTwo = false;
					currentComponent.WallNotTouching = false;
				}
				GUILayout.EndVertical();
				if(currentComponent.WallTouching == false && currentComponent.WallTouchingOne == false && currentComponent.WallTouchingTwo == false &&currentComponent.CornerTouching == false && currentComponent.CornerTouchingOne == false && currentComponent.CornerTouchingTwo == false && currentComponent.WallNotTouching == false && currentComponent.WallNoMatter == false){
					currentComponent.TouchingWall = "notMatter";
					currentComponent.WallNoMatter = true;
				}
			}
			if (currentComponent.CorridorWallObject == true || currentComponent.RoomWallObject == true || currentComponent.CorridorFloorObject == true || currentComponent.RoomFloorObject == true  ){
				GUILayout.BeginVertical("Object location in relation to the Ceiling",_itemText, GUILayout.ExpandWidth(true));
				currentComponent.CeilingTouching = GUILayout.Toggle(currentComponent.CeilingTouching,"Touching Ceiling");
				if (currentComponent.CeilingTouching == true){
					
					currentComponent.TouchingCeiling = "touching";
					
					currentComponent.CeilingTouchingOne = false;
					currentComponent.CeilingTouchingTwo = false;
					currentComponent.CeilingNotTouching = false;
					currentComponent.CeilingNoMatter = false;
				}
				currentComponent.CeilingTouchingOne = GUILayout.Toggle(currentComponent.CeilingTouchingOne,"One distance from Ceiling");
				if (currentComponent.CeilingTouchingOne == true){
					
					currentComponent.TouchingCeiling = "one";
					
					currentComponent.CeilingTouching = false;
					currentComponent.CeilingTouchingTwo = false;
					currentComponent.CeilingNotTouching = false;
					currentComponent.CeilingNoMatter = false;
				}
				currentComponent.CeilingTouchingTwo = GUILayout.Toggle(currentComponent.CeilingTouchingTwo,"Two distance from Ceiling");
				if (currentComponent.CeilingTouchingTwo == true){
					
					currentComponent.TouchingCeiling = "two";
					
					currentComponent.CeilingTouching = false;
					currentComponent.CeilingTouchingOne = false;
					currentComponent.CeilingNotTouching = false;
					currentComponent.CeilingNoMatter = false;
				}
				currentComponent.CeilingNotTouching = GUILayout.Toggle(currentComponent.CeilingNotTouching,"Not touching Ceiling");
				if (currentComponent.CeilingNotTouching == true){
					
					currentComponent.TouchingCeiling = "notTouching";
					
					currentComponent.CeilingTouching = false;
					currentComponent.CeilingTouchingOne = false;
					currentComponent.CeilingTouchingTwo = false;
					currentComponent.CeilingNoMatter = false;
				}
				currentComponent.CeilingNoMatter = GUILayout.Toggle(currentComponent.CeilingNoMatter,"Ceiling location not important");
				if (currentComponent.CeilingNoMatter == true){
					
					currentComponent.TouchingCeiling = "notMatter";
					
					currentComponent.CeilingTouching = false;
					currentComponent.CeilingTouchingOne = false;
					currentComponent.CeilingTouchingTwo = false;
					currentComponent.CeilingNotTouching = false;
				}
				GUILayout.EndVertical();
				if(currentComponent.CeilingTouching == false && currentComponent.CeilingTouchingOne == false && currentComponent.CeilingTouchingTwo == false &&currentComponent.CeilingNotTouching == false && currentComponent.CeilingNoMatter == false){
					currentComponent.TouchingCeiling = "notMatter";
					currentComponent.CeilingNoMatter = true;
				}
			}
			if (currentComponent.CorridorWallObject == true || currentComponent.RoomWallObject == true || currentComponent.CorridorCeilingObject == true || currentComponent.RoomCeilingObject == true  ){
				GUILayout.BeginVertical("Object location in relation to the Floor",_itemText, GUILayout.ExpandWidth(true));
				currentComponent.FloorTouching = GUILayout.Toggle(currentComponent.FloorTouching,"Touching Floor");
				if (currentComponent.FloorTouching == true){
					
					currentComponent.TouchingFloor = "touching";
					
					currentComponent.FloorTouchingOne = false;
					currentComponent.FloorTouchingTwo = false;
					currentComponent.FloorNotTouching = false;
					currentComponent.FloorNoMatter = false;
				}
				currentComponent.FloorTouchingOne = GUILayout.Toggle(currentComponent.FloorTouchingOne,"One distance from Floor");
				if (currentComponent.FloorTouchingOne == true){
					
					currentComponent.TouchingFloor = "one";
					
					currentComponent.FloorTouching = false;
					currentComponent.FloorTouchingTwo = false;
					currentComponent.FloorNotTouching = false;
					currentComponent.FloorNoMatter = false;
				}
				currentComponent.FloorTouchingTwo = GUILayout.Toggle(currentComponent.FloorTouchingTwo,"Two distance from Floor");
				if (currentComponent.FloorTouchingTwo == true){
					
					currentComponent.TouchingFloor = "two";
					
					currentComponent.FloorTouching = false;
					currentComponent.FloorTouchingOne = false;
					currentComponent.FloorNotTouching = false;
					currentComponent.FloorNoMatter = false;
				}
				currentComponent.FloorNotTouching = GUILayout.Toggle(currentComponent.FloorNotTouching,"Not touching Floor");
				if (currentComponent.FloorNotTouching == true){
					
					currentComponent.TouchingFloor = "notTouching";
					
					currentComponent.FloorTouching = false;
					currentComponent.FloorTouchingOne = false;
					currentComponent.FloorTouchingTwo = false;
					currentComponent.FloorNoMatter = false;
				}
				currentComponent.FloorNoMatter = GUILayout.Toggle(currentComponent.FloorNoMatter,"Floor location not important");
				if (currentComponent.FloorNoMatter == true){
					
					currentComponent.TouchingFloor = "notMatter";
					
					currentComponent.FloorTouching = false;
					currentComponent.FloorTouchingOne = false;
					currentComponent.FloorTouchingTwo = false;
					currentComponent.FloorNotTouching = false;
				}
				GUILayout.EndVertical();
				if(currentComponent.FloorTouching == false && currentComponent.FloorTouchingOne == false && currentComponent.FloorTouchingTwo == false &&currentComponent.FloorNotTouching == false && currentComponent.FloorNoMatter == false){
					currentComponent.TouchingFloor = "notMatter";
					currentComponent.FloorNoMatter = true;
				}
			}
			
			GUILayout.EndVertical();
			
			if (GUI.changed)EditorUtility.SetDirty(currentComponent);
		}
	}
	void ResetPositionValues(){
		var currentComponent = (ObjectScript) target; 
			currentComponent.WallTouching = false;
		currentComponent.WallTouchingOne = false;
		currentComponent.WallTouchingTwo = false;
		currentComponent.CornerTouching = false;
		currentComponent.CornerTouchingOne = false;
		currentComponent.CornerTouchingTwo = false;
		currentComponent.WallNotTouching = false;
		currentComponent.WallNoMatter = false;
		
		currentComponent.CeilingTouching = false;
		currentComponent.CeilingTouchingOne = false;
		currentComponent.CeilingTouchingTwo = false;
		currentComponent.CeilingNotTouching = false;
		currentComponent.CeilingNoMatter = false;
				
		currentComponent.FloorTouching = false;
		currentComponent.FloorTouchingOne = false;
		currentComponent.FloorTouchingTwo = false;
		currentComponent.FloorNotTouching = false;
		currentComponent.FloorNoMatter = false;	
	}
	/* void ClearChoices(){
		//reset values
		var currentComponent = (ObjectScript) target; 
		
		currentComponent.CorridorObject = false;
		currentComponent.RoomObject = false;
		
		currentComponent.Primary = false;
		currentComponent.Secundary = false;
		
		currentComponent.RoomCeilingObject = false;
		currentComponent.RoomWallObject = false;
		currentComponent.RoomFloorObject = false;
			
		currentComponent.CorridorCeilingObject = false;
		currentComponent.CorridorWallObject = false;
		currentComponent.CorridorFloorObject = false;
		
		currentComponent.TouchingWall = "notMatter";
		currentComponent.TouchingCeiling = "notMatter";
		currentComponent.TouchingFloor = "notMatter";
				
		currentComponent.WallTouching = false;
		currentComponent.WallTouchingOne = false;
		currentComponent.WallTouchingTwo = false;
		currentComponent.CornerTouching = false;
		currentComponent.CornerTouchingOne = false;
		currentComponent.CornerTouchingTwo = false;
		currentComponent.WallNotTouching = false;
		currentComponent.WallNoMatter = false;
		
		currentComponent.CeilingTouching = false;
		currentComponent.CeilingTouchingOne = false;
		currentComponent.CeilingTouchingTwo = false;
		currentComponent.CeilingNotTouching = false;
		currentComponent.CeilingNoMatter = false;
				
		currentComponent.FloorTouching = false;
		currentComponent.FloorTouchingOne = false;
		currentComponent.FloorTouchingTwo = false;
		currentComponent.FloorNotTouching = false;
		currentComponent.FloorNoMatter = false;
	}*/
}