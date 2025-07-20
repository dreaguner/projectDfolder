using System.IO;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;

public class ImportStatEffect
{
    [MenuItem("Tools/Import/StatEffect")]
    public static void Import()
    {
        string assetPath = "Assets/Resources/StatEffect.asset";
        string csvPath = Application.dataPath + "/Resources/stateffect.csv";
        string sheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRQCD-4ZQZXP7UrcNK76YWeOpIx2hpvnK5-z7_XVnmZ8Mn62Ayt0M-0DnOdNfCTyxXYuCuQVS13Sg83/pub?gid=937529787&single=true&output=csv";

        try
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(sheetUrl, csvPath);
                Debug.Log($"✅ StatEffect CSV 다운로드 완료: {csvPath}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ StatEffect 다운로드 실패: {ex.Message}");
            return;
        }

        if (!Directory.Exists(Application.dataPath + "/Resources"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources");
            AssetDatabase.Refresh();
        }

        StatEffect so = AssetDatabase.LoadAssetAtPath<StatEffect>(assetPath);
        if (so == null)
        {
            so = ScriptableObject.CreateInstance<StatEffect>();
            so.name = "StatEffect";
            AssetDatabase.CreateAsset(so, assetPath);
            Debug.Log($"✅ StatEffect.asset 새로 생성됨: {assetPath}");
        }

        string[] csvLines = File.ReadAllLines(csvPath, System.Text.Encoding.UTF8);
        StatData[] data = new StatData[csvLines.Length - 1];

        for (int i = 1; i < csvLines.Length; i++)
        {
            string[] cols = csvLines[i].Split(',');

            float.TryParse(cols[3].Trim(), out float val);

            data[i - 1] = new StatData
            {
                statId = cols[0].Trim(),
                statName = cols[1].Trim(),
                effectType = cols[2].Trim(),
                effectValue = val,
                effectPrecondition = cols[4].Trim()
            };
        }

        so.stats = data;

        EditorUtility.SetDirty(so);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"✅ StatEffect 임포트 완료! {so.stats.Length}개 로드됨");
    }
}
