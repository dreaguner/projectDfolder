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

        // �� ��ų �Ҵ� (��ų ID�� ����)
        List<SkillData> topSkills = SkillSelector.GetTop4ActiveSkills(npc, skillInfo);
        mob.skillIds = topSkills.Select(s => s.skillId).ToArray();

        // TODO: MobSkillData�� ���� �ε��ؼ� mob.mobSkills�� ä���ָ� ����

        // �� ����
        int partHP = npc.maxHP / 4;
        mob.mobParts = new List<MobPartData>
    {
        CreatePart("�Ӹ�", partHP, "instant"),
        CreatePart("����", partHP, "instant"),
        CreatePart("��", partHP, "delayed2"),
        CreatePart("�ٸ�", partHP, "delayed2")
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
