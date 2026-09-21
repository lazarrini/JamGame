using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainPanel, settingsPanel;
    [SerializeField] GameObject quitButton;
    [SerializeField] string gameScene = "Game";

    public void Play() => SceneLoader.Instance.Load(gameScene);
}
