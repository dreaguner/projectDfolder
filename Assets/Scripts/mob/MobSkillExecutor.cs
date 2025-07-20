using System.Collections.Generic;
using UnityEngine;

public static class MobSkillExecutor
{
    public static void Execute(MobSkillData skill, CombatUnit user, List<CombatUnit> party)
    {
        // ? 파츠 조건 확인
        if (!string.IsNullOrEmpty(skill.requiredPart))
        {
            var part = user.mobParts.Find(p => p.part == skill.requiredPart);
            if (part == null || part.IsDestroyed)
            {
                Debug.Log($"? 스킬 발동 실패: 파츠 '{skill.requiredPart}'가 파괴됨");
                return;
            }
        }

        // ? 타겟 선정
        var targets = SelectTargets(skill, user, party);

        foreach (var target in targets)
        {
            bool hit = MobSkillHitCheck.IsSkillHit(skill.accuracy, target.evasion);

            if (!hit)
            {
                Debug.Log($"? {skill.skillName} → {target.unitName}에게 빗나감!");
                continue;
            }

            // ? 효과 적용
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
                int count = Mathf.Min(3, party.Count); // 최대 3명
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
                Debug.Log($"?? {target.unitName}에게 {damage} 데미지!");
                if (target.mobParts.Count > 0)
                {
                    var part = target.mobParts[0]; // 추후 대상 파츠 직접 선택하도록 개선 가능
                    MobPartManager.ApplyDamage(part, target, damage);
                }
                break;

            case MobSkillEffectType.BuffAttack:
                Debug.Log($"??? {user.unitName} 공격력 {effect.value}% 증가 (임시)");
                user.ApplyBuff(MobSkillEffectType.BuffAttack, effect.value, effect.duration);
                break;

            case MobSkillEffectType.BuffSpeed:
                Debug.Log($"? {user.unitName} 스피드 {effect.value}% 증가 (임시)");
                user.ApplyBuff(MobSkillEffectType.BuffSpeed, effect.value, effect.duration);
                break;

            case MobSkillEffectType.Heal:
                Debug.Log($"?? {target.unitName} 회복 +{effect.value}");
                // TODO: 회복 로직 (현재 구조에는 HP 없음)
                break;

            default:
                Debug.Log($"? 커스텀 효과 {effect.effectType} 처리 필요");
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
