using UnityEngine;

public enum ArmorType { Head, Body, Legs, Arms, Feet }

[CreateAssetMenu(fileName = "ArmorData", menuName = "Game/Armor")]
public class ArmorData : ItemData
{
    [Header("�� Ÿ��")]
    public ArmorType armorType;  // eqtag �� ArmorType ��ȯ

    [Header("��� & �±�")]
    public string eqRank;        // eqrank
    public string eqTagId;       // eqtagid
    public string eqDTagId;      // eqdtagid

    [Header("�⺻ ����")]
    public string baseStatName;  // eqbasestat (���� def)
    public int baseStatValue;    // eqbaseststma

    [Header("�Ӽ�")]
    public string elementType;
    public string elementId;

    [Header("������ ����")]
    public string skillStatName;
    public float skillStatRatio;
}
