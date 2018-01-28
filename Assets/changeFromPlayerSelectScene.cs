using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeFromPlayerSelectScene : MonoBehaviour {
    
    public GameObject field1;
    public GameObject field2;
    public GameObject field3;
    public GameObject field4;

	public void ChangeToScene (string scenetochangeTo) {
        PlayerNames names = new PlayerNames();
        
        SaveNameToArray(field1, 1, names);
        SaveNameToArray(field2, 2, names);
        SaveNameToArray(field3, 3, names);
        SaveNameToArray(field4, 4, names);

        Debug.Log(names.names[0] + names.names[1] + names.names[2]  + names.names[3]);

        
        Application.LoadLevel(scenetochangeTo);		
	}

    private void SaveNameToArray(GameObject field, int playerNumber,PlayerNames names)
    {
        string input = field.GetComponent<InputField>().text;

        if (input.Length > 0)
            names.names[playerNumber-1] = input;
        else
            names.names[playerNumber -1] = "Player " + playerNumber;
    }
}
