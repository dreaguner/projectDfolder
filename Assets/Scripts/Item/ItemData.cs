using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string itemCode;      // eqid
    public string itemName;      // eqname
    [TextArea]
    public string description;   // eqdetail
    public int price;            // ������ �� �ʿ� ��

    public string baseStatName;
    public int baseStatValue;
    public string elementId;
    public string skillStatName;
    public float skillStatRatio;
}

