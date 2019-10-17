using UnityEngine;
using System.Collections;
//this script should be attached to the DoorSensor GameObject
public class DoorOpener : MonoBehaviour {
	public GameObject doorModel;
   
	public AudioClip openingSound;
	public AudioClip closingSound;
	
	public GameObject doorOpenPosition1;
	public GameObject doorOpenPosition2;
	public GameObject doorOpenSensor1;
	public GameObject doorOpenSensor2;
	public float openingSpeed = 2.0f;
	public float closingSpeed = 1.0f;
	public float delayTime = 1.0f;
	private int _openDoor = 0;
	private bool _playerOutOfDoorZone = true;
	private bool _doorClosing = true;
	private Quaternion _beginRotation;
	private Vector3 _beginPosition;
	private Vector3 _openPos1;
	private Vector3 _openPos2;
	private Quaternion _Pos1Rotation;
	private Quaternion _Pos2Rotation;
	private int _playonce = 0;
	void Start(){
		//set the closed door rotation and position
		_beginRotation = doorModel.transform.rotation;
		_beginPosition = doorModel.transform.position;
		
		_openPos1 = doorOpenPosition1.transform.position;
		_openPos2 = doorOpenPosition2.transform.position;
		_Pos1Rotation = doorOpenPosition1.transform.rotation;
		_Pos2Rotation = doorOpenPosition2.transform.rotation;
		//visual cleanup
		DestroyImmediate(doorOpenPosition1);
		DestroyImmediate(doorOpenPosition2);
		
	//	doorOpenSensor1.transform.renderer.enabled = false;
	//	doorOpenSensor2.transform.renderer.enabled = false;
	}
	void OnTriggerEnter(Collider other) {
		//is the door closed?
		if(_openDoor == 0  ){
           
			//which side of the door are we located?
			float distance1 = Vector3.Distance (other.transform.position, doorOpenSensor1.transform.position);
			float distance2 = Vector3.Distance (other.transform.position, doorOpenSensor2.transform.position);
			//Open door away from the closest sensor
			if(distance1 > distance2){
				_doorClosing = false;
				_openDoor = 1;
				StartCoroutine(Wait(delayTime));
			} else {
				_doorClosing = false;
				_openDoor = 2;
				StartCoroutine(Wait(delayTime));
			}
            
        }
       
		//we are located inside the doorTriggerZone
		_playerOutOfDoorZone = false;
    }
	//wait untill the door is allowed to be closed, then set it to true
    IEnumerator Wait(float delayTime) {
        yield return new WaitForSeconds(delayTime);
       	_doorClosing = true;
    }
	
	//we are leaving the doorTriggerZone
	void OnTriggerExit(Collider other) {
		_playerOutOfDoorZone = true;
       
    }
	
	void Update(){	
		//is the door allowed to close?
		if(_doorClosing == true && _playerOutOfDoorZone == true){
			_openDoor = 0;
		}
		//Rotate and move the door to the desired setting
		if(_openDoor == 1){
			doorModel.transform.rotation = Quaternion.Lerp (doorModel.transform.rotation, _Pos2Rotation, Time.deltaTime * openingSpeed);
			doorModel.transform.position = Vector3.Lerp (doorModel.transform.position, _openPos2, Time.deltaTime * openingSpeed);
			if(_playonce != _openDoor){
				_playonce = _openDoor;
                AudioSource Open = GetComponent<AudioSource>();
                openingSound = Open.GetComponent<AudioClip>();
                Open.Play(44100);
            }
		}
		if(_openDoor == 2){
			doorModel.transform.rotation = Quaternion.Lerp (doorModel.transform.rotation, _Pos1Rotation, Time.deltaTime * openingSpeed);
			doorModel.transform.position = Vector3.Lerp (doorModel.transform.position, _openPos1, Time.deltaTime * openingSpeed);
			if(_playonce != _openDoor){
				_playonce = _openDoor;
                AudioSource Open = GetComponent<AudioSource>();
                openingSound = Open.GetComponent<AudioClip>();
                Open.Play(44100);
            }
		}
		if(_openDoor == 0){
			
			doorModel.transform.rotation = Quaternion.Lerp (doorModel.transform.rotation, _beginRotation, Time.deltaTime * closingSpeed);
			doorModel.transform.position = Vector3.Lerp (doorModel.transform.position, _beginPosition, Time.deltaTime * closingSpeed);
			if(_playonce != _openDoor){
				_playonce = _openDoor;
                AudioSource closing = GetComponent<AudioSource>();
                closingSound = closing.GetComponent<AudioClip>();
                closing.Play(44100);
            }
		}
	}
	
}
