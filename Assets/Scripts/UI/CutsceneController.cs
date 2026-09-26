using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private GameObject[] images;
    [SerializeField] private CutsceneSoundGroup[] soundGroups;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string nextSceneName = "Game"; //поменять по индексу

    private int currentImage = 0;

    private void Start()
    {
        ShowImage(currentImage);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            NextImage();
        }
    }

    private void NextImage()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        currentImage++;

        if (currentImage >= images.Length)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        ShowImage(currentImage);
    }

    private void ShowImage(int index)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(i == index);
        }

        if (index < soundGroups.Length && soundGroups[index] != null)
        {
            soundGroups[index].PlaySounds(audioSource);
        }

    }


}
