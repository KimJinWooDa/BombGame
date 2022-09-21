using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPooling : MonoBehaviour
{
    public static NormalPooling Instance;

    [SerializeField] GameObject poolObject;

    Queue<Transform> poolQueue = new Queue<Transform>();

    private void Awake()
    {
        Instance = this;
        InitObject(10);
    }


    //Create
    Transform CreateObject()
    {
        Transform obj = Instantiate(poolObject, transform).GetComponent<Transform>();
        obj.gameObject.SetActive(false);

        return obj;
    }

    //Init
    void InitObject(int num)
    {
        for(int i=0; i < num; i++)
        {
            poolQueue.Enqueue(CreateObject());
        }
    }

    //Return
    public static void ReturnObject(Transform obj)
    {
        obj.SetParent(Instance.transform, true);
        obj.gameObject.SetActive(false);
        Instance.poolQueue.Enqueue(obj);
    }

    //Get
    public Transform GetObject()
    {
        if(poolQueue.Count > 0)
        {
            Transform obj = poolQueue.Dequeue();
            obj.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            Transform newObj = CreateObject();
            newObj.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }
}
