using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity = 1;

    public bool IsStackable => item is ConsumableData;
    public int MaxStack => IsStackable ? 5 : 1;

    public bool CanAddMore => quantity < MaxStack;
}
