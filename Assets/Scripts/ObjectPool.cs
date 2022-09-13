﻿using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private int maxPoolSize;
    private GameObject prefab = default;
    private Queue<GameObject> pool = default;

    public int Size => pool.Count;
    
    public void InitPool(GameObject obj, int amount = 30)
    {
        maxPoolSize = amount;
        prefab = obj;
        pool = new Queue<GameObject>();
        GrowPool();
    }

    /// <summary>
    /// Devuelve un objeto de la pool
    /// </summary>
    /// <returns></returns>
    public GameObject GetFromPool()
    {
        if(pool.Count <= 0)
            GrowPool();
        
        var nextObj = pool.Dequeue();
        nextObj.SetActive(true);
        return nextObj;
    }

    /// <summary>
    /// Retorna un objeto a la pool
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnToPool(GameObject obj, bool active = false)
    {
        obj.transform.SetAsLastSibling();
        obj.SetActive(active);
        pool.Enqueue(obj);
    }

    /// <summary>
    /// Resetea la pool
    /// </summary>
    public void ResetPool(bool active = false)
    {
        pool.Clear();
        
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ReturnToPool(transform.GetChild(i).gameObject, active);
            }
        }
    }

    /// <summary>
    /// Hace crecer la pool si fuera necesario
    /// </summary>
    private void GrowPool()
    {
        for (int i = 0; i < maxPoolSize; i++)
        {
            var newObj = Instantiate(prefab, transform);
            newObj.SetActive(false);
            pool.Enqueue(newObj);
        }
    }

    /// <summary>
    /// Devuelve la cantidad de objetos activos en la pool
    /// </summary>
    /// <returns></returns>
    public int GetAmountActiveObjects()
    {
        int counter = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            counter = (transform.GetChild(i).gameObject.activeInHierarchy) ? counter + 1 : counter;
        }
        
        return counter;
    }
}