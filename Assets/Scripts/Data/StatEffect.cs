using UnityEngine;

[CreateAssetMenu(fileName = "StatEffect", menuName = "GameData/StatEffect")]
public class StatEffect : ScriptableObject
{
    public StatData[] stats;
}

[System.Serializable]
public class StatData
{
    public string statId;             // 고유 ID (ex: dia)
    public string statName;           // 이름 (ex: 금강)
    public string effectType;         // hp, atk 등
    public float effectValue;         // 수치
    public string effectPrecondition; // 진화 조건
}
