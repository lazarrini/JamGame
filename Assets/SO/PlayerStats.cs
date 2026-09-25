using UnityEngine;
using System;
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Range(0, 10)]public int food;
    [Range(0, 10)]public int hearths;
  


    public event Action<int> OnFoodChanged;
    public event Action<int> OnHearthChanged;

    public void SetupStats(int foodAmount, int hearthAmount)
    {
        food = foodAmount;
        hearths = hearthAmount;
        OnFoodChanged?.Invoke(food);
        OnHearthChanged?.Invoke(hearths);
    }

    public void ChangeFood(int amount)
    {
        food = Math.Clamp(food + amount, 0, 10);
        OnFoodChanged?.Invoke(food);
    }

    public void ChangeHearths(int amount)
    {
        hearths = Math.Clamp(hearths + amount, 0, 10);
        OnHearthChanged?.Invoke(hearths);
    }

    
    
    public void ChangeStats(FrogManSO frogStats)
    {
        ChangeFood(frogStats.foodAmount);
        ChangeHearths(frogStats.hearthAmount);
        
    }
}
