using UnityEngine;

public enum AccessoryType { Ring, Necklace, Bracelet }

[CreateAssetMenu(fileName = "AccessoryData", menuName = "Game/Accessory")]
public class AccessoryData : ItemData
{
    [Header("악세사리 타입")]
    public AccessoryType accessoryType; // eqtag → AccessoryType 변환

    [Header("등급 & 태그")]
    public string eqRank;        // eqrank
    public string eqTagId;       // eqtagid
    public string eqDTagId;      // eqdtagid

    [Header("기본 스탯")]
    public string baseStatName;
    public int baseStatValue;

    [Header("속성")]
    public string elementType;
    public string elementId;

    [Header("스케일 스탯")]
    public string skillStatName;
    public float skillStatRatio;
}
