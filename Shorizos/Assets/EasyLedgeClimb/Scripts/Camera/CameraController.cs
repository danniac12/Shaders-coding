/// Easy Ledge Climb Character System / 3D Platformer Kit
/// CameraController.cs
///
/// Modified from John McElmurray's and Julian Adams' "ThirdPersonCamera.cs" script.
/// This script allows the camera to:
/// 1. Follow the player at a set speed, height, and distance.
/// 2. Lock on behind the player.
///
/// NOTE: *You should always set a layer for your player so that you can disable collisions with that layer (by unchecking it in the script's Collision Layers).
///	If you do not, the camera will collide with the player himself!*
///
/// (C) 2015-2016 Grant Marrs

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public Transform player; //the player set for the camera to follow
	public float playerHeight; //the height of the player
	public float cameraDistance = 2.95f; //the distance between the camera and the player
	public float cameraHeight = 1f; //the height of the camera
	public float movementDampening = 0.1f; //the amount of dampening applied to the movement of the camera
	public float rotationDampening = 0.5f; //the amount of dampening applied to the rotation of the camera
	
	public bool allowLockingOn = true; //determines whether or not to allow locking on
	public string lockOnInputButton = "Fire2"; //the button or axis (found in "Edit > Project Settings > Input") pressed to lock on
	public Texture barTexture; //the texture that appears on the top and bottom portion of the screen while locking on
	public float barCoverage = 28f; //the percentage of the screen that the bar texture covers
	public float lockOnSpeed = 2f; //the speed at which the camera locks on behind the player
	
	
	//camera following variables
	private bool following;
	private Vector3 playerOffset;
	private Vector3 playerPos;
	private Vector3 velocityCamSmooth;

	//camera locking on variables
	private bool lockingOn;
	private float barHeight;

	//camera rotation variables
	private Vector3 lookDir;
	private Vector3 velocityLookDir;

	//camera frustrum variables
	private Vector3[] viewFrustum;
	private Vector3 nearClipDimensions;
	private const int frustrumBottomLeftPoint = 0;
	private const int frustrumTopLeftPoint = 1;
	private const int frustrumTopRightPoint = 2;
	private const int frustrumBottomRightPoint = 3;
	private const int frustrumBottomLeftVec = 4;
	private const int frustrumTopLeftVec = 5;
	private const int frustrumTopRightVec = 6;
	private const int frustrumBottomRightVec = 7;
	private const int frustrumSize = 8;

	public LayerMask collisionLayers = -1; //the layers that the detectors (raycasts/linecasts) will collide with

	private void Start() {
		
		following = true;
		lockingOn = false;
		lookDir = player.transform.forward;
		playerOffset = player.transform.position + cameraHeight * player.transform.up;
		
	}

	private void FixedUpdate() {
		
		viewFrustum = CameraController.CalculateViewFrustum(GetComponent<Camera>(), ref nearClipDimensions);
		
		playerOffset = player.transform.position + cameraHeight * player.transform.up;
		playerPos = Vector3.zero;
		
		//determining camera state
		
		//locking on
		if (allowLockingOn && (Input.GetButton(lockOnInputButton) || Input.GetAxis(lockOnInputButton) != 0)){
			
			barHeight = Mathf.SmoothStep(barHeight, barCoverage * 2f * 2.5f, lockOnSpeed / 4f);
			lockingOn = true;
			following = false;
			
		}
		//following
		else {
			
			barHeight = Mathf.SmoothStep(barHeight, barCoverage * 2.5f, lockOnSpeed / 4f);
			if (lockingOn && (!Input.GetButton(lockOnInputButton) || Input.GetAxis(lockOnInputButton) == 0) || !allowLockingOn){
				
				following = true;
				lockingOn = false;
				
			}
			
		}
		
		if (following){
			
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime);
			Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			if (directionVector != Vector3.zero) {
				// Get the length of the directon vector and then normalize it
				// Dividing by the length is cheaper than normalizing when we already have the length anyway
				float directionLength = directionVector.magnitude;
				directionVector = directionVector / directionLength;

				// Make sure the length is no bigger than 1
				directionLength = Mathf.Min(1, directionLength);

				// Make the input vector more sensitive towards the extremes and less sensitive in the middle
				// This makes it easier to control slow speeds when using analog sticks
				directionLength *= directionLength;

				// Multiply the normalized direction vector by the modified length
				directionVector *= directionLength;
			}
	
            if (directionVector.magnitude > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.01f){
				
				Vector3 vector2 = Vector3.Lerp(player.transform.right * ((Input.GetAxis("Horizontal") >= 0f) ? -1f : 1f), player.transform.forward * ((Input.GetAxis("Vertical") >= 0f) ? 1f : -1f), Mathf.Abs(Vector3.Dot(transform.forward, player.transform.forward)));
				lookDir = Vector3.Normalize(playerOffset - transform.position);
				lookDir.y = 0f;
				lookDir = Vector3.SmoothDamp(lookDir, vector2, ref velocityLookDir, rotationDampening);
				
			}
			playerPos = playerOffset + player.transform.up * cameraHeight - Vector3.Normalize(lookDir) * cameraDistance;
			
		}
		if (lockingOn && allowLockingOn){
			
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime);
			lookDir = player.transform.forward;
			playerPos = playerOffset + player.transform.up * cameraHeight - lookDir * cameraDistance;
			
		}
		
		CompensateForWalls(playerOffset, ref playerPos);
		
		if (!lockingOn){
			transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocityCamSmooth, movementDampening);
		}
		else {
			transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocityCamSmooth, movementDampening / (lockOnSpeed * 0.5f));
		}
		transform.LookAt(player.transform.position + new Vector3(0f, playerHeight + 1f, 0f));
		
	}

	private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
	{
		//compensate for walls between camera
		RaycastHit raycastHit = new RaycastHit();
		if (Physics.Linecast(fromObject, toTarget, out raycastHit, collisionLayers)){
			toTarget = raycastHit.point;
		}
		
		//compensate for geometry intersecting with near clip plane
		Vector3 position = GetComponent<Camera>().transform.position;
		GetComponent<Camera>().transform.position = toTarget;
		viewFrustum = CameraController.CalculateViewFrustum(GetComponent<Camera>(), ref nearClipDimensions);
		
		for (int i = 0; i < viewFrustum.Length / 2; i++){
			
			RaycastHit raycastHit2 = new RaycastHit();
			RaycastHit raycastHit3 = new RaycastHit();
			while (Physics.Linecast(viewFrustum[i], viewFrustum[(i + 1) % (viewFrustum.Length / 2)], out raycastHit2) || Physics.Linecast(viewFrustum[(i + 1) % (viewFrustum.Length / 2)], viewFrustum[i], out raycastHit3)){
				
				Vector3 normal = raycastHit.normal;
				if (raycastHit.normal == Vector3.zero && raycastHit2.normal != Vector3.zero){
					normal = raycastHit2.normal;
				}
				
				toTarget += 0.2f * normal;
				GetComponent<Camera>().transform.position += toTarget;
				
				// Recalculate positions of near clip plane
				viewFrustum = CameraController.CalculateViewFrustum(GetComponent<Camera>(), ref nearClipDimensions);
				
			}
			
		}
		GetComponent<Camera>().transform.position = position;
		viewFrustum = CameraController.CalculateViewFrustum(GetComponent<Camera>(), ref nearClipDimensions);
		
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	private void OnGUI() {
		
		if (barTexture != null){
			//drawing top bar
			GUI.DrawTexture(new Rect(0f, -(barCoverage * 2.5f) / 500f * Screen.height, Screen.width, barHeight / 500f * Screen.height), barTexture);
			
			//drawing bottom bar
			GUI.DrawTexture(new Rect(0f, Screen.height * (barCoverage * 2.5f / 500f) + Screen.height, Screen.width, -(barHeight / 500f * Screen.height)), barTexture);
		}
		
	}

	public static Vector3[] CalculateViewFrustum(Camera cam, ref Vector3 dimensions) {
		
		Vector3[] frustrum = new Vector3[8];
		
		//near clipping plane bounds
		frustrum[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.nearClipPlane));
		frustrum[1] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, cam.nearClipPlane));
		frustrum[3] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, cam.nearClipPlane));
		frustrum[2] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, cam.nearClipPlane));
		
		//clipping planes (0 is left, 1 is right, 2 is bottom, 3 is top, 4 is near, and 5 is far)
		Plane[] frustrum2 = GeometryUtility.CalculateFrustumPlanes(cam);
		frustrum[4] = Vector3.Cross(frustrum2[0].normal, frustrum2[2].normal);
		frustrum[5] = Vector3.Cross(frustrum2[3].normal, frustrum2[0].normal);
		frustrum[6] = Vector3.Cross(frustrum2[1].normal, frustrum2[3].normal);
		frustrum[7] = Vector3.Cross(frustrum2[2].normal, frustrum2[1].normal);
		
		//dimensions
		dimensions.x = (frustrum[0] - frustrum[3]).magnitude;
		dimensions.y = (frustrum[1] - frustrum[0]).magnitude;
		dimensions.z = (frustrum[0] - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane))).magnitude;
		
		return frustrum;
		
	}
	
}