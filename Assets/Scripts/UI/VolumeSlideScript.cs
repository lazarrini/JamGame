using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

// Этот компонент отвечает ТОЛЬКО за один слайдер.
// Повесь его на зону музыки — настрой поля под музыку.
// Повесь его же (второй экземпляр) на зону эффектов — настрой поля под эффекты.
// Зона (RectTransform с Image, Raycast Target = true) должна покрывать
// область именно ЭТОГО слайдера, от первой до последней бусины,
// и НЕ пересекаться с зоной другого слайдера.
public class VolumeSliderScript : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private int volumeLevel = 5;

    [SerializeField] private GameObject[] beads;
    [SerializeField] private RectTransform slider;

    [SerializeField] private AudioMixer audioMixer;

    // Имя параметра, экспонированного в AudioMixer (Expose to script)
    [SerializeField] private string mixerParam = "MusicVolume";

    // Ключ для сохранения в PlayerPrefs — для музыки и эффектов должен быть разный
    [SerializeField] private string prefsKey = "MusicVolumeLevel";

    private const float MinVolumeDb = -80f;


    private void Start()
    {
        if (beads == null || beads.Length < 2)
        {
            Debug.LogWarning(name + ": нужно минимум 2 бусины для корректной работы слайдера.");
            return;
        }

        volumeLevel = PlayerPrefs.GetInt(prefsKey, volumeLevel);

        MoveSlider(volumeLevel);
        UpdateBeads(volumeLevel);
        ApplyVolumeToMixer(volumeLevel);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateSlider(eventData);
    }


    public void OnDrag(PointerEventData eventData)
    {
        UpdateSlider(eventData);
    }


    private void UpdateSlider(PointerEventData eventData)
    {
        if (beads == null || beads.Length < 2)
        {
            return;
        }

        int closestPosition = GetClosestPosition(eventData);

        volumeLevel = closestPosition;

        MoveSlider(volumeLevel);
        UpdateBeads(volumeLevel);
        ApplyVolumeToMixer(volumeLevel);

        PlayerPrefs.SetInt(prefsKey, volumeLevel);
        PlayerPrefs.Save();

        Debug.Log(name + ", уровень громкости: " + volumeLevel);
    }


    private int GetClosestPosition(PointerEventData eventData)
    {
        float mouseX = eventData.position.x;

        float[] positions = new float[beads.Length + 1];

        positions[0] = RectTransformUtility.WorldToScreenPoint(
            eventData.pressEventCamera,
            beads[0].transform.position
        ).x;

        for (int i = 1; i < beads.Length; i++)
        {
            Vector3 leftBead = beads[i - 1].transform.position;
            Vector3 rightBead = beads[i].transform.position;
            Vector3 middle = Vector3.Lerp(leftBead, rightBead, 0.5f);

            positions[i] = RectTransformUtility.WorldToScreenPoint(
                eventData.pressEventCamera,
                middle
            ).x;
        }

        Vector3 lastBead = beads[beads.Length - 1].transform.position;
        Vector3 previousBead = beads[beads.Length - 2].transform.position;
        Vector3 direction = lastBead - previousBead;
        Vector3 afterLastBead = lastBead + direction * 0.5f;

        positions[beads.Length] = RectTransformUtility.WorldToScreenPoint(
            eventData.pressEventCamera,
            afterLastBead
        ).x;

        int closestPosition = 0;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < positions.Length; i++)
        {
            float distance = Mathf.Abs(mouseX - positions[i]);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = i;
            }
        }

        return closestPosition;
    }


    private void ApplyVolumeToMixer(int level)
    {
        if (audioMixer == null || string.IsNullOrEmpty(mixerParam))
        {
            return;
        }

        float volumeDb;

        if (level <= 0)
        {
            volumeDb = MinVolumeDb;
        }
        else
        {
            float normalized = (float)level / beads.Length;
            volumeDb = Mathf.Log10(normalized) * 20f;
        }

        audioMixer.SetFloat(mixerParam, volumeDb);
    }


    private void MoveSlider(int level)
    {
        if (level == 0)
        {
            slider.position = beads[0].transform.position;
            return;
        }

        if (level == beads.Length)
        {
            Vector3 lastBead = beads[beads.Length - 1].transform.position;
            Vector3 previousBead = beads[beads.Length - 2].transform.position;
            Vector3 direction = lastBead - previousBead;

            slider.position = lastBead + direction * 0.5f;
            return;
        }

        Vector3 leftBead = beads[level - 1].transform.position;
        Vector3 rightBead = beads[level].transform.position;

        slider.position = Vector3.Lerp(leftBead, rightBead, 0.5f);
    }


    private void UpdateBeads(int level)
    {
        for (int i = 0; i < beads.Length; i++)
        {
            Image beadImage = beads[i].GetComponent<Image>();
            beadImage.enabled = i < level;
        }
    }
}