using System.Collections.Generic;
using UnityEngine;

public class MobAIController
{
    public static void TakeTurn(CombatUnit mob, List<CombatUnit> playerParty)
    {
        if (mob.mobSkills == null || mob.mobSkills.Count == 0)
        {
            Debug.LogWarning($"{mob.unitName} 은 스킬이 없습니다. 기본 공격!");
            ActionResolver.ResolveMobAction(mob);
            return;
        }

        // ✅ 사용 가능한 스킬 필터링
        List<MobSkillData> usableSkills = new();
        foreach (var skill in mob.mobSkills)
        {
            if (!mob.IsSkillOnCooldown(skill.skillId))
                usableSkills.Add(skill);
        }

        if (usableSkills.Count == 0)
        {
            Debug.Log($"⚠️ {mob.unitName} 쿨타임 중인 스킬뿐입니다. 기본 공격!");
            ActionResolver.ResolveMobAction(mob);
            return;
        }

        // ✅ 지능형 선택: 점수 기반 우선순위
        MobSkillData chosen = SelectBestSkill(mob, usableSkills, playerParty);

        Debug.Log($"🧠 {mob.unitName} 스킬 선택: {chosen.skillName}");
        MobSkillExecutor.Execute(chosen, mob, playerParty);
    }

    private static MobSkillData SelectBestSkill(CombatUnit mob, List<MobSkillData> skills, List<CombatUnit> playerParty)
    {
        MobSkillData bestSkill = null;
        int bestScore = int.MinValue;

        foreach (var skill in skills)
        {
            int score = EvaluateSkillScore(skill, mob, playerParty);
            if (score > bestScore)
            {
                bestScore = score;
                bestSkill = skill;
            }
        }

        return bestSkill ?? skills[Random.Range(0, skills.Count)];
    }

    private static int EvaluateSkillScore(MobSkillData skill, CombatUnit mob, List<CombatUnit> playerParty)
    {
        int score = 0;
        float hpRatio = 1f; // 헬 난이도에서만 고려
        if (mob.mobParts.Count > 0)
        {
            int current = 0, max = 0;
            foreach (var part in mob.mobParts)
            {
                current += Mathf.Max(0, part.currentHP);
                max += part.parthp;
            }
            hpRatio = max > 0 ? (float)current / max : 1f;
        }

        foreach (var effect in skill.effects)
        {
            switch (effect.effectType)
            {
                case MobSkillEffectType.BuffSpeed:
                    if (mob.turnGauge == 0f) score += 10; // 첫 턴
                    break;

                case MobSkillEffectType.BuffAttack:
                    score += 5;
                    break;

                case MobSkillEffectType.Heal:
                    if (hpRatio < 0.5f) score += 20;
                    break;

                case MobSkillEffectType.Damage:
                    score += Mathf.RoundToInt(effect.value); // 데미지 배율 기반
                    break;
            }
        }

        // ✅ 난이도 보정
        switch (GameManager.CurrentDifficulty)
        {
            case GameDifficulty.Normal:
                score = Mathf.RoundToInt(score * 0.75f);
                break;
            case GameDifficulty.Hard:
                score = Mathf.RoundToInt(score * 1.0f);
                break;
            case GameDifficulty.Hell:
                score = Mathf.RoundToInt(score * 1.5f + (1f - hpRatio) * 10f); // 체력 낮을수록 더 날카롭게
                break;
        }

        return score;
    }
}
