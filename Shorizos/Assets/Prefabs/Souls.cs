using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souls : MonoBehaviour
{ 
    public static int soulCount;
    // Start is called before the first frame update
    void Start()
    {
        soulCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            soulCount += 1;
            this.gameObject.SetActive(false);
        }
    }
}
