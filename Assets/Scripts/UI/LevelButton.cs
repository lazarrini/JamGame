using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private int sceneIndex = 2;

    [SerializeField] private bool playCutscene = false;
    [SerializeField] private int cutsceneSceneIndex = 1;

    [SerializeField] private GameObject lockImage;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        UpdateButton();
    }

    private void UpdateButton()
    {
        bool unlocked = LevelProgressManager.Instance.IsLevelUnlocked(levelNumber);

        button.interactable = unlocked;

        if (lockImage != null)
        {
            lockImage.SetActive(!unlocked);
        }

    }

    public void OpenLevel()
    {
        if (!LevelProgressManager.Instance.IsLevelUnlocked(levelNumber))
        {
            return;
        }

        if (playCutscene)
        {
            SceneManager.LoadScene(cutsceneSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }

        SceneManager.LoadScene("Level" + levelNumber);

    }


}
