using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "GameData/MobData")]
public class MobData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string mobId;
    public string mobName;
    public int level;
    public string type;       // ? �߰�

    [Header("�⺻ ����")]
    public int spd;
    public int atk;           // ? �߰�
    public int matk;          // ? �߰�

    [Header("���� ��ų ID")]
    public string[] skillIds; // ? �߰�

    [Header("���� ��ų")]
    public List<MobSkillData> mobSkills;

    [Header("�� ����")]
    public TurnInfo turnInfo;

    [Header("����")]
    public List<MobPartData> mobParts;
}
