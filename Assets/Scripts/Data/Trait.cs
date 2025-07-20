using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Trait", menuName = "GameData/Trait")]
public class Trait : ScriptableObject
{
    public string code;
    public int tp;
    public string traitName;
    [TextArea] public string description;

    public float morality;
    public float order;

    public List<TraitEffect> effects = new();  // ✅ 여러 스탯에 주는 영향
}
