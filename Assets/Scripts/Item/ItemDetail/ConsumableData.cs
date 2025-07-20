using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemEffect
{
    public string effectName;
    public int value;
    public bool isInstant;   // 즉시 효과 여부
    public float duration;   // 지속 시간 (isInstant == false)
}

[CreateAssetMenu(fileName = "ConsumableData", menuName = "Game/Consumable")]
public class ConsumableData : ItemData
{
    [Header("소비아이템 효과")]
    public List<ItemEffect> effects = new(); // 여러 효과 저장
}
