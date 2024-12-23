using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] MeleeWeaponDamageCollider meleeDamageCollider;

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
    }
}
