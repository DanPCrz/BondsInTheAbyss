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
        meleeDamageCollider.heavyAttack1Modifier = weapon.heavyAttack1Modifier;
        meleeDamageCollider.chargedAttack1Modifier = weapon.chargedAttack1Modifier;
    }
}
