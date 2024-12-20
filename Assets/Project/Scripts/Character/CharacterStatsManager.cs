using System.Globalization;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Stamina Regeneration")]
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    private float staminaRegenerationAmount = 2;
    [SerializeField] float staminaRegenerationDelay = 2;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;

        stamina = 100 + (10 * Mathf.Log(endurance));

        return Mathf.RoundToInt(stamina);
    }

    public int CalculateHealthBasedOnVitalityLevel(int vitality)
    {
        float health = 0;

        health = 125 + (15 * Mathf.Log(vitality));

        return Mathf.RoundToInt(health);
    }

    public virtual void RegenerateStamina()
    {
        if (!character.IsOwner || character.characterNetworkManager.isSprinting.Value || character.isPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }

    public void ResetStaminaRegenTimer(float PreviousStaminaAmount, float CurrentStaminaAmount)
    {
        if (CurrentStaminaAmount < PreviousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
    }
}
