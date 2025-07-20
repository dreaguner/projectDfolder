using System.Collections.Generic;

[System.Serializable]
public class TurnInfo
{
    public float baseAgi;            // �⺻ ��ø
    public List<float> agiBuffs;     // ex: [0.2, 0.1] �� +20%, +10%
    public float partMultiplier;     // ���� ���� ���� ex: �ٸ� �ı��� 0.5
    public float turnSpeed;          // ���� ���� �� �ӵ�
    public float turnGauge;          // ���� ������ 0 ~ 100

    public void CalculateTurnSpeed()
    {
        float buffSum = 1f; // �⺻ ����
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
            // �� ���� ó�� �� ������ �ʱ�ȭ
            turnGauge = 0f;
        }
    }
}