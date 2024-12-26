using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Damage Sound FX")]
    public AudioClip[] physicalDamageSFX;

    [Header("Actions Sound FX")]
    public AudioClip rollSFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] sfxArray)
    {
        int index = Random.Range(0, sfxArray.Length);
        return sfxArray[index];
    }
}
