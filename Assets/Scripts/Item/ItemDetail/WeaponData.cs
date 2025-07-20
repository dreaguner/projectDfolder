using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon")]
public class WeaponData : ItemData
{
    public Sprite icon;

    [Header("��� & �±�")]
    public string eqRank;        // eqrank (���: F ��)
    public string eqTag;         // eqtag (�з�: ���� ��)
    public string eqTagId;       // eqtagid (���� ID)
    public string eqDTagId;      // eqdtagid (�� �±�)


    [Header("�Ӽ�")]
    public string elementType;   // eqeletype

}
