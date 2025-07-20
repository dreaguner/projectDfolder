using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemCode;      // eqid
    public string itemName;      // eqname
    [TextArea]
    public string description;   // eqdetail
    public int price;            // 상점가 등 필요 시

    public string baseStatName;
    public int baseStatValue;
    public string elementId;
    public string skillStatName;
    public float skillStatRatio;
}

