using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{


    [SerializeField] UIStatBar staminaBar;
    [SerializeField] UIStatBar healthBar;


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
}
