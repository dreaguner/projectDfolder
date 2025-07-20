using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadout", menuName = "GameData/PlayerLoadout")]
public class PlayerLoadout : ScriptableObject
{
    [Header("���� ����")]
    public WeaponData[] weapons;

    [Header("���� ��")]
    public ArmorData[] armors;

    [Header("���� �Ǽ��縮")]
    public AccessoryData[] accessories;

    [Header("��ų")]
    public SkillInfo[] skillsInfo;
}
