using UnityEngine;

public enum ArmorType { Head, Body, Legs, Arms, Feet }

[CreateAssetMenu(fileName = "ArmorData", menuName = "Game/Armor")]
public class ArmorData : ItemData
{
    [Header("방어구 타입")]
    public ArmorType armorType;  // eqtag → ArmorType 변환

    [Header("등급 & 태그")]
    public string eqRank;        // eqrank
    public string eqTagId;       // eqtagid
    public string eqDTagId;      // eqdtagid

    [Header("기본 스탯")]
    public string baseStatName;  // eqbasestat (보통 def)
    public int baseStatValue;    // eqbaseststma

    [Header("속성")]
    public string elementType;
    public string elementId;

    [Header("스케일 스탯")]
    public string skillStatName;
    public float skillStatRatio;
}
