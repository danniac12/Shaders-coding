using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreateAnew : MonoBehaviour
{
    public int scenaNumb;
	void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(scenaNumb);
	    	//StartCoroutine(MyDelayMethod(0.1f));
    	}
		
	}

	IEnumerator MyDelayMethod(float delay)
    {
		
		var LevelSettingsScript = GameObject.FindWithTag("DungeonBuilder").GetComponent<DungeonGenerator>();
		
		yield return new WaitForSeconds(delay);
		
		LevelSettingsScript.PrintIt();
		
	}
}
