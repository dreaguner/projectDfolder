using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobSkillData", menuName = "GameData/MobSkillData")]
public class MobSkillData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string skillId;
    public string skillName;
    [TextArea] public string description;

    [Header("���߷� �� ����")]
    [Range(0, 100)] public int accuracy = 100;
    [Tooltip("��ų �ߵ��� �ʿ��� ���� (ex. '��', null�̸� ����)")]
    public string requiredPart;

    [Header("Ÿ�� ����")]
    public MobSkillTargetType targetType;
    public MobSkillTargetCondition targetCondition;

    [Header("��ų ȿ��")]
    public List<MobSkillEffect> effects = new();
}

[System.Serializable]
public class MobSkillEffect
{
    public MobSkillEffectType effectType;

    [Tooltip("��: 150 �� ���ݷ� 150%")]
    public float value;

    [Tooltip("���� �� ��. 0�̸� ��� ����")]
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
