using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "GameData/MobData")]
public class MobData : ScriptableObject
{
    [Header("기본 정보")]
    public string mobId;
    public string mobName;
    public int level;
    public string type;       // ? 추가

    [Header("기본 스탯")]
    public int spd;
    public int atk;           // ? 추가
    public int matk;          // ? 추가

    [Header("보유 스킬 ID")]
    public string[] skillIds; // ? 추가

    [Header("보유 스킬")]
    public List<MobSkillData> mobSkills;

    [Header("턴 정보")]
    public TurnInfo turnInfo;

    [Header("연결")]
    public List<MobPartData> mobParts;
}
