using System.Collections;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [Header("Debug Menu")]
    [SerializeField] bool respawnCharacter = false;

    [HideInInspector] public PlayerAnimationManager playerAnimationManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;

    protected override void Awake()
    {
        base.Awake();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;
        
        playerLocomotionManager.HandleAllMovement();
        playerStatsManager.RegenerateStamina();

        DebugMenu();
    }

    protected override void LateUpdate()
    {
        if (!IsOwner)
            return;

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
            
        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
            WorldGameSaveManager.instance.player = this;

            playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerHUDManager.SetNewHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerHUDManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;           
        }
        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;
    }

    public override IEnumerator ProcessDeathEvent(bool mannualySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            PlayerUIManager.instance.playerPopUpManager.SendYouDiedPopUp();
        }

        return base.ProcessDeathEvent(mannualySelectDeathAnimation);
    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        if (IsOwner)
        {
            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

            playerAnimationManager.PlayTargetAnimation("Get Up", false);
        }
    }

    public void SaveGameToCurrentSlot(ref SaveSlotData currentSlotData)
    {
        currentSlotData.saveName = playerNetworkManager.playerName.Value.ToString();
        currentSlotData.xPosition = transform.position.x;
        currentSlotData.yPosition = transform.position.y;
        currentSlotData.zPosition = transform.position.z;

        currentSlotData.currentHealth = playerNetworkManager.currentHealth.Value;
        currentSlotData.currentStamina = playerNetworkManager.currentStamina.Value;

        currentSlotData.vitality = playerNetworkManager.vitality.Value;
        currentSlotData.endurance = playerNetworkManager.endurance.Value;
    }

    public void LoadGameFromCurrentSlot(ref SaveSlotData currentSlotData)
    {
        playerNetworkManager.playerName.Value = currentSlotData.saveName;
        Vector3 myPosition = new Vector3(currentSlotData.xPosition, currentSlotData.yPosition, currentSlotData.zPosition);
        transform.position = myPosition;

        playerNetworkManager.vitality.Value = currentSlotData.vitality;
        playerNetworkManager.endurance.Value = currentSlotData.endurance;

        playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
        playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
        playerNetworkManager.currentStamina.Value = currentSlotData.currentStamina;
        playerNetworkManager.currentHealth.Value = currentSlotData.currentHealth;
        PlayerUIManager.instance.playerHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        PlayerUIManager.instance.playerHUDManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);
    }

    private void DebugMenu()
    {
        if (respawnCharacter)
        {
            respawnCharacter = false;
            ReviveCharacter();
        }
    }
}
