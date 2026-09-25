using UnityEngine;

public class GalleryManager : MonoBehaviour
{
    public static GalleryManager Instance;

    [SerializeField] private Transform content;
    [SerializeField] private GameObject frogItemPrefab;

    [SerializeField] private FrogData[] frogs;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateGallery();
    }

    public void CreateGallery()
    {
        for (int i = 0; i < frogs.Length; i++)
        {
            GameObject item = Instantiate(frogItemPrefab, content);
            FrogGalleryItem galleryItem = item.GetComponent<FrogGalleryItem>();
            galleryItem.Setup(frogs[i], false);

        }
    }
}
