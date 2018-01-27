using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager instance;
    public List<OverlayElement> overlayElements = new List<OverlayElement>();

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        overlayElements.RemoveAll(element => !element.Run());
    }

    public OverlayElement SpawnOverlayElement(string pooledName, Transform overlayParent, OverlayElement.OverlayMovement overlayType = OverlayElement.OverlayMovement.StayOnWorld)
    {
        OverlayElement offscreenOverlay = PoolManager.instance.SpawnPooledObject(pooledName).GetComponent<OverlayElement>();

        offscreenOverlay.movementType = overlayType;
        offscreenOverlay.worldParent = overlayParent;
        offscreenOverlay.isActive = true;
        offscreenOverlay.transform.parent = transform;
        offscreenOverlay.transform.localScale = new Vector3(1, 1, 1);
        overlayElements.Add(offscreenOverlay);
        offscreenOverlay.Tick();

        return offscreenOverlay;
    }

    public OverlayElement SpawnOverlayText(Transform overlayParent, string textContents, Color color, int fontSize, OverlayElement.OverlayMovement overlayType = OverlayElement.OverlayMovement.StayOnWorld)
    {
        OverlayElement offscreenOverlay = PoolManager.instance.SpawnPooledObject("overlayText").GetComponent<OverlayElement>();

        Text text = offscreenOverlay.GetComponent<Text>();
        text.text = textContents;
        text.color = color;
        text.fontSize = fontSize;

        offscreenOverlay.movementType = overlayType;
        offscreenOverlay.worldParent = overlayParent;
        offscreenOverlay.isActive = true;
        offscreenOverlay.transform.parent = transform;
        offscreenOverlay.transform.localScale = new Vector3(1, 1, 1);
        overlayElements.Add(offscreenOverlay);
        offscreenOverlay.Tick();

        return offscreenOverlay;
    }
}
