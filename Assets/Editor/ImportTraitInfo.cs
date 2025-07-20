using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net;
using System.Collections.Generic;

public class ImportTraitInfo
{
    [MenuItem("Tools/Import/Trait Info")]
    public static void Import()
    {
        string csvFileName = "traitinfo.csv";
        string csvLocalPath = Application.dataPath + "/Resources/" + csvFileName;

        string sheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRQCD-4ZQZXP7UrcNK76YWeOpIx2hpvnK5-z7_XVnmZ8Mn62Ayt0M-0DnOdNfCTyxXYuCuQVS13Sg83/pub?gid=1229126762&single=true&output=csv";

        using (var client = new WebClient())
        {
            client.DownloadFile(sheetUrl, csvLocalPath);
            Debug.Log($"✅ CSV 다운로드 완료: {csvLocalPath}");
        }

        string traitFolder = "Assets/Resources/Traits";
        if (!Directory.Exists(traitFolder)) Directory.CreateDirectory(traitFolder);
        AssetDatabase.Refresh();

        string[] lines = File.ReadAllLines(csvLocalPath);

        Dictionary<string, Trait> traitDict = new Dictionary<string, Trait>();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] cols = lines[i].Split(',');

            string code = cols[0];
            Trait trait;

            if (!traitDict.ContainsKey(code))
            {
                trait = ScriptableObject.CreateInstance<Trait>();
                trait.code = code;
                int.TryParse(cols[1], out trait.tp);
                trait.traitName = cols[2];
                trait.description = cols[5];
                float.TryParse(cols[6], out trait.morality);
                float.TryParse(cols[7], out trait.order);

                traitDict.Add(code, trait);
            }
            else
            {
                trait = traitDict[code];
            }

            TraitEffect effect = new TraitEffect
            {
                effectName = cols[3],
                value = int.TryParse(cols[4], out var val) ? val : 0,
            };

            trait.effects.Add(effect);
        }

        foreach (var pair in traitDict)
        {
            string assetPath = $"{traitFolder}/{pair.Value.code}_{pair.Value.traitName}.asset";
            AssetDatabase.CreateAsset(pair.Value, assetPath);
            Debug.Log($"✅ Trait 생성됨: {assetPath}");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("🎉 Trait CSV 임포트 완료!");
    }
}
