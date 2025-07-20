using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobSkillData", menuName = "GameData/MobSkillData")]
public class MobSkillData : ScriptableObject
{
    [Header("기본 정보")]
    public string skillId;
    public string skillName;
    [TextArea] public string description;

    [Header("명중률 및 조건")]
    [Range(0, 100)] public int accuracy = 100;
    [Tooltip("스킬 발동에 필요한 파츠 (ex. '팔', null이면 무관)")]
    public string requiredPart;

    [Header("타겟 설정")]
    public MobSkillTargetType targetType;
    public MobSkillTargetCondition targetCondition;

    [Header("스킬 효과")]
    public List<MobSkillEffect> effects = new();
}

[System.Serializable]
public class MobSkillEffect
{
    public MobSkillEffectType effectType;

    [Tooltip("예: 150 → 공격력 150%")]
    public float value;

    [Tooltip("지속 턴 수. 0이면 즉시 적용")]
    public int duration = 0;
}

public enum MobSkillTargetType
{
    Self,
    SingleRandomEnemy,
    MultiRandomEnemies,
    LowestAttackEnemy,
    LowestDefenseEnemy
}

public enum MobSkillTargetCondition
{
    None,
    LowestAttack,
    LowestDefense
}

public enum MobSkillEffectType
{
    Damage,
    BuffAttack,
    BuffSpeed,
    DebuffDefense,
    Heal,
    Custom
}
