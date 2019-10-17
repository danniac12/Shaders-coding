/// Easy Ledge Climb Character System / 3D Platformer Kit
/// PlayerController.cs
///
/// As long as the player has a CharacterController or Rigidbody component, this script allows the player to:
/// 1. Move and rotate (Movement).
/// 2. Slide down slopes (Movement).
/// 3. Perform any amount of jumps with different heights and animations (Jumping).
/// 4. Perform a double jump (Jumping).
/// 5. Wall jump (Wall Jumping).
///
/// NOTE: *You should always set a layer for your player so that you can disable collisions with that layer (by unchecking it in the script's Collision Layers).
///	If you do not, the raycasts and linecasts will collide with the player himself and keep the script from working properly!*
///
/// (C) 2015-2016 Grant Marrs

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public Transform playerCamera; //the camera set to follow the player
	public float gravity = 20.00f; //the amount of downward force, or "gravity," that is constantly being applied to the player
	public float slopeLimit = 25.00f; //the maximum angle of a slope you can stand on without sliding down
	
	//Grounded
	[System.Serializable]
	public class Grounded {
		public bool showGroundDetectionRays; //shows the rays that detect whether the player is grounded or not
		public float maxGroundedHeight = 0.2f; //the maximum height of the ground the ground detectors can hit to be considered grounded
		public float maxGroundedRadius = 0.2f; //the maximum radius of the area ground detectors can hit to be considered grounded
		public float maxGroundedDistance = 0.2f; //the maximum distance you can be from the ground to be considered grounded
		public bool currentlyGrounded; //determines if player is currently grounded/on the ground
	}
	
	//Movement
	[System.Serializable]
	public class Movement {
		public float forwardSpeed = 6.0f; //player's speed when running forward
		public float sideSpeed = 4.0f; //player's speed when running sideways
		public float backSpeed = 5.0f; //player's speed when running backwards
		public float midAirMovementSpeedMultiple = 1.1f; //player's movement speed in mid-air (multiplied by move speed)
		public float acceleration = 50; //how fast the player will reach their maximum speed
		public float movementFriction = 0; //the amount of friction applied to the player's movement
		public float rotationSpeed = 8; //player's rotation speed
		public float midAirRotationSpeedMultiple = 1; //player's rotation speed in mid-air (multiplied by rotationSpeed)
		public float slopeSlideSpeed = 1; //how quickly you slide down slopes
		public float slideFriction = 4; //the amount of friction applied to the player from sliding down a slope
	}
	
	//Jumping
	[System.Serializable]
	public class Jumping {
		public float [] numberAndHeightOfJumps = {6, 8, 12}; //the number of jumps the player can do and the height of the jumps (the elements)
		public float timeLimitBetweenJumps = 1; //the amount of time you have between each jump to continue the jump combo
		public bool allowJumpWhenSlidingFacingUphill = false; //determines whether or not you are allowed to jump when you are facing uphill and sliding down a slope
		public bool allowJumpWhenSlidingFacingDownhill = true; //determines whether or not you are allowed to jump when you are facing downhill and sliding down a slope
		public bool doNotIncreaseJumpNumberWhenSliding = true; //only allows the player to perform their first jump when sliding down a slope
		public GameObject jumpLandingEffect; //optional dust effect to appear after landing jump
		public bool allowDoubleJump = true; //determines whether or not you are allowed to double jump
		public bool doubleJumpPerformableOutOfWallJump = true; //(if allowDoubleJump is true) determines whether or not the player can perform their double jump if they are in mid-air as a result of wall jumping
		public bool doubleJumpPerformableIfInMidAirInGeneral = true; //(if allowDoubleJump is true) determines whether or not the player can perform their double jump simply because they are in mid-air (instead of having to be in mid-air as a result of jumping)
		public float doubleJumpHeight = 6; //height of double jump
		public GameObject doubleJumpEffect; //optional effect to appear when performing a double jump
		public float maxFallingSpeed = 90; //the maximum speed you can fall
	}
	
	//Wall Jumping
	[System.Serializable]
	public class WallJumping {
		public bool allowWallJumping = true; //determines whether or not you are allowed to wall jump
		public float minimumWallAngle = 80; //the minimum angle a wall can be to wall jump off of it
		public float wallJumpDistance = 6; //distance of wall jump
		public float wallJumpHeight = 10; //height of wall jump
		public float wallJumpDecelerationRate = 2; //how quickly the momentum from the wall jump stops
		public float overallMovementSpeed = 2; //player's movement speed in mid-air
		public float forwardMovementSpeedMultiple = 0.85f; //player's speed when moving forward in mid air (multiplied by overallMovementSpeed)
		public float sideMovementSpeedMultiple = 0.85f; //player's speed when moving sideways in mid air (multiplied by overallMovementSpeed)
		public float backMovementSpeedMultiple = 0.75f; //player's speed when moving backwards in mid air (multiplied by overallMovementSpeed)
		public float rotationSpeedMultiple = 0; //player's rotation speed in mid-air (multiplied by rotationSpeed)
		public float distanceToKeepFromWallWhenOnWall = 1; //the distance the player keeps from the wall he is currently stuck to
		public bool useWallJumpTimeLimit = true; //allows the use of a time limit to wall jump when on walls
		public float wallJumpTimeLimit = 2; //the amount of time you can stay on a wall before falling
		public bool slideDownWalls = true; //allows player to slide down if on a wall
		public float slideDownSpeed = 8; //the speed at which the player slides down walls
		public float rotationToWallSpeed = 6; //how quickly the player rotates onto a wall for a wall jump
		public float inputPercentageNeededToWallJump = 50; //the amount of input needed to be applied to the joystick or key in order to stick to a wall for a wall jump
		public bool showWallJumpDetectors = false; //determines whether to show or hide the detectors that allow wall jumping
		public float spaceOnWallNeededToWallJumpUpAmount = 0.0f; //moves the rays that detect the amount of open space on a wall up and down
		public float spaceOnWallNeededToWallJumpHeight = 0.0f; //changes the height of the rays that detect the amount of open space on a wall
		public float spaceOnWallNeededToWallJumpLength = 0.0f; //changes the length of the rays that detect the amount of open space on a wall
		public float spaceOnWallNeededToWallJumpWidth = 0.0f; //changes the width of the rays that detect the amount of open space on a wall
		public float spaceBelowNeededToWallJump = 0.0f; //changes the minimum distance from the ground you must be in order to wall jump
	}
	
	public Grounded grounded = new Grounded(); //variables that detect whether the player is grounded or not
	public Movement movement = new Movement(); //variables that control the player's movement
	public Jumping jumping = new Jumping(); //variables that control the player's jumping
	public WallJumping wallJumping = new WallJumping(); //variables that control the player's jumping
	
	//Grounded variables without class name
	private Vector3 maxGroundedHeight2;
	private float maxGroundedRadius2;
	private float maxGroundedDistance2;
	private Vector3 maxGroundedDistanceDown;
	
	//Movement variables without class name
	private float forwardSpeed2;
	private float sideSpeed2;
	private float backSpeed2;
	private float midAirMovementSpeedMultiple2;
	private float acceleration2;
	private float rotationSpeed2;
	private float slopeSlideSpeed2;
	
	//Jumping variables without class name
	private float [] jumpsToPerform;
	private bool allowDoubleJump2;
	private bool doubleJumpPerformableIfInMidAirInGeneral2;
	private float doubleJumpHeight2;
	private float timeLimitBetweenJumps2;
	private float maxFallingSpeed2;
	private GameObject jumpLandingEffect2;
	private GameObject doubleJumpEffect2;
	
	//WallJumping variables without class name
	private Vector3 spaceOnWallNeededToWallJumpUpAmount2;
	private float spaceOnWallNeededToWallJumpHeight2;
	private float spaceOnWallNeededToWallJumpLength2;
	private float spaceOnWallNeededToWallJumpWidth2;
	private Vector3 spaceBelowNeededToWallJump2;
	
	//private movement variables
	private Vector3 moveDirection; //the direction that the player moves in
	private float moveSpeed; //the current speed of the player
	private float moveSpeedAndFriction; //the current speed of the player with friction applied
	private float accelerationRate; //how fast the player is accelerating
	private float deceleration = 1; //how fast the player will reach the speed of 0
	private float decelerationRate; //how fast the player is decelerating
	private float h; //the absolute value of the "Horizontal" axis minus the absolute value of the "Vertical" axis
	private float v; //the absolute value of the "Vertical" axis minus the absolute value of the "Horizontal" axis
	private bool inBetweenSlidableSurfaces; //determines whether you are in between two slidable surfaces or not
	private bool uphill; //determines whether you are going uphill on a slope or not
	private bool angHit; //determines whether or not a raycast going straight down (with a distance of 1) is hitting
	private float collisionSlopeAngle; //the angle of the surface you are currently standing on
	private float raycastSlopeAngle; //the angle of the surface being raycasted on
	private float slidingAngle; //the angle of the last slidable surface you collided with or are currently colliding with
	private bool slidePossible; //determines whether you can slide down a slope or not
	private bool sliding; //determines whether you are sliding down a slope or not
	private float slideSpeed = 6; //player's downward speed on slopes
	private Vector3 slidingVector; //the normal of the object you are colliding with
	private Vector3 slideMovement; //Vector3 that slerps to the normal of the object you are colliding with (slidingVector)
	
	//private jumping variables
	private int currentJumpNumber; //the number of the most current jump performed
	private int totalJumpNumber; //the total amount of jumps set
	private float airSpeed; //player's movement speed in mid-air
	private float jumpTimer; //time since last jump was performed
	private float jumpPerformedTime; //time since last jump was first performed
	private bool inMidAirFromJump; //player is in mid-air as a result of jumping
	private bool jumpEnabled; //enables jumping while the script is enabled and diables jumping when the script is disabled
	private bool jumpPossible; //determines whether a jump is possible or not
	private bool doubleJumpPossible = true; //determines whether a double jump is possible or not
	private bool jumpPressed; //"Jump" button was pressed
	private float jumpPressedTimer; //time since "Jump" button was last pressed
	private bool jumpPerformed; //determines whether a jump was just performed
	private bool headHit; //determines if player's head hit the ceiling
	private float yPos; //player's position on the y-axis
	private float yVel; //player's y velocity
	private Vector3 pos; //position and collider bounds of the player
	private Vector3 contactPoint; //the specific point where the player and another object are colliding
	private float noCollisionTimer; //time since last collision
	
	//private wall jumping variables
	private bool currentlyOnWall;
	private bool onWallLastUpdate;
	private bool middleWallJumpable;
	private bool leftWallJumpable;
	private bool rightWallJumpable;
	private Vector3 wallNormal;
	private Vector3 wallHitPoint;
	private float forwardDir;
	private float rightDir;
	private float originalForwardDir;
	private float originalRightDir;
	private bool jumpedOffWallForWallJump;
	private Vector3 originalWallJumpDirection;
	private Vector3 wallJumpDirection;
	private float angleBetweenPlayerAndWall;
	private bool wallBackHit;
	private float distFromWall;
	private float firstDistFromWall;
	private bool inMidAirFromWallJump;
	private float wallJumpTimer;
	private float slideDownSpeed2;
	private bool onWallAnimation;
	private bool rbUsesGravity;
	private bool canChangeRbGravity;
	
	private RaycastHit hit = new RaycastHit(); //information on the hit point of a raycast
	private Animator animator; //the "Animator" component of the script holder
	public LayerMask collisionLayers = -1; //the layers that the detectors (raycasts/linecasts) will collide with
	
	// Use this for initialization
	void Start () {
		StartUp();
	}
	
	void StartUp () {
		//resetting script to make sure that everything initializes
		enabled = false;
		enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentlyOnWall){
			jumpedOffWallForWallJump = false;
		}
		else if (inMidAirFromWallJump && noCollisionTimer >= 5){
			jumpedOffWallForWallJump = true;
		}
		if (jumpedOffWallForWallJump && noCollisionTimer < 5 && inMidAirFromWallJump){
			transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
			inMidAirFromWallJump = false;
		}
		
		//storing values to variables
		//Grounded variables
		maxGroundedHeight2 = transform.up * grounded.maxGroundedHeight;
		maxGroundedRadius2 = grounded.maxGroundedRadius - 0.0075f;
		maxGroundedDistance2 = grounded.maxGroundedDistance;
		maxGroundedDistanceDown = Vector3.down*grounded.maxGroundedDistance;
		//Movement variables
		forwardSpeed2 = movement.forwardSpeed;
		sideSpeed2 = movement.sideSpeed;
		backSpeed2 = movement.backSpeed;
		if (!inMidAirFromWallJump){
			midAirMovementSpeedMultiple2 = movement.midAirMovementSpeedMultiple;
		}
		//wall jumps have their own mid-air speed and dampening, so during a wall jump, we set midAirMovementSpeedMultiple2 to 0 to avoid affecting it
		else {
			midAirMovementSpeedMultiple2 = 0;
		}
		acceleration2 = movement.acceleration;
		slopeSlideSpeed2 = movement.slopeSlideSpeed;
		//Jumping variables
		jumpsToPerform = jumping.numberAndHeightOfJumps;
		timeLimitBetweenJumps2 = jumping.timeLimitBetweenJumps;
		jumpLandingEffect2 = jumping.jumpLandingEffect;
		allowDoubleJump2 = jumping.allowDoubleJump;
		doubleJumpPerformableIfInMidAirInGeneral2 = jumping.doubleJumpPerformableIfInMidAirInGeneral;
		doubleJumpHeight2 = jumping.doubleJumpHeight;
		doubleJumpEffect2 = jumping.doubleJumpEffect;
		maxFallingSpeed2 = jumping.maxFallingSpeed;
		//Wall Jumping variables
		spaceOnWallNeededToWallJumpUpAmount2 = transform.up * wallJumping.spaceOnWallNeededToWallJumpUpAmount;
		spaceOnWallNeededToWallJumpHeight2 = wallJumping.spaceOnWallNeededToWallJumpHeight;
		spaceOnWallNeededToWallJumpLength2 = wallJumping.spaceOnWallNeededToWallJumpLength - 0.5f;
		spaceOnWallNeededToWallJumpWidth2 = wallJumping.spaceOnWallNeededToWallJumpWidth;
		spaceBelowNeededToWallJump2 = transform.up * wallJumping.spaceBelowNeededToWallJump;
		//

		//getting position and collider information for raycasts
		pos = transform.position;
        pos.y = GetComponent<Collider>().bounds.min.y + 0.1f;
		
		
		if (wallJumping.showWallJumpDetectors){
			//middle
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)), Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, Color.yellow);
			//left
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			//right
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			Debug.DrawLine(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, Color.yellow);
			//below
			Debug.DrawLine(transform.position, transform.position - transform.up*0.3f - spaceBelowNeededToWallJump2, Color.cyan);
		}
		
		
		//setting the player's "Animator" component (if player has one) to animator
		if (GetComponent<Animator>()){
			animator = GetComponent<Animator>();
		}
		
		if (GetComponent<CharacterController>() && GetComponent<CharacterController>().enabled){
			//checking to see if player's head hit the ceiling
			if (GetComponent<CharacterController>().velocity.y < 0.5 && moveDirection.y > 0.5 && !grounded.currentlyGrounded){
				headHit = true;
			}
			else {
				headHit = false;
			}
			yVel = GetComponent<CharacterController>().velocity.y;
		}
		else if (GetComponent<Rigidbody>()){
			//checking to see if player's head hit the ceiling
			if (yPos == transform.position.y && GetComponent<Rigidbody>().velocity.y + 0.1f > yVel && !jumpPerformed && noCollisionTimer == 0 && !sliding && moveDirection.y > 0 && !grounded.currentlyGrounded){
				if (collisionSlopeAngle < slopeLimit || collisionSlopeAngle > 91){
					headHit = true;
				}
				else {
					headHit = false;
				}
			}
			else {
				headHit = false;
			}
			yPos = transform.position.y;
			yVel = GetComponent<Rigidbody>().velocity.y;
		}
		
		//if user set jumps, totalJumpNumber equals the number set
		if (jumpsToPerform.Length > 0){
			totalJumpNumber = jumpsToPerform.Length;
		}
		//if user did not set jumps, totalJumpNumber equals 0
		else {
			totalJumpNumber = 0;
		}
		
		//if the "Jump" button was pressed, jumpPressed equals true
		if (Input.GetButtonDown("Jump")){
			jumpPressedTimer = 0.0f;
			jumpPressed = true;
		}
		else{
			jumpPressedTimer += 0.02f;
		}
		
		//wait 0.2 seconds for jumpPressed to become false
		//this allows the player to press the "Jump" button slightly early and still jump once they have landed
		if (jumpPressedTimer > 0.2f){
			jumpPressed = false;
		}

		//jump
		if (grounded.currentlyGrounded){
			if (jumpPressed && jumpPossible && !jumpPerformed && totalJumpNumber > 0 && !onWallLastUpdate && jumpEnabled && (raycastSlopeAngle > slopeLimit && (uphill && jumping.allowJumpWhenSlidingFacingUphill || !uphill && jumping.allowJumpWhenSlidingFacingDownhill || inBetweenSlidableSurfaces) || raycastSlopeAngle <= slopeLimit)){
				Jump();
			}
			doubleJumpPossible = true;
		}
		
		//double jump
		if (Input.GetButtonDown("Jump") && doubleJumpPossible && !grounded.currentlyGrounded && allowDoubleJump2 && jumpEnabled && (doubleJumpPerformableIfInMidAirInGeneral2 || !doubleJumpPerformableIfInMidAirInGeneral2 && inMidAirFromJump) && (jumping.doubleJumpPerformableOutOfWallJump || !inMidAirFromWallJump) && !onWallLastUpdate){
			if (!Physics.Raycast(pos, Vector3.down, out hit, 0.5f, collisionLayers) && moveDirection.y < 0 || !grounded.currentlyGrounded && moveDirection.y >= 0){
				DoubleJump();
				jumpPressed = false;
				doubleJumpPossible = false;
			}
		}
		
		//enabling jumping while the script is enabled
		jumpEnabled = true;
		
		//checking to see if player was on the wall in the last update
		if (currentlyOnWall){
			onWallLastUpdate = true;
		}
		else {
			onWallLastUpdate = false;
		}
		
		//counting how long it has been since last jump was first performed
		if (jumpPerformed){
			jumpPerformedTime += 0.02f;
		}
		else {
			jumpPerformedTime = 0;
		}
		
		//if in mid air as a result of jumping
		if (inMidAirFromJump){
			
			if (grounded.currentlyGrounded){
				
				if (!jumpPerformed){
					//creating the optional dust effect after landing a jump
					if (jumpLandingEffect2 != null){
						Instantiate(jumpLandingEffect2, transform.position + new Vector3(0, 0.05f, 0), jumpLandingEffect2.transform.rotation);
					}
				
					//once player has landed jump, stop jumping animation and return to movement
					if (animator != null && animator.runtimeAnimatorController != null){
						animator.CrossFade("Movement", 0.2f);
					}
				
					//once player has landed jump, set inMidAirFromJump to false
					inMidAirFromJump = false;
				}
				
				if (jumpTimer == 0 && jumpPerformedTime > 0.1f){
					//creating the optional dust effect after landing a jump
					if (jumpLandingEffect2 != null){
						Instantiate(jumpLandingEffect2, transform.position + new Vector3(0, 0.05f, 0), jumpLandingEffect2.transform.rotation);
					}
					jumpPerformed = false;
					inMidAirFromJump = false;
				}
				
			}
			
		}
		
		
		//if player is not in mid-air as a result of jumping, increase the jump timer
		if (!inMidAirFromJump) {
			jumpTimer += 0.02f;
		}
		
		//if the jump timer is greater than the jump time limit, reset current jump number
		if (jumpTimer > timeLimitBetweenJumps2 && timeLimitBetweenJumps2 > 0) {
			currentJumpNumber = totalJumpNumber;
		}

		//set animator's float parameter, "jumpNumber," to currentJumpNumber
		if (animator != null && animator.runtimeAnimatorController != null){
			animator.SetFloat("jumpNumber", currentJumpNumber);
		}

		
		//after jump is performed and jumpPerformed is true, set jumpPerformed to false
		if (jumpPerformed){
			if (!grounded.currentlyGrounded || headHit){
				jumpPerformed = false;
			}
		}
		
	}
	
	void FixedUpdate () {
		
		//increase the noCollisionTimer (if there is a collision, the noCollisionTimer is later set to 0)
		noCollisionTimer++;
		
		//getting the direction to rotate towards
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (directionVector != Vector3.zero) {
			
            //getting the length of the direction vector and normalizing it
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;

            //setting the maximum direction length to 1
            directionLength = Mathf.Min(1, directionLength);

            directionLength *= directionLength;

            //multiply the normalized direction vector by the modified direction length
            directionVector *= directionLength;
			
        }
		
		//checking to see if player (if using a Rigidbody) is using "Use Gravity"
		if (GetComponent<Rigidbody>()){
			if (GetComponent<Rigidbody>().useGravity && !currentlyOnWall){
				rbUsesGravity = true;
			}
			
			if (currentlyOnWall){
				canChangeRbGravity = true;
				GetComponent<Rigidbody>().useGravity = false;
			}
			else if (rbUsesGravity && canChangeRbGravity){
				GetComponent<Rigidbody>().useGravity = true;
				canChangeRbGravity = false;
			}
			
			if (!GetComponent<Rigidbody>().useGravity && !currentlyOnWall){
				rbUsesGravity = false;
			}
		}
		
		//checking to see if wall jumping is possible
		//middle
		if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)), out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  noCollisionTimer < 5 && !grounded.currentlyGrounded && directionVector.magnitude >= wallJumping.inputPercentageNeededToWallJump/100){
			middleWallJumpable = true;
		}
		else {
			middleWallJumpable = false;
		}
		//left
		if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f - (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  noCollisionTimer < 5 && !grounded.currentlyGrounded && directionVector.magnitude >= wallJumping.inputPercentageNeededToWallJump/100){
			leftWallJumpable = true;
		}
		else {
			leftWallJumpable = false;
		}
		//right
		if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f + (transform.right * (spaceOnWallNeededToWallJumpWidth2 + 1))/3, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle
		&&  noCollisionTimer < 5 && !grounded.currentlyGrounded && directionVector.magnitude >= wallJumping.inputPercentageNeededToWallJump/100){
			rightWallJumpable = true;
		}
		else {
			rightWallJumpable = false;
		}
		
		if (!headHit && wallJumping.allowWallJumping && !grounded.currentlyGrounded){
			if ((middleWallJumpable && leftWallJumpable || middleWallJumpable && rightWallJumpable) && !Physics.Linecast(transform.position, transform.position - transform.up*0.3f - spaceBelowNeededToWallJump2, out hit, collisionLayers)){
				if (inMidAirFromWallJump){
					transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
					inMidAirFromWallJump = false;
				}
				wallJumpDirection = Vector3.zero;
				if (!currentlyOnWall){
					wallJumpTimer = 0.0f;
					slideDownSpeed2 = 0;
					if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)), out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle){
						wallNormal = hit.normal;
						wallHitPoint = hit.point + hit.normal*(wallJumping.distanceToKeepFromWallWhenOnWall/3.75f);
					}
					else if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle){
						wallNormal = hit.normal;
						wallHitPoint = hit.point + hit.normal*(wallJumping.distanceToKeepFromWallWhenOnWall/3.75f);
					}
					else if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle){
						wallNormal = hit.normal;
						wallHitPoint = hit.point + hit.normal*(wallJumping.distanceToKeepFromWallWhenOnWall/3.75f);
					}
					else if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle){
						wallNormal = hit.normal;
						wallHitPoint = hit.point + hit.normal*(wallJumping.distanceToKeepFromWallWhenOnWall/3.75f);
					}
					else if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle){
					wallNormal = hit.normal;
					wallHitPoint = hit.point + hit.normal*(wallJumping.distanceToKeepFromWallWhenOnWall/3.75f);
					}
					else if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle){
						wallNormal = hit.normal;
						wallHitPoint = hit.point + hit.normal*(wallJumping.distanceToKeepFromWallWhenOnWall/3.75f);
					}
					else if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, out hit, collisionLayers) && 90 - Mathf.Abs(90 - Vector3.Angle(Vector3.up, hit.normal)) >= wallJumping.minimumWallAngle){
						wallNormal = hit.normal;
						wallHitPoint = hit.point + hit.normal*(wallJumping.distanceToKeepFromWallWhenOnWall/3.75f);
					}
					
					angleBetweenPlayerAndWall = Vector3.Angle(transform.right, hit.normal);
				}
				if (!sliding){
					currentlyOnWall = true;
				}
			}
		}
		
		//getting directions of wall jump
		//new directions (using wallJumpDirection, which is modified by the joystick)
		//forward
		if (Vector3.Dot(transform.forward, wallJumpDirection) + 1 > 0){
			forwardDir = Vector3.Dot(transform.forward, wallJumpDirection) + 1;
		}
		//back
		else if (Mathf.Abs(Vector3.Dot(transform.forward, wallJumpDirection)) != 0){
			forwardDir = 1 / Mathf.Abs(Vector3.Dot(transform.forward, wallJumpDirection));
		}
		//right
		if (Vector3.Dot(transform.right, wallJumpDirection) + 1 > 0){
			rightDir = Vector3.Dot(transform.right, wallJumpDirection) + 1;
		}
		//left
		else if (Mathf.Abs(Vector3.Dot(transform.right, wallJumpDirection)) != 0){
			rightDir = 1 / Mathf.Abs(Vector3.Dot(transform.right, wallJumpDirection));
		}
		
		//original directions (using originalWallJumpDirection, which is not modified by the joystick)
		//original forward
		if (Vector3.Dot(transform.forward, originalWallJumpDirection) + 1 >= 0){
			originalForwardDir = Vector3.Dot(transform.forward, originalWallJumpDirection) + 1;
		}
		//original back
		else if (Mathf.Abs(Vector3.Dot(transform.forward, originalWallJumpDirection)) != 0){
			originalForwardDir = 1 / Mathf.Abs(Vector3.Dot(transform.forward, originalWallJumpDirection));
		}
		//original right
		if (Vector3.Dot(transform.right, originalWallJumpDirection) + 1 >= 0){
			originalRightDir = Vector3.Dot(transform.right, originalWallJumpDirection) + 1;
		}
		//original left
		else if (Mathf.Abs(Vector3.Dot(transform.right, originalWallJumpDirection)) != 0){
			originalRightDir = 1 / Mathf.Abs(Vector3.Dot(transform.right, originalWallJumpDirection));
		}
		
		//checking to make sure the player did not slide off of the wall
		if (Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2, transform.position + spaceOnWallNeededToWallJumpUpAmount2 - (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)), out hit, collisionLayers)
		||  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 - (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.1875f, out hit, collisionLayers)
		||  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 - (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.375f, out hit, collisionLayers)
		||  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 - (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.5625f, out hit, collisionLayers)
		||  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 - (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.75f, out hit, collisionLayers)
		||  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 - (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*0.9375f, out hit, collisionLayers)
		||  Physics.Linecast(transform.position + spaceOnWallNeededToWallJumpUpAmount2 + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, transform.position + spaceOnWallNeededToWallJumpUpAmount2 - (transform.forward*(spaceOnWallNeededToWallJumpLength2 + 1)) + (transform.up*(spaceOnWallNeededToWallJumpHeight2 + 1))*1.125f, out hit, collisionLayers)){
			wallBackHit = true;
			distFromWall = Vector3.Distance(new Vector3(hit.point.x, 0, hit.point.z), new Vector3(transform.position.x, 0, transform.position.z));
		}
		else {
			wallBackHit = false;
		}
		//if player did slide off wall, set currentlyOnWall to false
		if (wallNormal != Vector3.zero
		&&  Quaternion.Angle(transform.rotation, Quaternion.LookRotation(wallNormal)) < 0.1f && !wallBackHit){
			if (animator != null && currentlyOnWall){
				onWallAnimation = false;
			}
			currentlyOnWall = false;
		}
		
		if (wallJumping.allowWallJumping && currentlyOnWall){
			if (animator != null && !onWallAnimation && !sliding){
				
				//if the angle between the player and wall is greater than or equal to 90, turn to the left
				if (angleBetweenPlayerAndWall >= 90){
					animator.SetFloat("wallState", 4);
				}
				//if the angle between the player and wall is less than 90, turn to the right
				else {
					animator.SetFloat("wallState", 3);
				}
				animator.CrossFade("WallJump", 0f, -1, 0f);
				onWallAnimation = true;

			}
			
			wallJumpTimer += 0.02f;
			moveDirection = Vector3.zero;
			if (GetComponent<Rigidbody>()){
				GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
			if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(wallNormal)) < 0.1f && wallJumping.slideDownWalls && noCollisionTimer < 5){
				wallHitPoint += transform.forward/125;
			}
			transform.position = Vector3.Lerp(transform.position, new Vector3(wallHitPoint.x, transform.position.y, wallHitPoint.z), 10 * Time.deltaTime);
			if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(wallNormal)) >= 0.01f){
				firstDistFromWall = distFromWall;
			}
			//if the wall becomes further away from the player (most likely because it is sloped), push the player backwards towards the wall
			else if (distFromWall - firstDistFromWall > 0.01f) {
				transform.position -= transform.forward/125;
			}
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(wallNormal), (wallJumping.rotationToWallSpeed*2) * Time.deltaTime);
			originalWallJumpDirection = (transform.forward * wallJumping.wallJumpDistance) + (transform.up * wallJumping.wallJumpHeight);
			wallJumpDirection = (transform.forward * wallJumping.wallJumpDistance) + (transform.up * wallJumping.wallJumpHeight);
			
			//sliding down walls
			if (wallJumping.slideDownWalls && wallJumping.slideDownSpeed != 0){
				
				if (slideDownSpeed2 < gravity){
					slideDownSpeed2 += (wallJumping.slideDownSpeed/100) * wallJumpTimer;
				}
				else {
					slideDownSpeed2 = gravity;
				}
				
				if (GetComponent<CharacterController>() && GetComponent<CharacterController>().enabled){
					GetComponent<CharacterController>().Move(new Vector3(0, -slideDownSpeed2, 0) * Time.deltaTime);
				}
				else if (GetComponent<Rigidbody>()){
					GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(0, -slideDownSpeed2, 0) * Time.deltaTime);
				}
				
				//if the player has finished rotating and is colliding with the wall, push the player slightly forward to avoid becoming stuck
				if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(wallNormal)) < 0.1f && noCollisionTimer < 5){
					wallHitPoint = transform.position + transform.forward/125;
				}
				
			}
			
			if (Input.GetButtonDown("Jump")){
				inMidAirFromWallJump = true;
				doubleJumpPossible = true;
				WallJump();
				currentlyOnWall = false;
			}
			if (wallJumping.useWallJumpTimeLimit && wallJumpTimer > wallJumping.wallJumpTimeLimit){
				currentlyOnWall = false;
			}
		}
		else if (animator != null){
			onWallAnimation = false;
		}
		
		if (grounded.currentlyGrounded){
			currentlyOnWall = false;
			if (inMidAirFromWallJump){
				transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
				inMidAirFromWallJump = false;
			}
			if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("WallJump") && animator.GetFloat("wallState") == 0){
				animator.CrossFade("Movement", 0f, -1, 0f);
			}
		}
		
		//animating jumping on to wall
		if (inMidAirFromWallJump){
			if (animator != null && onWallAnimation){
				
				//if the angle between the player and wall is greater than or equal to 90, turn to the left
				if (angleBetweenPlayerAndWall >= 90){
					animator.SetFloat("wallState", 2);
				}
				//if the angle between the player and wall is less than 90, turn to the right
				else {
					animator.SetFloat("wallState", 1);
				}
				animator.CrossFade("WallJump", 0f, -1, 0f);
				onWallAnimation = false;
			}
			
			if (jumpedOffWallForWallJump){
				originalWallJumpDirection = Vector3.Lerp(originalWallJumpDirection, new Vector3(0, originalWallJumpDirection.y, 0), wallJumping.wallJumpDecelerationRate * Time.deltaTime);
				wallJumpDirection = Vector3.Lerp(wallJumpDirection, new Vector3(0, wallJumpDirection.y, 0), wallJumping.wallJumpDecelerationRate * Time.deltaTime);
				if (noCollisionTimer >= 5 && !currentlyOnWall){
					wallJumpDirection += (Input.GetAxis("Horizontal")*(wallJumping.overallMovementSpeed/8) * playerCamera.transform.right);
					wallJumpDirection += (Input.GetAxis("Vertical")*(wallJumping.overallMovementSpeed/8) * playerCamera.transform.forward);
					
					//moving forward
					if (forwardDir > originalForwardDir){
						
						wallJumpDirection -= (transform.forward*(forwardDir/originalForwardDir))*(-(wallJumping.forwardMovementSpeedMultiple*0.1f + 0.9f) + 1);
						
					}
					//moving backwards
					else if (forwardDir < originalForwardDir){
						
						wallJumpDirection += (transform.forward/(forwardDir/originalForwardDir))*(-(wallJumping.backMovementSpeedMultiple*0.1f + 0.9f) + 1);
						
					}
					
					//moving right
					if (rightDir > originalRightDir){
						
						wallJumpDirection -= (transform.right*((rightDir - originalRightDir)*originalRightDir))*(-(wallJumping.sideMovementSpeedMultiple/2 + 0.5f) + 1);
						
					}
					//moving left
					else if (rightDir < originalRightDir){
						
						wallJumpDirection += (transform.right/((rightDir/originalRightDir)*2))*(-(wallJumping.sideMovementSpeedMultiple*0.075f + 0.925f) + 1);
						
					}
					
				}
			}
			
			if (GetComponent<CharacterController>() && GetComponent<CharacterController>().enabled){
				GetComponent<CharacterController>().Move(new Vector3(wallJumpDirection.x, 0, wallJumpDirection.z) * Time.deltaTime);
			}
			else if (GetComponent<Rigidbody>()){
				GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(wallJumpDirection.x, 0, wallJumpDirection.z) * Time.deltaTime);
			}
			
		}
		
		//if player is no longer on wall or is on the ground, set wallState to 0
		if (animator != null){
			if (!currentlyOnWall && (animator.GetFloat("wallState") == 3 || animator.GetFloat("wallState") == 4) || grounded.currentlyGrounded){
				animator.SetFloat("wallState", 0);
			}
		}
		
		
		//if joystick/arrow keys are being pushed
		if (directionVector.magnitude > 0 && !currentlyOnWall){
			
			float myAngle = Mathf.Atan2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")) * Mathf.Rad2Deg;
			float bodyRotation = myAngle + playerCamera.eulerAngles.y;
			//if in mid-air from wall jumping, rotate using the rotationSpeedMultiple
			if (inMidAirFromWallJump){
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler(0, bodyRotation, 0), (rotationSpeed2*wallJumping.rotationSpeedMultiple) * Time.deltaTime);
			}
			//if the camera is not attached to the player or the player is not moving straight backwards, continue to rotate
			else if (playerCamera.transform.parent != transform || Input.GetAxis ("Vertical") >= 0 || Input.GetAxis ("Horizontal") != 0){
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler(0, bodyRotation, 0), rotationSpeed2 * Time.deltaTime);
			}
			
			//increase the player's acceleration until accelerationRate has reached 1
			if (accelerationRate < 1){
				accelerationRate += 0.01f*acceleration2;
			}
			else {
				accelerationRate = 1;
			}
			
		}
		else {
			accelerationRate = 0;
		}
		
		//setting the speed of the player
		h = Mathf.Lerp(h, (Mathf.Abs(Input.GetAxisRaw ("Horizontal")) - Mathf.Abs(Input.GetAxisRaw ("Vertical")) + 1)/2, 8 * Time.deltaTime);
		v = Mathf.Lerp(v, (Mathf.Abs(Input.GetAxisRaw ("Vertical")) - Mathf.Abs(Input.GetAxisRaw ("Horizontal")) + 1)/2, 8 * Time.deltaTime);
		if (directionVector.magnitude != 0){
			if (Input.GetAxis("Vertical") >= 0){
				
				moveSpeed = (Mathf.Lerp(moveSpeed, h*sideSpeed2 + v*forwardSpeed2, 8 * Time.deltaTime)*directionVector.magnitude)*accelerationRate;
			
			}
			else {
				
				moveSpeed = (Mathf.Lerp(moveSpeed, h*sideSpeed2 + v*backSpeed2, 8 * Time.deltaTime)*directionVector.magnitude)*accelerationRate;
				
			}
			
		}
		if (animator != null && animator.runtimeAnimatorController != null){
			animator.SetFloat ("speed", moveSpeed);
		}
		
		decelerationRate += deceleration/10;
		airSpeed = moveSpeed * midAirMovementSpeedMultiple2;
		
		//applying friction to the player's movement
		if (movement.movementFriction > 0){
			moveSpeedAndFriction = Mathf.Lerp(moveSpeedAndFriction, moveSpeed, (24/movement.movementFriction) * Time.deltaTime);
		}
		else {
			moveSpeedAndFriction = moveSpeed;
		}
		
		//determining whether the player is grounded or not
		//drawing ground detection rays
		if (grounded.showGroundDetectionRays){
			Debug.DrawLine(pos + maxGroundedHeight2, pos + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos - transform.forward*(maxGroundedRadius2/2) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2/2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos + transform.forward*(maxGroundedRadius2/2) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2/2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos - transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos - transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos + transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos + transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos - transform.forward*(maxGroundedRadius2/2) - transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2/2) - transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos + transform.forward*(maxGroundedRadius2/2) + transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2/2) + transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos - transform.forward*(maxGroundedRadius2/2) + transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2/2) + transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos + transform.forward*(maxGroundedRadius2/2) - transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2/2) - transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos - transform.forward*(maxGroundedRadius2) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos + transform.forward*(maxGroundedRadius2) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos - transform.right*(maxGroundedRadius2) + maxGroundedHeight2, pos - transform.right*(maxGroundedRadius2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos + transform.right*(maxGroundedRadius2) + maxGroundedHeight2, pos + transform.right*(maxGroundedRadius2) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos - transform.forward*(maxGroundedRadius2*0.75f) - transform.right*(maxGroundedRadius2*0.75f) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2*0.75f) - transform.right*(maxGroundedRadius2*0.75f) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos + transform.forward*(maxGroundedRadius2*0.75f) + transform.right*(maxGroundedRadius2*0.75f) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2*0.75f) + transform.right*(maxGroundedRadius2*0.75f) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos - transform.forward*(maxGroundedRadius2*0.75f) + transform.right*(maxGroundedRadius2*0.75f) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2*0.75f) + transform.right*(maxGroundedRadius2*0.75f) + maxGroundedDistanceDown, Color.yellow);
			Debug.DrawLine(pos + transform.forward*(maxGroundedRadius2*0.75f) - transform.right*(maxGroundedRadius2*0.75f) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2*0.75f) - transform.right*(maxGroundedRadius2*0.75f) + maxGroundedDistanceDown, Color.yellow);
		}
		//determining if grounded
		if (Physics.Linecast(pos + maxGroundedHeight2, pos + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos - transform.forward*(maxGroundedRadius2/2) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2/2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos + transform.forward*(maxGroundedRadius2/2) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2/2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos - transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos - transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos + transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos + transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos - transform.forward*(maxGroundedRadius2/2) - transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2/2) - transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos + transform.forward*(maxGroundedRadius2/2) + transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2/2) + transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos - transform.forward*(maxGroundedRadius2/2) + transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2/2) + transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos + transform.forward*(maxGroundedRadius2/2) - transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2/2) - transform.right*(maxGroundedRadius2/2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos - transform.forward*(maxGroundedRadius2) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos + transform.forward*(maxGroundedRadius2) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos - transform.right*(maxGroundedRadius2) + maxGroundedHeight2, pos - transform.right*(maxGroundedRadius2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos + transform.right*(maxGroundedRadius2) + maxGroundedHeight2, pos + transform.right*(maxGroundedRadius2) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos - transform.forward*(maxGroundedRadius2*0.75f) - transform.right*(maxGroundedRadius2*0.75f) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2*0.75f) - transform.right*(maxGroundedRadius2*0.75f) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos + transform.forward*(maxGroundedRadius2*0.75f) + transform.right*(maxGroundedRadius2*0.75f) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2*0.75f) + transform.right*(maxGroundedRadius2*0.75f) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos - transform.forward*(maxGroundedRadius2*0.75f) + transform.right*(maxGroundedRadius2*0.75f) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2*0.75f) + transform.right*(maxGroundedRadius2*0.75f) + maxGroundedDistanceDown, out hit, collisionLayers)
		||	Physics.Linecast(pos + transform.forward*(maxGroundedRadius2*0.75f) - transform.right*(maxGroundedRadius2*0.75f) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2*0.75f) - transform.right*(maxGroundedRadius2*0.75f) + maxGroundedDistanceDown, out hit, collisionLayers)){
			if (!angHit){
				raycastSlopeAngle = (Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f;
			}
			grounded.currentlyGrounded = true;
		}
		else {
			grounded.currentlyGrounded = false;
		}

		//determining the slope of the surface you are currently standing on
		float myAng2 = 0.0f;
		if (Physics.Raycast(pos, Vector3.down, out hit, 1f, collisionLayers)){
			angHit = true;
			myAng2 = (Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f;
		}
		else {
			angHit = false;
		}
		
		//raycasting to determine whether sliding is possible or not
		RaycastHit altHit = new RaycastHit();
		if (Physics.Raycast(pos, Vector3.down, out hit, maxGroundedDistance2, collisionLayers)){
			slidePossible = true;
			if (Physics.Raycast(pos + transform.forward/10, Vector3.down, out altHit, maxGroundedDistance2, collisionLayers)
			&& ((Mathf.Acos(Mathf.Clamp(altHit.normal.y, -1f, 1f))) * 57.2958f) > ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f)
			||  Physics.Raycast(pos - transform.forward/10, Vector3.down, out altHit, maxGroundedDistance2, collisionLayers)
			&& ((Mathf.Acos(Mathf.Clamp(altHit.normal.y, -1f, 1f))) * 57.2958f) > ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f)
			||  Physics.Raycast(pos + transform.right/10, Vector3.down, out altHit, maxGroundedDistance2, collisionLayers)
			&& ((Mathf.Acos(Mathf.Clamp(altHit.normal.y, -1f, 1f))) * 57.2958f) > ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f)
			||  Physics.Raycast(pos - transform.right/10, Vector3.down, out altHit, maxGroundedDistance2, collisionLayers)
			&& ((Mathf.Acos(Mathf.Clamp(altHit.normal.y, -1f, 1f))) * 57.2958f) > ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f)
			||  Physics.Raycast(pos + transform.forward/10 + transform.right/10, Vector3.down, out altHit, maxGroundedDistance2, collisionLayers)
			&& ((Mathf.Acos(Mathf.Clamp(altHit.normal.y, -1f, 1f))) * 57.2958f) > ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f)
			||  Physics.Raycast(pos + transform.forward/10 - transform.right/10, Vector3.down, out altHit, maxGroundedDistance2, collisionLayers)
			&& ((Mathf.Acos(Mathf.Clamp(altHit.normal.y, -1f, 1f))) * 57.2958f) > ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f)
			||  Physics.Raycast(pos - transform.forward/10 + transform.right/10, Vector3.down, out altHit, maxGroundedDistance2, collisionLayers)
			&& ((Mathf.Acos(Mathf.Clamp(altHit.normal.y, -1f, 1f))) * 57.2958f) > ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f)
			||  Physics.Raycast(pos - transform.forward/10 - transform.right/10, Vector3.down, out altHit, maxGroundedDistance2, collisionLayers)
			&& ((Mathf.Acos(Mathf.Clamp(altHit.normal.y, -1f, 1f))) * 57.2958f) > ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f)){
				raycastSlopeAngle = (Mathf.Acos(Mathf.Clamp(altHit.normal.y, -1f, 1f))) * 57.2958f;
			}
			else {
				raycastSlopeAngle = (Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f;
			}
		}
		else if (Physics.Raycast(contactPoint + Vector3.up, Vector3.down, out hit, 5f, collisionLayers) && collisionSlopeAngle < 90 && collisionSlopeAngle > slopeLimit){
			if (angHit && myAng2 > slopeLimit || !angHit){
				slidePossible = true;
				if (angHit){
					raycastSlopeAngle = (Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f;
				}
			}

		}
		else if (Physics.Raycast(pos, Vector3.down, out hit, 1f, collisionLayers)){
			slidePossible = true;
			if (angHit){
				raycastSlopeAngle = (Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f;
			}
		}
		else {
			slidePossible = false;
		}
		
		
		//checking to see if player is stuck between two slopes
		CheckIfInBetweenSlopes();
		
		
		//checking to see if player is facing uphill or downhill on a slope
		RaycastHit frontHit = new RaycastHit();
		RaycastHit backHit = new RaycastHit();
		if (Physics.Raycast(pos + transform.forward/2 + transform.up, Vector3.down, out frontHit, 5f, collisionLayers) && Physics.Raycast(pos - transform.forward/2 + transform.up, Vector3.down, out backHit, 5f, collisionLayers)){
			if (frontHit.point.y >= backHit.point.y){
				uphill = true;
			}
			else{
				uphill = false;
			}
		}
		else if (Physics.Raycast(pos + transform.forward/2 + transform.up, Vector3.down, out frontHit, 5f, collisionLayers)){
			uphill = true;
		}
		else {
			uphill = false;
		}
		
		
		if (grounded.currentlyGrounded && (noCollisionTimer < 5 || Physics.Raycast(pos, Vector3.down, maxGroundedDistance2, collisionLayers))) {
			//since we are grounded, recalculate move direction directly from axes
			if (!jumpPerformed){
				moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			}
			else {
				moveDirection = new Vector3(Input.GetAxis("Horizontal"), moveDirection.y, Input.GetAxis("Vertical"));
			}
			if (directionVector.magnitude != 0){
				//if the camera is not attached to the player or the player is not moving straight backwards, do not move straight backwards
				if (playerCamera.transform.parent != transform || Input.GetAxis ("Vertical") >= 0 || Input.GetAxis ("Horizontal") != 0){
					moveDirection = new Vector3(transform.forward.x, moveDirection.y, transform.forward.z);
				}
				//if the camera is attached to the player and the player is moving straight backwards, move straight backwards
				else {
					moveDirection = new Vector3(-transform.forward.x, moveDirection.y, -transform.forward.z);
				}
				decelerationRate = 0;
			}
			
			
			if (directionVector.magnitude == 0){
				//if the camera is not attached to the player or the player is not moving straight backwards, do not move straight backwards
				if (playerCamera.transform.parent != transform || Input.GetAxis ("Vertical") >= 0 || Input.GetAxis ("Horizontal") != 0){
					moveDirection = new Vector3(transform.forward.x, moveDirection.y, transform.forward.z);
				}
				//if the camera is attached to the player and the player is moving straight backwards, move straight backwards
				else {
					moveDirection = new Vector3(-transform.forward.x, moveDirection.y, -transform.forward.z);
				}
				if(moveSpeed > 0){
					moveSpeed -= decelerationRate * moveSpeed;
				}
				if (moveSpeed <= 0){
					moveSpeed = 0;
				}
			}
			moveDirection.x *= moveSpeedAndFriction;
			moveDirection.z *= moveSpeedAndFriction;
			
			rotationSpeed2 = movement.rotationSpeed;
		}
		else {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), moveDirection.y, Input.GetAxis("Vertical"));
			if (directionVector.magnitude != 0){
				//if the camera is not attached to the player or the player is not moving straight backwards, do not move straight backwards
				if (playerCamera.transform.parent != transform || Input.GetAxis ("Vertical") >= 0 || Input.GetAxis ("Horizontal") != 0){
					moveDirection = new Vector3(transform.forward.x, moveDirection.y, transform.forward.z);
				}
				//if the camera is attached to the player and the player is moving straight backwards, move straight backwards
				else {
					moveDirection = new Vector3(-transform.forward.x, moveDirection.y, -transform.forward.z);
				}
			}
			else {
				moveSpeed = 0;
			}
			moveDirection.x *= airSpeed;
			moveDirection.z *= airSpeed;
			
			rotationSpeed2 = movement.rotationSpeed * movement.midAirRotationSpeedMultiple;
		}
		
		if (noCollisionTimer >= 5 && !grounded.currentlyGrounded || inMidAirFromJump || jumpPerformed || !sliding){
			slidingVector = Vector3.zero;
		}
		if (!angHit && noCollisionTimer < 5 && slidingVector != Vector3.zero && moveDirection.y <= -gravity){
			moveDirection.y = -gravity;
		}
		
		//sliding
		if (raycastSlopeAngle > slopeLimit && collisionSlopeAngle < 89 && !jumpPerformed && !inMidAirFromJump && slidePossible && !inBetweenSlidableSurfaces){
			if (!inBetweenSlidableSurfaces && (uphill && !jumping.allowJumpWhenSlidingFacingUphill || !uphill && !jumping.allowJumpWhenSlidingFacingDownhill)){
				jumpPossible = false;
			}
			else {
				jumpPossible = true;
			}

			if (noCollisionTimer < 5 || grounded.currentlyGrounded){
				if (!sliding){
					slideSpeed = 1.0f;
				}
				sliding = true;
				if (jumping.doNotIncreaseJumpNumberWhenSliding){
					currentJumpNumber = 0;
				}
				slideMovement = Vector3.Slerp(slideMovement, new Vector3(slidingVector.x, -slidingVector.y, slidingVector.z), 6 * Time.deltaTime);
				moveDirection.x += (slideMovement*(slideSpeed*slopeSlideSpeed2) + new Vector3(0, -8, 0)).x;
				moveDirection.z += (slideMovement*(slideSpeed*slopeSlideSpeed2) + new Vector3(0, -8, 0)).z;
				if (noCollisionTimer < 2 || !jumpPossible){
					
					if (GetComponent<CharacterController>() && GetComponent<CharacterController>().enabled){
						moveDirection.y += (slideMovement*(slideSpeed*slopeSlideSpeed2) + new Vector3(0, -8, 0)).y;
					}
					else if (GetComponent<Rigidbody>()){
						
						if (yVel < -0.01f * gravity && Physics.Raycast(pos, Vector3.down, out hit, 1f, collisionLayers) && ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f) > slopeLimit){
							if (transform.position.y - hit.point.y < 0.2f){
								transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
							}
							else {
								moveDirection.y += (slideMovement*(slideSpeed*slopeSlideSpeed2) + new Vector3(0, -8, 0)).y;
							}
						}
						
					}
					
				}
				slideSpeed += -slideMovement.y * Time.deltaTime * gravity;
				if (noCollisionTimer > 2 && !grounded.currentlyGrounded || moveDirection.y <= -gravity){
					if (!inMidAirFromJump && GetComponent<CharacterController>() && GetComponent<CharacterController>().enabled){
						moveDirection.y = -gravity;
					}
				}
			}
			else if (!inMidAirFromJump && GetComponent<CharacterController>() && GetComponent<CharacterController>().enabled){
				if (sliding){
					moveDirection.y = -gravity;
				}
				sliding = false;
			}
			else {
				sliding = false;
			}
			
		}
		else {
			jumpPossible = true;
			sliding = false;
		}
		
		//applying friction after sliding
		if (!sliding){
			if (movement.slideFriction > 0){
				if (!inMidAirFromJump){
					slideMovement = Vector3.Slerp(slideMovement, Vector3.zero, (24/movement.slideFriction) * Time.deltaTime);
				}
				else {
					slideMovement = Vector3.Slerp(slideMovement, Vector3.zero, (24/(movement.slideFriction*1.5f)) * Time.deltaTime);
				}
				if (slideMovement != Vector3.zero){
					if (GetComponent<Rigidbody>() && !jumpPerformed && !inMidAirFromJump && grounded.currentlyGrounded && noCollisionTimer < 5 && Physics.Raycast(pos, Vector3.down, out hit, 1f, collisionLayers)){
						transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
					}
					moveDirection.x += (slideMovement*(slideSpeed*slopeSlideSpeed2) + new Vector3(0, -8, 0)).x;
					moveDirection.z += (slideMovement*(slideSpeed*slopeSlideSpeed2) + new Vector3(0, -8, 0)).z;
					slideSpeed += -slideMovement.y * Time.deltaTime * gravity;
				}
			}
			else {
				slideSpeed = 1.0f;
			}
		}
		
		
		if (grounded.currentlyGrounded || angHit){
			if (jumpPerformed && noCollisionTimer < 5 && !inMidAirFromJump){
				jumpPerformed = false;
			}
		}

		
		//keeping player from bouncing down slopes
		if (Physics.Raycast(pos, Vector3.down, out hit, 1f, collisionLayers) && !GetComponent<Rigidbody>() || Physics.Raycast(pos + transform.forward/10, Vector3.down, out hit, 1f, collisionLayers) && GetComponent<Rigidbody>()){
			if (grounded.currentlyGrounded && !jumpPerformed){
				
				if (GetComponent<CharacterController>() && GetComponent<CharacterController>().enabled && (raycastSlopeAngle > 1 || raycastSlopeAngle < -1)){
					//applying a downward force to keep the player from bouncing down slopes
					moveDirection.y -= hit.point.y;
					if (Physics.Raycast(pos + transform.forward/2 + transform.up, Vector3.down, out frontHit, 5f, collisionLayers) && Physics.Raycast(pos - transform.forward/2 + transform.up, Vector3.down, out backHit, 5f, collisionLayers)){
						
						if (frontHit.point.y < backHit.point.y){
							moveDirection.y -= hit.normal.y;
						}
						
					}
					
				}
				else if (GetComponent<Rigidbody>() && ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f) <= slopeLimit){
					//applying a downward force to keep the player from bouncing down slopes
					if (!sliding){
						moveDirection.y -= hit.point.y;
						if (Physics.Raycast(pos + transform.forward/2 + transform.up, Vector3.down, out frontHit, 5f, collisionLayers) && Physics.Raycast(pos - transform.forward/2 + transform.up, Vector3.down, out backHit, 5f, collisionLayers)){
							
							if (frontHit.point.y < backHit.point.y){
								moveDirection.y -= hit.normal.y;
							}
							
						}
					}
					
				}

			}
		}
		
		//apply gravity
		if (!jumpPressed || !grounded.currentlyGrounded){
			if (!currentlyOnWall){
				moveDirection.y -= gravity * Time.deltaTime;
			}
		}
		
		//telling the player to not fall faster than the maximum falling speed
		if (moveDirection.y <= -maxFallingSpeed2){
			moveDirection.y = -maxFallingSpeed2;
		}

		//if head is blocked/hits the ceiling, stop going up
		if (headHit){
			moveDirection.y = 0;
		}
		
		if (!currentlyOnWall){
			//if player is using a CharacterController
			if (GetComponent<CharacterController>() && GetComponent<CharacterController>().enabled){
				
				// move the player
				GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
				
			}
			//if player is using a Rigidbody
			else if (GetComponent<Rigidbody>()){
			
				//applying a downward force to keep the player falling instead of slowly floating to the ground
				if (grounded.currentlyGrounded && Mathf.Abs(GetComponent<Rigidbody>().velocity.y) > 1f && moveDirection.y > 0 && noCollisionTimer > 5 && !sliding && !jumpPerformed){
					if (Physics.Raycast(pos, Vector3.down, out hit, 1f, collisionLayers) && ((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f) <= slopeLimit){
						moveDirection.y -= -transform.position.y + hit.normal.y;
					}
				}
			
				// move the player
				if (grounded.currentlyGrounded && !sliding && noCollisionTimer < 5
				&&  Physics.Linecast(pos + maxGroundedHeight2, pos + (maxGroundedDistanceDown * ((raycastSlopeAngle/90) + 1)), out hit, collisionLayers)
				&&	Physics.Linecast(pos - transform.forward*(maxGroundedRadius2/2) + maxGroundedHeight2, pos - transform.forward*(maxGroundedRadius2/2) + (maxGroundedDistanceDown * ((raycastSlopeAngle/90) + 1)), out hit, collisionLayers)
				&&	Physics.Linecast(pos + transform.forward*(maxGroundedRadius2/2) + maxGroundedHeight2, pos + transform.forward*(maxGroundedRadius2/2) + (maxGroundedDistanceDown * ((raycastSlopeAngle/90) + 1)), out hit, collisionLayers)
				&&	Physics.Linecast(pos - transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos - transform.right*(maxGroundedRadius2/2) + (maxGroundedDistanceDown * ((raycastSlopeAngle/90) + 1)), out hit, collisionLayers)
				&&	Physics.Linecast(pos + transform.right*(maxGroundedRadius2/2) + maxGroundedHeight2, pos + transform.right*(maxGroundedRadius2/2) + (maxGroundedDistanceDown * ((raycastSlopeAngle/90) + 1)), out hit, collisionLayers)){
					if (raycastSlopeAngle <= slopeLimit){
						GetComponent<Rigidbody>().velocity = moveDirection + new Vector3(0, transform.position.y, 0);
					}
					else {
						GetComponent<Rigidbody>().velocity = moveDirection;
					}
				}
				else {
					GetComponent<Rigidbody>().velocity = moveDirection;
				}
			
			}
		}

	}
	
	void CheckIfInBetweenSlopes () {
		
		//checking to see if player is stuck between two slopes
		
		RaycastHit hit2 = new RaycastHit();
		
		//checking left and right
		if (Physics.Raycast(pos - transform.right/5, Vector3.down, out hit, maxGroundedDistance2, collisionLayers) && Physics.Raycast(pos + transform.right/5, Vector3.down, out hit2, maxGroundedDistance2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		|| Physics.Linecast(transform.position - transform.right/5 + transform.up/4, transform.position - transform.right/5 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position + transform.right/5 + transform.up/4, transform.position + transform.right/5 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)	
		|| Physics.Linecast(transform.position - transform.right/25 + transform.up/4, transform.position - transform.right/25 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position + transform.right/25 + transform.up/4, transform.position + transform.right/25 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		
		//checking forward left and back right
		|| Physics.Raycast(pos + transform.forward/25 - transform.right/5, Vector3.down, out hit, maxGroundedDistance2, collisionLayers) && Physics.Raycast(pos - transform.forward/25 + transform.right/5, Vector3.down, out hit2, maxGroundedDistance2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		|| Physics.Linecast(transform.position + transform.forward/50 - transform.right/5 + transform.up/4, transform.position + transform.forward/50 - transform.right/5 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/50 - transform.right/5 + transform.up/4, transform.position - transform.forward/50 - transform.right/5 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		|| Physics.Linecast(transform.position + transform.forward/25 - transform.right/5 + transform.up/4, transform.position + transform.forward/25 - transform.right/5 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/25 + transform.right/5 + transform.up/4, transform.position - transform.forward/25 + transform.right/5 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		|| Physics.Linecast(transform.position + transform.forward/25 - transform.right/25 + transform.up/4, transform.position + transform.forward/25 - transform.right/25 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/25 + transform.right/25 + transform.up/4, transform.position - transform.forward/25 + transform.right/25 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		
		//checking forward right and back left
		|| Physics.Raycast(pos + transform.forward/25 + transform.right/5, Vector3.down, out hit, maxGroundedDistance2, collisionLayers) && Physics.Raycast(pos - transform.forward/25 - transform.right/5, Vector3.down, out hit2, maxGroundedDistance2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		|| Physics.Linecast(transform.position + transform.forward/50 + transform.right/5 + transform.up/4, transform.position + transform.forward/50 + transform.right/5 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/50 + transform.right/5 + transform.up/4, transform.position - transform.forward/50 + transform.right/5 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		|| Physics.Linecast(transform.position + transform.forward/25 + transform.right/5 + transform.up/4, transform.position + transform.forward/25 + transform.right/5 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/25 - transform.right/5 + transform.up/4, transform.position - transform.forward/25 - transform.right/5 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		|| Physics.Linecast(transform.position + transform.forward/25 + transform.right/25 + transform.up/4, transform.position + transform.forward/25 + transform.right/25 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/25 - transform.right/25 + transform.up/4, transform.position - transform.forward/25 - transform.right/25 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		
		//checking forward and back
		|| Physics.Linecast(transform.position + transform.forward/10 + transform.up/4, transform.position + transform.forward/10 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/10 + transform.up/4, transform.position - transform.forward/10 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		|| Physics.Linecast(transform.position + transform.forward/25 + transform.up/4, transform.position + transform.forward/25 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/25 + transform.up/4, transform.position - transform.forward/25 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		
		//checking forward left and back left
		|| Physics.Linecast(transform.position + transform.forward/25 - transform.right/3 + transform.up/4, transform.position + transform.forward/25 - transform.right/3 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/25 - transform.right/3 + transform.up/4, transform.position - transform.forward/25 - transform.right/3 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		|| Physics.Linecast(transform.position + transform.forward/50 - transform.right/3 + transform.up/4, transform.position + transform.forward/50 - transform.right/3 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/50 - transform.right/3 + transform.up/4, transform.position - transform.forward/50 - transform.right/3 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		
		//checking forward right and back right
		|| Physics.Linecast(transform.position + transform.forward/25 + transform.right/3 + transform.up/4, transform.position + transform.forward/25 + transform.right/3 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/25 + transform.right/3 + transform.up/4, transform.position - transform.forward/25 + transform.right/3 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)
		|| Physics.Linecast(transform.position + transform.forward/50 + transform.right/3 + transform.up/4, transform.position + transform.forward/50 + transform.right/3 - transform.up*maxGroundedDistance2, out hit, collisionLayers) && Physics.Linecast(transform.position - transform.forward/50 + transform.right/3 + transform.up/4, transform.position - transform.forward/50 + transform.right/3 - transform.up*maxGroundedDistance2, out hit2, collisionLayers) && (hit.normal.z < 0 && hit2.normal.z > 0 || hit.normal.z > 0 && hit2.normal.z < 0)){
			
			if (((Mathf.Acos(Mathf.Clamp(hit.normal.y, -1f, 1f))) * 57.2958f) > slopeLimit && ((Mathf.Acos(Mathf.Clamp(hit2.normal.y, -1f, 1f))) * 57.2958f) > slopeLimit){
			
				inBetweenSlidableSurfaces = true;
				uphill = false;
				
			}
			else {
				inBetweenSlidableSurfaces = false;
			}
			
		}
		else if (!inMidAirFromJump){
			inBetweenSlidableSurfaces = false;
		}
		
	}

	void Jump () {
		jumpPerformed = true;
		if (currentJumpNumber == totalJumpNumber || timeLimitBetweenJumps2 <= 0 || jumping.doNotIncreaseJumpNumberWhenSliding && sliding){
			currentJumpNumber = 0;
		}
		currentJumpNumber++;
		if (animator != null && animator.runtimeAnimatorController != null){
			animator.CrossFade("Jump", 0f, -1, 0f);
		}
		jumpTimer = 0.0f;
		moveDirection.y = jumpsToPerform[currentJumpNumber - 1];
		inMidAirFromJump = true;
		jumpPressed = false;
		return;

	}
	
	void DoubleJump () {
		inMidAirFromJump = true;
		if (inMidAirFromWallJump){
			transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
			inMidAirFromWallJump = false;
		}
		if (moveDirection.y > 0){
			moveDirection.y += doubleJumpHeight2;
			if (GetComponent<CharacterController>() && GetComponent<CharacterController>().enabled){
				GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
			}
			if (GetComponent<Rigidbody>()){
				GetComponent<Rigidbody>().velocity = moveDirection;
			}
		}
		else {
			moveDirection.y = doubleJumpHeight2;
			if (GetComponent<CharacterController>() && GetComponent<CharacterController>().enabled){
				GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
			}
			if (GetComponent<Rigidbody>()){
				GetComponent<Rigidbody>().velocity = moveDirection;
			}
		}
		if (animator != null && animator.runtimeAnimatorController != null){
			animator.CrossFade("DoubleJump", 0f, -1, 0f);
		}
		if (doubleJumpEffect2 != null){
			Instantiate(doubleJumpEffect2, transform.position + new Vector3(0, 0.2f, 0), doubleJumpEffect2.transform.rotation);
		}
		return;
	}
	
	void WallJump () {
		inMidAirFromJump = true;
		moveDirection = wallJumpDirection;
		return;
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit) {
		
		contactPoint = hit.point;
		collisionSlopeAngle = Vector3.Angle(Vector3.up, hit.normal);
		noCollisionTimer = 0;
		
		slidingAngle = Vector3.Angle(hit.normal, Vector3.up);
        if (slidingAngle >= slopeLimit) {
            slidingVector = hit.normal;
            if (slidingVector.y == 0){
				slidingVector = Vector3.zero;
			}
        }
		else {
            slidingVector = Vector3.zero;
        }
 
        slidingAngle = Vector3.Angle(hit.normal, moveDirection - Vector3.up * moveDirection.y);
        if (slidingAngle > 90) {
            slidingAngle -= 90.0f;
            if (slidingAngle > slopeLimit){
				slidingAngle = slopeLimit;
			}
            if (slidingAngle < slopeLimit){
				slidingAngle = 0;
			}
        }
		
	}
	
	void OnCollisionStay (Collision hit) {
		
		foreach (ContactPoint contact in hit.contacts) {
			contactPoint = contact.point;
			collisionSlopeAngle = Vector3.Angle(Vector3.up, contact.normal);
			noCollisionTimer = 0;
			
			slidingAngle = Vector3.Angle(contact.normal, Vector3.up);
			if (slidingAngle >= slopeLimit) {
				slidingVector = contact.normal;
				if (slidingVector.y == 0){
					slidingVector = Vector3.zero;
				}
			}
			else {
				slidingVector = Vector3.zero;
			}
 
			slidingAngle = Vector3.Angle(contact.normal, moveDirection - Vector3.up * moveDirection.y);
			if (slidingAngle > 90) {
				slidingAngle -= 90.0f;
				if (slidingAngle > slopeLimit){
					slidingAngle = slopeLimit;
				}
				if (slidingAngle < slopeLimit){
					slidingAngle = 0;
				}
			}
        }
		
		

	}
	
	void OnDisable() {
		
		//resetting values
		
		if (GetComponent<Rigidbody>()){
			if (!GetComponent<CharacterController>() || GetComponent<CharacterController>() && !GetComponent<CharacterController>().enabled){
				GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
		}
		inMidAirFromJump = false;
		inMidAirFromWallJump = false;
        currentJumpNumber = totalJumpNumber;
		moveDirection.y = 0;
		moveSpeed = 0;
		jumpPressed = false;
		jumpPossible = false;
		jumpEnabled = false;
		doubleJumpPossible = true;
		middleWallJumpable = false;
		leftWallJumpable = false;
		rightWallJumpable = false;
		currentlyOnWall = false;
		
    }
	
}