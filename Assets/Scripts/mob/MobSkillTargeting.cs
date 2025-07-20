using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class MobSkillTargeting
{
    public static CombatUnit SelectTarget(MobSkillTargetType type, List<CombatUnit> party)
    {
        switch (type)
        {
            case MobSkillTargetType.SingleRandomEnemy:
                return party[Random.Range(0, party.Count)];

            case MobSkillTargetType.LowestAttackEnemy:
                return GetLowestStatTarget(party, "attack");

            case MobSkillTargetType.LowestDefenseEnemy:
                return GetLowestStatTarget(party, "defense");

            default:
                return party[Random.Range(0, party.Count)];
        }
    }

    private static CombatUnit GetLowestStatTarget(List<CombatUnit> party, string stat)
    {
        int min = int.MaxValue;
        List<CombatUnit> targets = new();

        foreach (var unit in party)
        {
            int value = stat switch
            {
                "attack" => unit.finalAttack,
                "defense" => unit.finalPhysicalDefense,
                _ => 9999
            };

            if (value < min)
            {
                targets.Clear();
                targets.Add(unit);
                min = value;
            }
            else if (value == min)
            {
                targets.Add(unit);
            }
        }

        return targets[Random.Range(0, targets.Count)];
    }
}

public static class MobSkillHitCheck
{
    public static bool IsSkillHit(int skillAccuracy, int targetEvasion)
    {
        int chance = Mathf.Clamp(skillAccuracy - targetEvasion, 5, 100);
        return Random.Range(0, 100) < chance;
    }
}
