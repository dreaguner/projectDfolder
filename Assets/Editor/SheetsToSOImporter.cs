using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net;

/// <summary>
/// 구글 스프레드시트에서 JSON 다운로드 → ScriptableObject 자동 덮어쓰기
/// Tools > Download Sheets JSON & Import 메뉴에서 실행
/// </summary>
public class SheetsToSOImporter : EditorWindow
{
    [MenuItem("Tools/Download Sheets JSON & Import")]
    static void DownloadAndImport()
    {
        // 예시: 각각 구글 시트 CSV/JSON 공개 URL로 변경!
        ImportSheet<MobInfo, MobData>("mobinfo.json", "Assets/Resources/MobInfo.asset");
        ImportSheet<MobPartData, MobPartData>("mobpartinfo.json", "Assets/Resources/MobPartInfo.asset");
    }

    static void ImportSheet<TSO, TData>(string jsonFileName, string soPath)
        where TSO : ScriptableObject
    {
        // 1) Resources 폴더에 있는 JSON 파일 경로
        string jsonPath = Application.dataPath + "/Resources/" + jsonFileName;

        if (!File.Exists(jsonPath))
        {
            Debug.LogWarning($"{jsonFileName} 파일이 없습니다. 구글 시트에서 먼저 가져와야 합니다!");
            return;
        }

        // 2) JSON 파싱
        string json = File.ReadAllText(jsonPath);
        TData[] data = JsonHelper.FromJson<TData>(json);

        // 3) 기존 SO 가져오기 또는 새로 생성
        TSO so = AssetDatabase.LoadAssetAtPath<TSO>(soPath);
        if (so == null)
        {
            so = ScriptableObject.CreateInstance<TSO>();
            AssetDatabase.CreateAsset(so, soPath);
        }

        // 4) 첫 배열 필드에 데이터 덮어쓰기
        var field = so.GetType().GetFields()[0];
        field.SetValue(so, data);

        EditorUtility.SetDirty(so);
        Debug.Log($"✅ {jsonFileName} → {typeof(TSO).Name} 덮어쓰기 완료!");
    }
}
