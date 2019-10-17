using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class RandomRotate : MonoBehaviour {
	
	void Start(){
		var rotationVariation = Random.Range(-180,180);
		transform.Rotate(Vector3.up * rotationVariation);
			DestroyImmediate(this);	
	}
}
