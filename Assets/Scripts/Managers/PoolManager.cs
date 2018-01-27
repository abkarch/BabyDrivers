using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolEntry
    {
        public string pooledObjectName;
        public PooledObject pooledObject;
        public int countToPool = 1;
        public int layerOverride = -1;
    }

    public static PoolManager instance;
    public PoolEntry[] pooledObjectPrefabs;
    private Dictionary<string, PooledObject[]> pooledObjects;
    public List<PooledObject> tickingObjects;

    // Use this for initialization
    void Awake()
    {
        pooledObjects = new Dictionary<string, PooledObject[]>(pooledObjectPrefabs.Length);
        instance = this;

        for (int e = 0; e < pooledObjectPrefabs.Length; e++)
        {
            PooledObject[] tempArray = new PooledObject[pooledObjectPrefabs[e].countToPool];
            for (int x = 0; x < pooledObjectPrefabs[e].countToPool; x++)
            {
                tempArray[x] = POInstantiate(x, e);
            }
            pooledObjects.Add(pooledObjectPrefabs[e].pooledObject.dictionaryName, tempArray);
        }

        tickingObjects = new List<PooledObject>();
    }

    void Update()
    {
        for (int i = 0; i < tickingObjects.Count; i++)
        {
            if (tickingObjects[i].inPlay == false)
            {
                tickingObjects.RemoveAt(i);
                i--;
                continue;
            }

            tickingObjects[i].Tick();
        }
    }

    public void clearTickingObjects()
    {
        for (int i = 0; i < tickingObjects.Count; i++)
        {
            tickingObjects[i].ReturnToPool();
            tickingObjects.RemoveAt(i);
            i--;
        }
    }

    public void ReturnAllOfType(string pooledType)
    {
        if (pooledObjects.ContainsKey(pooledType))
        {
            foreach (PooledObject po in pooledObjects[pooledType])
            {
                po.ReturnToPool();
            }
        }
    }
    public void ReturnAllOfTypeContaining(string pooledType)
    {
        foreach (string s in pooledObjects.Keys)
        {
            if (s.Contains(pooledType))
            {
                foreach (PooledObject po in pooledObjects[s])
                {
                    po.ReturnToPool();
                }
            }
        }
    }

    public PooledObject POInstantiate(int x, int e)
    {
        PooledObject tempObject = Instantiate(pooledObjectPrefabs[e].pooledObject, new Vector3(-50000, -50000 + (x * 100), -50000 + (e * 100)), Quaternion.identity) as PooledObject;
        tempObject.name = tempObject.dictionaryName + " " + x.ToString();
        tempObject.transform.parent = transform;
        tempObject.poolStorageLocation = tempObject.transform.position;
        tempObject.arrayIndex = e;
        tempObject.WakeUp();
        tempObject.gameObject.SetActive(false);
        if (pooledObjectPrefabs[e].layerOverride != -1)
            tempObject.gameObject.SetLayerRecursively(pooledObjectPrefabs[e].layerOverride);
        DontDestroyOnLoad(tempObject.gameObject);

        return tempObject;
    }

    public PooledObject SpawnPooledObject(string objectType, Vector3 position, Quaternion rotation)
    {
        return SpawnPooledObject(objectType, position, rotation.eulerAngles);
    }

    public PooledObject SpawnPooledObject(string objectType)
    {
        return SpawnPooledObject(objectType, transform.position, transform.rotation);
    }

    public PooledObject SpawnPooledObject(string objectType, Vector3 position, Vector3 eulerAngles)
    {
        PooledObject[] tempArray = pooledObjects[objectType];
        if (tempArray == null)
            return null;

        for (int x = 0; x < tempArray.Length; x++)
        {
            if (!tempArray[x].inPlay)
            {
                tempArray[x].gameObject.SetActive(true);
                tempArray[x].inPlay = true;
                tempArray[x].transform.position = position;
                tempArray[x].transform.eulerAngles = eulerAngles;
                tempArray[x].Initialize();
                return tempArray[x];
            }
        }

        //need to increase array size 1.5x
        PooledObject[] newArray = new PooledObject[Mathf.CeilToInt((float)tempArray.Length * 1.5f)];
        Debug.LogWarning("increasing object type " + objectType + " array length to " + newArray.Length);
        for (int i = 0; i < tempArray.Length; i++)
            newArray[i] = tempArray[i];

        int retIndex = tempArray.Length;

        //instantiate new objects to fill pool
        for (int i = tempArray.Length; i < newArray.Length; i++)
        {
            newArray[i] = POInstantiate(i, newArray[0].arrayIndex);
        }

        pooledObjects[objectType] = newArray;

        {
            newArray[retIndex].gameObject.SetActive(true);
            newArray[retIndex].inPlay = true;
            newArray[retIndex].transform.position = position;
            newArray[retIndex].transform.eulerAngles = eulerAngles;
            newArray[retIndex].Initialize();
            return newArray[retIndex];
        }
    }

    [ContextMenu("give them some helpy names")]
    void HelpyNames()
    {
        for (int i = 0; i < pooledObjectPrefabs.Length; i++)
        {
            pooledObjectPrefabs[i].pooledObjectName = pooledObjectPrefabs[i].pooledObject.dictionaryName;
        }
    }
}
