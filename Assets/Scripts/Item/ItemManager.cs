using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [Header("����")]
    public List<WeaponData> weaponItems = new List<WeaponData>();

    [Header("��")]
    public List<ArmorData> armorItems = new List<ArmorData>();

    [Header("�Һ������")]
    public List<ItemData> consumableItems = new List<ItemData>();

    [Header("��� (��ȭ)")]
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

        Debug.Log($"[ItemManager] ���� {weaponItems.Count}�� | �� {armorItems.Count}�� | �Һ������ {consumableItems.Count}�� �ε��");
    }

    [ContextMenu("Print All Items")]
    public void PrintAllItems()
    {
        Debug.Log("==== ���� ====");
        foreach (var item in weaponItems) Debug.Log(item.itemName);

        Debug.Log("==== �� ====");
        foreach (var item in armorItems) Debug.Log(item.itemName);

        Debug.Log("==== �Һ������ ====");
        foreach (var item in consumableItems) Debug.Log(item.itemName);

        Debug.Log($"==== ���: {gold} ====");
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
