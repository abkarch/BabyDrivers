using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SplitScreen : MonoBehaviour {

    public int numOfPanels=1;
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera cam4;

    //Call this with int 1-4 to choose amount of screens
    public void NumSplitScreenPanels(int panels)
    {
        switch (panels)
        {
            case 1:
                Debug.Log("1 screen");
                cam1.enabled = true;
               // cam2.enabled = false;
              //  cam3.enabled = !true;
               // cam4.enabled = !true;
                cam1.rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                Debug.Log("2 screens");
                cam1.enabled = true;
                cam2.enabled = true;
            //    cam3.enabled = !true;
             //   cam4.enabled = !true;
                cam1.rect = new Rect(0, 0.5f, 1, 0.5f);
                cam2.rect = new Rect(0, 0, 1, 0.5f);
                break;
            case 3:
                Debug.Log("3 screens");
                cam1.enabled = true;
                cam2.enabled = true;
                cam3.enabled = true;
                cam4.enabled = !true;
                cam1.rect = new Rect(0, 0.5f, 1, 0.5f);
                cam2.rect = new Rect(0, 0, 0.5f, 0.5f);
                cam3.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                break;
            case 4:
                Debug.Log("4 screens");
                cam1.enabled = true;
                cam2.enabled = true;
                cam3.enabled = true;
                cam4.enabled = true;
                cam1.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                cam2.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                cam3.rect = new Rect(0, 0, 0.5f, 0.5f);
                cam4.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                break;
               
            
                }
    }

    
    public void setCam(int camNumber, Camera c) {
        if(camNumber == 1) {
            cam1 = c;
            PlayerNames.player1Cam = c.transform;
        } else if (camNumber == 2) {
            cam2 = c;
            PlayerNames.player2Cam = c.transform;
        } else if (camNumber == 3) {
            cam3 = c;
            PlayerNames.player3Cam = c.transform;
        } else {
            cam4 = c;
            PlayerNames.player4Cam = c.transform;
        }
    }
	
}
