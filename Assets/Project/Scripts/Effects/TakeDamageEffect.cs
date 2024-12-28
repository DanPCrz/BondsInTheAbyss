using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]

public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 10;
    public float magicalDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float poisonDamage = 0;

    [Header("Final Damage")]
    private int finalDamageDealt = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySetDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool playDamageSFX = true;
    public AudioClip elementalDamageSFX;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessInstantEffect(CharacterManager character)
    {
        base.ProcessInstantEffect(character);

        if (character.isDead.Value)
            return;
        CalculateDamage(character);
        PlayDirectionalBasedDamageAnimation(character);
        PlayDamageVFX(character);
        PlayDamageSFX(character);
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (characterCausingDamage != null)
        {

        }

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicalDamage + fireDamage + lightningDamage + poisonDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        finalDamageDealt = Mathf.Clamp(finalDamageDealt, 0, int.MaxValue);
        Debug.Log("Damage Dealt: " + finalDamageDealt);
        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);
        character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
    }

    private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
    {
        if (!character.IsOwner || character.isDowned.Value || character.isDead.Value)
            return;

        poiseIsBroken = true;

        if (angleHitFrom >= 145 && angleHitFrom <= 180)
        {
            damageAnimation = character.characterAnimationManager.hit_Forward;
        }
        else if (angleHitFrom <= -145 && angleHitFrom >= -180)
        {
            damageAnimation = character.characterAnimationManager.hit_Forward;
        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            damageAnimation = character.characterAnimationManager.hit_Backward;
        }
        else if (angleHitFrom >= -144 && angleHitFrom <= -45)
        {
            damageAnimation = character.characterAnimationManager.hit_Left;
        }
        else if (angleHitFrom >= 45 && angleHitFrom <= 144)
        {
            damageAnimation = character.characterAnimationManager.hit_Right;
        }

        if (poiseIsBroken)
        {
            character.characterAnimationManager.PlayTargetAnimation(damageAnimation, true);
        }
    }
}
