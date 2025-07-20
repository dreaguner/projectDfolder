using UnityEngine;

[CreateAssetMenu(fileName = "StatEffect", menuName = "GameData/StatEffect")]
public class StatEffect : ScriptableObject
{
    public StatData[] stats;
}

[System.Serializable]
public class StatData
{
    public string statId;             // ���� ID (ex: dia)
    public string statName;           // �̸� (ex: �ݰ�)
    public string effectType;         // hp, atk ��
    public float effectValue;         // ��ġ
    public string effectPrecondition; // ��ȭ ����
}
