﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asignate : MonoBehaviour
{
    public Transform centro;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //le da la posicion del jugador al area pintada 
        GetComponent<Renderer>().material.SetVector("_Position", centro.position);
    }
}
