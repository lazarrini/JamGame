using UnityEngine;

[CreateAssetMenu(fileName = "FrogManSO", menuName = "Scriptable Objects/FrogManSO")]
public class FrogManSO : ScriptableObject
{
    public Sprite frogSprite;
    public int foodAmount;
    public int hearthAmount;
    public AreaType area;
    public bool isHungry;

    public enum AreaType
    {
        None,
        Swamp,
        Dirt,
        Forest
    }
}
