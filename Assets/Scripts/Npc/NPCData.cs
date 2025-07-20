using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "GameData/NPCData")]
public class NPCData : ScriptableObject
{
    [Header("기본 정보")]
    public string npcId;
    public string npcName;

    [Header("성향")]
    [Range(-100, 100)] public float morality;
    [Range(-100, 100)] public float order;

    [Header("영입/적대 조건")]
    public float recruitMoralityMin;
    public float recruitMoralityMax;
    public float recruitOrderMin;
    public float recruitOrderMax;
    public List<NPCCondition> specialConditions = new();

    [Header("스탯")]
    public int maxHP;
    public int finalAttack;
    public int finalMagicAttack;
    public int finalPhysicalDefense;
    public int finalMagicDefense;

    public int dex;
    public int accuracy;
    public int evasion;
    public int criticalChance;

    [Header("스킬")]
    public List<string> skillIds = new(); // learnedSkillIds와 동일 구조

    [Header("장비")]
    public ItemData equippedWeapon;
    public ArmorData[] equippedArmors = new ArmorData[5];
    public AccessoryData[] equippedAccessories = new AccessoryData[3];

    [Header("상태")]
    public bool isRecruitable = false;
    public bool isEnemy = false;
}

[System.Serializable]
public class NPCCondition
{
    public string conditionType; // 예: "QuestClear", "TraitOwned"
    public string parameter;     // 예: "quest_004", "해부학"
}
