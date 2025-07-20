using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net;
using System.Collections.Generic;

public class ImportMobTraitInfo
{
    [MenuItem("Tools/Import/MobTraitInfo (1 effect per trait)")]
    public static void Import()
    {
        string sheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRQCD-4ZQZXP7UrcNK76YWeOpIx2hpvnK5-z7_XVnmZ8Mn62Ayt0M-0DnOdNfCTyxXYuCuQVS13Sg83/pub?gid=1179506214&single=true&output=csv";
        string csvPath = Application.dataPath + "/Resources/mobtraitinfo.csv";

        using (var client = new WebClient())
            client.DownloadFile(sheetUrl, csvPath);

        string[] lines = File.ReadAllLines(csvPath);
        string folder = "Assets/Resources/MobTraits";
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        Dictionary<int, MobTraitData> traitMap = new();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] cols = lines[i].Split(',');
            if (cols.Length < 6) continue;

            int.TryParse(cols[0], out int mtid);
            string mtn = cols[1].Trim(); // 파일명에도 사용됨
            string mttag = cols[2].Trim();
            float.TryParse(cols[3], out float mttagma);
            string mttagtype = cols[4].Trim();
            int.TryParse(cols[5], out int mttagpr);

            string assetPath = $"{folder}/{mtn}.asset";

            // 항상 재생성
            var trait = ScriptableObject.CreateInstance<MobTraitData>();
            trait.mtid = mtid;
            trait.mtn = mtn;
            trait.mttag = mttag;
            trait.mttagma = mttagma;
            trait.mttagtype = mttagtype;
            trait.mttagpr = mttagpr;

            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.CreateAsset(trait, assetPath);

            traitMap[mtid] = trait;
        }

        // ✅ 몹 파트에 자동 연결
        string[] partGuids = AssetDatabase.FindAssets("t:MobPartData", new[] { "Assets/Resources/Mobs" });
        int connected = 0;

        foreach (var guid in partGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var part = AssetDatabase.LoadAssetAtPath<MobPartData>(path);
            if (part == null) continue;

            part.traits = new List<MobTraitData>();

            if (traitMap.TryGetValue(part.mtid, out var trait))
            {
                part.traits.Add(trait);
                EditorUtility.SetDirty(part);
                connected++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"✅ 몹 특성 임포트 완료 (1 효과 per 특성, {connected}개 파트에 연결됨)");
    }
}
