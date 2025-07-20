using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;

public class ImportMobInfo
{
    [MenuItem("Tools/Import/MobInfo (Split)")]
    public static void Import()
    {
        string sheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRQCD-4ZQZXP7UrcNK76YWeOpIx2hpvnK5-z7_XVnmZ8Mn62Ayt0M-0DnOdNfCTyxXYuCuQVS13Sg83/pub?gid=495168673&single=true&output=csv";
        string csvPath = Application.dataPath + "/Resources/mobinfo.csv";

        using (var client = new WebClient())
            client.DownloadFile(sheetUrl, csvPath);

        string[] lines = File.ReadAllLines(csvPath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] cols = lines[i].Split(',');
            if (cols.Length < 7) continue;

            string mobId = cols[0].Trim();
            string name = cols[1].Trim();
            int.TryParse(cols[2], out int level);
            string type = cols[3].Trim();
            int.TryParse(cols[4], out int spd);
            int.TryParse(cols[5], out int atk);
            int.TryParse(cols[6], out int matk);

            string folder = $"Assets/Resources/Mobs/{mobId}";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string assetPath = $"{folder}/{mobId}.asset";
            if (File.Exists(assetPath)) continue;

            MobData mob = ScriptableObject.CreateInstance<MobData>();
            mob.mobId = mobId;
            mob.mobName = name;
            mob.level = level;
            mob.type = type;
            mob.spd = spd;
            mob.atk = atk;
            mob.matk = matk;

            // ✅ 몹 스킬 자동 연결 (선택 사항 — 현재 구조상 비어 있을 수도 있음)
            MobSkillData[] allSkills = Resources.LoadAll<MobSkillData>("MobSkills");
            List<MobSkillData> selected = allSkills
                .Where(s => s.skillId.Contains(mobId) && s.effects.Count > 0)
                .OrderBy(s => string.IsNullOrEmpty(s.requiredPart) ? 0 : 1)
                .Take(4)
                .ToList();

            mob.mobSkills = selected;
            mob.skillIds = selected.Select(s => s.skillId).ToArray();

            // ✅ 파츠 자동 연결 (mobId 기반으로 검색)
            string[] partPaths = Directory.GetFiles(folder, $"{mobId}_*.asset");
            List<MobPartData> parts = new();
            foreach (string path in partPaths)
            {
                MobPartData part = AssetDatabase.LoadAssetAtPath<MobPartData>(path);
                if (part != null)
                    parts.Add(part);
            }
            mob.mobParts = parts;

            AssetDatabase.CreateAsset(mob, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("✅ 몹 정보 임포트 완료 (파츠 자동 연결 포함)");
    }
}
