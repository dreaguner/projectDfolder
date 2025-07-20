using System.Collections.Generic;
using UnityEngine;

public static class NPCConverter
{
    public static NPCData ConvertPlayerToNPC(PlayerStats stats)
    {
        NPCData npc = ScriptableObject.CreateInstance<NPCData>();
        npc.npcName = stats.playerName;
        npc.morality = stats.morality;
        npc.order = stats.order;

        npc.maxHP = stats.CurrentHP;
        npc.finalAttack = stats.finalAttack;
        npc.finalMagicAttack = stats.finalMagicAttack;
        npc.finalPhysicalDefense = stats.finalPhysicalDefense;
        npc.finalMagicDefense = stats.finalMagicDefense;

        npc.dex = stats.dex;
        npc.accuracy = stats.accuracy;
        npc.evasion = stats.evasion;
        npc.criticalChance = stats.criticalChance;

        npc.skillIds = new List<string>(stats.learnedSkillIds);

        npc.equippedWeapon = EquipmentManager.Instance.equippedWeapon;

        npc.equippedArmors = new ArmorData[] {
            EquipmentManager.Instance.headArmor,
            EquipmentManager.Instance.bodyArmor,
            EquipmentManager.Instance.legArmor,
            EquipmentManager.Instance.armArmor,
            EquipmentManager.Instance.feetArmor
        };

        npc.equippedAccessories = new AccessoryData[] {
            EquipmentManager.Instance.ringAccessory,
            EquipmentManager.Instance.necklaceAccessory,
            EquipmentManager.Instance.braceletAccessory
        };

        return npc;
    }
}
