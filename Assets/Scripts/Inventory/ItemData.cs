using UnityEngine;


public enum ItemType
{
    PlayerWeapon,
    Note
};

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea(5, 10)] public string itemDescription;
    [TextArea(5, 10)] public string fullNoteContent; // Used to give the player the ability to re-read notes they already picked up

    public ItemType type;

   

    public Sprite icon;
}