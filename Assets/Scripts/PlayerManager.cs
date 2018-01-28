using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public int numberOfPlayers;
    public GameObject[] playerPrefabs;
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
        for (int i = 0; i < numberOfPlayers; i++) {
            GameObject g = GameObject.Instantiate(playerPrefabs[i]);
            Baby b = g.GetComponent<Baby>();
            if (b != null)
            {
                b.SetPlayerNum(i + 1);
            }
            g.transform.position = gameObject.transform.position;
            g.transform.parent = gameObject.transform;
			GameObject cam = GameObject.Instantiate(CameraPrefab);
			ThirdPersonCamera tpc = cam.GetComponent<ThirdPersonCamera>();
			tpc.Initialize(g);
			ss.setCam(i + 1, tpc.Cam);
        }
        gameObject.GetComponent<SplitScreen>().NumSplitScreenPanels(numberOfPlayers);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
