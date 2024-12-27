using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Target Info")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetDirection;

    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    [SerializeField] float minimumDetectionAngle = -40;
    [SerializeField] float maximumDetectionAngle = 40;

    public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
    {
        if (currentTarget != null)
            return;

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());
        
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if (targetCharacter == null || targetCharacter == aiCharacter || targetCharacter.isDead.Value)
                continue;

            if (WorldUtilityManager.instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
            {
                Vector3 targetDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                if (viewableAngle < maximumDetectionAngle && viewableAngle > minimumDetectionAngle)
                {
                    if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, 
                        targetCharacter.characterCombatManager.lockOnTransform.position, 
                        WorldUtilityManager.instance.GetEnvironmentLayers()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, 
                            targetCharacter.characterCombatManager.lockOnTransform.position, Color.red);
                        Debug.Log("Blocked");
                    }
                    else
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position,
                            targetCharacter.characterCombatManager.lockOnTransform.position, Color.green);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                        //PivotTowardsTarget(aiCharacter);   
                    }
                }
            }
                
        }   
    }

    //private void PivotTowardsTarget(AICharacterManager aiCharacter)
    //{
    //    if (aiCharacter.isPerformingAction)
    //        return;

    //    if (viewableAngle >= 20 && viewableAngle <= 60)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Right 45", true);
    //    }
    //    else if (viewableAngle <= -20 && viewableAngle >= -60)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Left 45", true);
    //    }
    //    if (viewableAngle >= 61 && viewableAngle <= 110)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Right 90", true);
    //    }
    //    else if (viewableAngle <= -61 && viewableAngle >= -110)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Left 90", true);
    //    }
    //    if (viewableAngle >= 111 && viewableAngle <= 145)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Right 135   ", true);
    //    }
    //    else if (viewableAngle <= -111 && viewableAngle >= -145)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Left 135", true);
    //    }
    //    if (viewableAngle >= 146 && viewableAngle <= 180)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Right 180", true);
    //    }
    //    else if (viewableAngle <= -146 && viewableAngle >= -180)
    //    {
    //        aiCharacter.characterAnimationManager.PlayTargetAnimation("Turn Left 180", true);
    //    }
    //}   
}
