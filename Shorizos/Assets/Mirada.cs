using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirada : MonoBehaviour
{
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player.transform.position);
    }
}
