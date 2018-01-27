using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OverlayElement : MonoBehaviour
{
    public enum OverlayMovement
    {
        StayOnWorld,
        BloomOnWorld,
        StayInPlace,
        BloomInPlace,
        Hover,
        Orbit,
        ClampToScreenEdge
    }

    public float spawnTime = 0;
    public float lifetime;
    public float zDepth;
    public OverlayMovement movementType = OverlayMovement.StayOnWorld;
    public Transform worldParent; //plus world position
    public Vector3 worldPosition; //if world parent is null, will use this badboy instead
    public Vector3 screenDifference; //additive
    public bool isActive = false;
    public Image sprite;
    public Text label;
    public float pixelBorder = 15;

    public float FIXEDHEIGHT = 1080f;

    /// <summary>
    /// Ticks for one frame if this element is active.
    /// </summary>
    /// <returns>True if active.</returns>
    public bool Run()
    {
        if (isActive)
        {
            Tick();
            return true;
        }
        else
        {
            PooledObject po = GetComponent<PooledObject>();
            if (po) { po.ReturnToPool(); }
            return false;
        }

    }


    public virtual void Tick()
    {
        if (GameManager.instance == null)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        if (lifetime != 0 && Time.realtimeSinceStartup - lifetime > spawnTime)
            isActive = false;

        Vector3 pos = Vector3.zero;
        if (worldParent)
            pos = worldParent.position;
        pos += worldPosition;
        float screenHeight = Screen.height;//GameUIManager.getScreenHeight();
        float screenWidth = Screen.width;//GameUIManager.getScreenWidth();
        //Debug.Log("screen height: " + screenHeight + ", screen width: " + screenWidth);
        //FIXEDHEIGHT = screenHeight;

        if (movementType == OverlayMovement.BloomOnWorld)
        {
            Vector3 screenPos = GameManager.instance.ActiveCamera.WorldToScreenPoint(pos) + screenDifference + new Vector3(0, ((Time.realtimeSinceStartup - spawnTime) / lifetime) * 100f, 0);
            /*int pseudoHorizontalRes = Mathf.RoundToInt(screenWidth);
            transform.localPosition = new Vector3((screenPos.x - (screenWidth / 2)) * (pseudoHorizontalRes / (float)screenWidth),
                (screenPos.y - (screenHeight / 2)), zDepth);*/

            int pseudoHorizontalRes = Mathf.RoundToInt(((float)Screen.width / (float)Screen.height) * FIXEDHEIGHT);
            transform.localPosition = new Vector3((screenPos.x - (Screen.width / 2)) * (pseudoHorizontalRes / (float)Screen.width),
                (screenPos.y - (Screen.height / 2)) * (FIXEDHEIGHT / (float)Screen.height), zDepth);
        }
        else if (movementType == OverlayMovement.StayOnWorld)
        {
            Vector3 screenPos = GameManager.instance.ActiveCamera.WorldToScreenPoint(pos) + screenDifference;

            if (screenPos.z < 0)
            { //not visible, because it is behind
                if (sprite)
                {
                    sprite.enabled = false;
                }
            }
            else
            {
                if (sprite)
                {
                    sprite.enabled = true;
                }
            }

            //Debug.Log("screen pos: " + screenPos + ", world pos: " + pos);
            /*int pseudoHorizontalRes = Mathf.RoundToInt(screenWidth);
            transform.localPosition = new Vector3((screenPos.x - (screenWidth / 2)) * (pseudoHorizontalRes / (float)screenWidth),
                (screenPos.y - (screenHeight / 2)), zDepth);*/
            int pseudoHorizontalRes = Mathf.RoundToInt(((float)Screen.width / (float)Screen.height) * FIXEDHEIGHT);
            transform.localPosition = new Vector3((screenPos.x - (Screen.width / 2)) * (pseudoHorizontalRes / (float)Screen.width),
                (screenPos.y - (Screen.height / 2)) * (FIXEDHEIGHT / (float)Screen.height), zDepth);
        }
        else if (movementType == OverlayMovement.Hover)
        {
            Vector3 screenPos = GameManager.instance.ActiveCamera.WorldToScreenPoint(pos) + screenDifference;
            /*int pseudoHorizontalRes = Mathf.RoundToInt(screenWidth);
            transform.localPosition = new Vector3((screenPos.x - (screenWidth / 2)) * (pseudoHorizontalRes / (float)screenWidth),
                ((screenPos.y - (screenHeight / 2))) + (Mathf.PingPong(DeadTimer.time, 1) * 30), zDepth);*/
            int pseudoHorizontalRes = Mathf.RoundToInt(((float)Screen.width / (float)Screen.height) * FIXEDHEIGHT);
            transform.localPosition = new Vector3((screenPos.x - (Screen.width / 2)) * (pseudoHorizontalRes / (float)Screen.width),
                ((screenPos.y - (Screen.height / 2)) * (FIXEDHEIGHT / (float)Screen.height)) + (Mathf.PingPong(Time.time, 1) * 30), zDepth);
        }
        else if (movementType == OverlayMovement.Orbit)
        {
            Vector3 screenPos = GameManager.instance.ActiveCamera.WorldToScreenPoint(pos) + screenDifference;
            int pseudoHorizontalRes = Mathf.RoundToInt(((float)Screen.width / (float)Screen.height) * FIXEDHEIGHT);
            transform.localPosition = new Vector3(((screenPos.x - (Screen.width / 2)) * (pseudoHorizontalRes / (float)Screen.width)) + (Mathf.Cos(Time.time * 2) * 50),
                ((screenPos.y - (Screen.height / 2)) * (FIXEDHEIGHT / (float)Screen.height)) + (Mathf.Sin(Time.time * 2) * 50), zDepth);
        }
        else if (movementType == OverlayMovement.ClampToScreenEdge)
        {
            if (worldParent == null)
            {
                isActive = false;
            }

            //clamp to a position on screen

            // if ((screenPos.x < -Screen.width * .5f) || (screenPos.x > Screen.width * .5f)
            //    || (screenPos.y < -Screen.height * .5f) || (screenPos.y > Screen.height * .5f))
            if (gameObject.activeSelf)
            {
                Vector3 screenPos = GameManager.instance.ActiveCamera.WorldToScreenPoint(pos) + screenDifference;

                float borderSize = FIXEDHEIGHT / Screen.height * pixelBorder;
                if (screenPos.z < 0)
                {
                    screenPos *= -1;
                }
                screenPos = new Vector3(
                    Mathf.Clamp(screenPos.x, borderSize, Screen.width - borderSize),
                    Mathf.Clamp(screenPos.y, borderSize, Screen.height - borderSize),
                    zDepth);
                int pseudoHorizontalRes = Mathf.RoundToInt(((float)Screen.width / (float)Screen.height) * FIXEDHEIGHT);
                transform.localPosition = new Vector3(
                    (screenPos.x - (Screen.width / 2)) * (pseudoHorizontalRes / (float)Screen.width),
                    (screenPos.y - (Screen.height / 2)) * (FIXEDHEIGHT / (float)Screen.height),
                    zDepth);
            }
        }
    }
}
