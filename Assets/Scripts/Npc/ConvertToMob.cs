using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NPCMobConverter
{
    public static MobData ConvertToMob(NPCData npc, SkillInfo skillInfo)
    {
        var mob = ScriptableObject.CreateInstance<MobData>();
        mob.mobName = npc.npcName;
        mob.atk = npc.finalAttack;
        mob.matk = npc.finalMagicAttack;
        mob.spd = npc.dex;

        // 몹 스킬 할당 (스킬 ID만 예시)
        List<SkillData> topSkills = SkillSelector.GetTop4ActiveSkills(npc, skillInfo);
        mob.skillIds = topSkills.Select(s => s.skillId).ToArray();

        // TODO: MobSkillData를 따로 로드해서 mob.mobSkills도 채워주면 좋음

        // 몹 파츠
        int partHP = npc.maxHP / 4;
        mob.mobParts = new List<MobPartData>
    {
        CreatePart("머리", partHP, "instant"),
        CreatePart("심장", partHP, "instant"),
        CreatePart("팔", partHP, "delayed2"),
        CreatePart("다리", partHP, "delayed2")
    };

        return mob;
    }

    private static MobPartData CreatePart(string part, int hp, string deathCondition)
    {
        var mpd = ScriptableObject.CreateInstance<MobPartData>();
        mpd.part = part;
        mpd.parthp = hp;
        mpd.currentHP = hp;
        mpd.deathCondition = deathCondition == "instant" ? "instant" : "delayed";
        mpd.deathDelayTurns = deathCondition.StartsWith("delayed") ? int.Parse(deathCondition.Replace("delayed", "")) : 0;
        mpd.effects = new List<MobPartEffect>();

        if (deathCondition == "instant")
            mpd.effects.Add(new MobPartEffect { type = "death", formula = "instant" });
        else
            mpd.effects.Add(new MobPartEffect { type = "death", formula = deathCondition });

        return mpd;
    }

}
