using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AICharacterNetworkManager : CharacterNetworkManager
{
    AICharacterManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();
        aiCharacter = GetComponent<AICharacterManager>();
    }
    public void SetNewMaxHealthValue(int oldVitality, int newVitality)
    {
        aiCharacter.aiCharacterNetworkManager.maxHealth.Value = aiCharacter.aiCharacterStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
        aiCharacter.aiCharacterNetworkManager.currentHealth.Value = aiCharacter.aiCharacterNetworkManager.maxHealth.Value;
    }
}
