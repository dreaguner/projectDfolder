using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ImportMobPartsMinimal
{
    [MenuItem("Tools/Import/Mob Parts (Multiple Traits)")]
    public static void Import()
    {
        string sheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRQCD-4ZQZXP7UrcNK76YWeOpIx2hpvnK5-z7_XVnmZ8Mn62Ayt0M-0DnOdNfCTyxXYuCuQVS13Sg83/pub?gid=1385097197&single=true&output=csv";
        string csvPath = Application.dataPath + "/Resources/mobpartinfo.csv";

        using (var client = new WebClient())
            client.DownloadFile(sheetUrl, csvPath);

        string[] lines = File.ReadAllLines(csvPath);
        if (lines.Length <= 1)
        {
            Debug.LogWarning("⚠️ CSV 데이터 없음");
            return;
        }

        Dictionary<string, MobPartData> partMap = new();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] cols = lines[i].Split(',');
            if (cols.Length < 13)
            {
                Debug.LogWarning($"⚠️ 열 부족 스킵: {lines[i]}");
                continue;
            }

            string mobId = cols[0].Trim();
            string part = cols[1].Trim();
            string key = $"{mobId}_{part}";

            int.TryParse(cols[3], out int mtid);
            int.TryParse(cols[4], out int add);
            int.TryParse(cols[5], out int apd);
            int.TryParse(cols[6], out int parthp);
            int.TryParse(cols[9], out int parteva);
            int.TryParse(cols[12], out int deathDelay);

            string formula = cols[7].Trim();
            string effectType = cols[8].Trim();
            string deathCond = cols[11].Trim();

            // 📦 기존 파츠 or 새로 생성
            if (!partMap.TryGetValue(key, out var partData))
            {
                partData = ScriptableObject.CreateInstance<MobPartData>();
                partData.mobid = mobId;
                partData.part = part;
                partData.mtm = cols[2].Trim();
                partData.mtid = mtid;
                partData.add = add;
                partData.apd = apd;
                partData.parthp = parthp;
                partData.currentHP = parthp;
                partData.parteva = parteva;
                partData.deathCondition = deathCond;
                partData.deathDelayTurns = deathDelay;
                partData.effects = new List<MobPartEffect>();
                partData.traits = new List<MobTraitData>();
                partMap[key] = partData;
            }

            // 🧠 효과 누적
            if (!string.IsNullOrEmpty(effectType))
            {
                partData.effects.Add(new MobPartEffect
                {
                    type = effectType,
                    formula = formula
                });
            }

            // 🧠 trait 연결 (중복 방지)
            string[] traitGUIDs = AssetDatabase.FindAssets("t:MobTraitData", new[] { "Assets/Resources/MobTraits" });
            foreach (string guid in traitGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var trait = AssetDatabase.LoadAssetAtPath<MobTraitData>(path);
                if (trait != null && trait.mtid == mtid && !partData.traits.Contains(trait))
                {
                    partData.traits.Add(trait);
                    break;
                }
            }
        }

        // 💾 저장
        foreach (var kvp in partMap)
        {
            string mobId = kvp.Value.mobid;
            string relativeFolder = $"Assets/Resources/Mobs/{mobId}/Parts";
            string absoluteFolder = Path.Combine(Application.dataPath, $"Resources/Mobs/{mobId}/Parts");

            if (!Directory.Exists(absoluteFolder))
                Directory.CreateDirectory(absoluteFolder);

            string assetPath = $"{relativeFolder}/{kvp.Key}.asset";
            AssetDatabase.DeleteAsset(assetPath); // 항상 새로
            AssetDatabase.CreateAsset(kvp.Value, assetPath);
            Debug.Log($"✅ 생성됨: {assetPath} ({kvp.Value.traits.Count} traits)");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("✅ 몹 파트 임포트 완료 (복수 trait 지원)");
    }
}
