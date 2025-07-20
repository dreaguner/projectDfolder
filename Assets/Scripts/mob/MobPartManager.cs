using UnityEngine;

public static class MobPartManager
{
    public static void ApplyDamage(MobPartData part, CombatUnit combatUnit, int damage)
    {
        part.currentHP -= damage;
        Debug.Log($"[ApplyDamage] {part.part} HP: {part.currentHP}/{part.parthp}");

        if (part.IsDestroyed)
        {
            Debug.Log($"[���� �ı�!] {part.part} �ı���!");
            ProcessDestroyEffects(part, combatUnit);
        }
    }

    private static void ProcessDestroyEffects(MobPartData part, CombatUnit combatUnit)
    {
        foreach (var effect in part.effects)
        {
            Debug.Log($"[���� ȿ��] {effect.type} : {effect.formula}");

            if (effect.type == "death")
            {
                if (effect.formula == "instant")
                {
                    Debug.Log($"[��� ���] {combatUnit.unitName} ��� ���!");
                    combatUnit.OnDeath();
                }
                else if (effect.formula.StartsWith("delayed"))
                {
                    if (int.TryParse(effect.formula.Replace("delayed", ""), out int delay))
                    {
                        Debug.Log($"[���� ���] {delay}�� �� ��� ����!");
                        combatUnit.pendingDeaths.Add(new PendingDeath(part.part, delay));
                    }
                }
                else if (effect.formula == "none")
                {
                    Debug.Log($"[��ȿ ���] �� ������ �ı��ŵ� ���� �� ����");
                }
            }
        }
    }
}
