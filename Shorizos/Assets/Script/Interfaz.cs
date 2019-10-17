using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Interfaz : MonoBehaviour
{
    public static bool permitir;
    public static bool denegar;
    public TextMeshProUGUI souls;
    public TextMeshProUGUI soulsCount;
    public GameObject deny;
    int temporal;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        souls.text ="x"+Souls.soulCount.ToString();
        soulsCount.text =Souls.soulCount.ToString();
        if(Souls.soulCount == 15)
        {
            permitir = true;
            
        }
        if(denegar == true)
        {
            deny.SetActive(true);
            Invoke("SetOff", 5);
        }
      
            
    }
    void SetOff()
    {
        deny.SetActive(false);
        denegar = false;
    }
}
