using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance;

    [SerializeField] private int levelCount = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public bool IsLevelUnlocked(int level)
    {
        if (level == 1)
        {
            return true;
        }

        return GetStars(level - 1) >= 1;
    }

    public int GetStars(int level)
    {
        return PlayerPrefs.GetInt("Level_" + level + "_Stars", 0);
    }

    public void SetStars(int level, int stars)
    {
        int oldStars = GetStars(level);

        if (stars > oldStars)
        {
            PlayerPrefs.SetInt("Level_" + level + "_Stars", stars);
            PlayerPrefs.Save();
        }
    }

}
