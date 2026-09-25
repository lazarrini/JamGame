using UnityEngine;
using UnityEngine.UI;

public class FrogGalleryItem : MonoBehaviour
{
    [SerializeField] private Image frogImage;
    [SerializeField] private Image lockedImage;

    public void Setup(FrogData frog, bool unlocked)
    {
        frogImage.sprite = frog.frogSprite;

        frogImage.gameObject.SetActive(unlocked);
        lockedImage.gameObject.SetActive(!unlocked);
    }


}
