using System.Collections.Generic;
using UnityEngine;

public static class TraitProcessor
{
    public static float GetTraitMultiplier(string weaponTag, List<MobTraitData> traits)
    {
        foreach (MobTraitData trait in traits)
        {
            if (trait.mttag == weaponTag)
                return trait.mttagma;
        }
        return 1f;
    }

}
