using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public bool hasFloatingHPBar = true;
    public UICharacterHPBar characterHPBar;

    public void OnHPChanged(int oldValue, int newValue)
    {
        characterHPBar.oldHealthValue = oldValue;
        characterHPBar.SetStat(newValue);
    }
}
