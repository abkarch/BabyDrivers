using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBabyBase : MonoBehaviour
{
    public GameObject BabyBase1;
    public GameObject BabyBase2;
    public GameObject BabyBase3;
    public GameObject BabyBase4;


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

        if(Input.GetKeyDown("1"))
        {
            BabyBase1.SetActive(true);
        }
        else
        if(Input.GetKeyDown("2"))
        {
            BabyBase2.SetActive(true);
        }
        else
        if(Input.GetKeyDown("3"))
        {
            BabyBase3.SetActive(true);
        }
        else
        if(Input.GetKeyDown("4"))
        {          
            BabyBase4.SetActive(true);
        }

    }
}

 
