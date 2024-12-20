using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager player;

    public NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>("Player", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void SetNewMaxHealthValue(int oldVitality, int newVitality)
    {
        maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
        PlayerUIManager.instance.playerHUDManager.SetMaxHealthValue(maxHealth.Value);
        currentHealth.Value = maxHealth.Value;
    }

    public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
    {
        maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
        PlayerUIManager.instance.playerHUDManager.SetMaxStaminaValue(maxStamina.Value);
        currentStamina.Value = maxStamina.Value;
    }
}