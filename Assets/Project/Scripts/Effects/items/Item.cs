using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public int itemID;
    public Sprite itemIcon;
    [TextArea(3, 10)] public string itemDescription;
}
