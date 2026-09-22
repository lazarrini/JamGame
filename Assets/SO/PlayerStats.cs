using UnityEngine;
using System;
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Range(-5, 5)]public int food;
    [Range(-5, 5)]public int hearths;
    [Range(-5, 5)]public int area;


    public event Action<int> OnFoodChanged;
    public event Action<int> OnHearthChanged;
    public event Action<int> OnAreaChanged;
    

    public void SetFood(int amount)
    {
        
    }
     
    public void ChangeStats(FrogManSO frogStats)
    {
        food += frogStats.foodAmount;
        hearths += frogStats.hearthAmount;
        area += frogStats.areaAmount;
    }
}
