using UnityEngine;

public class AICharacterStatsManager : CharacterStatsManager
{
    AICharacterManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();
        aiCharacter = GetComponent<AICharacterManager>();
    }

    protected override void Start()
    {
        base.Start();

        CalculateHealthBasedOnVitalityLevel(aiCharacter.aiCharacterNetworkManager.vitality.Value);
        CalculateStaminaBasedOnEnduranceLevel(aiCharacter.aiCharacterNetworkManager.endurance.Value);
    }
}
