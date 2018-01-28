﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeFromPlayerSelectScene : MonoBehaviour {
    
    public GameObject field1;
    public GameObject field2;
    public GameObject field3;
    public GameObject field4;

    public GameObject BabyBase1;
    public GameObject BabyBase2;
    public GameObject BabyBase3;
    public GameObject BabyBase4;

    
    public Material defaultColor;

    void Start()
    {
        PlayerNamesData.playerColor = new Material[4];

        PlayerNamesData.playerColor[0]= defaultColor;
        PlayerNamesData.playerColor[1] = defaultColor;
        PlayerNamesData.playerColor[2] = defaultColor;
        PlayerNamesData.playerColor[3] = defaultColor;
    }

    void Update()
    {
        if (Input.GetButtonUp("StartGame"))
        {
            StartGame();
        }
    }

    public void ChangeToScene (string scenetochangeTo) {
        string[] names = PlayerNamesData.names;
        
        SaveNameToArray(field1, 1, names);
        SaveNameToArray(field2, 2, names);
        SaveNameToArray(field3, 3, names);
        SaveNameToArray(field4, 4, names);

        Debug.Log(names[0] + names[1] + names[2]  + names[3]);

        PlayerNamesData.playerColor[0] = BabyBase1.GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
        PlayerNamesData.playerColor[1] = BabyBase2.GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
        PlayerNamesData.playerColor[2] = BabyBase3.GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
        PlayerNamesData.playerColor[3] = BabyBase4.GetComponentInChildren<SkinnedMeshRenderer>().materials[0];



        if (BabyBase2.activeSelf)
            PlayerNamesData.playerCount++;
        if (BabyBase3.activeSelf)
            PlayerNamesData.playerCount++;
        if (BabyBase4.activeSelf)
            PlayerNamesData.playerCount++;

        Debug.Log("Num of players:" + PlayerNamesData.playerCount);

        Application.LoadLevel(scenetochangeTo);		
	}

    private void SaveNameToArray(GameObject field, int playerNumber,string[] names)
    {
        string input = field.GetComponent<InputField>().text;

        if (input.Length > 0)
           names[playerNumber-1] = input;
        else
            names[playerNumber -1] = "Player " + playerNumber;
    }

    public void StartGame()
    {
        ChangeToScene("FullTest");
    }
}
