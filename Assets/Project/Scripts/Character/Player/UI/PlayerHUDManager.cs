using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] UIStatBar staminaBar;

    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxStaminaValue(int maxValue)
    {
        staminaBar.SetMaxStat(maxValue);
    }
}
