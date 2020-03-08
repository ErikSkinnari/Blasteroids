using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public GameObject ObjectToPool;
    public int ObjectStartAmount;
    private Queue<GameObject> objectPoolQueue = new Queue<GameObject>();

    void Awake()
    {
    }

    void Start()
    {
        for (int i = 0; i < ObjectStartAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(ObjectToPool);
            obj.SetActive(false);
            objectPoolQueue.Enqueue(obj);
        }
    }

    public GameObject GetPoolObject()
    {
        if(objectPoolQueue.Count > 0)
        {
            GameObject go = objectPoolQueue.Dequeue();
            return go;
        }
        else
        {
            GameObject newGo = (GameObject)Instantiate(ObjectToPool);
            newGo.SetActive(false);
            return newGo;
        }
    }

    public void ReturnGameObject(GameObject go)
    {
        if (go.activeSelf)
        {
            go.SetActive(false);
        }

        objectPoolQueue.Enqueue(go);
    }
}
