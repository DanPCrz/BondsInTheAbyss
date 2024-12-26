using UnityEngine;

public class WeaponItem : Item
{
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int strengthRequirement = 0;
    public int dexterityRequirement = 0;
    public int intelligenceRequirement = 0;
    public int faithRequirement = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicalDamage = 0;
    public int fireDamage = 0;
    public int lightningDamage = 0;
    public int poisonDamage = 0;

    [Header("Weapon Base Poise Damage")]
    public float poiseDamage = 10;

    [Header("Weapon Attack Modifiers")]
    public float lightAttack1Modifier = 1;

    [Header("Weapon Stamina Cost Modifiers")]
    public float baseStaminaCost = 20;
    public float lightAttack1StaminaCostMultiplier = 0.9f;

    [Header("Weapon Actions")]
    public WeaponItemAction rbAction1H;
    public WeaponItemAction rtAction1H;

}
