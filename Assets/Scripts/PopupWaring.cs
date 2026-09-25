using UnityEngine;
using DG.Tweening;

public class PopupWaring : MonoBehaviour
{
    private float popupScale = 1.3f;
    private RectTransform _rt;

    private void Awake()
    {
        _rt = GetComponent<RectTransform>();
    }
    
    public void Play()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_rt.DOScale(popupScale, 0.5f).SetEase(Ease.OutBack));
        seq.Append(_rt.DOScale(1f, 0.5f));
        seq.OnComplete(() => gameObject.SetActive(false));
    }
}
