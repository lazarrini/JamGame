using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Popochka : MonoBehaviour
{
    [SerializeField] private float _counter;

    private void Awake()
    {
        _counter = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _counter++;
    }
}
