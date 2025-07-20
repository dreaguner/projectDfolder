using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon")]
public class WeaponData : ItemData
{
    public Sprite icon;

    [Header("등급 & 태그")]
    public string eqRank;        // eqrank (등급: F 등)
    public string eqTag;         // eqtag (분류: 도검 등)
    public string eqTagId;       // eqtagid (내부 ID)
    public string eqDTagId;      // eqdtagid (상세 태그)


    [Header("속성")]
    public string elementType;   // eqeletype

}
