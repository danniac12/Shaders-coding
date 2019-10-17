using UnityEngine;
using System.Collections;

public class PlacePlayerOnSpawn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		var buildPointer = GameObject.FindWithTag("BuildPoint");
		if (buildPointer != null ){
			this.transform.position = buildPointer.transform.position;
		} else {
			print ("generate a level before playing one");
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
