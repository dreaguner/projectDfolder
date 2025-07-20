using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSaveData
{
    public string playerName;
    public int level;
    public int age;

    public int currentFloor;

    public int currentHP;
    public int currentMP;

    public int gold;

    public float morality;
    public float order;
    public int curse;
    public int blessing;

    public List<string> learnedSkillIds = new();
    public List<string> traitCodes = new();
    public List<StatDataSave> stats = new();

    public List<InventoryItemSave> inventory = new();
    public EquippedDataSave equipped = new();
}

[Serializable]
public class InventoryItemSave
{
    public string itemId;
    public int quantity;
}

[Serializable]
public class EquippedDataSave
{
    public string weaponId;
    public string headArmorId;
    public string bodyArmorId;
    // 나머지 슬롯도 여기에!
}

[Serializable]
public class StatDataSave
{
    public string name;
    public int value;
}
