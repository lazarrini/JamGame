using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class PopupNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float popScale = 1.3f;
    [SerializeField] private float moveDistance = 1f;
    
    
    private RectTransform _rt;

    private void Awake()
    {
        _rt = GetComponent<RectTransform>();
    }

    public void Play(string value, Color textColor)
    {
        text.text = value;
        text.color = textColor;
        _rt.localScale = Vector3.zero;
        Vector3 startPos = _rt.position;
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-300, 300), 0, 0);

        Sequence seq = DOTween.Sequence();

        seq.Append(_rt.DOScale(popScale, 0.15f).SetEase(Ease.OutBack));
        seq.Append(_rt.DOScale(1f, 0.1f));

        seq.Join(_rt.DOMove(startPos + Vector3.up * moveDistance * 100f + randomOffset, duration)
            .SetEase(Ease.OutCubic));

        seq.Insert(duration * 0.4f, text.DOFade(0f, duration * 0.6f));
        
        seq.OnComplete(() => PopupPool.Instance.Return(gameObject));

    }
    
}
