using UnityEngine;

[CreateAssetMenu(fileName = "MobInfo", menuName = "GameData/MobInfo")]
public class MobInfo : ScriptableObject
{
    public MobBasicData[] mobs;
}

[System.Serializable]
public class MobBasicData
{
    public string mobid;
    public string name;
    public int level;
    public string type;
    public int spd;
    public int str;
}
