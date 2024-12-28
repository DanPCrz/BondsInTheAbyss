using UnityEngine;

public class SkeletonDamageCollider : DamageCollider
{
    [SerializeField] AICharacterManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();
        damageCollider = GetComponent<Collider>();
        aiCharacter = GetComponentInParent<AICharacterManager>();
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
        damageEffect.angleHitFrom = Vector3.SignedAngle(aiCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);

        //damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

        if (damageTarget.IsOwner)
        {
            damageTarget.characterNetworkManager.NotifyCharacterDamageToServerRPC(
                damageTarget.NetworkObjectId, damageEffect.physicalDamage, 
                damageEffect.magicalDamage, damageEffect.fireDamage, 
                damageEffect.lightningDamage, damageEffect.poisonDamage, 
                damageEffect.poiseDamage, aiCharacter.NetworkObjectId, 
                damageEffect.angleHitFrom, damageEffect.contactPoint.x, 
                damageEffect.contactPoint.y, damageEffect.contactPoint.z);
        }
    }
}
