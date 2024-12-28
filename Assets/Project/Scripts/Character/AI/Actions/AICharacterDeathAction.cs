using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Death")]
public class AICharacterDeathAction : ScriptableObject
{
    [Header("Attack Action")]
    [SerializeField] private string deathAnimation;
    public void AttemptToPerformAction(AICharacterManager aiCharacter)
    {
        aiCharacter.characterAnimationManager.PlayTargetAnimation(deathAnimation, true, true);
    }
}
