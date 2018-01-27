using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public BabyCarController babyCarController;

    public int score;

    public Camera ActiveCamera
    {
        get
        {
            return Camera.main;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //The following is just testing to prove the concept of overlay elements - TODO: remove
        if (babyCarController != null)
        {
            if (OverlayManager.instance != null)
            {
                OverlayManager.instance.SpawnOverlayText(babyCarController.transform, "CAR!", Color.blue, 50);
            }
        }
    }

}
