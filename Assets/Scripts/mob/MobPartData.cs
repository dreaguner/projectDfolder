using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobPartData", menuName = "GameData/MobPartData")]
public class MobPartData : ScriptableObject
{
    public string mobid;
    public string part;
    public string mtm;
    public int mtid;
    public int add;
    public int apd;
    public int parthp;
    public int currentHP;
    public bool IsDestroyed => currentHP <= 0;
    public int parteva;
    public string deathCondition;
    public int deathDelayTurns;
    public List<MobPartEffect> effects;
    public List<MobTraitData> traits;
}

[System.Serializable]
public class MobPartEffect
{
    public string type;
    public string formula;
}
