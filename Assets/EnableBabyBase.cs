using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBabyBase : MonoBehaviour
{
    public GameObject BabyBase1;
    public GameObject BabyBase2;
    public GameObject BabyBase3;
    public GameObject BabyBase4;

    public int playerCount;

    void Start()
    {
        //BabyBase = GetComponent<GameObject>();
        BabyBase1.SetActive(false);
        BabyBase2.SetActive(false);
        BabyBase3.SetActive(false);
        BabyBase4.SetActive(false);
    }


    void Update()
    {

        if(Input.GetButton("EnterPositionP1"))
        {
            BabyBase1.SetActive(true);
          
        }
        else
        if(Input.GetButton("EnterPositionP2"))
        {
            BabyBase2.SetActive(true);
            
        }
        else
        if(Input.GetButton("EnterPositionP3"))
        {
            BabyBase3.SetActive(true);
            
        }
        else
        if(Input.GetButton("EnterPositionP4"))
        {          
            BabyBase4.SetActive(true);
            
        }
        
        

    }
}

 
