using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Phone : MonoBehaviour
{
    public PhoneButton[] phoneButtons;
    [SerializeField] private int greenButCount;
    [SerializeField] private int redButCount;
    private List<int> _greenButs = new List<int>();
    private List<int> _redButs = new List<int>();

    private void Awake()
    {
        ChooseButtonsIndexes();
    }

    private void ChooseButtonsIndexes()
    {
        List<int> pool = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
            
        }
        
        for (int g = 0; g < greenButCount; g++)
        {
            _greenButs.Add(pool[g]);
            Debug.Log("Зелёным загорается кнопка: " + pool[g]);
            
        }

        for (int r = greenButCount; r < (redButCount + greenButCount); r++)
        {
            _redButs.Add(pool[r]);
            
            Debug.Log("Красным загорается кнопка: " + pool[r]);
        }
        
        
        
        
    }



}
