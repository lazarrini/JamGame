using UnityEngine;
using System.Collections.Generic;
public class FrogHarem : MonoBehaviour
{
    public static FrogHarem Instance;
    
    
    public List<FrogManSO> harem = new List<FrogManSO>();
    
    [SerializeField] private PlayerStats playerStats;        
    private float timer;

    [SerializeField] private FrogHaremDisplay haremDisplay;
    private void Update()
    {
        
        timer += Time.deltaTime;
        if(timer > 3f)
        {
            foreach (var frog in harem)
            {
                playerStats.ChangeFood(frog.foodAmount);
            }
            timer = 0;
        }
    }
}
