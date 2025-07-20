using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadout", menuName = "GameData/PlayerLoadout")]
public class PlayerLoadout : ScriptableObject
{
    [Header("ÀåÂø ¹«±â")]
    public WeaponData[] weapons;

    [Header("ÀåÂø ¹æ¾î±¸")]
    public ArmorData[] armors;

    [Header("ÀåÂø ¾Ç¼¼»ç¸®")]
    public AccessoryData[] accessories;

    [Header("½ºÅ³")]
    public SkillInfo[] skillsInfo;
}
