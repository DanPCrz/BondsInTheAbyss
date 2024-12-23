using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public WeaponItem currentRightHandWeapon;
    public WeaponItem currentLeftHandWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
    public int rightHandSlotIndex = 0;
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];
    public int leftHandSlotIndex = 0;
}
