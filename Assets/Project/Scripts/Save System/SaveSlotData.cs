using UnityEngine;

[System.Serializable]
public class SaveSlotData
{
    [Header("Save Info")]
    public string saveName = "player";

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Stats")]
    public int currentHealth;
    public float currentStamina;

    [Header("Attributes")]
    public int vitality;
    public int endurance;
}
