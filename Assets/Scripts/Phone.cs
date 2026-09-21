using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class Phone : MonoBehaviour
{
    [SerializeField] private Image[] phoneButtons;
    [SerializeField] private int greenButCount;
    [SerializeField] private int redButCount;
    private List<int> _greenButs = new List<int>();
    private List<int> _redButs = new List<int>();
    public Sprite redSprite;
    public Sprite greenSprite;
    public Sprite graySprite;

    public Image currentFrogImage;
    public Image nextFrogImage;
    
    float timer = 0;

    private Vector2 startPos;
    private Quaternion startRot;
    private void Awake()
    {
        timer = 0;
        
        startPos = currentFrogImage.rectTransform.anchoredPosition;
        startRot = currentFrogImage.rectTransform.localRotation;
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
            foreach (Image img in phoneButtons)
            {
                img.sprite = graySprite;
                
            }
            
            for (int i = 0; i < _redButs.Count; i++)
            {
                
                phoneButtons[_redButs[i] - 1].sprite = redSprite;
            }
            for (int i = 0; i < _greenButs.Count; i++)
            {
                
                phoneButtons[_greenButs[i] - 1].sprite = greenSprite;
            }
            
            _redButs.Clear();
            _greenButs.Clear();

            timer = 0;
        }
        
    }

    public void SwipeFrogAnimation(float direction)
    {
        RectTransform rectTransform = currentFrogImage.rectTransform;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + 300f * direction, 1f).SetEase(Ease.OutElastic));
        seq.Join(rectTransform.DORotate(new Vector3(0, 0, -15f * direction), 1f).SetEase(Ease.OutCubic));
        seq.Join(currentFrogImage.DOFade(0f, 0.4f));
        seq.OnComplete(() =>
        {
            currentFrogImage.gameObject.SetActive(false);
            currentFrogImage.rectTransform.anchoredPosition = startPos;
            currentFrogImage.rectTransform.localRotation = startRot;
            currentFrogImage.color = new Color(currentFrogImage.color.r, currentFrogImage.color.g,
                currentFrogImage.color.b, 1f);
            currentFrogImage = nextFrogImage;
            nextFrogImage = currentFrogImage;
        });
    }
    public void SwipeFrog()
    {
        SwipeFrogAnimation(-1);
        
    }



}
