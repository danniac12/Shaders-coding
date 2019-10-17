using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proxcimacion : MonoBehaviour
{
    public Transform other;
    float dis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       dis =  Vector3.Distance(gameObject.transform.position, other.position);
        if(dis < 9)
        {
            GetComponent<Renderer>().material.SetFloat("_Valor", dis);
        }
        else
        {
            GetComponent<Renderer>().material.SetFloat("_Valor", 9);
        }
        if (dis <= 1)
        {

            GetComponent<Renderer>().material.SetFloat("_Valor", 1);
        }

     
        
    }
}
