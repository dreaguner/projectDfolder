using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemEffect
{
    public string effectName;
    public int value;
    public bool isInstant;   // ��� ȿ�� ����
    public float duration;   // ���� �ð� (isInstant == false)
}

[CreateAssetMenu(fileName = "ConsumableData", menuName = "Game/Consumable")]
public class ConsumableData : ItemData
{
    [Header("�Һ������ ȿ��")]
    public List<ItemEffect> effects = new(); // ���� ȿ�� ����
}
