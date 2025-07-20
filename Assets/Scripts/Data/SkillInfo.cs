using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SkillInfo", menuName = "GameData/SkillInfo")]
public class SkillInfo : ScriptableObject
{
    public SkillData[] skillList;

    // ✅ 딕셔너리 추가!
    private Dictionary<string, SkillData> skillDict;

    /// <summary>
    /// SkillInfo 딕셔너리 캐싱 (한 번만 초기화)
    /// </summary>
    public void Initialize()
    {
        if (skillDict == null)
        {
            skillDict = new Dictionary<string, SkillData>();
            foreach (var skill in skillList)
            {
                skillDict[skill.skillId] = skill;
            }
        }
    }

    /// <summary>
    /// SkillId로 SkillData 가져오기
    /// </summary>
    public SkillData GetSkillById(string id)
    {
        if (skillDict == null) Initialize();

        if (skillDict.TryGetValue(id, out SkillData s))
        {
            return s;
        }

        Debug.LogWarning($"Skill ID '{id}' not found in SkillInfo!");
        return null;
    }
}

[System.Serializable]
public class SkillData
{
    public string skillId;
    public string skillName;
    public string skillType;           // passive / active
    public string description;         // 스킬 설명
    public string effectType;          // ex: atk up, damage
    public string costType;            // ex: mp
    public int cost;                   // 소모량
    public int cooldown;
    public string effectPrecondition;  // 무기 타입 ID (ex: sword)
    public string effectStat;          // 연동 스탯
    public float effectMultiplier;     // 배율 ex: 5% → 0.05f
    public string rank;                // C/B/A/S
    public int maxLevel;               // 최대 레벨
    public string evolvedSkillId;      // 진화 결과 스킬 ID
}
