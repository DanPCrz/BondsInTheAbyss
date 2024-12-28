using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerAurasUI : UIStatBar
{
    private CharacterManager character;
    private PlayerManager player;

    public int currentAuras = 0;
    [SerializeField] TextMeshProUGUI playerAuras;

    protected override void Awake()
    {
        base.Awake();
        character = GetComponentInParent<CharacterManager>();

        if (character != null)
        {
            player = character as PlayerManager;
        }
    }

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }

    public override void SetStat(int newValue)
    {
        playerAuras.text = player.playerNetworkManager.Auras.Value.ToString();
        float oldValue = currentAuras;
        currentAuras = Mathf.RoundToInt(currentAuras + (oldValue - newValue));

        if (currentAuras < 0)
        {
            currentAuras = Mathf.Abs(currentAuras);
            playerAuras.text = "+ " + currentAuras.ToString();
        }
        else
        {
            playerAuras.text = "- " + currentAuras.ToString();
        }

        playerAuras.text = newValue.ToString();
        
        gameObject.SetActive(true);
        
    }
}
