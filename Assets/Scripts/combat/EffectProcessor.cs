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
                    Debug.Log($"[Effect] ��ø ���� ����: x{agiMult}");
                }
                break;

            case "death":
                if (effect.formula == "instant")
                {
                    Debug.Log($"[Effect] �� {mob.mobName} ���!");
                    // ���⿡ ��� ó�� ���� �߰�
                }
                break;

            default:
                Debug.Log($"[Effect] ������ ȿ�� Ÿ��: {effect.type}");
                break;
        }
    }
}
