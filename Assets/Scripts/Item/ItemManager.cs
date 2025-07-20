using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [Header("무기")]
    public List<WeaponData> weaponItems = new List<WeaponData>();

    [Header("방어구")]
    public List<ArmorData> armorItems = new List<ArmorData>();

    [Header("소비아이템")]
    public List<ItemData> consumableItems = new List<ItemData>();

    [Header("골드 (재화)")]
    public int gold = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("Load All Items")]
    public void LoadAllItems()
    {
        weaponItems.Clear();
        armorItems.Clear();
        consumableItems.Clear();

        var weapons = Resources.LoadAll<WeaponData>("Items/Weapons");
        var armors = Resources.LoadAll<ArmorData>("Items/Armors");
        var consumables = Resources.LoadAll<ItemData>("Items/Consumables");

        weaponItems.AddRange(weapons);
        armorItems.AddRange(armors);
        consumableItems.AddRange(consumables);

        Debug.Log($"[ItemManager] 무기 {weaponItems.Count}개 | 방어구 {armorItems.Count}개 | 소비아이템 {consumableItems.Count}개 로드됨");
    }

    [ContextMenu("Print All Items")]
    public void PrintAllItems()
    {
        Debug.Log("==== 무기 ====");
        foreach (var item in weaponItems) Debug.Log(item.itemName);

        Debug.Log("==== 방어구 ====");
        foreach (var item in armorItems) Debug.Log(item.itemName);

        Debug.Log("==== 소비아이템 ====");
        foreach (var item in consumableItems) Debug.Log(item.itemName);

        Debug.Log($"==== 골드: {gold} ====");
    }
    public ItemData GetItemByCode(string code)
    {
        var found = weaponItems.FirstOrDefault(w => w.itemCode == code);
        if (found != null) return found;

        var armor = armorItems.FirstOrDefault(a => a.itemCode == code);
        if (armor != null) return armor;

        return consumableItems.FirstOrDefault(c => c.itemCode == code);
    }
}
