using System;
using UnityEngine;
using UnityEngine.UI;
public class FrogHaremDisplay : MonoBehaviour
{
    [SerializeField] private Transform haremList;
    [SerializeField] private FrogSlot frogSlotPrefab;
    [SerializeField] private Phone phone;


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
    }
    
    
}
