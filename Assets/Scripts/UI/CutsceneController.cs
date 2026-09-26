using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private GameObject[] images;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip changeSound;
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

        if (audioSource != null && changeSound != null)
        {
            audioSource.PlayOneShot(changeSound);
        }

    }


}
