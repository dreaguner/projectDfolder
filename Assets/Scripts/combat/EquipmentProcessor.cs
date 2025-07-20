using UnityEngine;

public static class EquipmentProcessor
{
    public static float GetWeaponStatContribution(WeaponData weapon, int playerStat)
    {
        if (weapon == null) return 0;

        return playerStat * weapon.skillStatRatio;
    }

    public static float GetArmorStatContribution(ArmorData armor, int playerStat)
    {
        if (armor == null) return 0;

        return playerStat * armor.skillStatRatio;
    }

    public static float GetAccessoryStatContribution(AccessoryData accessory, int playerStat)
    {
        if (accessory == null) return 0;

        return playerStat * accessory.skillStatRatio;
    }
}
