using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PlayerStatsDisplay : MonoBehaviour
{
    public enum StatType { Food, Hearths, Area }

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private StatType statType;

    [Header("5 ячеек, по порядку слева направо")]
    [SerializeField] private Image[] cells; // размер 5

    [SerializeField] private Color filledColor = Color.white;
    [SerializeField] private Color emptyColor = new Color(1, 1, 1, 0.2f);

    private int currentValue;

    private void OnEnable()
    {
        switch (statType)
        {
            case StatType.Food:
                playerStats.OnFoodChanged += HandleChanged;
                currentValue = playerStats.food;
                break;
            case StatType.Hearths:
                playerStats.OnHearthChanged += HandleChanged;
                currentValue = playerStats.hearths;
                break;
            case StatType.Area:
                playerStats.OnAreaChanged += HandleChanged;
                currentValue = playerStats.area;
                break;
        }

        DrawInstant(currentValue);
    }

    private void OnDisable()
    {
        switch (statType)
        {
            case StatType.Food: playerStats.OnFoodChanged -= HandleChanged; break;
            case StatType.Hearths: playerStats.OnHearthChanged -= HandleChanged; break;
            case StatType.Area: playerStats.OnAreaChanged -= HandleChanged; break;
        }
    }

    private void HandleChanged(int newValue)
    {
        AnimateChange(currentValue, newValue);
        currentValue = newValue;
    }

    private void DrawInstant(int value)
    {
        for (int i = 0; i < cells.Length; i++)
            cells[i].color = i < value ? filledColor : emptyColor;
    }

    private void AnimateChange(int from, int to)
    {
        if (to > from)
        {
            for (int i = from; i < to; i++)
            {
                int index = i;
                cells[index].DOColor(filledColor, 0.15f).SetDelay((index - from) * 0.08f);
                PopCell(cells[index], (index - from) * 0.08f);
            }
        }
        else if (to < from)
        {
            for (int i = from - 1; i >= to; i--)
            {
                int index = i;
                float delay = (from - 1 - index) * 0.08f;
                cells[index].DOColor(emptyColor, 0.15f).SetDelay(delay);
                PopCell(cells[index], delay);
            }
        }
    }

    private void PopCell(Image cell, float delay)
    {
        cell.transform.DOKill();
        cell.transform.localScale = Vector3.one;
        cell.transform.DOScale(1.3f, 0.1f)
            .SetDelay(delay)
            .SetEase(Ease.OutBack)
            .OnComplete(() => cell.transform.DOScale(1f, 0.1f));
    }
}
