using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRollSoundFX(AudioClip clip)
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
    }
}
