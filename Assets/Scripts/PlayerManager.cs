using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public int numberOfPlayers;
    public GameObject[] playerPrefabs;
    private SplitScreen ss;


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
            g.transform.position = gameObject.transform.position;
            g.transform.parent = gameObject.transform;
            ss.setCam(i + 1, g.GetComponentInChildren<Camera>());
        }
        gameObject.GetComponent<SplitScreen>().NumSplitScreenPanels(numberOfPlayers);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
