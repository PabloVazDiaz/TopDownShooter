using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum PoolObject
    {
        PlayerBullet,
        EnemyBullet,
        BunnyEnemy,
        BearEnemy
    }
public class ObjectPooler : MonoBehaviour
{


    [System.Serializable]
    public class Pool
    {
        public PoolObject tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<PoolObject, Queue<GameObject>> poolDictionary;
    public Transform poolRecipient;

    public static ObjectPooler instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        poolDictionary = new Dictionary<PoolObject, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPoll = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, poolRecipient);
                obj.SetActive(false);
                objectPoll.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPoll);
        }
    }


    public GameObject SpawnFromPool( PoolObject tag, Vector3 position, Quaternion rotation)
    {
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        objToSpawn.GetComponent<IPooledObject>().OnObjectSpawn();
        objToSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }
}
