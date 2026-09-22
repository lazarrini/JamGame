using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class Phone : MonoBehaviour
{
    [SerializeField] private Button[] phoneButtons;
    [SerializeField] private int greenButCount;
    [SerializeField] private int redButCount;
    
    public Sprite redSprite;
    public Sprite greenSprite;
    public Sprite graySprite;

    public Image currentFrogImage;
    public Image nextFrogImage;

    [SerializeField] private FrogManSO[] frogs;
    
    
    private float timer = 0;

    private List<int> _greenButs = new List<int>();
    private List<int> _redButs = new List<int>();
    
    private Vector2 startPos;
    private Quaternion startRot;

    private bool isSwiped;
    private void Awake()
    {
        timer = 0;
        
        startPos = currentFrogImage.rectTransform.anchoredPosition;
        startRot = currentFrogImage.rectTransform.localRotation;
        
        DrawFrogOnScreen();
    }

    private void ChooseButtonsIndexes()
    {
        List<int> pool = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
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
        if (timer > 2f)
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
                phoneButtons[_redButs[i] - 1].onClick.AddListener(() => SwipeFrogAnimation(-1));
            }
            for (int i = 0; i < _greenButs.Count; i++)
            {
                
                phoneButtons[_greenButs[i] - 1].gameObject.GetComponent<Image>().sprite = greenSprite;
                phoneButtons[_greenButs[i] - 1].onClick.AddListener(() => SwipeFrogAnimation(1));
            }
            
            _redButs.Clear();
            _greenButs.Clear();

            timer = 0;
        }
        
    }

    public void SwipeFrogAnimation(float direction)
    {
        if (isSwiped) return;
        isSwiped = true;
        RectTransform rectTransform = currentFrogImage.rectTransform;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + 300f * direction, 2f).SetEase(Ease.OutElastic));
        seq.Join(rectTransform.DORotate(new Vector3(0, 0, -15f * direction), 1f).SetEase(Ease.OutCubic));
        seq.Join(currentFrogImage.DOFade(0f, 1f));
        seq.OnComplete(() =>
        {
            currentFrogImage.rectTransform.anchoredPosition = startPos;
            currentFrogImage.rectTransform.localRotation = startRot;
            currentFrogImage.color = new Color(currentFrogImage.color.r, currentFrogImage.color.g, 
                currentFrogImage.color.b, 1f);
            SwipeFrog();
            isSwiped = false;
        });
    }
    private void SwipeFrog()
    {
        currentFrogImage.sprite = nextFrogImage.sprite;     
        nextFrogImage.sprite = DrawFrogOnScreen().frogSprite; 

    }

    private FrogManSO DrawFrogOnScreen()
    {
        int randomIndex = UnityEngine.Random.Range(0, frogs.Length);
        var frog = frogs[randomIndex];
        return frog;

    }



}
