using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatUnit
{
    public string unitName;
    public bool isPlayer;

    public MobPartAnchorManager mobPartAnchorManager;

    public int spd;
    public float cappedSpd;
    public float turnGauge = 0f;
    public const float TURN_GAUGE_MAX = 100f;

    public int finalAttack;
    public int finalMagicAttack;
    public int finalPhysicalDefense;
    public int finalMagicDefense;


    public int accuracy;
    public int evasion;
    public int criticalChance;

    public List<Trait> traits = new();

    public PlayerStats playerStats;
    public MobData mobData;

    public List<MobPartData> mobParts = new();
    public List<PendingDeath> pendingDeaths = new();

    public List<MobSkillData> mobSkills = new();

    // ✅ 버프 & 쿨타임 시스템
    public List<BuffInstance> activeBuffs = new();
    public Dictionary<string, int> skillCooldowns = new();

    public CombatUnit(PlayerStats stats)
    {
        isPlayer = true;
        unitName = stats.playerName;
        playerStats = stats;

        spd = stats.dex;
        finalAttack = stats.finalAttack;
        finalPhysicalDefense = stats.finalPhysicalDefense;
        traits = stats.traits;

        accuracy = stats.accuracy;
        evasion = stats.evasion;
        criticalChance = stats.criticalChance;

        cappedSpd = spd;
    }

    public CombatUnit(MobData mob, MobPartAnchorManager anchorManager)
    {
        isPlayer = false;
        unitName = mob.mobName;
        mobData = mob;

        spd = mob.spd;
        finalAttack = mob.atk;
        finalMagicAttack = mob.matk;

        finalPhysicalDefense = 0;
        finalMagicDefense = 0;


        if (mob.mobSkills != null)
            mobSkills = mob.mobSkills;


        mobPartAnchorManager = anchorManager;

        foreach (var part in mob.mobParts)
        {
            MobPartData runtimePart = ScriptableObject.CreateInstance<MobPartData>();
            runtimePart.mobid = part.mobid;
            runtimePart.part = part.part;
            runtimePart.mtm = part.mtm;
            runtimePart.mtid = part.mtid;
            runtimePart.add = part.add;
            runtimePart.apd = part.apd;
            runtimePart.parthp = part.parthp;
            runtimePart.parteva = part.parteva;
            runtimePart.deathCondition = part.deathCondition;
            runtimePart.deathDelayTurns = part.deathDelayTurns;
            runtimePart.traits = new List<MobTraitData>(part.traits);
            runtimePart.effects = new List<MobPartEffect>(
                part.effects.Select(e => new MobPartEffect
                {
                    type = e.type,
                    formula = e.formula
                })
            );

            runtimePart.currentHP = part.parthp;

            mobParts.Add(runtimePart);
        }

        cappedSpd = spd;
    }

    public void AddTurnGauge(float deltaTime)
    {
        turnGauge += cappedSpd * deltaTime;
        if (turnGauge > TURN_GAUGE_MAX) turnGauge = TURN_GAUGE_MAX;
    }

    public bool CanAct() => turnGauge >= TURN_GAUGE_MAX;

    public void SpendTurn() => turnGauge = 0f;

    public void CheckPendingDeaths()
    {
        for (int i = pendingDeaths.Count - 1; i >= 0; i--)
        {
            pendingDeaths[i].remainingTurns--;

            if (pendingDeaths[i].remainingTurns <= 0)
            {
                Debug.Log($"[지연 사망 발동!] {unitName} {pendingDeaths[i].partName} 파괴로 몹 사망!");
                OnDeath();
                pendingDeaths.RemoveAt(i);
            }
        }
    }

    public void OnDeath()
    {
        Debug.Log($"⚔️ {unitName} 사망 처리!");

        if (!isPlayer)
        {
            TurnManager.Instance.RemoveUnit(this);
        }

        // TODO: 사망 연출, 드롭 등
    }

    // ✅ 버프 적용
    public void ApplyBuff(MobSkillEffectType statType, float value, int turns)
    {
        activeBuffs.Add(new BuffInstance(statType, value, turns));
        Debug.Log($"{unitName} → {statType} +{value}% 버프 ({turns}턴)");
        RecalculateBuffedStats();
    }

    public void TickBuffs()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].remainingTurns--;
            if (activeBuffs[i].remainingTurns <= 0)
            {
                Debug.Log($"{unitName} → {activeBuffs[i].statType} 버프 만료");
                activeBuffs.RemoveAt(i);
            }
        }

        RecalculateBuffedStats();
    }

    public void RecalculateBuffedStats()
    {
        finalAttack = isPlayer ? playerStats.finalAttack : mobData.atk;
        cappedSpd = spd;

        foreach (var buff in activeBuffs)
        {
            switch (buff.statType)
            {
                case MobSkillEffectType.BuffAttack:
                    finalAttack += Mathf.RoundToInt(finalAttack * (buff.value / 100f));
                    break;
                case MobSkillEffectType.BuffSpeed:
                    cappedSpd += cappedSpd * (buff.value / 100f);
                    break;
            }
        }

        Debug.Log($"{unitName} → 버프 적용 후 공격력 {finalAttack}, 스피드 {cappedSpd}");
    }

    // ✅ 스킬 쿨타임 등록
    public void RegisterSkillCooldown(SkillData skill)
    {
        int cooldownTurns = skill.cooldown;

        if (!isPlayer)
        {
            int mp = skill.cost;
            if (mp <= 10) cooldownTurns = 0;
            else if (mp <= 50) cooldownTurns = 1;
            else cooldownTurns = 2;
        }

        if (cooldownTurns > 0)
        {
            skillCooldowns[skill.skillId] = cooldownTurns;
            Debug.Log($"{unitName} → {skill.skillName} 쿨타임 {cooldownTurns}턴 등록");
        }
    }

    public void TickSkillCooldowns()
    {
        var keys = new List<string>(skillCooldowns.Keys);

        foreach (var key in keys)
        {
            skillCooldowns[key]--;
            if (skillCooldowns[key] <= 0)
            {
                skillCooldowns.Remove(key);
                Debug.Log($"{unitName} → 스킬 {key} 쿨타임 종료!");
            }
        }
    }

    public bool IsSkillOnCooldown(string skillId)
    {
        return skillCooldowns.ContainsKey(skillId);
    }
}

public class PendingDeath
{
    public string partName;
    public int remainingTurns;

    public PendingDeath(string name, int turns)
    {
        partName = name;
        remainingTurns = turns;
    }
}
