using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundControllerScript : MonoBehaviour, IDragHandler
{
    [SerializeField] private int volumeLevel = 5;

    [SerializeField] private GameObject[] beads;
    [SerializeField] private RectTransform soundSlider;
    [SerializeField] private RectTransform beadsArea;

    [SerializeField] private AudioMixer audioMixer;

    private int maxVolumeLevel;

    private void Start()
    {
        maxVolumeLevel = beads.Length;
        
        Debug.Log("Количество бусин: " + beads.Length);
        Debug.Log("Максимальный уровень: " + maxVolumeLevel);
    }


    public void OnDrag(PointerEventData eventData)
    {
        float mouseX = eventData.position.x;

        // Сначала получаем экранные X всех 11 позиций
        float[] positions = new float[beads.Length + 1];

        // Позиция 0 — центр первой бусины
        positions[0] = RectTransformUtility.WorldToScreenPoint(
            eventData.pressEventCamera,
            beads[0].transform.position
        ).x;

        // Позиции 1-9 — между соседними бусинами
        for (int i = 1; i < beads.Length; i++)
        {
            Vector3 leftBead = beads[i - 1].transform.position;
            Vector3 rightBead = beads[i].transform.position;

            Vector3 middle = Vector3.Lerp(
                leftBead,
                rightBead,
                0.5f
            );

            positions[i] = RectTransformUtility.WorldToScreenPoint(
                eventData.pressEventCamera,
                middle
            ).x;
        }

        // Позиция 10 — после последней бусины
        Vector3 lastBead = beads[beads.Length - 1].transform.position;
        Vector3 previousBead = beads[beads.Length - 2].transform.position;

        Vector3 direction = lastBead - previousBead;

        Vector3 afterLastBead = lastBead + direction * 0.5f;

        positions[beads.Length] = RectTransformUtility.WorldToScreenPoint(
            eventData.pressEventCamera,
            afterLastBead
        ).x;


        // Ищем ближайшую позицию к курсору
        int closestPosition = 0;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < positions.Length; i++)
        {
            float distance = Mathf.Abs(
                mouseX - positions[i]
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = i;
            }
        }

        volumeLevel = closestPosition;

        MoveSlider();
        UpdateBeads();

        Debug.Log("Уровень громкости: " + volumeLevel);
    }

    private void MoveSlider()
    {
        // Позиция 0 — центр первой бусины
        if (volumeLevel == 0)
        {
            soundSlider.position = beads[0].transform.position;
            return;
        }

        // Позиция 10 — после последней бусины
        if (volumeLevel == beads.Length)
        {
            Vector3 lastBead = beads[beads.Length - 1].transform.position;
            Vector3 previousBead = beads[beads.Length - 2].transform.position;

            Vector3 direction = lastBead - previousBead;

            soundSlider.position = lastBead + direction * 0.5f;
            return;
        }

        // Все остальные позиции — между соседними бусинами
        Vector3 leftBead = beads[volumeLevel - 1].transform.position;
        Vector3 rightBead = beads[volumeLevel].transform.position;

        soundSlider.position = Vector3.Lerp(
            leftBead,
            rightBead,
            0.5f
        );
    }

    private void UpdateBeads()
    {
        for (int i = 0; i < beads.Length; i++)
        {
            Image beadImage = beads[i].GetComponent<Image>();

            if (i < volumeLevel)
            {
                beadImage.enabled = true;
            }
            else
            {
                beadImage.enabled = false;
            }
        }
    }



}
