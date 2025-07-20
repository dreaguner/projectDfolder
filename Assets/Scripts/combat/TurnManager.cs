using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public List<CombatUnit> units = new();

    private void Awake()
    {
        Instance = this;
    }

    public void InitializeCappedSpeeds()
    {
        int maxSpd = 0;
        foreach (var unit in units)
        {
            if (unit.spd > maxSpd) maxSpd = unit.spd;
        }

        foreach (var unit in units)
        {
            float ratio = Mathf.Clamp((float)unit.spd / maxSpd, 0.2f, 5f);
            unit.cappedSpd = maxSpd * ratio;
            Debug.Log($"{unit.unitName} spd {unit.spd} ¡æ capped {unit.cappedSpd}");
        }
    }

    public void UpdateTurnGauges(float deltaTime)
    {
        foreach (var unit in units)
        {
            unit.AddTurnGauge(deltaTime);
        }
    }

    public CombatUnit GetNextActingUnit()
    {
        foreach (var unit in units)
        {
            if (unit.CanAct())
                return unit;
        }
        return null;
    }

    public void SpendTurn(CombatUnit unit)
    {
        unit.SpendTurn();
        unit.TickBuffs();
        unit.TickSkillCooldowns();

        if (!unit.isPlayer)
        {
            unit.CheckPendingDeaths();
        }
    }
    public void RemoveUnit(CombatUnit unit)
    {
        if (Instance.units.Contains(unit))
        {
            Instance.units.Remove(unit);
            Debug.Log($"[TurnManager] {unit.unitName} Á¦°ÅµÊ! ³²Àº À¯´Ö ¼ö: {Instance.units.Count}");
        }
    }

}
