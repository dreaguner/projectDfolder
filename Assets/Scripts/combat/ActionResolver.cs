using UnityEngine;
using BattleSystem;

public static class ActionResolver
{
    public static void ResolvePlayerAction(CombatUnit player)
    {
        Debug.Log("플레이어 행동 선택: 공격 / 스킬 / 아이템 / 무기 교체 / 도망 / 관찰");
        PlayerActionPanel.Instance.Open(player);
    }

    public static void ResolveMobAction(CombatUnit mob)
    {
        Debug.Log($"{mob.unitName} 의 기본 공격!");
        // TODO: 몹 AI 기본 공격 처리
    }

    public static void ResolveBasicAttack(CombatUnit attacker, CombatUnit target, MobPartData selectedPart)
    {
        Debug.Log($"{attacker.unitName} 의 기본 공격!");

        // === 0) 명중/회피 판정 ===
        bool hitSuccess = Random.Range(0, 100) < Mathf.Clamp(attacker.accuracy - target.evasion, 5, 95);
        if (!hitSuccess)
        {
            Debug.Log($"Miss! {attacker.unitName}의 공격이 빗나감");
            attacker.SpendTurn();
            return;
        }

        // === 1) 물리 데미지 ===
        float baseDamage = attacker.finalAttack;
        float physicalDefense = target.finalPhysicalDefense;
        float damageAfterDefense = Mathf.Max(1, baseDamage - physicalDefense);

        Debug.Log($"[물리] {baseDamage} - 방어력 {physicalDefense} = {damageAfterDefense}");

        // === 2) 속성 데미지 ===
        float totalElementDamage = 0;
        foreach (var kv in attacker.playerStats.elementalAttack)
        {
            ElementType ele = kv.Key;
            int eleAtkValue = kv.Value;

            float eleMultiplier = GetElementResistanceMultiplier(target, ele);
            float eleDamage = eleAtkValue * eleMultiplier;

            Debug.Log($"[속성] {ele} {eleAtkValue} × {eleMultiplier} = {eleDamage}");
            totalElementDamage += eleDamage;
        }

        float totalDamage = damageAfterDefense + totalElementDamage;

        // === 3) 크리티컬 판정 ===
        bool isCrit = Random.Range(0, 100) < attacker.criticalChance;
        if (isCrit)
        {
            totalDamage *= 1.5f;
            Debug.Log($"[크리티컬!] 데미지 ×1.5 → {totalDamage}");
        }

        Debug.Log($"➡️ 최종 데미지: 물리 {damageAfterDefense} + 속성 {totalElementDamage} = {totalDamage}");

        // === 4) DamagePopup 출력 ===
        if (target.mobPartAnchorManager != null)
        {
            Transform anchor = target.mobPartAnchorManager.GetAnchor("Head");
            Vector3 popupPos = anchor.position + Vector3.up * 0.5f;
            bool isElemental = totalElementDamage > 0;

            DamagePopup.Show(popupPos, Mathf.RoundToInt(totalDamage), isCrit, isElemental);
        }
        else
        {
            Debug.LogWarning("❗️ target.mobPartAnchorManager 없음! DamagePopup 위치 확인 필요!");
        }

        // === 5) 파츠별 HP 차감 연결 예시 ===
        if (target.mobParts != null && target.mobParts.Count > 0)
        {
            var part = target.mobParts[0]; // TODO: 선택된 파츠로 변경
            MobPartManager.ApplyDamage(part, target, Mathf.RoundToInt(totalDamage));
        }
        else
        {
            Debug.LogWarning("❗️ 몹 파츠 없음! 데미지 미적용");
        }

        attacker.SpendTurn();
    }

    private static float GetElementResistanceMultiplier(CombatUnit target, ElementType ele)
    {
        float multiplier = 1f;

        if (target.mobParts != null)
        {
            foreach (var part in target.mobParts)
            {
                if (part.traits == null) continue;

                foreach (var trait in part.traits)
                {
                    if (trait == null) continue;

                    if (trait.mttag == ele.ToString() && trait.mttagtype == "배율")
                    {
                        multiplier *= trait.mttagma;
                        Debug.Log($"[특성] {trait.mtn} {ele} 배율 적용: {trait.mttagma}");
                    }
                }
            }
        }

        return multiplier;
    }


    public static void UseSkill(CombatUnit attacker, SkillData skill, CombatUnit target)
    {
        if (attacker.IsSkillOnCooldown(skill.skillId))
        {
            Debug.Log($"❌ {skill.skillName} 은 현재 쿨타임 중입니다!");
            return;
        }

        Debug.Log($"스킬 사용: {skill.skillName}");

        // === 명중 판정 (스킬은 명중률 따로 설정할 수도 있음) ===
        bool hitSuccess = Random.Range(0, 100) < Mathf.Clamp(attacker.accuracy - target.evasion, 5, 95);
        if (!hitSuccess)
        {
            Debug.Log($"Miss! 스킬이 빗나감");
            attacker.SpendTurn();
            attacker.RegisterSkillCooldown(skill);
            return;
        }

        if (skill.costType == "mp")
        {
            Debug.Log($"MP {skill.cost} 차감");
            attacker.playerStats.SpendMP(skill.cost);
        }

        if (!string.IsNullOrEmpty(skill.effectPrecondition))
        {
            Debug.Log($"무기 타입 검사 필요: {skill.effectPrecondition}");
            // TODO: 무기 타입 검사
        }

        float baseDamage = attacker.finalAttack * skill.effectMultiplier;
        float physicalDefense = target.finalPhysicalDefense;
        float damageAfterDefense = Mathf.Max(1, baseDamage - physicalDefense);

        float totalElementDamage = 0;
        foreach (var kv in attacker.playerStats.elementalAttack)
        {
            ElementType ele = kv.Key;
            int eleAtkValue = kv.Value;

            float eleMultiplier = GetElementResistanceMultiplier(target, ele);
            totalElementDamage += eleAtkValue * eleMultiplier;
        }

        float totalDamage = damageAfterDefense + totalElementDamage;

        bool isCrit = Random.Range(0, 100) < attacker.criticalChance;
        if (isCrit)
        {
            totalDamage *= 1.5f;
        }

        Debug.Log($"➡️ 스킬 최종 데미지: {totalDamage} (크리티컬: {isCrit})");

        if (target.mobPartAnchorManager != null)
        {
            Transform anchor = target.mobPartAnchorManager.GetAnchor("Head");
            Vector3 popupPos = anchor.position + Vector3.up * 0.5f;
            bool isElemental = totalElementDamage > 0;

            DamagePopup.Show(popupPos, Mathf.RoundToInt(totalDamage), isCrit, isElemental);
        }

        if (target.mobParts != null && target.mobParts.Count > 0)
        {
            var part = target.mobParts[0]; // TODO: 선택된 파츠로 변경
            MobPartManager.ApplyDamage(part, target, Mathf.RoundToInt(totalDamage));
        }

        attacker.SpendTurn();
        attacker.RegisterSkillCooldown(skill);
    }

    public static void UseItem(CombatUnit player, ItemData item)
    {
        Debug.Log($"소비 아이템 사용: {item.itemName}");
        InventoryManager.Instance.RemoveItem(item);
        player.SpendTurn();
    }

    public static void TryEscape(CombatUnit player, CombatUnit mob)
    {
        float chance = 50f + (player.spd - mob.spd) * 2f;
        bool success = Random.Range(0f, 100f) < chance;

        Debug.Log(success ? "도망 성공!" : "도망 실패!");
        player.SpendTurn();
    }

    public static void ChangeWeapon(CombatUnit player)
    {
        if (QuickSlotManager.Instance.TryChangeWeapon())
        {
            Debug.Log("무기 교체 성공!");
            player.SpendTurn();
        }
    }
}
