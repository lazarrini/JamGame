using System;
using UnityEngine;
using System.Collections.Generic;

public class PopupPool : MonoBehaviour
{
    
    public static PopupPool Instance;
    private Queue<GameObject> pool = new Queue<GameObject>();
    [SerializeField] private int initialSize = 20;
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private Transform canvasParent;
    
    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(popupPrefab, canvasParent);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    
    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(popupPrefab, canvasParent);
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
