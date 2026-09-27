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

    [SerializeField] private float foodMultyply = 0.1f;
    [SerializeField] private float hearthsMultyply = 0.05f;
    
    [SerializeField] private FrogHaremDisplay haremDisplay;
    private void Update()
    {
        if (haremDisplay._frogSlots.Count != 0)
        {
            for (int i = 0; i < haremDisplay._frogSlots.Count; i++)
            {

                if (haremDisplay._frogSlots[i].isHungry)
                {
                
                    haremDisplay._frogSlots[i].fillBar.fillAmount += hearthsMultyply * Time.deltaTime;
                    if (haremDisplay._frogSlots[i].fillBar.fillAmount >= 1f)
                    {
                        ChangeFoodParameter();
                        ChangeHearthsParameter();
                        haremDisplay._frogSlots[i].fillBar.fillAmount = 0f;
                        FoodIsOverCheck();
                    
                    }
                }
                else
                {
                    haremDisplay._frogSlots[i].fillBar.fillAmount += foodMultyply * Time.deltaTime;
                    if (haremDisplay._frogSlots[i].fillBar.fillAmount >= 1f)
                    {
                        ChangeFoodParameter();
                        haremDisplay._frogSlots[i].fillBar.fillAmount = 0f;
                        FoodIsOverCheck();
                    
                    }
                }
                
                
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
        for (int i = 0; i < harem.Count; i++)
        {
            playerStats.ChangeFood(harem[i].foodAmount);
            phone.ShowPopup(harem[i]);
            FoodIsOverCheck();
        }
        
    }

    private void ChangeHearthsParameter()
    {
        for (int i = 0; i < harem.Count; i++)
        {
            playerStats.ChangeHearths(harem[i].hearthAmount);
            phone.ShowPopup(harem[i]);
            FoodIsOverCheck();
        }
    }
}
