using UnityEngine;

public class SaveMenuManager : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Title Screen Inputs")]
    [SerializeField] bool deleteCharacterslot = false;

    private void Update()
    {
        if (deleteCharacterslot)
        {
            deleteCharacterslot = false;
            TitleScreenManager.instance.AttemptDeleteSaveSlot();
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.UI.X.performed += ctx => deleteCharacterslot = true;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
