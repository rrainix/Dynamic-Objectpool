using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicPool : MonoBehaviour
{
    //The List of dynamic pools, can be set as private or public.
    public List<Pool> pools = new List<Pool>();

     //Singleton in order to acces the functions from this class more easily
    internal static DynamicPool instance;

    private void Awake()
    {
        instance = this;
        pools.Clear();
    }
                                           //No World objects, only prefabs.
    public static GameObject GetPoolObject(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        Pool pool = GetPoolByGameObject(prefab);

        bool poolExists = instance.pools.Count > 0 && pool != null;

        GameObject newObject = null;

        if (poolExists)
        {
             //Search for an InActiveObject in the pool.
             newObject = GetInActivePoolGameObject(pool);

            if(newObject != null && !newObject.activeInHierarchy)
            {
                newObject.transform.position = pos;
                newObject.transform.rotation = rotation;

                newObject.SetActive(true);
            }
            else //Create a new object.
            {
                newObject = Instantiate(prefab, pos, rotation);

                pool.poolObjects.Add(newObject);
            }
        }
        else //Create new pool + new Object within it.
        {
            newObject = Instantiate(prefab, pos, rotation, instance.transform);

            if(pool == null) //Last check weather the pool is null,
            {
                instance.pools.Add(new Pool(prefab, newObject));
            }
        }

        return newObject;
    }

     //These methods are for the main method GetPoolObject().
    public static Pool GetPoolByGameObject(GameObject gameObject) => instance.pools.FirstOrDefault(pool => pool.keyObject == gameObject);
    public static GameObject GetInActivePoolGameObject(Pool pool) => pool.poolObjects.FirstOrDefault(gameObject => !gameObject.activeInHierarchy);
}

[System.Serializable]
public class Pool
{
    public GameObject keyObject;

    public List<GameObject> poolObjects = new List<GameObject>();

    //Constructor with a keyobject to acces the pool and a start object.
    public Pool(GameObject keyObject, GameObject newObject)
    {
        this.keyObject = keyObject;
        this.poolObjects.Add(newObject);
    }
}
