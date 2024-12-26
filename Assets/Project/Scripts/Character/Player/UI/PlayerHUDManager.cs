using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("Stat Bars")]
    [SerializeField] UIStatBar staminaBar;
    [SerializeField] UIStatBar healthBar;

    [Header("Quick Slots")]
    [SerializeField] Image rightWeaponQuickSlot;
    [SerializeField] Image leftWeaponQuickSlot;
    [SerializeField] Image itemQuickSlot;
    [SerializeField] Image abilityQuickSlot;

    public void RefreshHUD() 
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
    }
    public void SetNewStaminaValue(float oldStamina, float newStamina)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newStamina));
    }

    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina);
    }

    public void SetNewHealthValue(int oldHealth, int newHealth)
    {
        healthBar.SetStat(newHealth);
    }

    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }

    public void SetLeftWeaponQuickSlot(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponItemByID(weaponID);

        if (weapon == null)
        {
            Debug.Log("Item is null");
            leftWeaponQuickSlot.enabled = false;
            leftWeaponQuickSlot.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("Icon is null");
            leftWeaponQuickSlot.enabled = false;
            leftWeaponQuickSlot.sprite = null;
            return;
        }
        leftWeaponQuickSlot.sprite = weapon.itemIcon;
        leftWeaponQuickSlot.enabled = true;
    }

    public void SetRightWeaponQuickSlot(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponItemByID(weaponID);

        if (weapon == null)
        {
            Debug.Log("Item is null");
            rightWeaponQuickSlot.enabled = false;
            rightWeaponQuickSlot.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("Icon is null");
            rightWeaponQuickSlot.enabled = false;
            rightWeaponQuickSlot.sprite = null;
            return;
        }
        rightWeaponQuickSlot.sprite = weapon.itemIcon;
        rightWeaponQuickSlot.enabled = true;
    }
}
