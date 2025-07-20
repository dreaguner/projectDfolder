using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<InventorySlot> slots = new();
    public int maxSlots = 100;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool AddItem(ItemData item)
    {
        if (item is ConsumableData)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item && slot.CanAddMore)
                {
                    slot.quantity++;
                    Debug.Log($"? {item.itemName} 스택됨 ({slot.quantity})");
                    return true;
                }
            }
        }

        if (slots.Count >= maxSlots)
        {
            Debug.LogWarning("? 인벤토리 가득 참!");
            return false;
        }

        InventorySlot newSlot = new InventorySlot { item = item, quantity = 1 };
        slots.Add(newSlot);
        Debug.Log($"? {item.itemName} 인벤토리에 추가됨");
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item)
            {
                slots[i].quantity--;
                if (slots[i].quantity <= 0)
                    slots.RemoveAt(i);

                Debug.Log($"??? {item.itemName} 제거됨");
                return;
            }
        }
        Debug.LogWarning($"? 인벤토리에 {item.itemName} 없음");
    }

    public void EquipItemFromInventory(ItemData item)
    {
        if (item is WeaponData weapon)
        {
            if (EquipmentManager.Instance.equippedWeapon != null)
                AddItem(EquipmentManager.Instance.equippedWeapon);
            EquipmentManager.Instance.EquipWeapon(weapon);
        }
        else if (item is ArmorData armor)
        {
            ArmorData previous = null;
            switch (armor.armorType)
            {
                case ArmorType.Head: previous = EquipmentManager.Instance.headArmor; break;
                case ArmorType.Body: previous = EquipmentManager.Instance.bodyArmor; break;
                case ArmorType.Legs: previous = EquipmentManager.Instance.legArmor; break;
                case ArmorType.Arms: previous = EquipmentManager.Instance.armArmor; break;
                case ArmorType.Feet: previous = EquipmentManager.Instance.feetArmor; break;
            }
            if (previous != null) AddItem(previous);
            EquipmentManager.Instance.EquipArmor(armor);
        }
        else if (item is AccessoryData accessory)
        {
            AccessoryData previous = null;
            switch (accessory.accessoryType)
            {
                case AccessoryType.Ring: previous = EquipmentManager.Instance.ringAccessory; break;
                case AccessoryType.Necklace: previous = EquipmentManager.Instance.necklaceAccessory; break;
                case AccessoryType.Bracelet: previous = EquipmentManager.Instance.braceletAccessory; break;
            }
            if (previous != null) AddItem(previous);
            EquipmentManager.Instance.EquipAccessory(accessory);
        }

        RemoveItem(item);
        Debug.Log($"? {item.itemName} 장착됨");
    }

    public void UnequipItemToInventory(string slotType, System.Enum type = null)
    {
        if (slotType == "Weapon")
        {
            if (EquipmentManager.Instance.equippedWeapon != null)
            {
                AddItem(EquipmentManager.Instance.equippedWeapon);
                EquipmentManager.Instance.UnequipWeapon();
                Debug.Log("??? 무기 해제됨");
            }
        }
        else if (slotType == "Armor" && type is ArmorType armorType)
        {
            ArmorData armor = null;
            switch (armorType)
            {
                case ArmorType.Head: armor = EquipmentManager.Instance.headArmor; break;
                case ArmorType.Body: armor = EquipmentManager.Instance.bodyArmor; break;
                case ArmorType.Legs: armor = EquipmentManager.Instance.legArmor; break;
                case ArmorType.Arms: armor = EquipmentManager.Instance.armArmor; break;
                case ArmorType.Feet: armor = EquipmentManager.Instance.feetArmor; break;
            }
            if (armor != null)
            {
                AddItem(armor);
                EquipmentManager.Instance.UnequipArmor(armorType);
                Debug.Log($"??? {armorType} 해제됨");
            }
        }
        else if (slotType == "Accessory" && type is AccessoryType accessoryType)
        {
            AccessoryData accessory = null;
            switch (accessoryType)
            {
                case AccessoryType.Ring: accessory = EquipmentManager.Instance.ringAccessory; break;
                case AccessoryType.Necklace: accessory = EquipmentManager.Instance.necklaceAccessory; break;
                case AccessoryType.Bracelet: accessory = EquipmentManager.Instance.braceletAccessory; break;
            }
            if (accessory != null)
            {
                AddItem(accessory);
                EquipmentManager.Instance.UnequipAccessory(accessoryType);
                Debug.Log($"??? {accessoryType} 해제됨");
            }
        }
    }
    public bool AddItem(ItemData item, int quantity)
    {
        if (item is ConsumableData)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item && slot.CanAddMore)
                {
                    int availableSpace = slot.MaxStack - slot.quantity;
                    int toAdd = Mathf.Min(availableSpace, quantity);
                    slot.quantity += toAdd;
                    quantity -= toAdd;

                    if (quantity <= 0)
                    {
                        Debug.Log($"✅ {item.itemName} {toAdd}개 추가 완료 (스택)");
                        return true;
                    }
                }
            }
        }

        while (quantity > 0)
        {
            if (slots.Count >= maxSlots)
            {
                Debug.LogWarning("⚠️ 인벤토리 최대 슬롯 초과!");
                return false;
            }

            int toAdd = item is ConsumableData ? Mathf.Min(quantity, 5) : 1;
            InventorySlot newSlot = new InventorySlot { item = item, quantity = toAdd };
            slots.Add(newSlot);
            quantity -= toAdd;

            Debug.Log($"✅ {item.itemName} 새 슬롯에 {toAdd}개 추가");
        }

        return true;
    }
}
