using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net;

public class ImportSkillInfo
{
    [MenuItem("Tools/Import/SkillInfo")]
    public static void Import()
    {
        string assetPath = "Assets/Resources/SkillInfo.asset";
        string csvPath = Application.dataPath + "/Resources/skillinfo.csv";
        string sheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRQCD-4ZQZXP7UrcNK76YWeOpIx2hpvnK5-z7_XVnmZ8Mn62Ayt0M-0DnOdNfCTyxXYuCuQVS13Sg83/pub?gid=77303487&single=true&output=csv";

        try
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(sheetUrl, csvPath);
                Debug.Log($"✅ SkillInfo CSV 다운로드 완료: {csvPath}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ SkillInfo 다운로드 실패: {ex.Message}");
            return;
        }

        if (!Directory.Exists(Application.dataPath + "/Resources"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources");
            AssetDatabase.Refresh();
        }

        SkillInfo so = AssetDatabase.LoadAssetAtPath<SkillInfo>(assetPath);
        if (so == null)
        {
            so = ScriptableObject.CreateInstance<SkillInfo>();
            so.name = "SkillInfo";
            AssetDatabase.CreateAsset(so, assetPath);
            Debug.Log($"✅ SkillInfo.asset 새로 생성됨: {assetPath}");
        }

        string[] csvLines = File.ReadAllLines(csvPath, System.Text.Encoding.UTF8);
        SkillData[] data = new SkillData[csvLines.Length - 1];

        for (int i = 1; i < csvLines.Length; i++)
        {
            string[] cols = csvLines[i].Split(',');

            int.TryParse(cols[6].Trim(), out int cost);
            float.TryParse(cols[9].Trim().Replace("%", ""), out float mul);
            int.TryParse(cols[11].Trim(), out int maxLv);

            data[i - 1] = new SkillData
            {
                skillId = cols[0].Trim(),
                skillName = cols[1].Trim(),
                skillType = cols[2].Trim(),
                description = cols[3].Trim(),
                effectType = cols[4].Trim(),
                costType = cols[5].Trim(),
                cost = cost,
                effectPrecondition = cols[7].Trim(),
                effectStat = cols[8].Trim(),
                effectMultiplier = mul,
                rank = cols[10].Trim(),
                maxLevel = maxLv,
                evolvedSkillId = cols[12].Trim()
            };
        }

        so.skillList = data;

        EditorUtility.SetDirty(so);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"✅ SkillInfo 임포트 완료! {so.skillList.Length}개 로드됨");
    }
}
