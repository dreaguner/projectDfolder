using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventManager))]
public class EventManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EventManager manager = (EventManager)target;

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Try Trigger Event"))
            {
                manager.TryTriggerEvent();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Play 모드에서만 테스트 버튼이 활성화됩니다.", MessageType.Info);
        }
    }
}
