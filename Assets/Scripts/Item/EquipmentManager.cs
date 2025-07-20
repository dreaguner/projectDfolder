using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    [Header("무기 슬롯")]
    public WeaponData equippedWeapon;

    [Header("방어구 슬롯")]
    public ArmorData headArmor;
    public ArmorData bodyArmor;
    public ArmorData legArmor;
    public ArmorData armArmor;
    public ArmorData feetArmor;

    [Header("악세사리 슬롯")]
    public AccessoryData ringAccessory;
    public AccessoryData necklaceAccessory;
    public AccessoryData braceletAccessory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region 무기
    public void EquipWeapon(WeaponData weapon)
    {
        equippedWeapon = weapon;
        Debug.Log($"무기 장착: {weapon.itemName}");
    }

    public void UnequipWeapon()
    {
        equippedWeapon = null;
        Debug.Log($"무기 해제");
    }
    #endregion

    #region 방어구
    public void EquipArmor(ArmorData armor)
    {
        switch (armor.armorType) // ✅ ArmorType 참조만!
        {
            case ArmorType.Head: headArmor = armor; break;
            case ArmorType.Body: bodyArmor = armor; break;
            case ArmorType.Legs: legArmor = armor; break;
            case ArmorType.Arms: armArmor = armor; break;
            case ArmorType.Feet: feetArmor = armor; break;
        }
        Debug.Log($"방어구 장착: {armor.itemName} ({armor.armorType})");
    }

    public void UnequipArmor(ArmorType type) // ✅ ArmorType 참조만!
    {
        switch (type)
        {
            case ArmorType.Head: headArmor = null; break;
            case ArmorType.Body: bodyArmor = null; break;
            case ArmorType.Legs: legArmor = null; break;
            case ArmorType.Arms: armArmor = null; break;
            case ArmorType.Feet: feetArmor = null; break;
        }
        Debug.Log($"{type} 방어구 해제");
    }
    #endregion

    #region 악세사리
    public void EquipAccessory(AccessoryData accessory)
    {
        switch (accessory.accessoryType) // ✅ AccessoryType 참조만!
        {
            case AccessoryType.Ring: ringAccessory = accessory; break;
            case AccessoryType.Necklace: necklaceAccessory = accessory; break;
            case AccessoryType.Bracelet: braceletAccessory = accessory; break;
        }
        Debug.Log($"악세사리 장착: {accessory.itemName} ({accessory.accessoryType})");
    }

    public void UnequipAccessory(AccessoryType type) // ✅ AccessoryType 참조만!
    {
        switch (type)
        {
            case AccessoryType.Ring: ringAccessory = null; break;
            case AccessoryType.Necklace: necklaceAccessory = null; break;
            case AccessoryType.Bracelet: braceletAccessory = null; break;
        }
        Debug.Log($"{type} 악세사리 해제");
    }
    #endregion

    [ContextMenu("Print Equipped Items")]
    public void PrintEquippedItems()
    {
        Debug.Log($"🗡️ 무기: {(equippedWeapon != null ? equippedWeapon.itemName : "없음")}");
        Debug.Log($"🛡️ 머리: {(headArmor != null ? headArmor.itemName : "없음")}");
        Debug.Log($"🛡️ 상체: {(bodyArmor != null ? bodyArmor.itemName : "없음")}");
        Debug.Log($"🛡️ 하체: {(legArmor != null ? legArmor.itemName : "없음")}");
        Debug.Log($"🛡️ 팔: {(armArmor != null ? armArmor.itemName : "없음")}");
        Debug.Log($"🛡️ 발: {(feetArmor != null ? feetArmor.itemName : "없음")}");
        Debug.Log($"💍 반지: {(ringAccessory != null ? ringAccessory.itemName : "없음")}");
        Debug.Log($"📿 목걸이: {(necklaceAccessory != null ? necklaceAccessory.itemName : "없음")}");
        Debug.Log($"🔗 팔찌: {(braceletAccessory != null ? braceletAccessory.itemName : "없음")}");
    }
}
