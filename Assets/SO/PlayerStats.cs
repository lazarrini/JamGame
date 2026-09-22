using UnityEngine;
using System;
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Range(0, 5)]public int food;
    [Range(0, 5)]public int hearths;
    [Range(0, 5)]public int area;


    public event Action<int> OnFoodChanged;
    public event Action<int> OnHearthChanged;
    public event Action<int> OnAreaChanged;
    

    public void ChangeFood(int amount)
    {
        food = Math.Clamp(food + amount, 0, 5);
        OnFoodChanged?.Invoke(food);
    }

    public void ChangeHearths(int amount)
    {
        hearths = Math.Clamp(hearths + amount, 0, 5);
        OnHearthChanged?.Invoke(hearths);
    }

    public void ChangeArea(int amount)
    {
        area = Math.Clamp(area + amount, 0, 5);
        OnAreaChanged?.Invoke(area);
    }
    
    public void ChangeStats(FrogManSO frogStats)
    {
        ChangeFood(frogStats.foodAmount);
        ChangeHearths(frogStats.hearthAmount);
        ChangeArea(frogStats.areaAmount);
    }
}
