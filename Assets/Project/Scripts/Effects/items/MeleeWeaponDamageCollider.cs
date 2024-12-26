using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage;

    [Header("Weapon Attack Modifiers")]
    public float lightAttack1Modifier;
    public float lightAttack2Modifier;
    public float heavyAttack1Modifier;
    public float chargedAttack1Modifier;

    protected override void Awake()
    {
        base.Awake();

        if (damageCollider == null)
        {
            damageCollider = GetComponent<Collider>();
        }
        damageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            if (damageTarget == characterCausingDamage)
                return;

            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(damageTarget);
        }
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        if (charactersDamaged.Contains(damageTarget))
            return;

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicalDamage = magicalDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.poisonDamage = poisonDamage;
        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        switch (characterCausingDamage.characterCombatManager.currentAttackType)
        {
            case AttackType.LightAttack1:
                ApplyAttackDamageModifiers(lightAttack1Modifier, damageEffect);
                break;
            case AttackType.LightAttack2:
                ApplyAttackDamageModifiers(lightAttack2Modifier, damageEffect);
                break;
            case AttackType.HeavyAttack1:
                ApplyAttackDamageModifiers(heavyAttack1Modifier, damageEffect);
                break;
            case AttackType.ChargedAttack1:
                ApplyAttackDamageModifiers(chargedAttack1Modifier, damageEffect);
                break;
            default:
                break;
        }

        //damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

        if (characterCausingDamage.IsOwner)
        {
            damageTarget.characterNetworkManager.NotifyCharacterDamageToServerRPC(damageTarget.NetworkObjectId, damageEffect.physicalDamage, damageEffect.magicalDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.poisonDamage, damageEffect.poiseDamage,
                characterCausingDamage.NetworkObjectId, damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
        }
    }

    private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
    {
        damage.physicalDamage *= modifier;
        damage.magicalDamage *= modifier;
        damage.fireDamage *= modifier;
        damage.lightningDamage *= modifier;
        damage.poisonDamage *= modifier;
    }
}
