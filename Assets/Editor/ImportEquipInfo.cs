using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ImportEquipmentInfo
{
    [MenuItem("Tools/Import/EquipmentInfo")]
    public static void Import()
    {
        string csvFileName = "equipmentinfo.csv";
        string csvLocalPath = Application.dataPath + "/Resources/" + csvFileName;

        // ✅ Google Spreadsheet CSV URL
        string sheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRQCD-4ZQZXP7UrcNK76YWeOpIx2hpvnK5-z7_XVnmZ8Mn62Ayt0M-0DnOdNfCTyxXYuCuQVS13Sg83/pub?gid=409508977&single=true&output=csv";

        // ✅ 다운로드
        using (var client = new WebClient())
        {
            client.DownloadFile(sheetUrl, csvLocalPath);
            Debug.Log($"✅ CSV 다운로드 완료: {csvLocalPath}");
        }

        // ✅ 저장 폴더
        string weaponsFolder = "Assets/Resources/Items/Weapons";
        string armorsFolder = "Assets/Resources/Items/Armors";
        string accessoriesFolder = "Assets/Resources/Items/Accessories";

        if (!Directory.Exists(weaponsFolder)) Directory.CreateDirectory(weaponsFolder);
        if (!Directory.Exists(armorsFolder)) Directory.CreateDirectory(armorsFolder);
        if (!Directory.Exists(accessoriesFolder)) Directory.CreateDirectory(accessoriesFolder);

        AssetDatabase.Refresh();

        // ✅ CSV 읽기
        string[] csvLines = File.ReadAllLines(csvLocalPath);

        for (int i = 1; i < csvLines.Length; i++)
        {
            string line = csvLines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] cols = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            string eqid = cols[0].Trim();
            string eqtype = cols[1].Trim().ToLower();
            string eqname = cols[2].Trim();
            string eqrank = cols[3].Trim();
            string eqtag = cols[4].Trim();
            string eqtagid = cols[5].Trim();
            string eqdtagid = cols[6].Trim();
            string eqbasestat = cols[7].Trim();
            int.TryParse(cols[8].Trim(), out int eqbaseststma);
            string eqeletype = cols[9].Trim();
            string eqeled = cols[10].Trim();
            string eqskailstat = cols[11].Trim();
            float.TryParse(cols[12].Trim(), out float eqskailma);
            string eqdetail = cols[13].Trim();

            if (eqtype == "weapon")
            {
                string assetPath = $"{weaponsFolder}/{eqid}_{eqname}.asset";

                WeaponData weapon = ScriptableObject.CreateInstance<WeaponData>();
                weapon.itemCode = eqid;
                weapon.itemName = eqname;
                weapon.eqRank = eqrank;
                weapon.eqTag = eqtag;
                weapon.eqTagId = eqtagid;
                weapon.eqDTagId = eqdtagid;
                weapon.baseStatName = eqbasestat;
                weapon.baseStatValue = eqbaseststma;
                weapon.elementType = eqeletype;
                weapon.elementId = eqeled;
                weapon.skillStatName = eqskailstat;
                weapon.skillStatRatio = eqskailma;
                weapon.description = eqdetail; // ✅ 수정!

                AssetDatabase.CreateAsset(weapon, assetPath);
                Debug.Log($"✅ Weapon 생성됨: {assetPath}");
            }
            else if (eqtype == "armor")
            {
                string assetPath = $"{armorsFolder}/{eqid}_{eqname}.asset";

                ArmorData armor = ScriptableObject.CreateInstance<ArmorData>();
                armor.itemCode = eqid;
                armor.itemName = eqname;
                armor.eqRank = eqrank;

                if (System.Enum.TryParse(eqtag, true, out ArmorType armorType))
                    armor.armorType = armorType;
                else
                    Debug.LogWarning($"⚠️ ArmorType 변환 실패: {eqtag}");

                armor.eqTagId = eqtagid;
                armor.eqDTagId = eqdtagid;
                armor.baseStatName = eqbasestat;
                armor.baseStatValue = eqbaseststma;
                armor.elementType = eqeletype;
                armor.elementId = eqeled;
                armor.skillStatName = eqskailstat;
                armor.skillStatRatio = eqskailma;
                armor.description = eqdetail; // ✅ 수정!

                AssetDatabase.CreateAsset(armor, assetPath);
                Debug.Log($"✅ Armor 생성됨: {assetPath}");
            }
            else if (eqtype == "accessory")
            {
                string assetPath = $"{accessoriesFolder}/{eqid}_{eqname}.asset";

                AccessoryData accessory = ScriptableObject.CreateInstance<AccessoryData>();
                accessory.itemCode = eqid;
                accessory.itemName = eqname;
                accessory.eqRank = eqrank;

                if (System.Enum.TryParse(eqtag, true, out AccessoryType accessoryType))
                    accessory.accessoryType = accessoryType;
                else
                    Debug.LogWarning($"⚠️ AccessoryType 변환 실패: {eqtag}");

                accessory.eqTagId = eqtagid;
                accessory.eqDTagId = eqdtagid;
                accessory.baseStatName = eqbasestat;
                accessory.baseStatValue = eqbaseststma;
                accessory.elementType = eqeletype;
                accessory.elementId = eqeled;
                accessory.skillStatName = eqskailstat;
                accessory.skillStatRatio = eqskailma;
                accessory.description = eqdetail; // ✅ 수정!

                AssetDatabase.CreateAsset(accessory, assetPath);
                Debug.Log($"✅ Accessory 생성됨: {assetPath}");
            }
            else
            {
                Debug.LogWarning($"⚠️ 알 수 없는 eqtype: {eqtype}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("🎉 Equipment CSV 임포트 완료!");
    }
}
