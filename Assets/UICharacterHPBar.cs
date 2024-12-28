using TMPro;
using UnityEngine;

public class UICharacterHPBar : UIStatBar
{
    private CharacterManager character;
    private AICharacterManager aiCharacter;
    private PlayerManager player;

    [SerializeField] float defaultTimeBeforeBarHides = 3;
    [SerializeField] float hideTimer = 0;
    [SerializeField] float currentDamageTaken = 0;
    [SerializeField] TextMeshProUGUI characterDamage;
    
    [HideInInspector]public int oldHealthValue = 0;

    protected override void Awake()
    {
        base.Awake();
        character = GetComponentInParent<CharacterManager>();

        if (character != null)
        {
            aiCharacter = character as AICharacterManager;
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
        slider.maxValue = character.characterNetworkManager.maxHealth.Value;
        float oldDamage = currentDamageTaken;
        currentDamageTaken = Mathf.RoundToInt(currentDamageTaken + (oldHealthValue - newValue));

        if (currentDamageTaken < 0)
        {
            currentDamageTaken = Mathf.Abs(currentDamageTaken);
            characterDamage.text = "+ " + currentDamageTaken.ToString();
        }
        else
        {
            characterDamage.text = "- " + currentDamageTaken.ToString();
        }

        slider.value = newValue;

        if (character.characterNetworkManager.currentHealth.Value != character.characterNetworkManager.maxHealth.Value)
        {
            hideTimer = defaultTimeBeforeBarHides;
            gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);

        if (hideTimer > 0)
        {
            hideTimer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        currentDamageTaken = 0;
    }
}
