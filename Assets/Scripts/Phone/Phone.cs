using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public enum ResourceType
{
    Raindrop,
    Leaf,
    Stone
}

public class Phone : MonoBehaviour
{
    
    public FrogHarem frogHarem;
    
    [SerializeField] private Button[] phoneButtons;
    [SerializeField] private int greenButCount;
    [SerializeField] private int redButCount;
    [SerializeField] private int areaButCount;

    [SerializeField] private PlayerStats stats;

    [SerializeField] private Transform foodStatsTransform;
    [SerializeField] private Transform hearthsStatsTransform;
    [SerializeField] private Transform areaStatsTransform;
    
    
    public event Action<FrogManSO> OnFrogAddedToHarem;
    
    public Sprite redSprite;
    public Sprite greenSprite;
    public Sprite graySprite;
    public Sprite skipSprite;
    public Sprite selectSprite;
    public Sprite leafButtonSprite;
    public Sprite stoneButtonSprite;
    public Sprite raindropButtonSprite;
    
    public Image currentFrogImage;
    public Image nextFrogImage;

    [SerializeField] private FrogManSO[] frogs;
    
    
    private float timer = 0;
    private float chargeTimer = 0;


    private int leafButtonIndex;
    private int stoneButtonIndex;
    private int raindropButtonIndex;
    
    private List<int> _greenButs = new List<int>();
    private List<int> _redButs = new List<int>();
    
    private Vector2 startPos;
    private Quaternion startRot;

    private bool isSwiped;

    private FrogManSO currentFrog;

    [SerializeField] private RectTransform phoneTransform;

    public int leafCount;
    private void Awake()
    {
        leafCount = 0;
        
        
        
        timer = 0;
        chargeTimer = 0;

        stats.food = 5;
        stats.hearths = 0;
        
        
        
        startPos = currentFrogImage.rectTransform.anchoredPosition;
        startRot = currentFrogImage.rectTransform.localRotation;

        currentFrog = TakeRandomFrog();
        
        
        RandomActivateButtons();
        
    }

    private void Start()
    {
        stats.SetupStats(5, 0);
    }
    
    private void ChooseButtonsIndexes()
    {
        List<int> pool = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
            
        }

        leafButtonIndex = pool[pool.Count - 1];
        stoneButtonIndex = pool[pool.Count - 2];
        raindropButtonIndex = pool[pool.Count - 3];
        
        
        for (int g = 0; g < greenButCount; g++)
        {
            _greenButs.Add(pool[g]);
            
        }

        for (int r = greenButCount; r < (redButCount + greenButCount); r++)
        {
            _redButs.Add(pool[r]);
            
        }
        

        
        
    }
    private void Update()
    {
        
        timer += Time.deltaTime;
        chargeTimer += Time.deltaTime;
        
        if (timer > 1f)
        {
            float randNum = UnityEngine.Random.Range(0, 3);
            if (randNum >= 0)
            {
                RandomActivateButtons();
            }
                
            
            timer = 0;
        }
        
        
        
    }

    private void AddResourceToInventory(ResourceType resourceType)
    {
        Debug.Log("добавился " + resourceType);
    }

    private void RandomActivateButtons()
    {
        ChooseButtonsIndexes();
        DeactivateButtons();
            
        for (int i = 0; i < _redButs.Count; i++)
        {
                
            phoneButtons[_redButs[i] - 1].gameObject.GetComponent<Image>().sprite = redSprite;
            phoneButtons[_redButs[i] - 1].onClick.AddListener(() =>
            {
                    
                SwipeFrogAnimation(-1);
                DeactivateButtons();
            });
        }

        for (int i = 0; i < _greenButs.Count; i++)
        {

            phoneButtons[_greenButs[i] - 1].gameObject.GetComponent<Image>().sprite = greenSprite;
            phoneButtons[_greenButs[i] - 1].onClick.AddListener(() =>
            {
                SwipeFrogAnimation(1);
                DeactivateButtons();
                    
            });
        }
        

        phoneButtons[leafButtonIndex].gameObject.GetComponent<Image>().sprite = leafButtonSprite;
        phoneButtons[stoneButtonIndex].gameObject.GetComponent<Image>().sprite = stoneButtonSprite;
        phoneButtons[raindropButtonIndex].gameObject.GetComponent<Image>().sprite = raindropButtonSprite;
    
        phoneButtons[leafButtonIndex].onClick.AddListener(() =>
        {
                
            DeactivateButtons();
            AddResourceToInventory(ResourceType.Leaf);
                    
        });
        phoneButtons[stoneButtonIndex].onClick.AddListener(() =>
        {
                
            DeactivateButtons();
            AddResourceToInventory(ResourceType.Stone);
                    
        });
        phoneButtons[raindropButtonIndex].onClick.AddListener(() =>
        {
                
            DeactivateButtons();
            AddResourceToInventory(ResourceType.Raindrop);
                    
        });
        
        
            
        _redButs.Clear();
        _greenButs.Clear();
        
    }

    private void DeactivateButtons()
    {
        foreach (Button but in phoneButtons)
        {
            but.gameObject.GetComponent<Image>().sprite = graySprite;
            but.onClick.RemoveAllListeners();
        }
    }

    public void SwipeFrogAnimation(float direction)
    {
        
        if (isSwiped) return;
        SwipeFrog(direction);
        currentFrogImage.sprite = nextFrogImage.sprite;
        if (direction == 1)
        {
           
            currentFrogImage.sprite = selectSprite;
        }
        else
        {
            ShakePhone();
            currentFrogImage.sprite = skipSprite;
        }
        isSwiped = true;
        
        RectTransform rectTransform = currentFrogImage.rectTransform;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + 300f * -direction, 1f).SetEase(Ease.OutElastic));
        seq.Join(rectTransform.DORotate(new Vector3(0, 0, -15f * -direction), 1f).SetEase(Ease.OutCubic));
        seq.Join(currentFrogImage.DOFade(0f, 1f));
        seq.OnComplete(() =>
        {
            currentFrogImage.rectTransform.anchoredPosition = startPos;
            currentFrogImage.rectTransform.localRotation = startRot;
            currentFrogImage.color = new Color(currentFrogImage.color.r, currentFrogImage.color.g, 
                currentFrogImage.color.b, 1f);
            currentFrogImage.sprite = nextFrogImage.sprite;
            
            isSwiped = false;
        });
    }
    private void SwipeFrog(float direction)
    {
        
        if(direction == 1)
        {
            
            frogHarem.harem.Add(currentFrog);
            OnFrogAddedToHarem?.Invoke(currentFrog);
               
        }

        currentFrog = TakeRandomFrog();
        nextFrogImage.sprite = currentFrog.frogSprite;
        
        
    }

    public void ShowPopup(FrogManSO frogMan)
    {
        GameObject popupFood = PopupPool.Instance.Get();
        GameObject popupHearths = PopupPool.Instance.Get();
     
        
        popupFood.transform.position = foodStatsTransform.position;
        popupHearths.transform.position = hearthsStatsTransform.position;
  
        
        PopupNumber foodPopupNumber = popupFood.GetComponent<PopupNumber>();
        PopupNumber hearthsPopupNumber = popupHearths.GetComponent<PopupNumber>();

        
        foodPopupNumber.Play(frogMan.foodAmount.ToString(), IsPositive(frogMan.foodAmount)); 
        hearthsPopupNumber.Play(frogMan.hearthAmount.ToString(), IsPositive(frogMan.hearthAmount)); 
        
        
    }

    private Color IsPositive(int statAmount)
    {
        if (statAmount > 0)
        {
            return Color.greenYellow;
        }
        else
        {
            return Color.crimson;
        }
    }

    private FrogManSO TakeRandomFrog()
    {
        int randomIndex = UnityEngine.Random.Range(0, frogs.Length);
        var frog = frogs[randomIndex];
        currentFrogImage.sprite = frog.frogSprite;
        return frog;

    }

    private void HandleStatBonus(FrogManSO frog)
    {
        stats.ChangeStats(frog);   
    }

    private void ShakePhone()
    {
        phoneTransform.DOShakePosition(
            duration: 0.3f,
            strength: new Vector3(10f, 10f, 0f),
            vibrato: 20,
            randomness: 90,
            snapping: false,
            fadeOut: true
        );
    }

    private void MovePhone()
    {
        
    } 
}
