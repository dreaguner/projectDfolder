using System.Collections.Generic;
using System.Linq;

public static class SkillSelector
{
    public static List<SkillData> GetTop4ActiveSkills(NPCData npc, SkillInfo skillInfo)
    {
        var skillList = new List<SkillData>();

        foreach (var id in npc.skillIds)
        {
            var skill = skillInfo.GetSkillById(id);
            if (skill != null && skill.skillType == "active")
                skillList.Add(skill);
        }

        return skillList
            .OrderByDescending(s => GetRankValue(s.rank))
            .ThenByDescending(s => s.maxLevel)
            .Take(4)
            .ToList();
    }

    private static int GetRankValue(string rank)
    {
        return rank switch
        {
            "S" => 7,
            "A" => 6,
            "B" => 5,
            "C" => 4,
            "D" => 3,
            "E" => 2,
            "F" => 1,
            _ => 0
        };
    }
}
