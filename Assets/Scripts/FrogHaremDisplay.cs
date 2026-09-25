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

    [SerializeField] private FrogHarem harem;

    [SerializeField] private PopupWaring fullHaremPopup; 


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
        if (_frogSlots.Count == harem.HaremSize)
        {
            fullHaremPopup.gameObject.SetActive(true);
            fullHaremPopup.Play();
            return;
        }
            
        
        FrogSlot slot = Instantiate(frogSlotPrefab, haremList);
        slot.foodBonus.text = frog.foodAmount.ToString();
        slot.slotImage.sprite = frog.frogSprite;
        slot.foodFillBar.fillAmount = 0;
        slot.isHungry = frog.isHungry;
        _frogSlots.Add(slot);
        
    }
    
    
}
