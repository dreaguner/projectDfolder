using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "GameData/NPCData")]
public class NPCData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string npcId;
    public string npcName;

    [Header("����")]
    [Range(-100, 100)] public float morality;
    [Range(-100, 100)] public float order;

    [Header("����/���� ����")]
    public float recruitMoralityMin;
    public float recruitMoralityMax;
    public float recruitOrderMin;
    public float recruitOrderMax;
    public List<NPCCondition> specialConditions = new();

    [Header("����")]
    public int maxHP;
    public int finalAttack;
    public int finalMagicAttack;
    public int finalPhysicalDefense;
    public int finalMagicDefense;

    public int dex;
    public int accuracy;
    public int evasion;
    public int criticalChance;

    [Header("��ų")]
    public List<string> skillIds = new(); // learnedSkillIds�� ���� ����

    [Header("���")]
    public ItemData equippedWeapon;
    public ArmorData[] equippedArmors = new ArmorData[5];
    public AccessoryData[] equippedAccessories = new AccessoryData[3];

    [Header("����")]
    public bool isRecruitable = false;
    public bool isEnemy = false;
}

[System.Serializable]
public class NPCCondition
{
    public string conditionType; // ��: "QuestClear", "TraitOwned"
    public string parameter;     // ��: "quest_004", "�غ���"
}
