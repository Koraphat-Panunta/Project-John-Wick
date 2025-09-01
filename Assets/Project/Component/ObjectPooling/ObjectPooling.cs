using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling<T> where T : Component
{
    public T prefab { get; protected set; }
    public Queue<T> pool { get; protected set; }
    public int poolMaxSize { get; protected set; }
    public Vector3 voidPos { get; protected set; }
    public ObjectPooling(T prefab,int poolMaxSize,int initiateSize,Vector3 voidPos)
    {
        this.prefab = prefab;
        this.poolMaxSize = poolMaxSize;
        this.voidPos = voidPos;

        pool = new Queue<T>();

        for (int i = 0; i < initiateSize; i++) 
        {
            T obj = GameObject.Instantiate(prefab,this.voidPos,Quaternion.identity);
            pool.Enqueue(obj);
            obj.gameObject.SetActive(false);
        }
    }

    public T Get()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : GameObject.Instantiate(prefab, voidPos,Quaternion.identity);
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        if(pool.Count < poolMaxSize)
            pool.Enqueue(obj);
        else
            GameManager.Destroy(obj.gameObject);
    }
}
