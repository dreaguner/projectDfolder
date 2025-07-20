using UnityEngine;

public static class MobPartManager
{
    public static void ApplyDamage(MobPartData part, CombatUnit combatUnit, int damage)
    {
        part.currentHP -= damage;
        Debug.Log($"[ApplyDamage] {part.part} HP: {part.currentHP}/{part.parthp}");

        if (part.IsDestroyed)
        {
            Debug.Log($"[ÆÄÃ÷ ÆÄ±«!] {part.part} ÆÄ±«µÊ!");
            ProcessDestroyEffects(part, combatUnit);
        }
    }

    private static void ProcessDestroyEffects(MobPartData part, CombatUnit combatUnit)
    {
        foreach (var effect in part.effects)
        {
            Debug.Log($"[ÆÄÃ÷ È¿°ú] {effect.type} : {effect.formula}");

            if (effect.type == "death")
            {
                if (effect.formula == "instant")
                {
                    Debug.Log($"[Áï½Ã »ç¸Á] {combatUnit.unitName} Áï½Ã »ç¸Á!");
                    combatUnit.OnDeath();
                }
                else if (effect.formula.StartsWith("delayed"))
                {
                    if (int.TryParse(effect.formula.Replace("delayed", ""), out int delay))
                    {
                        Debug.Log($"[Áö¿¬ »ç¸Á] {delay}ÅÏ ÈÄ »ç¸Á ¿¹Á¤!");
                        combatUnit.pendingDeaths.Add(new PendingDeath(part.part, delay));
                    }
                }
                else if (effect.formula == "none")
                {
                    Debug.Log($"[¹«È¿ »ç¸Á] ÀÌ ÆÄÃ÷´Â ÆÄ±«µÅµµ ¸÷Àº ¾È Á×À½");
                }
            }
        }
    }
}
