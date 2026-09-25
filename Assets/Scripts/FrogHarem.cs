using UnityEngine;
using System.Collections.Generic;
public class FrogHarem : MonoBehaviour
{
    public static FrogHarem Instance;
    public Phone phone;
    
    public List<FrogManSO> harem = new List<FrogManSO>();
    
    [SerializeField] private PlayerStats playerStats;        
    private float _timer;

    [SerializeField] private FrogHaremDisplay haremDisplay;
    private void Update()
    {
        
        _timer += Time.deltaTime;
        if(_timer > 3f)
        {
            ChangeFoodParameter();
            ChangeHearthsParameter();
            if (playerStats.food == 0)
            {
                foreach(var frog in harem)
                {
                    if(frog.isHungry && frog != null)
                        harem.Remove(frog);
                }
                foreach(var frogSlot in haremDisplay._frogSlots)
                {
                    
                    if(frogSlot.isHungry && frogSlot != null)
                        Destroy(frogSlot.gameObject);
                }
            }
            _timer = 0;
        }
    }

    private void ChangeFoodParameter()
    {
        foreach (var frog in harem)
        {
            playerStats.ChangeFood(frog.foodAmount);
            phone.ShowPopup(frog);
                
        }
    }

    private void ChangeHearthsParameter()
    {
        
    }
}
