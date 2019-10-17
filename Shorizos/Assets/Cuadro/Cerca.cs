using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerca : MonoBehaviour
{
    public Texture Hits;
    public GameObject Player;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.SetTexture("_Hit",Hits);
    }

    // Update is called once per frame
    void Update()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
        float Distance = Vector3.Distance(this.transform.position, Player.transform.position);
        Debug.Log(Distance);
        GetComponent<Renderer>().material.SetFloat("_Valor",(Distance* 0.3f));
    }
}
