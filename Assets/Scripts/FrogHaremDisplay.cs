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
    [SerializeField] private Sprite foodFillSprite;
    [SerializeField] private Sprite hearthsFillSprite;

    [SerializeField] private Sprite foodSlotSprite;
    [SerializeField] private Sprite hearthsSlotSprite;

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
        slot.avatarImage.sprite = frog.frogSprite;
        slot.fillBar.fillAmount = 0;
        slot.isHungry = frog.isHungry;
        if (!slot.isHungry)
        {
            slot.slotImage.sprite = foodSlotSprite;
            slot.fillBar.sprite = foodFillSprite;
        }
            
        else
        {
            slot.slotImage.sprite = hearthsSlotSprite;
            slot.fillBar.sprite = hearthsFillSprite;
        }
        _frogSlots.Add(slot);
        
    }
    
    
}
