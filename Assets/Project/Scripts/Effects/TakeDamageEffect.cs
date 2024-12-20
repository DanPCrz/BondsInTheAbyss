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
    private int finalDamageDealt;

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
        Debug.Log("Damage Dealt: " + finalDamageDealt);
        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
    }
}
