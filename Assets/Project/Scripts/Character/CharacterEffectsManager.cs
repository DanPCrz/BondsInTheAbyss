using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("VFX")]
    [SerializeField] GameObject bloodSplatterVFX;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        effect.ProcessInstantEffect(character);
    }

    public void PlayBloodSplatterVFX(Vector3 contactPoint)
    {
        if (bloodSplatterVFX != null)
        {
            GameObject bloodVFX = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        else 
        {
            GameObject bloodVFX = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }
}
    