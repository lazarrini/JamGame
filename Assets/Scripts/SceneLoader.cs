using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] CanvasGroup fade;
        //[SerializeField] AudioMixer mixer;          
    [SerializeField] float fadeTime = 0.3f;

    bool loading;

    void Awake()
    {
        
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //ApplyVolume("Music");
        //ApplyVolume("SFX");
    }

    public void Load(string sceneName)
    {
        if (!loading) StartCoroutine(LoadRoutine(sceneName));
    }

    IEnumerator LoadRoutine(string sceneName)
    {
        loading = true;
        yield return Fade(1f);
        Time.timeScale = 1f; 
        var op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;
        yield return Fade(0f);
        loading = false;
    }

    IEnumerator Fade(float target)
    {
        fade.blocksRaycasts = true;
        float start = fade.alpha;
        for (float t = 0; t < 1f; t += Time.unscaledDeltaTime / fadeTime)
        {
            fade.alpha = Mathf.Lerp(start, target, t);
            yield return null;
        }
        fade.alpha = target;
        fade.blocksRaycasts = target > 0f;
    }

    /*public void SetVolume(string param, float value01)
    {
        value01 = Mathf.Clamp(value01, 0.0001f, 1f);
        mixer.SetFloat(param, Mathf.Log10(value01) * 20f); 
        PlayerPrefs.SetFloat(param, value01);
    }

    void ApplyVolume(string param) => SetVolume(param, PlayerPrefs.GetFloat(param, 0.8f));
*/
}

