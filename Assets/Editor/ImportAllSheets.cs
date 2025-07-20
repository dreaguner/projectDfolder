using UnityEditor;

public class ImportAllSheets
{
    [MenuItem("Tools/Import/Import ALL Sheets")]
    static void ImportAll()
    {
     
        ImportMobInfo.Import();       // ✅ 본체 나중

        EditorUtility.DisplayDialog("Import 완료!", "모든 시트를 가져왔습니다!", "OK");
    }
}
