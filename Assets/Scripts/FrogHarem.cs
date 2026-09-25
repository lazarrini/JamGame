using UnityEngine;
using System.Collections.Generic;
public class FrogHarem : MonoBehaviour
{
    public static FrogHarem Instance;
    public Phone phone;
    
    public List<FrogManSO> harem = new List<FrogManSO>();
    
    [SerializeField] private PlayerStats playerStats;
    public int HaremSize = 6;
    private float _timer;

    [SerializeField] private FrogHaremDisplay haremDisplay;
    private void Update()
    {
        
        foreach (var frog in haremDisplay._frogSlots)
        {
            frog.foodFillBar.fillAmount += 0.3f * Time.deltaTime;
            if (frog.foodFillBar.fillAmount >= 1f)
            {
                ChangeFoodParameter();
                ChangeHearthsParameter();
                FoodIsOverCheck();
                frog.foodFillBar.fillAmount = 0f;
            }
        }
        
        
    }

    private void FoodIsOverCheck()
    {
        if (playerStats.food == 0)
        {
            for(int k = harem.Count -1; k >= 0; k--)
            {
                if(harem[k].isHungry)
                    harem.Remove(harem[k]);
            }
            
            for (int i = haremDisplay._frogSlots.Count - 1; i >= 0; i--)
            {
                if (haremDisplay._frogSlots[i].isHungry)
                {
                    Destroy(haremDisplay._frogSlots[i].gameObject);
                    haremDisplay._frogSlots.Remove(haremDisplay._frogSlots[i]);
                }
            }
        }
    }

    private void ChangeFoodParameter()
    {
        foreach (var frog in harem)
        {
            playerStats.ChangeFood(frog.foodAmount);
            phone.ShowPopup(frog);
            FoodIsOverCheck();

        }
    }

    private void ChangeHearthsParameter()
    {
        foreach (var frog in harem)
        {
            playerStats.ChangeHearths(frog.foodAmount);
            phone.ShowPopup(frog);
                
        }
    }
}
