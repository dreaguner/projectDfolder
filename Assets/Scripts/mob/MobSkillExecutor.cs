using System.Collections.Generic;
using UnityEngine;

public static class MobSkillExecutor
{
    public static void Execute(MobSkillData skill, CombatUnit user, List<CombatUnit> party)
    {
        // ? ���� ���� Ȯ��
        if (!string.IsNullOrEmpty(skill.requiredPart))
        {
            var part = user.mobParts.Find(p => p.part == skill.requiredPart);
            if (part == null || part.IsDestroyed)
            {
                Debug.Log($"? ��ų �ߵ� ����: ���� '{skill.requiredPart}'�� �ı���");
                return;
            }
        }

        // ? Ÿ�� ����
        var targets = SelectTargets(skill, user, party);

        foreach (var target in targets)
        {
            bool hit = MobSkillHitCheck.IsSkillHit(skill.accuracy, target.evasion);

            if (!hit)
            {
                Debug.Log($"? {skill.skillName} �� {target.unitName}���� ������!");
                continue;
            }

            // ? ȿ�� ����
            foreach (var effect in skill.effects)
            {
                ApplyEffect(effect, user, target);
            }
        }

        user.SpendTurn();
    }

    private static List<CombatUnit> SelectTargets(MobSkillData skill, CombatUnit user, List<CombatUnit> party)
    {
        var result = new List<CombatUnit>();

        switch (skill.targetType)
        {
            case MobSkillTargetType.Self:
                result.Add(user);
                break;

            case MobSkillTargetType.SingleRandomEnemy:
                result.Add(party[Random.Range(0, party.Count)]);
                break;

            case MobSkillTargetType.MultiRandomEnemies:
                int count = Mathf.Min(3, party.Count); // �ִ� 3��
                result.AddRange(Shuffle(party).GetRange(0, count));
                break;

            case MobSkillTargetType.LowestAttackEnemy:
                result.Add(GetLowestStatTarget(party, "attack"));
                break;

            case MobSkillTargetType.LowestDefenseEnemy:
                result.Add(GetLowestStatTarget(party, "defense"));
                break;
        }

        return result;
    }

    private static void ApplyEffect(MobSkillEffect effect, CombatUnit user, CombatUnit target)
    {
        switch (effect.effectType)
        {
            case MobSkillEffectType.Damage:
                int raw = user.finalAttack;
                int damage = Mathf.RoundToInt(raw * (effect.value / 100f));
                Debug.Log($"?? {target.unitName}���� {damage} ������!");
                if (target.mobParts.Count > 0)
                {
                    var part = target.mobParts[0]; // ���� ��� ���� ���� �����ϵ��� ���� ����
                    MobPartManager.ApplyDamage(part, target, damage);
                }
                break;

            case MobSkillEffectType.BuffAttack:
                Debug.Log($"??? {user.unitName} ���ݷ� {effect.value}% ���� (�ӽ�)");
                user.ApplyBuff(MobSkillEffectType.BuffAttack, effect.value, effect.duration);
                break;

            case MobSkillEffectType.BuffSpeed:
                Debug.Log($"? {user.unitName} ���ǵ� {effect.value}% ���� (�ӽ�)");
                user.ApplyBuff(MobSkillEffectType.BuffSpeed, effect.value, effect.duration);
                break;

            case MobSkillEffectType.Heal:
                Debug.Log($"?? {target.unitName} ȸ�� +{effect.value}");
                // TODO: ȸ�� ���� (���� �������� HP ����)
                break;

            default:
                Debug.Log($"? Ŀ���� ȿ�� {effect.effectType} ó�� �ʿ�");
                break;
        }
    }

    private static CombatUnit GetLowestStatTarget(List<CombatUnit> units, string stat)
    {
        int min = int.MaxValue;
        List<CombatUnit> lowest = new();

        foreach (var unit in units)
        {
            int value = stat switch
            {
                "attack" => unit.finalAttack,
                "defense" => unit.finalPhysicalDefense,
                _ => 9999
            };

            if (value < min)
            {
                lowest.Clear();
                lowest.Add(unit);
                min = value;
            }
            else if (value == min)
            {
                lowest.Add(unit);
            }
        }

        return lowest[Random.Range(0, lowest.Count)];
    }

    private static List<T> Shuffle<T>(List<T> list)
    {
        List<T> copy = new(list);
        for (int i = 0; i < copy.Count; i++)
        {
            int rand = Random.Range(i, copy.Count);
            (copy[i], copy[rand]) = (copy[rand], copy[i]);
        }
        return copy;
    }
}
