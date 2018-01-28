using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour {

    public int numberOfPlayers;
    public GameObject playerPrefab;
    private SplitScreen ss;

	public GameObject CameraPrefab;

    // Make this game object and all its transform children
    // survive when loading a new scene.
    void Awake() {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {
        ss = GetComponent<SplitScreen>();
        startMatch();
    }

    public void startMatch() {
        numberOfPlayers = PlayerNamesData.playerCount;
        if(numberOfPlayers < 1) {
            numberOfPlayers = 1;
        }
        for (int i = 0; i < numberOfPlayers; i++) {
            GameObject g = GameObject.Instantiate(playerPrefab);
            Baby b = g.GetComponent<Baby>();
            if (b != null)
            {
                b.SetPlayerNum(i + 1);
                ChangeColor(b,i+1);
            }
            g.transform.position = gameObject.transform.position;
            g.transform.parent = gameObject.transform;
			GameObject cam = GameObject.Instantiate(CameraPrefab);
			ThirdPersonCamera tpc = cam.GetComponent<ThirdPersonCamera>();
            if (tpc != null)
            {
                tpc.Initialize(g);
                ss.setCam(i + 1, tpc.Cam);
            }
			PhysicsPlayerController ppc = g.GetComponent<PhysicsPlayerController>();
			if (ppc != null)
			{
				ppc.Initialize(tpc.transform);
			}
        }
        gameObject.GetComponent<SplitScreen>().NumSplitScreenPanels(numberOfPlayers);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void ChangeColor(Baby infant,int playerNum)
    {
         SkinnedMeshRenderer[] bodyParts = infant.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer skin in bodyParts)
        {
            //if(skin.) Make diaper and eyes not match color
            switch (playerNum)
            {
                case 1:
                    Material[] newMat = new Material[] { PlayerNamesData.playerColor1 };
                    skin.materials = newMat;
                    break;
                case 2:
                    Material[] newMat2 = new Material[] { PlayerNamesData.playerColor1 };
                    skin.materials = newMat2;
                    break;
                case 3:
                    Material[] newMat3 = new Material[] { PlayerNamesData.playerColor1 };
                    skin.materials = newMat3;
                    break;
                case 4:
                    Material[] newMat4 = new Material[] { PlayerNamesData.playerColor1 };
                    skin.materials = newMat4;
                    break;
             }
        }
    }

}
