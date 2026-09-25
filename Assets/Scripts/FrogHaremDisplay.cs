using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class FrogHaremDisplay : MonoBehaviour
{
    [SerializeField] private Transform haremList;
    [SerializeField] private FrogSlot frogSlotPrefab;
    [SerializeField] private Phone phone;
    
    public List<FrogSlot> _frogSlots = new List<FrogSlot>();


    private void OnEnable()
    {
        phone.OnFrogAddedToHarem += UpdateHaremList;
    }

    private void OnDisable()
    {
        phone.OnFrogAddedToHarem -= UpdateHaremList;
    }

    private void UpdateHaremList(FrogManSO frog)
    {
        FrogSlot slot = Instantiate(frogSlotPrefab, haremList);
        slot.foodBonus.text = frog.foodAmount.ToString();
        slot.slotImage.sprite = frog.frogSprite;
        slot.isHungry = frog.isHungry;
        _frogSlots.Add(slot);
        
    }
    
    
}
