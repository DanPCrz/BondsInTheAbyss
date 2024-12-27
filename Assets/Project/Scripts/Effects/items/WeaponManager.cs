using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider meleeDamageCollider;

    public void Awake()
    {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>(); 
    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
    {
        meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
        meleeDamageCollider.physicalDamage = weapon.physicalDamage;
        meleeDamageCollider.magicalDamage = weapon.magicalDamage;
        meleeDamageCollider.fireDamage = weapon.fireDamage;
        meleeDamageCollider.lightningDamage = weapon.lightningDamage;
        meleeDamageCollider.poisonDamage = weapon.poisonDamage;

        meleeDamageCollider.lightAttack1Modifier = weapon.lightAttack1Modifier;
        meleeDamageCollider.lightAttack2Modifier = weapon.lightAttack2Modifier;
        meleeDamageCollider.lightAttack3Modifier = weapon.lightAttack3Modifier;
        meleeDamageCollider.heavyAttack1Modifier = weapon.heavyAttack1Modifier;
        meleeDamageCollider.heavyAttack2Modifier = weapon.heavyAttack2Modifier;
        meleeDamageCollider.chargedAttack1Modifier = weapon.chargedAttack1Modifier;
        meleeDamageCollider.chargedAttack2Modifier = weapon.chargedAttack2Modifier; 
    }
}
