using UnityEngine;

public static class BootsTrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        var prefab = Resources.Load<GameObject>("[Systems]");
        var go = Object.Instantiate(prefab);
        go.name = "[Systems]";
        Object.DontDestroyOnLoad(go);
    }
    
}
