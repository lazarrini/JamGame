using UnityEngine;
using UnityEngine.EventSystems;

public class SoundControllerScript : MonoBehaviour, IDragHandler
{
    [SerializeField] private int volumeLevel = 5;

    [SerializeField] private GameObject[] beads;
    [SerializeField] private RectTransform soundSlider;
    [SerializeField] private RectTransform beadsArea;

    private int maxVolumeLevel;

    private void Start()
    {
        maxVolumeLevel = beads.Length - 1;
        
        Debug.Log("Количество бусин: " + beads.Length);
        Debug.Log("Максимальный уровень: " + maxVolumeLevel);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            beadsArea,
            eventData.position,
            eventData.pressEventCamera,
            out mousePosition
            );

        float firstBeadX = ((RectTransform)beads[0].transform).anchoredPosition.x;
        float lastBeadX = ((RectTransform)beads[beads.Length - 1].transform).anchoredPosition.x;

        float normalizedPosition = Mathf.InverseLerp(
            firstBeadX, lastBeadX, mousePosition.x
            );

        int newVolumeLevel = Mathf.RoundToInt(normalizedPosition * maxVolumeLevel);

        volumeLevel = Mathf.Clamp(newVolumeLevel, 0, maxVolumeLevel);

        RectTransform selectedBead = beads[volumeLevel].GetComponent<RectTransform>();
        soundSlider.position = selectedBead.position;

        Debug.Log("Уровень громкости: " + volumeLevel);
    }


}
