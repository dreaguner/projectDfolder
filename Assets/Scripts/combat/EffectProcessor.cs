using UnityEngine;

public static class EffectProcessor
{
    public static void ApplyEffect(MobPartEffect effect, MobData mob)
    {
        switch (effect.type)
        {
            case "agi":
                if (float.TryParse(effect.formula, out float agiMult))
                {
                    mob.turnInfo.partMultiplier = agiMult;
                    Debug.Log($"[Effect] 민첩 배율 변경: x{agiMult}");
                }
                break;

            case "death":
                if (effect.formula == "instant")
                {
                    Debug.Log($"[Effect] 몹 {mob.mobName} 즉사!");
                    // 여기에 사망 처리 로직 추가
                }
                break;

            default:
                Debug.Log($"[Effect] 미지원 효과 타입: {effect.type}");
                break;
        }
    }
}
