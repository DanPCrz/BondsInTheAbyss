using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightWeaponModel;
    public GameObject leftWeaponModel;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
        InitializaWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();
        LoadWeaponsOnBothHands();
    }

    private void InitializaWeaponSlots()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    public void SwitchRightWeapon()
    {
        if (!player.IsOwner)
            return;
        player.playerAnimationManager.PlayTargetAnimation("Switch Right Weapon", false, true, true, true);

        WeaponItem selectedWeapon = null;

        player.playerInventoryManager.rightHandSlotIndex += 1;

        if (player.playerInventoryManager.rightHandSlotIndex < 0 || player.playerInventoryManager.rightHandSlotIndex > 2)
        {
            player.playerInventoryManager.rightHandSlotIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount++;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }
            if (weaponCount <= 1)
            {
                player.playerInventoryManager.rightHandSlotIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.rightHandSlotIndex = firstWeaponPosition;
                player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
        {
            if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandSlotIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandSlotIndex];
                player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                return;
            }
        }

        if (selectedWeapon != null && player.playerInventoryManager.rightHandSlotIndex <= 2)
        {
            SwitchRightWeapon();
        }
    }

    public void LoadRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon != null)
        {
            rightHandSlot.UnloadWeapon();
            rightWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);    
            rightHandSlot.LoadWeapon(rightWeaponModel);
            rightWeaponManager = rightWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    public void SwitchLeftWeapon()
    {
        if (!player.IsOwner)
            return;
        player.playerAnimationManager.PlayTargetAnimation("Switch Right Weapon", false, true, true, true);

        WeaponItem selectedWeapon = null;

        player.playerInventoryManager.leftHandSlotIndex += 1;

        if (player.playerInventoryManager.leftHandSlotIndex < 0 || player.playerInventoryManager.leftHandSlotIndex > 2)
        {
            player.playerInventoryManager.leftHandSlotIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount++;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }
            if (weaponCount <= 1)
            {
                player.playerInventoryManager.leftHandSlotIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.leftHandSlotIndex = firstWeaponPosition;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
        {
            if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandSlotIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandSlotIndex];
                player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                return;
            }
        }

        if (selectedWeapon != null && player.playerInventoryManager.leftHandSlotIndex <= 2)
        {
            SwitchLeftWeapon();
        }
    }

    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            leftHandSlot.UnloadWeapon();
            leftWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftWeaponModel);
            leftWeaponManager = leftWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }
}
