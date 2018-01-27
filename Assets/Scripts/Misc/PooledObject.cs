using UnityEngine;
using System.Collections;

[System.Serializable]
public class PooledObject : MonoBehaviour
{
    public string dictionaryName;
    [System.NonSerialized]
    public int arrayIndex;
    [System.NonSerialized]
    public bool inPlay = false;
    public MonoBehaviour[] toDisableInPool;
    [System.NonSerialized]
    public Vector3 poolStorageLocation; //where the thing hangs out while not in play

    public virtual void WakeUp()
    {
        ReturnToPool();
    }

    public virtual void Initialize()
    {
        gameObject.SetActive(true);
        foreach (MonoBehaviour b in toDisableInPool)
            b.enabled = true;
    }

    [ContextMenu("Return to pool")]
    public virtual void ReturnToPool()
    {
        foreach (MonoBehaviour b in toDisableInPool)
            b.enabled = false;
        transform.position = poolStorageLocation;
        if (PoolManager.instance != null)
        {
            if (transform.parent == null)
                transform.parent = PoolManager.instance.transform;
            gameObject.SetActive(false);
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
        inPlay = false;
    }

    public virtual bool Tick()
    {
        return true;
    }
}
