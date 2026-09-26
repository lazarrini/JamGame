using UnityEngine;

public class CutsceneSoundGroup : MonoBehaviour
{
    [SerializeField] private AudioClip[] sound;

    public void PlaySounds(AudioSource audioSource)
    {
        foreach (AudioClip sound in sound)
        {
            if (sound != null)
            {
                audioSource.PlayOneShot(sound);
            }
        }
    }

}
