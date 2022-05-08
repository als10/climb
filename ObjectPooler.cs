using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
[System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;

        public List<Sprite> sprites;
    }

    public List<Pool> pools;
    public Dictionary<string, List<Queue<GameObject>>> poolDict;

    public static ObjectPooler Instance;


    void Awake() 
    {
        Instance = this;

        poolDict = new Dictionary<string, List<Queue<GameObject>>>();

        foreach (Pool pool in pools)
        {
            List<Queue<GameObject>> objPoolLst = new List<Queue<GameObject>>();

                foreach (Sprite sprite in pool.sprites)
                {
                    Queue<GameObject> objectPool = new Queue<GameObject>();

                    for (int i = 0; i < pool.size; i++)
                    {
                        GameObject obj = Instantiate(pool.prefab);
                        obj.GetComponent<SpriteRenderer>().sprite = sprite;
                        obj.SetActive(false);
                        objectPool.Enqueue(obj);
                    }
                    objPoolLst.Add(objectPool);
                }
            poolDict.Add(pool.tag, objPoolLst);
        }
    }

    public GameObject spawnFromPool (float height, int colour)
    {
        string tag = getRandomDirection();
        GameObject objToSpawn = poolDict[tag][colour].Dequeue();

        objToSpawn.SetActive(true);
        Vector3 pos = objToSpawn.transform.position;
        pos.y = height;
        objToSpawn.transform.position = pos;

        poolDict[tag][colour].Enqueue(objToSpawn);
        return objToSpawn;
    }

    private string getRandomDirection()
    {
        int random = Random.Range(0, 100);
        if (random < 50) { return "left"; }
        return "right";
    }
}