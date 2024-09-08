using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicPool : MonoBehaviour
{
    public List<Pool> pools = new List<Pool>();

    internal static DynamicPool instance;

    private void Awake()
    {
        instance = this;
        pools.Clear();
    }

    public static GameObject GetPoolObject(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        Pool pool = GetPoolByGameObject(prefab);

        bool poolExists = instance.pools.Count > 0 && pool != null;

        GameObject newObject = null;

        if (poolExists)
        {
             newObject = GetInActivePoolGameObject(pool);

            if(newObject != null)
            {
                newObject.transform.position = pos;
                newObject.transform.rotation = rotation;

                newObject.SetActive(true);
            }
            else
            {
                newObject = Instantiate(prefab, pos, rotation);

                pool.poolObjects.Add(newObject);
            }
        }
        else
        {
            newObject = Instantiate(prefab, pos, rotation, instance.transform);

            if(pool == null)
            {
                instance.pools.Add(new Pool(prefab, newObject));
            }
        }

        return newObject;
    }

    public static Pool GetPoolByGameObject(GameObject gameObject) => instance.pools.FirstOrDefault(pool => pool.keyObject == gameObject);
    public static GameObject GetInActivePoolGameObject(Pool pool) => pool.poolObjects.FirstOrDefault(gameObject => !gameObject.activeInHierarchy);
}

[System.Serializable]
public class Pool
{
    public GameObject keyObject;

    public List<GameObject> poolObjects = new List<GameObject>();

    public Pool(GameObject keyObject, GameObject newObject)
    {
        this.keyObject = keyObject;
        this.poolObjects.Add(newObject);
    }
}
