using UnityEngine;

[System.Serializable]
public class BuffInstance
{
    public MobSkillEffectType statType;
    public float value; // %로 받음 (예: 20f → +20%)
    public int remainingTurns;

    public BuffInstance(MobSkillEffectType type, float value, int turns)
    {
        this.statType = type;
        this.value = value;
        this.remainingTurns = turns;
    }
}
