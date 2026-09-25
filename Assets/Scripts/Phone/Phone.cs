using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class Phone : MonoBehaviour
{
    public FrogHarem frogHarem;
    
    [SerializeField] private Button[] phoneButtons;
    [SerializeField] private int greenButCount;
    [SerializeField] private int redButCount;

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

    public Image currentFrogImage;
    public Image nextFrogImage;

    [SerializeField] private FrogManSO[] frogs;
    
    
    private float timer = 0;

    private List<int> _greenButs = new List<int>();
    private List<int> _redButs = new List<int>();
    
    private Vector2 startPos;
    private Quaternion startRot;

    private bool isSwiped;

    private FrogManSO currentFrog;

    [SerializeField] private RectTransform phoneTransform;
    private void Awake()
    {
        timer = 0;

        stats.food = 5;
        stats.hearths = 0;
        
        startPos = currentFrogImage.rectTransform.anchoredPosition;
        startRot = currentFrogImage.rectTransform.localRotation;

        currentFrog = TakeRandomFrog();
        
        RandomActivateButtons();
        
    }

    private void ChooseButtonsIndexes()
    {
        List<int> pool = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
            
        }
        
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
        if (timer > 1f)
        {
            float randNum = UnityEngine.Random.Range(0, 2);
            if (randNum >= 1)
            {
                RandomActivateButtons();
            }
                
            
            timer = 0;
        }
        
    }

    private void RandomActivateButtons()
    {
        ChooseButtonsIndexes();
        foreach (Button but in phoneButtons)
        {
            but.gameObject.GetComponent<Image>().sprite = graySprite;
            but.onClick.RemoveAllListeners();
        }
            
        for (int i = 0; i < _redButs.Count; i++)
        {
                
            phoneButtons[_redButs[i] - 1].gameObject.GetComponent<Image>().sprite = redSprite;
            phoneButtons[_redButs[i] - 1].onClick.AddListener(() =>
            {
                    
                SwipeFrogAnimation(-1);
            });
        }

        for (int i = 0; i < _greenButs.Count; i++)
        {

            phoneButtons[_greenButs[i] - 1].gameObject.GetComponent<Image>().sprite = greenSprite;
            phoneButtons[_greenButs[i] - 1].onClick.AddListener(() =>
            {
                SwipeFrogAnimation(1);
                    
                    
            });
        }
            
        _redButs.Clear();
        _greenButs.Clear();
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
