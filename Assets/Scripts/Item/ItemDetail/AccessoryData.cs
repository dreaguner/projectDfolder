using UnityEngine;

public enum AccessoryType { Ring, Necklace, Bracelet }

[CreateAssetMenu(fileName = "AccessoryData", menuName = "Game/Accessory")]
public class AccessoryData : ItemData
{
    [Header("�Ǽ��縮 Ÿ��")]
    public AccessoryType accessoryType; // eqtag �� AccessoryType ��ȯ

    [Header("��� & �±�")]
    public string eqRank;        // eqrank
    public string eqTagId;       // eqtagid
    public string eqDTagId;      // eqdtagid

    [Header("�⺻ ����")]
    public string baseStatName;
    public int baseStatValue;

    [Header("�Ӽ�")]
    public string elementType;
    public string elementId;

    [Header("������ ����")]
    public string skillStatName;
    public float skillStatRatio;
}
