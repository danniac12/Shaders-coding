using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ObjectScript : MonoBehaviour {
	
	public Object prefabModel;
	
	public int ObjectLocation = 3;
	
	//booleans used to measure objects size and positional fit
	private bool EnoughPlace;
	private bool CorrectPosition;
	private string typeOfCheck;
	
	//some variables used in determining fit
	private int tempWallLeftRightTrue;
	private int tempWallForwardBackTrue;
	private bool firstLeft;
	private bool firstRight;
	private bool firstUp;
	private bool firstDown;
	
	//information about the objects rotation and location
	public int PlacementRotation = 0;
	private int RotationCounter = 0;
	public GameObject ParentCollider;
	public string ObjectOrientation;
	
	private int ObjectRotation;
	public bool _rightWallTouching;
	public bool _leftWallTouching;
	public bool _frontWallTouching;
	public bool _backWallTouching;
	public bool _ceilingTouching;
	public bool _floorTouching;
	
	//type of object, room/corridor and primary/secondary
	public bool RoomObject = false;
	public bool CorridorObject = false;
	public bool Primary = false;
	public bool Secundary = false;
	
	//type of object location, wall/ceiling/floor
	public bool RoomWallObject = false;
	public bool RoomCeilingObject = false;
	public bool RoomFloorObject = false;
	public bool CorridorWallObject = false;
	public bool CorridorCeilingObject = false;
	public bool CorridorFloorObject = false;
	
	//size of the object, small object means not sticking out, others mean the size around the object. set by user
	public bool smallObject = true;
	public int LeftSize = 0;
	public int RightSize = 0;
	public int FrontSize = 0;
	public int BackSize = 0;
	public int AboveSize = 0;
	public int BelowSize = 0;
		
	private Vector3 startPosition = new Vector3 (0,0,0);
	private Vector3 cornerStartPosition = new Vector3 (0,0,0);

	//object location to walls, ceilings etc, to place object, used in editor only
	public bool WallTouching;
	public bool WallTouchingOne;
	public bool WallTouchingTwo;
	public bool CornerTouching;
	public bool CornerTouchingOne;
	public bool CornerTouchingTwo;
	public bool WallNotTouching;
	public bool WallNoMatter;
	
	public bool CeilingTouching;
	public bool CeilingTouchingOne;
	public bool CeilingTouchingTwo;
	public bool CeilingNotTouching;
	public bool CeilingNoMatter;
	
	public bool FloorTouching;
	public bool FloorTouchingOne;
	public bool FloorTouchingTwo;
	public bool FloorNotTouching;
	public bool FloorNoMatter;
	
	//values used to position the object in relationship ot walls, celings and floors, stringvalues can be: notMatter, notTouching, touching, one, two, corner, cornerOne, cornerTwo
	public string TouchingWall = "notMatter";
	public string TouchingCeiling = "notMatter";
	public string TouchingFloor = "notMatter";
	
	//list used to store surrounding parts to modify places when the object is placed
	private List<GameObject> TempColliders;
		
	//strings voor checking against strings needed for c#
	private string right = "rightWallModelSpot";
	private string left = "leftWallModelSpot";
	private string front = "frontWallModelSpot";
	private string back = "backWallModelSpot";
	private string ceiling = "ceilingModelSpot";
	private string floor = "floorModelSpot";
	private string sizeCheck = "SizeCheck";
	private string positionCheck = "PositionCheck";
	
	private Vector3 tempRotation;
	private int loopCounter =0;
	private bool objectPlaced = false;
	
	
	
	//======================= start of functions =============//
	
	private Vector3 VectorToRight = new Vector3();
	private Vector3 VectorToFront = new Vector3();
	public bool CheckingPrimary;
	private bool WallObject = false;
	private bool FloorObject = false;
	private bool CeilingObject = false;
	private bool firstLayer = true;
	
	private int _x = 0;
	private int _y = 0;
	private int _z = 0;
	
	
	private bool goingDown = false;
	private int CurrentHeight = 1;
	
	public void ExecuteObject(){
		//check and set what type of object it is
		if(RoomWallObject || CorridorWallObject){
			WallObject = true;
		}
		if(RoomCeilingObject || CorridorCeilingObject){
			CeilingObject = true;
		}
		if(RoomFloorObject || CorridorFloorObject){
			FloorObject = true;
		}
		//
		TempColliders = new List<GameObject>();
		RotationVector();
		EnoughPlace = SizeCheckSequence();
		
		if (EnoughPlace == false){
			if (ObjectOrientation == ceiling || ObjectOrientation == floor){
				if (RotationCounter < 4){
					loopCounter +=1;
					RetryRotation();
					loopCounter -=1;
				} else {
					if(CheckingPrimary){
						ParentCollider.GetComponent<ColliderScript>().GeneratePrimaryObject();
					}else{
						ParentCollider.GetComponent<ColliderScript>().GenerateSecundaryObject();
					}
				}
			} else {
				if(CheckingPrimary){
					ParentCollider.GetComponent<ColliderScript>().GeneratePrimaryObject();
				}else{
					ParentCollider.GetComponent<ColliderScript>().GenerateSecundaryObject();
				}
			}
		} else {
			CorrectPosition = PrimePositionCheck ();
		}
		
		if (CorrectPosition == true){
			if (objectPlaced == false){
				objectPlaced = true;
				ObjectPlacement();
			}
		} else {
			if (ObjectOrientation == ceiling || ObjectOrientation == floor){
				if (RotationCounter < 4){
					loopCounter +=1;
					RetryRotation();
					loopCounter -=1;
				} else {
					if(CheckingPrimary){
						ParentCollider.GetComponent<ColliderScript>().GeneratePrimaryObject();
					}else{
						ParentCollider.GetComponent<ColliderScript>().GenerateSecundaryObject();
					}
				}
			} else {
				if(CheckingPrimary){
					ParentCollider.GetComponent<ColliderScript>().GeneratePrimaryObject();
				}else{
					ParentCollider.GetComponent<ColliderScript>().GenerateSecundaryObject();
				}
			}
		}
			
		if (loopCounter <= 0){
			if (CorrectPosition == false || EnoughPlace == false){
			DestroyImmediate(this.transform.gameObject);
			}
		}
	}
	
	void RotationVector(){
		if (PlacementRotation == 0){
			VectorToRight = new Vector3(5,0,0);
			VectorToFront = new Vector3(0,0,5);
		}else if (PlacementRotation == -180){
			VectorToRight = new Vector3(-5,0,0);
			VectorToFront = new Vector3(0,0,-5);
		}else if (PlacementRotation == 90){
			VectorToRight = new Vector3(0,0,-5);
			VectorToFront = new Vector3(5,0,0);	
		} else if (PlacementRotation == 270){
			VectorToRight = new Vector3(0,0,5);
			VectorToFront = new Vector3(-5,0,0);
		}
	}
	bool SizeCheckSequence (){
		typeOfCheck = "SizeCheck";
		var parentColliderScript = ParentCollider.GetComponent<ColliderScript>();
		
		_x = 0;
		_z = 0;
		_y = 0;
		
		
		//check if there is any place no begin with
		if( smallObject == false ){
			if (parentColliderScript.leftWallModelSpot == 1){
				return false;
			}
			if (parentColliderScript.rightWallModelSpot == 1){
				return false;
			}
			if (parentColliderScript.frontWallModelSpot == 1){
				return false;
			}
			if (parentColliderScript.backWallModelSpot == 1){
				return false;
			}
			if (parentColliderScript.ceilingModelSpot == 1){
				return false;
			}
			if (parentColliderScript.floorModelSpot == 1){
				return false;
			}
		}
		startPosition = ParentCollider.transform.position;
		//checking all locations around the objects centre (4x)
		var tempEnoughPlace = true;
		
		//wallobject sequence
		if(WallObject){
			firstLayer = true;
			goingDown = false;
			if (RightSize > 0){
				_x = 1;
				_z = 0;
				_y = 0;
				tempEnoughPlace = CheckRight(startPosition);
			}
			if (!tempEnoughPlace)return false;
			if (AboveSize > 0){
				_x = 0;
				_z = 0;
				_y = 1;
				tempEnoughPlace = CheckUpDown(startPosition);
			}
			if (!tempEnoughPlace)return false;
			firstLayer = false;
			if (FrontSize > 0){
				_x = 0;
				_z = 1;
				_y = 0;
				tempEnoughPlace = CheckFront(startPosition);
			}	
		}
		//floorobject sequence
		if(FloorObject){
			firstLayer = true;
			goingDown = false;
			if (RightSize > 0){
				tempEnoughPlace = CheckRight(startPosition);
			}
			if (!tempEnoughPlace)return false;
			if (FrontSize > 0){
				tempEnoughPlace = CheckFront(startPosition);
			}	
			if (!tempEnoughPlace)return false;
			firstLayer = false;
			if (AboveSize > 0){
				tempEnoughPlace = CheckUpDown(startPosition);
			}
		}
		//ceilingobject sequence
		if(CeilingObject){
			firstLayer = true;
			goingDown = true;
			if (RightSize > 0){
				tempEnoughPlace = CheckRight(startPosition);
			}
			if (!tempEnoughPlace)return false;
			if (FrontSize > 0){
				tempEnoughPlace = CheckFront(startPosition);
			}	
			if (!tempEnoughPlace)return false;
			firstLayer = false;
			if (BelowSize > 0){
				AboveSize = BelowSize;
				tempEnoughPlace = CheckUpDown(startPosition);
			}
		}
		
		//final check
		if (tempEnoughPlace == true){
			return true;
		} else {
			return false;
		}
	}
	void RetryRotation (){
		RotationCounter +=1;
		if (PlacementRotation == 0){
			PlacementRotation = -180;
			tempRotation = new Vector3(0,-180,0);
			this.transform.localEulerAngles = tempRotation;
			
		} else if(PlacementRotation == -180){
			PlacementRotation = 90;
			tempRotation = new Vector3(0,90,0);
			this.transform.localEulerAngles = tempRotation;
			
		} else if(PlacementRotation == 90){
			PlacementRotation = 270;
			tempRotation = new Vector3(0,270,0);
			this.transform.localEulerAngles = tempRotation;
			
		} else if(PlacementRotation == 270){
			PlacementRotation = 0;
			tempRotation = new Vector3(0,0,0);
			this.transform.localEulerAngles = tempRotation;
			
		}
		ExecuteObject();
	}
	
	bool SizeCheck (GameObject tempCollider){
		//checking if a bigger object is fully supported by the surface its placed on
		var tempColliderScript = tempCollider.GetComponent<ColliderScript>();
		
		if (tempColliderScript.leftWallModelSpot == 1){
			return false;
		}
		if (tempColliderScript.rightWallModelSpot == 1){
			return false;
		}
		if (tempColliderScript.frontWallModelSpot == 1){
			return false;
		}
		if (tempColliderScript.backWallModelSpot == 1){
			return false;
		}
		if (tempColliderScript.ceilingModelSpot == 1){
			return false;
		}
		if (tempColliderScript.floorModelSpot == 1){
			return false;
		}
		if(WallObject && firstLayer){
			if (ObjectOrientation == right){
				if (tempColliderScript.rightWallModelSpot != 2){
					return false;
				}
			}else if (ObjectOrientation == left){
				if (tempColliderScript.leftWallModelSpot != 2){
					return false;
				}
			}else if (ObjectOrientation == front){
				if (tempColliderScript.frontWallModelSpot != 2){
					return false;
				}
			}else if (ObjectOrientation == back){
				if (tempColliderScript.backWallModelSpot != 2){
					return false;
				}
			}
		}
		if(WallObject && firstLayer == false){
			if (ObjectOrientation == right){
				if (tempColliderScript.rightWallModelSpot == 2){
					return false;
				}
			}else if (ObjectOrientation == left){
				if (tempColliderScript.leftWallModelSpot == 2){
					return false;
				}
			}else if (ObjectOrientation == front){
				if (tempColliderScript.frontWallModelSpot == 2){
					return false;
				}
			}else if (ObjectOrientation == back){
				if (tempColliderScript.backWallModelSpot == 2){
					return false;
				}
			}
		}
		if (FloorObject && firstLayer){
			if (tempColliderScript.floorModelSpot != 2){
				return false;
			}
		}
		if (FloorObject && firstLayer == false){
			if (tempColliderScript.floorModelSpot == 2){
				return false;
			}
		}
			
		
		if (CeilingObject && firstLayer){
			if (tempColliderScript.ceilingModelSpot != 2){
				return false;
			}
		}
		if (CeilingObject && firstLayer == false){
			if (tempColliderScript.ceilingModelSpot == 2){
				return false;
			}
		}
		return true;
	}
	
	
	// =================| directionchecks |=============
	//check right
	bool CheckRight (Vector3 tempStartPosition){
		
		var positionCounter = 1;
		var colliderFound = 0;
		while ( positionCounter <= RightSize){
			var lookAtLocation = (VectorToRight*positionCounter) +tempStartPosition;
			var basicsInRange = Physics.OverlapSphere(lookAtLocation, 0.5f);
			if (basicsInRange.Length > 0){
				foreach(Collider tempCollider in basicsInRange){
					if(tempCollider.transform.tag == "Collider"){
						colliderFound += 1;
						if (tempCollider.transform.name == ParentCollider.transform.name){
							var tempColliderScript = tempCollider.GetComponent<ColliderScript>();
						
							//different checks, first is for size, does the object fit?
							
							if (typeOfCheck == sizeCheck){
								
								var tempEnoughPlaceCurrentCollider = SizeCheck(tempCollider.transform.gameObject);
								if (tempEnoughPlaceCurrentCollider == false){
									return false;
								}
								if (WallObject == true && TouchingFloor == "touching"){
									
									if (tempColliderScript.buildFloor == false && _y == 0){
										//print ("no floor to support me");
										return false;	
									}
									if (_y > 0 && tempColliderScript.buildFloor == true){
										//print ("floor floating");
										return false;
									}
								}
								
								
								TempColliders.Add(tempCollider.transform.gameObject);
							
							// does the object have a correct position towards walls, ceiling and/or floor
							
							}else if (typeOfCheck == positionCheck){
													
								
								//check left wall
								if (_x == 0){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								//check right wall
								if (_x == RightSize){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								//check front wall
								if (_z == FrontSize){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								//check back wall
								if (_z == 0){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								//check ceiling
								if (_y == 0 && goingDown == true){
									if (tempColliderScript.buildCeiling){
										_ceilingTouching = true;
									}
								}
								if (_y == AboveSize && goingDown == false){
									if (tempColliderScript.buildCeiling){
										_ceilingTouching = true;
									}
								}
								//check floor
								
								if (_y == 0 && goingDown == false){
									if (tempColliderScript.buildFloor){
										_floorTouching = true;
									}
								}
								if (_y == BelowSize && goingDown == true){
									if (tempColliderScript.buildFloor){
										_floorTouching = true;
									}
								}
							}
						} else {
							return false;
						}
					}
				}
			} else {
				return false;
			}
			positionCounter += 1;
			_x += 1;
		}
		if (colliderFound == RightSize){
			return true;
		} else {
			return false;
		}
	}
	
	//check front
	bool CheckFront (Vector3 tempStartPosition){
		
		var positionCounter = 1;
		var colliderFound = 0;
		var tempEnoughPlace = true;
		while ( positionCounter <= FrontSize){
			var lookAtLocation = (VectorToFront*positionCounter) +tempStartPosition;
			var basicsInRange = Physics.OverlapSphere(lookAtLocation, 0.5f);
			if (basicsInRange.Length > 0){
				foreach(Collider tempCollider in basicsInRange){
					if(tempCollider.transform.tag == "Collider"){
						colliderFound +=1;
						if (tempCollider.transform.name == ParentCollider.transform.name){
							var tempColliderScript = tempCollider.GetComponent<ColliderScript>();

							if (typeOfCheck == sizeCheck){
								
								var tempEnoughPlaceCurrentCollider = SizeCheck(tempCollider.transform.gameObject);
								if (tempEnoughPlaceCurrentCollider == false){
									return false;
								}
								
								if (WallObject == true && TouchingFloor == "touching"){
									
									if (tempColliderScript.buildFloor == false && _y == 0){
										//print ("no floor to support me");
										return false;	
									}
									if (_y > 0 && tempColliderScript.buildFloor == true){
										//print ("floor floating");
										return false;
									}
								}
								
								
								TempColliders.Add(tempCollider.transform.gameObject);
								
								if(WallObject){
									_x = 1;
									_y = 0;
									tempEnoughPlace = CheckRight(lookAtLocation);
									if (tempEnoughPlace == false){
										return false;
									}
									_x = 0;
									_y = 1;
									tempEnoughPlace = CheckUpDown(lookAtLocation);
									if (tempEnoughPlace == false){
										return false;
									}
									_y = 0;
								}
								if(FloorObject || CeilingObject){
									tempEnoughPlace = CheckRight(lookAtLocation);	
									if (tempEnoughPlace == false){
										return false;
									}
								}	
							}else if (typeOfCheck == positionCheck){
								//check left wall
								if (_x == 0){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								//check right wall
								if (_x == RightSize){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								//check front wall
								if (_z == FrontSize){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								//check back wall
								if (_z == 0){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								
								//check ceiling
								if (_y == 0 && goingDown == true){
									if (tempColliderScript.buildCeiling){
										_ceilingTouching = true;
									}
								}
								if (_y == AboveSize && goingDown == false){
									if (tempColliderScript.buildCeiling){
										_ceilingTouching = true;
									}
								}
								//check floor
								if (_y == 0 && goingDown == false){
									if (tempColliderScript.buildFloor){
										_floorTouching = true;
									}
								}
								if (_y == BelowSize && goingDown == true){
									if (tempColliderScript.buildFloor){
										_floorTouching = true;
									}
								}
								//direction
								_x = 1;
								tempEnoughPlace = CheckRight(lookAtLocation);
								if (tempEnoughPlace == false){
									return false;
								}
								
							}
						} else {
							return false;
						}
					}
				}
			} else {
				return false;
			}
			positionCounter += 1;
			_z += 1;
		}
		if (colliderFound == FrontSize){
			return true;
		} else {
			return false;
		}
	}
	
	//check up
	bool CheckUpDown (Vector3 tempStartPosition){
		
		var positionCounter = 1;
		var upDownMultiplyer = 1;
		if(goingDown){
			upDownMultiplyer = -1;	
		}
		var colliderFound = 0;
		var tempEnoughPlace = true;
		while (positionCounter <= AboveSize){
			var lookAtLocation = new Vector3(0,(5*positionCounter*upDownMultiplyer),0) +tempStartPosition;
			var basicsInRange = Physics.OverlapSphere(lookAtLocation, 0.5f);
			if (basicsInRange.Length > 0){
				foreach(Collider tempCollider in basicsInRange){
					if(tempCollider.transform.tag == "Collider"){
						colliderFound += 1;
						if (tempCollider.transform.name == ParentCollider.transform.name){
							var tempColliderScript = tempCollider.GetComponent<ColliderScript>();
							if (typeOfCheck == sizeCheck){
								
								//check if there is a floor in the way
								if(tempColliderScript.buildFloor == true){
									
									return false;
								}
								
								var tempEnoughPlaceCurrentCollider = SizeCheck(tempCollider.transform.gameObject);
								if (tempEnoughPlaceCurrentCollider == false){
									return false;
								}
								TempColliders.Add(tempCollider.transform.gameObject);
								
								if(WallObject){
									_x = 1;
									tempEnoughPlace = CheckRight(lookAtLocation);	
									if (tempEnoughPlace == false){
										return false;
									}
								}
								if(FloorObject || CeilingObject){
									tempEnoughPlace = CheckRight(lookAtLocation);	
									if (tempEnoughPlace == false){
										return false;
									}
									tempEnoughPlace = CheckFront(lookAtLocation);
									if (tempEnoughPlace == false){
										return false;
									}
								}
							}else if (typeOfCheck == positionCheck){
								//check left wall
								if (_x == 0){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								//check right wall
								if (_x == RightSize){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								//check front wall
								if (_z == FrontSize){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								//check back wall
								if (_z == 0){
									if (tempColliderScript.LeftWall){
										_leftWallTouching = true;
									}
									if (tempColliderScript.RightWall){
										_rightWallTouching = true;
									}
									if (tempColliderScript.FrontWall){
										_frontWallTouching = true;
									}
									if (tempColliderScript.BackWall){
										_backWallTouching = true;
									}
								}
								
							
								
								//check ceiling
								if (_y == 0 && goingDown == true){
									if (tempColliderScript.buildCeiling){
										_ceilingTouching = true;
									}
								}
								if (_y == AboveSize && goingDown == false){
									if (tempColliderScript.buildCeiling){
										_ceilingTouching = true;
									}
								}
								
								//check floor
								if (_y == 0 && goingDown == false){
									if (tempColliderScript.buildFloor){
										_floorTouching = true;
									}
								}
								if (_y == BelowSize && goingDown == true){
									if (tempColliderScript.buildFloor){
										_floorTouching = true;
									}
								}
								//direction
								_x = 1;
								tempEnoughPlace = CheckRight(lookAtLocation);
								if (tempEnoughPlace == false){
									return false;
								}
								_x = 0;
								_z = 1;
								tempEnoughPlace = CheckFront(lookAtLocation);
								if (tempEnoughPlace == false){
									return false;
								}
							}
						} else {
							return false;
						}
					}
				}
			} else {
				return false;
			}
			positionCounter += 1;
			CurrentHeight += 1;
			
			_y +=1;
		}
		if (colliderFound == AboveSize){
			return true;
		} else {
			return false;
		}
	}

	
	bool PrimePositionCheck (){
		
		var parentColliderScript = ParentCollider.GetComponent<ColliderScript>();
		
		_rightWallTouching = false;
		_leftWallTouching = false;
		_frontWallTouching = false;
		_backWallTouching = false;
		_ceilingTouching = false;
		_floorTouching = false;
		
		
		//checking a little further out to check for placement towards walls, ceiling and floor
		if (TouchingWall == "one" || TouchingWall == "cornerOne"){
			RightSize = RightSize +1;
			FrontSize = FrontSize +1;
			LeftSize = LeftSize +1;
			BackSize = BackSize +1;
		}else if (TouchingWall == "two"|| TouchingWall == "cornerTwo"){
			RightSize = RightSize +2;
			FrontSize = FrontSize +2;
			LeftSize = LeftSize +2;
			BackSize = BackSize +2;
		}
		if (TouchingCeiling == "one"){
			AboveSize = AboveSize +1;
		}
		if (TouchingCeiling == "two"){
			AboveSize = AboveSize +2;
		}
		if (TouchingFloor == "one"){
			BelowSize = BelowSize +1;
		}
		if (TouchingFloor == "two"){
			BelowSize = BelowSize +2;
		}
		
		//checking original part		
		if (TouchingWall == "notTouching"){
			if (parentColliderScript.RightWall || parentColliderScript.LeftWall || parentColliderScript.FrontWall || parentColliderScript.BackWall){
				return false;
			}
		}
		if (TouchingCeiling == "notTouching"){
			if (parentColliderScript.buildCeiling){
				return false;
			}
		}
		if (TouchingFloor == "notTouching"){
			if (parentColliderScript.buildFloor){
				return false;
			}
		}
		if (TouchingWall == "touching" || TouchingWall == "corner"){
			
			if (RightSize < 1){
				if (parentColliderScript.RightWall){
					_rightWallTouching = true;
				}
			}
			if (LeftSize < 1){
				if (parentColliderScript.LeftWall){
					_leftWallTouching = true;
				}
			}
			if (FrontSize < 1){
				if (parentColliderScript.FrontWall){
					_frontWallTouching = true;
				}
			}
			if (BackSize < 1){
				if (parentColliderScript.BackWall){
					_backWallTouching = true;
				}
			}
		}
		if (AboveSize < 1){
			if (parentColliderScript.buildCeiling){
				_ceilingTouching = true;
			}
			
			if (TouchingCeiling == "touching"){
				if (!parentColliderScript.buildCeiling){
					return false;
				} 
			}
		}
		if (BelowSize < 1){
			if (parentColliderScript.buildFloor){
				_floorTouching = true;
			}
			if (TouchingFloor == "touching"){	
				if (!parentColliderScript.buildFloor){
					return false;
				} 
			}
		}
		
		var position = PositionCheckSequence();
		
		if (position){
			return true;	
		}else{
			return false;	
		}
	}
		
	bool PositionCheckSequence(){
		
		typeOfCheck = "PositionCheck";
		startPosition = ParentCollider.transform.position;
		var correctPosition = true;
		
		_x = 0;
		_y = 0;
		_z = 0;
		
		//sequence
		if(WallObject){
			goingDown = false;
			CurrentHeight = 1;
			
			
			if (RightSize > 0){
				_x = 1;
				correctPosition = CheckRight(startPosition);
			}
			if (!correctPosition)return false;
			if (FrontSize > 0){
				_x = 0;
				_z = 1;
				correctPosition = CheckFront(startPosition);
			}	
			if (!correctPosition)return false;
			
			
			CurrentHeight = 1;
			if (AboveSize > 0){
				_x = 0;
				_z = 0;
				_y = 1;
				correctPosition = CheckUpDown(startPosition);
			}
			
			CurrentHeight = 1;
			if (BelowSize > 0){
				
				goingDown = true;
				
				var oldAbove = AboveSize;
				AboveSize = BelowSize;
				_x = 0;
				_z = 0;
				_y = 1;
				correctPosition = CheckUpDown(startPosition);
				AboveSize = oldAbove;
			}
		}
		if(FloorObject){
			goingDown = false;
			
			cornerStartPosition = startPosition - (LeftSize*VectorToRight)- (BackSize*VectorToFront);
			RightSize = RightSize + LeftSize;
			FrontSize = FrontSize + BackSize;
			
			if(RightSize > 0){
				_x = 1;
				correctPosition = CheckRight(cornerStartPosition);
			}
			if (!correctPosition)return false;
			if(FrontSize > 0){
				_z = 1;
				_x = 0;
				correctPosition = CheckFront(cornerStartPosition);
			}
			if (!correctPosition)return false;
			CurrentHeight = 1;
			if (AboveSize > 0){
				_y = 1;
				_z = 0;
				_x = 0;
				correctPosition = CheckUpDown(cornerStartPosition);
			}
			
			
		}
		if(CeilingObject){
			goingDown = true;
			
			cornerStartPosition = startPosition - (LeftSize*VectorToRight)- (BackSize*VectorToFront);
			RightSize = RightSize + LeftSize;
			FrontSize = FrontSize + BackSize;
			
			if(RightSize > 0){
				_x = 1;
				correctPosition = CheckRight(cornerStartPosition);
			}
			if (!correctPosition)return false;
			if(FrontSize > 0){
				_z = 1;
				_x = 0;
				correctPosition = CheckFront(cornerStartPosition);
			}
			if (!correctPosition)return false;
			CurrentHeight = 1;
			if (BelowSize > 0){
				var oldAbove = AboveSize;
				AboveSize = BelowSize;
				_y = 1;
				_z = 0;
				_x = 0;
				correctPosition = CheckUpDown(cornerStartPosition);
				AboveSize = oldAbove;
			}	
		}
		
		bool WallCheck = false;
		bool CeilingCheck = false;
		bool FloorCheck = false;
		
		if (correctPosition == true){
			
			
			//checking values obtained through the positioncheck
			
			//walls checks
			if (TouchingWall == "touching" || TouchingWall == "one" || TouchingWall == "two"){
				if (_rightWallTouching == true || _leftWallTouching == true || _frontWallTouching == true || _backWallTouching == true){
					WallCheck = true;	
				}
			} else if (TouchingWall == "corner" ||  TouchingWall == "cornerOne" || TouchingWall == "cornerTwo"){
				if (_rightWallTouching == true && _leftWallTouching == true){
					return false;
				}
				if (_frontWallTouching == true && _backWallTouching == true){
					return false;
				}
				if (_rightWallTouching == true && _frontWallTouching == true){
					WallCheck = true;	
				}
				if (_rightWallTouching == true && _backWallTouching == true){
					WallCheck = true;	
				}
				if (_leftWallTouching == true && _frontWallTouching == true){
					WallCheck = true;	
				}
				if (_leftWallTouching == true && _backWallTouching == true){
					WallCheck = true;	
				}
				//rotating small object to face into the room
				if( smallObject == true ){
					if (_rightWallTouching == true && _frontWallTouching == true){
						ObjectRotation = 135 - PlacementRotation ;
					}
					if (_leftWallTouching == true && _frontWallTouching == true){
						ObjectRotation = -135 - PlacementRotation;
					}
					if (_rightWallTouching == true && _backWallTouching == true){
						ObjectRotation = 45 - PlacementRotation;
					}
					if (_leftWallTouching == true && _backWallTouching == true){
						ObjectRotation = -45 - PlacementRotation;
					}
				}
			} else if (TouchingWall == "notTouching"){
				if (_rightWallTouching == false && _leftWallTouching == false && _frontWallTouching == false && _backWallTouching == false){
					
					WallCheck = true;	
					
				}	
			} else {
				WallCheck = true;
			}
			
			//Ceilingcheck
			if (TouchingCeiling == "touching" || TouchingCeiling == "one" || TouchingCeiling == "two"){
				if (_ceilingTouching){
					CeilingCheck = true;
				}
			} else if (TouchingCeiling == "notTouching"){
				if (_ceilingTouching == false){
					CeilingCheck = true;	
				}
			} else {
				CeilingCheck = true;
			}
						
			//Floorcheck
			if (TouchingFloor == "touching" || TouchingFloor == "one" || TouchingFloor == "two"){
				if (_floorTouching){
					FloorCheck = true;
				}
			} else if (TouchingFloor == "notTouching"){
				if (_floorTouching == false){
					FloorCheck = true;	
				}
			} else {
				FloorCheck = true;
			}
			
		} else {
			return false;	
		}
		
		//final check
		if (WallCheck == true && CeilingCheck == true && FloorCheck == true ){
			return true;
		}else {
			return false;	
		}
	}
	void ObjectPlacement (){
		if (TouchingWall == "corner" || TouchingWall == "cornerOne" || TouchingWall == "cornerTwo" ){
			this.transform.localEulerAngles = new Vector3(this.transform.localRotation.x, this.transform.localRotation.y + ObjectRotation, this.transform.localRotation.z);
		}
		var parentColliderScript = ParentCollider.GetComponent<ColliderScript>();
		if (ObjectOrientation == right){
			parentColliderScript.rightWallModelSpot = 1;
		}else if (ObjectOrientation == left){
			parentColliderScript.leftWallModelSpot = 1;
		}else if (ObjectOrientation == front){
			parentColliderScript.frontWallModelSpot = 1;
		}else if (ObjectOrientation == back){
			parentColliderScript.backWallModelSpot = 1;
		}else if (ObjectOrientation == ceiling){
			parentColliderScript.ceilingModelSpot = 1;
		}else if (ObjectOrientation == floor){
			parentColliderScript.floorModelSpot = 1;
		}
		
		if (smallObject == false){
			//if (parentColliderScript.rightWallModelSpot > 0){
				parentColliderScript.rightWallModelSpot = 1;
			//}
			//if (parentColliderScript.leftWallModelSpot > 0){
				parentColliderScript.leftWallModelSpot = 1;
			//}
			//if (parentColliderScript.frontWallModelSpot > 0){
				parentColliderScript.frontWallModelSpot = 1;
			//}
			//if (parentColliderScript.backWallModelSpot > 0){
				parentColliderScript.backWallModelSpot = 1;
			//}
			//if (parentColliderScript.ceilingModelSpot > 0){
				parentColliderScript.ceilingModelSpot = 1;
			//}
			//if (parentColliderScript.floorModelSpot > 0){
				parentColliderScript.floorModelSpot = 1;
			//}
			if (TempColliders.Count > 0){
				foreach(GameObject TempCollider in TempColliders){
					var tempColliderScript = TempCollider.GetComponent<ColliderScript>();
					
					//if (tempColliderScript.rightWallModelSpot > 0){
						tempColliderScript.rightWallModelSpot = 1;
					//}
					//if (tempColliderScript.leftWallModelSpot > 0){
						tempColliderScript.leftWallModelSpot = 1;
					//}
					//if (tempColliderScript.frontWallModelSpot > 0){
						tempColliderScript.frontWallModelSpot = 1;
					//}
					//if (tempColliderScript.backWallModelSpot > 0){
						tempColliderScript.backWallModelSpot = 1;
					//}
					//if (tempColliderScript.ceilingModelSpot > 0){
						tempColliderScript.ceilingModelSpot = 1;
					//}
					//if (tempColliderScript.floorModelSpot > 0){
						tempColliderScript.floorModelSpot = 1;
					//}
				}
			}
		}
		
		var theObject = Instantiate(prefabModel, this.transform.position,  this.transform.rotation) as GameObject;
		theObject.transform.parent = ParentCollider.transform;
		theObject.transform.name = prefabModel.name;
	
		var LevelSettingsScript = GameObject.FindWithTag("DungeonBuilder").GetComponent<DungeonGenerator>();
		LevelSettingsScript.objectsBuildCounter += 1;
	}
	
	//void PositionCheck{
	
	
	public void DestroyAllTraces(){
		DestroyImmediate(this.transform.gameObject);	
	}
}
