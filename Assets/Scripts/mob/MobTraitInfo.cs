using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MobTraitData", menuName = "GameData/MobTraitData")]
public class MobTraitData : ScriptableObject
{
    public int mtid;
    public string mtn;
    public string mttag;
    public float mttagma;
    public string mttagtype;
    public int mttagpr;
}
