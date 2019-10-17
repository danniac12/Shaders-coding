using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class RandomDestroy : MonoBehaviour {

	void Start(){
		var randomDestroy = Random.Range(0,2);
		if(randomDestroy == 0){
			DestroyImmediate(this.transform.gameObject);	
		}else{
			DestroyImmediate(this);	
		}
	}
}
