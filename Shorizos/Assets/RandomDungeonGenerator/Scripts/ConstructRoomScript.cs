using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ConstructRoomScript : MonoBehaviour {
	
	public string roomType;
	public string finishingType;
	public GameObject roomPart;
	
	private GameObject theLevel;
	
	public int roomLenghtNow;
	public int roomWidthNow;
	public int roomHeightNow;
	
	private Vector3 LenghtVector;
	private Vector3 WidthVector;
	private Vector3 HeightVector;
	
	private int roomDirection;
	
	//======================= start of functions =============//
	
	public void Construct (){
		
		theLevel = GameObject.FindWithTag("DGRandomLevel");		
		var LevelSettingsScript = GameObject.FindWithTag("DungeonBuilder").GetComponent<DungeonGenerator>();	
				
		roomDirection = Random.Range(1,5);
		if(roomDirection == 1){
			LenghtVector = new Vector3(0,0,5);
			WidthVector = new Vector3(5,0,0);
			HeightVector = new Vector3(0,4.5f,0);
		}else if(roomDirection == 2){
			LenghtVector = new Vector3(5,0,0);
			WidthVector = new Vector3(0,0,-5);
			HeightVector = new Vector3(0,4.5f,0);
		}else if(roomDirection == 3){
			LenghtVector = new Vector3(0,0,-5);
			WidthVector = new Vector3(-5,0,0);
			HeightVector = new Vector3(0,4.5f,0);
		}else if(roomDirection == 4){
			LenghtVector = new Vector3(-5,0,0);
			WidthVector = new Vector3(0,0,5);
			HeightVector = new Vector3(0,4.5f,0);
		}
			
		var roomLenghtCountNow = 0;
		var roomWidthCountNow = 0;
		var roomHeightCountNow = 0;
	
		//building the room
		while (roomLenghtCountNow < roomLenghtNow){
			transform.position = transform.position + LenghtVector;
			while (roomWidthCountNow < roomWidthNow){
				while (roomHeightCountNow < roomHeightNow){
					var basicsInRange = Physics.OverlapSphere(this.transform.position, 1);
					if(basicsInRange.Length == 0){	
						var theRoomCollider = Instantiate(roomPart, transform.position,Quaternion.identity) as GameObject;
	 					theRoomCollider.transform.parent = theLevel.transform;
						theRoomCollider.name = roomPart.name;
					
						var AttachedColliderScript = theRoomCollider.GetComponent<ColliderScript>();
						AttachedColliderScript.partType = roomType;
						AttachedColliderScript.finishingType = finishingType;
						AttachedColliderScript.direction = roomDirection;
	 				
	 					if(roomHeightCountNow == 0){
	 						AttachedColliderScript.buildFloor = true;
		 				}
		 				LevelSettingsScript.roomPartList.Add(theRoomCollider);
		 				
					} else {
						var countAll = 0;
 						while (countAll < basicsInRange.Length){
							if(roomHeightCountNow == 0){
								if(basicsInRange[countAll] != null){
									if(basicsInRange[countAll].transform.name == "RoomPart"){
										if(roomHeightCountNow == 0){
											if (LevelSettingsScript.deadSpots == true){
												basicsInRange[countAll].gameObject.GetComponent<ColliderScript>().buildFloor = true;
											}
										}
									}
									if(basicsInRange[countAll].transform.name == "CorridorPart"){
										var theRoomCollider2 = Instantiate(roomPart, transform.position,Quaternion.identity) as GameObject;
			 							theRoomCollider2.transform.parent = theLevel.transform;
										theRoomCollider2.name = roomPart.name;
																			
										var AttachedColliderScript2 = theRoomCollider2.GetComponent<ColliderScript>();
										AttachedColliderScript2.partType = roomType;
										AttachedColliderScript2.finishingType = finishingType;
										AttachedColliderScript2.direction = roomDirection;
										AttachedColliderScript2.buildFloor = true;
																			
										LevelSettingsScript.roomPartList.Add(theRoomCollider2);
										for (int i = LevelSettingsScript.corridorPartList.Count-1; i > -1 ; i--){
											var basicsInRangeScript = basicsInRange[countAll].GetComponent<ColliderScript>();
											var basicsNumberCollider = basicsInRangeScript.corridorPartCount;
											var corridorColliderListScript = LevelSettingsScript.corridorPartList[i].GetComponent<ColliderScript>();
											var colliderListNumberCollider = corridorColliderListScript.corridorPartCount;
											if(basicsInRangeScript.beginPointer == true){
												AttachedColliderScript2.beginPointer = true;
												AttachedColliderScript2.floorModelSpot =1;
												AttachedColliderScript2.leftWallModelSpot =1;
												AttachedColliderScript2.rightWallModelSpot =1;
												AttachedColliderScript2.frontWallModelSpot =1;
												AttachedColliderScript2.backWallModelSpot =1;
											}
											
											if (basicsNumberCollider == colliderListNumberCollider){
												LevelSettingsScript.corridorPartList.RemoveAt(i);
												DestroyImmediate(basicsInRange[countAll].gameObject);
												break;
											}
										}
									}
								}
							}
							countAll = countAll+1;
						}
					}
				transform.position = transform.position + HeightVector;	
		 		roomHeightCountNow=roomHeightCountNow+1;
		 		}
		 	transform.position = transform.position - HeightVector*roomHeightNow;
			roomHeightCountNow = 0;
			transform.position = transform.position + WidthVector; 	
			roomWidthCountNow=roomWidthCountNow+1;
			}
		transform.position = transform.position - WidthVector*roomWidthNow;
		roomWidthCountNow = 0;
		roomLenghtCountNow=roomLenghtCountNow+1;
		}	
	DestroyImmediate(this.gameObject);	
	}
}