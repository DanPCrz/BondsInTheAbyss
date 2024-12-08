using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ButtonUIManager : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject disabledOverlay;
    [SerializeField] private Button button;

    private CharacterSelectionManager characterSelect;

    public CharacterAssetData Character { get; private set; }
    public bool IsDisabled { get; private set; }

    public void SetCharacter(CharacterSelectionManager characterSelect, CharacterAssetData character)
    {
        iconImage.sprite = character.Icon;

        this.characterSelect = characterSelect;

        Character = character;
    }

    public void SelectCharacter()
    {
        characterSelect.Select(Character);
    }

    public void SetDisabled()
    {
        IsDisabled = true;
        disabledOverlay.SetActive(true);
        button.interactable = false;
    }
}