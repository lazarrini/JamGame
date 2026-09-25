using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundControllerScript : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    // ===== МУЗЫКА =====

    [SerializeField] private int musicVolumeLevel = 5;

    [SerializeField] private GameObject[] musicBeads;
    [SerializeField] private RectTransform musicSlider;


    // ===== ЗВУКОВЫЕ ЭФФЕКТЫ =====

    [SerializeField] private int sfxVolumeLevel = 5;

    [SerializeField] private GameObject[] sfxBeads;
    [SerializeField] private RectTransform sfxSlider;


    // ===== AUDIO MIXER =====

    [SerializeField] private AudioMixer audioMixer;

    // Имена параметров должны совпадать с теми,
    // что "экспонированы" (Exposed) в AudioMixer в Unity
    [SerializeField] private string musicVolumeParam = "MusicVolume";
    [SerializeField] private string sfxVolumeParam = "SFXVolume";

    // Громкость при уровне 0 (полная тишина)
    private const float MinVolumeDb = -80f;

    // Ключи для сохранения настроек между запусками игры
    private const string MusicPrefsKey = "MusicVolumeLevel";
    private const string SfxPrefsKey = "SfxVolumeLevel";


    // true = двигаем музыку
    // false = двигаем звуковые эффекты
    private bool isMusicSlider;


    private void Start()
    {
        // Подгружаем сохранённые уровни, если они есть,
        // иначе остаёмся на значениях из инспектора
        musicVolumeLevel = PlayerPrefs.GetInt(MusicPrefsKey, musicVolumeLevel);
        sfxVolumeLevel = PlayerPrefs.GetInt(SfxPrefsKey, sfxVolumeLevel);

        // Ставим оба слайдера в начальные позиции
        MoveSlider(musicBeads, musicSlider, musicVolumeLevel);
        MoveSlider(sfxBeads, sfxSlider, sfxVolumeLevel);

        // Показываем нужное количество бусин
        UpdateBeads(musicBeads, musicVolumeLevel);
        UpdateBeads(sfxBeads, sfxVolumeLevel);

        // Применяем громкость к AudioMixer сразу при старте
        ApplyVolumeToMixer(musicVolumeParam, musicVolumeLevel, musicBeads.Length);
        ApplyVolumeToMixer(sfxVolumeParam, sfxVolumeLevel, sfxBeads.Length);
    }


    // Вызывается для музыкального слайдера
    public void SetMusicSlider()
    {
        isMusicSlider = true;
    }


    // Вызывается для слайдера звуковых эффектов
    public void SetSfxSlider()
    {
        isMusicSlider = false;
    }


    // Клик сразу по треку слайдера (без перетаскивания)
    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateActiveSlider(eventData);
    }


    // Перетаскивание слайдера
    public void OnDrag(PointerEventData eventData)
    {
        UpdateActiveSlider(eventData);
    }


    // Общая логика для клика и перетаскивания
    private void UpdateActiveSlider(PointerEventData eventData)
    {
        GameObject[] currentBeads;
        RectTransform currentSlider;
        string mixerParam;

        if (isMusicSlider)
        {
            currentBeads = musicBeads;
            currentSlider = musicSlider;
            mixerParam = musicVolumeParam;
        }
        else
        {
            currentBeads = sfxBeads;
            currentSlider = sfxSlider;
            mixerParam = sfxVolumeParam;
        }

        if (currentBeads == null || currentBeads.Length < 2)
        {
            Debug.LogWarning("Нужно минимум 2 бусины для корректной работы слайдера.");
            return;
        }

        int closestPosition = GetClosestPosition(eventData, currentBeads);

        if (isMusicSlider)
        {
            musicVolumeLevel = closestPosition;
        }
        else
        {
            sfxVolumeLevel = closestPosition;
        }

        // Передвигаем ползунок
        MoveSlider(currentBeads, currentSlider, closestPosition);

        // Показываем/скрываем бусины
        UpdateBeads(currentBeads, closestPosition);

        // Реально меняем громкость в AudioMixer
        ApplyVolumeToMixer(mixerParam, closestPosition, currentBeads.Length);

        // Сохраняем выбор игрока
        SaveVolumeLevel();

        Debug.Log((isMusicSlider ? "Музыка" : "Эффекты") + ", уровень громкости: " + closestPosition);
    }


    // Находит ближайшую к курсору позицию среди бусин
    private int GetClosestPosition(PointerEventData eventData, GameObject[] currentBeads)
    {
        float mouseX = eventData.position.x;

        // Всего positions.Length позиций:
        // 0 — центр первой бусины
        // 1..N-1 — между бусинами
        // N — после последней бусины
        float[] positions = new float[currentBeads.Length + 1];

        // Позиция 0 — центр первой бусины
        positions[0] = RectTransformUtility.WorldToScreenPoint(
            eventData.pressEventCamera,
            currentBeads[0].transform.position
        ).x;

        // Позиции между бусинами
        for (int i = 1; i < currentBeads.Length; i++)
        {
            Vector3 leftBead = currentBeads[i - 1].transform.position;
            Vector3 rightBead = currentBeads[i].transform.position;
            Vector3 middle = Vector3.Lerp(leftBead, rightBead, 0.5f);
            positions[i] = RectTransformUtility.WorldToScreenPoint(
                eventData.pressEventCamera,
                middle
            ).x;
        }

        // Последняя позиция — после последней бусины
        Vector3 lastBead = currentBeads[currentBeads.Length - 1].transform.position;
        Vector3 previousBead = currentBeads[currentBeads.Length - 2].transform.position;
        Vector3 direction = lastBead - previousBead;

        Vector3 afterLastBead = lastBead + direction * 0.5f;

        positions[currentBeads.Length] = RectTransformUtility.WorldToScreenPoint(
            eventData.pressEventCamera,
            afterLastBead
        ).x;


        // Ищем ближайшую позицию к курсору
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


    // Переводит уровень (0..maxLevel) в децибелы и применяет к AudioMixer
    private void ApplyVolumeToMixer(string parameterName, int level, int maxLevel)
    {
        if (audioMixer == null || string.IsNullOrEmpty(parameterName))
        {
            return;
        }

        float volumeDb;

        if (level <= 0)
        {
            // Уровень 0 — полная тишина
            volumeDb = MinVolumeDb;
        }
        else
        {
            // Громкость линейна по уровню, но AudioMixer работает в децибелах,
            // поэтому переводим через логарифм (иначе средние деления
            // будут звучать намного тише, чем кажется на глаз)
            float normalized = (float)level / maxLevel;
            volumeDb = Mathf.Log10(normalized) * 20f;
        }

        audioMixer.SetFloat(parameterName, volumeDb);
    }

    // Сохраняет текущие уровни громкости на диск
    private void SaveVolumeLevel()
    {
        PlayerPrefs.SetInt(MusicPrefsKey, musicVolumeLevel);
        PlayerPrefs.SetInt(SfxPrefsKey, sfxVolumeLevel);
        PlayerPrefs.Save();
    }


    // Передвигает конкретный слайдер
    private void MoveSlider(GameObject[] currentBeads, RectTransform currentSlider, int volumeLevel)
    {
        // Позиция 0 — центр первой бусины
        if (volumeLevel == 0)
        {
            currentSlider.position = currentBeads[0].transform.position;
            return;
        }

        // Последняя позиция — после последней бусины
        if (volumeLevel == currentBeads.Length)
        {
            Vector3 lastBead = currentBeads[currentBeads.Length - 1].transform.position;
            Vector3 previousBead = currentBeads[currentBeads.Length - 2].transform.position;
            Vector3 direction = lastBead - previousBead;

            currentSlider.position = lastBead + direction * 0.5f;
            return;
        }

        // Все остальные позиции — между бусинами
        Vector3 leftBead = currentBeads[volumeLevel - 1].transform.position;
        Vector3 rightBead = currentBeads[volumeLevel].transform.position;

        currentSlider.position = Vector3.Lerp(leftBead, rightBead, 0.5f);
    }

    // Показывает только заполненные бусины
    private void UpdateBeads(GameObject[] currentBeads, int volumeLevel)
    {
        for (int i = 0; i < currentBeads.Length; i++)
        {
            Image beadImage = currentBeads[i].GetComponent<Image>();
            beadImage.enabled = i < volumeLevel;
        }
    }

}