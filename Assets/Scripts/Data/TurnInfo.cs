using System.Collections.Generic;

[System.Serializable]
public class TurnInfo
{
    public float baseAgi;            // 기본 민첩
    public List<float> agiBuffs;     // ex: [0.2, 0.1] → +20%, +10%
    public float partMultiplier;     // 부위 상태 배율 ex: 다리 파괴시 0.5
    public float turnSpeed;          // 최종 계산된 턴 속도
    public float turnGauge;          // 현재 게이지 0 ~ 100

    public void CalculateTurnSpeed()
    {
        float buffSum = 1f; // 기본 배율
        foreach (var buff in agiBuffs)
        {
            buffSum += buff; // ex: +20% = +0.2
        }

        turnSpeed = baseAgi * buffSum * partMultiplier;
    }

    public void UpdateTurnGauge(float deltaTime)
    {
        turnGauge += turnSpeed * deltaTime;
        if (turnGauge >= 100f)
        {
            // 턴 도착 처리 후 게이지 초기화
            turnGauge = 0f;
        }
    }
}