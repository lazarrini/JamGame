using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    [Header("Статы")] 
    public int area;
    public int food;
    public int connection;

    public void AddArea(int amount)
    {
        food++; 
    }

    public void AddFood(int amount)
    {
        
    }
}
