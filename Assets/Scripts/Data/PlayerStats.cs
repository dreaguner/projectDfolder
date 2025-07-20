using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    Fire, Water, Earth, Wind, Light, Dark
}

[System.Serializable]
public class Skill
{
    public string name;
    [TextArea] public string description;
}

public class PlayerStats : MonoBehaviour
{
    [Header("플레이어 기본 정보")]
    public string playerName;
    public int level;
    public int age;

    [Header("특성")]
    public List<Trait> traits = new();

    [Header("기본 스탯")]
    public int healthStat;
    public int manaStat;
    public int str;
    public int dex;
    public int intelligence;

    [Header("기본 스탯 리스트")]
    public List<Stat> stats = new();

    [Header("장비로 합산된 최종 스탯")]
    public int finalAttack;
    public int finalMagicAttack;
    public int finalPhysicalDefense;
    public int finalMagicDefense;
    public Dictionary<ElementType, int> elementalAttack = new();

    [Header("플레이어가 배운 스킬 ID 리스트")]
    public List<string> learnedSkillIds = new();

    // ✅ 신규 추가 필드
    [Header("보정 배율")]
    public int expRate = 100;
    public int goldRate = 100;
    public int skillExpRate = 100; // 전체용 기본

    public Dictionary<string, int> skillExpRates = new(); // 스킬별 경험치 배율

    [Header("성향/저주/축복")]
    [Range(-100, 100)] public float morality = 0f; // 선 vs 악
    [Range(-100, 100)] public float order = 0f;    // 질서 vs 혼돈
    public int curse = 0;
    public int blessing = 0;


    [Header("전투 판정 확률")]
    [Range(0, 100)] public int accuracy = 50;
    [Range(0, 100)] public int evasion = 10;
    [Range(0, 100)] public int criticalChance = 0;

    public int CurrentHP => healthStat * 10;
    public int CurrentMP => manaStat * 10;
    public int currentMP;

    public void SpendMP(int amount)
    {
        currentMP = Mathf.Max(0, currentMP - amount);
        Debug.Log($"MP {amount} 소모 → 현재 MP: {currentMP}");
    }

    [Header("골드")]
    public int gold = 0;


    /// <summary>
    /// 장비 포함 모든 스탯 다시 계산
    /// </summary>
    public void RecalculateStats()
    {
        finalAttack = 0;
        finalMagicAttack = 0;
        finalPhysicalDefense = 0;
        finalMagicDefense = 0;
        elementalAttack.Clear();

        var weapon = EquipmentManager.Instance.equippedWeapon;

        // ? 무기 계산
        if (weapon != null)
        {
            AddItemStats(weapon);
        }
        else
        {
            // 무기 없음이면 str x 1배만
            finalAttack += str * 1;
        }

        // ? 방어구 계산
        ArmorData[] armors = {
            EquipmentManager.Instance.headArmor,
            EquipmentManager.Instance.bodyArmor,
            EquipmentManager.Instance.legArmor,
            EquipmentManager.Instance.armArmor,
            EquipmentManager.Instance.feetArmor
        };

        foreach (var armor in armors)
        {
            if (armor != null)
                AddItemStats(armor);
        }

        // ? 악세사리 계산
        AccessoryData[] accessories = {
            EquipmentManager.Instance.ringAccessory,
            EquipmentManager.Instance.necklaceAccessory,
            EquipmentManager.Instance.braceletAccessory
        };

        foreach (var acc in accessories)
        {
            if (acc != null)
                AddItemStats(acc);
        }

        Debug.Log($"? 최종 스탯: 공:{finalAttack} / 마공:{finalMagicAttack} / 방:{finalPhysicalDefense} / 마방:{finalMagicDefense}");
    }

    /// <summary>
    /// 아이템 하나 기준으로 스탯 추가 (base + 스케일)
    /// </summary>
    void AddItemStats(ItemData item)
    {
        // baseStat
        switch (item.baseStatName.ToLower())
        {
            case "atk": finalAttack += item.baseStatValue; break;
            case "mtk": finalMagicAttack += item.baseStatValue; break;
            case "add": finalPhysicalDefense += item.baseStatValue; break;
            case "apd": finalMagicDefense += item.baseStatValue; break;
            case "ele":
                if (System.Enum.TryParse(item.elementId, true, out ElementType eleType))
                {
                    if (!elementalAttack.ContainsKey(eleType))
                        elementalAttack[eleType] = 0;
                    elementalAttack[eleType] += item.baseStatValue;
                }
                break;
        }

        // 스케일 스탯
        int scaleStatValue = GetStatByName(item.skillStatName);
        int scaled = Mathf.RoundToInt(scaleStatValue * item.skillStatRatio);

        switch (item.baseStatName.ToLower())
        {
            case "atk": finalAttack += scaled; break;
            case "mtk": finalMagicAttack += scaled; break;
            case "add": finalPhysicalDefense += scaled; break;
            case "apd": finalMagicDefense += scaled; break;
            case "ele":
                if (System.Enum.TryParse(item.elementId, true, out ElementType eleType))
                {
                    if (!elementalAttack.ContainsKey(eleType))
                        elementalAttack[eleType] = 0;
                    elementalAttack[eleType] += scaled;
                }
                break;
        }
    }

    int GetStatByName(string statName)
    {
        return statName switch
        {
            "str" => str,
            "dex" => dex,
            "int" => intelligence,
            _ => 0
        };
    }

    /// <summary>
    /// 출력용 상태 출력 예시
    /// </summary>
    public void PrintStatus()
    {
        Debug.Log($"이름: {playerName}");
        Debug.Log($"레벨: {level}");
        Debug.Log($"나이: {age}");
        Debug.Log($"HP: {CurrentHP}");
        Debug.Log($"MP: {CurrentMP}");

        if (finalAttack != 0) Debug.Log($"공격력: {finalAttack}");
        if (finalMagicAttack != 0) Debug.Log($"마공: {finalMagicAttack}");
        if (finalPhysicalDefense != 0) Debug.Log($"방: {finalPhysicalDefense}");
        if (finalMagicDefense != 0) Debug.Log($"마방: {finalMagicDefense}");

        foreach (var ele in elementalAttack)
        {
            if (ele.Value != 0)
                Debug.Log($"{ele.Key} 속성 공격력: {ele.Value}");
        }

        Debug.Log("특성:");
        foreach (var t in traits)
            Debug.Log($"- {t.name}");

        Debug.Log("스킬 ID:");
        foreach (var id in learnedSkillIds)
            Debug.Log($"- {id}");

        Debug.Log("기본 스탯:");
        Debug.Log($"체력 스탯: {healthStat}");
        Debug.Log($"마나 스탯: {manaStat}");
        Debug.Log($"힘: {str}");
        Debug.Log($"민첩: {dex}");
        Debug.Log($"마력: {intelligence}");
    }

}
