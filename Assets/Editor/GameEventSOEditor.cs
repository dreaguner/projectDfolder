using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEventSO))]
public class GameEventSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameEventSO gameEvent = (GameEventSO)target;

        EditorGUILayout.LabelField("이벤트 이름", EditorStyles.boldLabel);
        gameEvent.eventName = EditorGUILayout.TextField(gameEvent.eventName);

        EditorGUILayout.Space();
        gameEvent.probability = EditorGUILayout.Slider("발생 확률", gameEvent.probability, 0f, 1f);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("조건", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Condition() 메서드를 커스텀하거나 코드에서 처리하세요.", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("실행 액션", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("eventAction"), true);

        serializedObject.ApplyModifiedProperties();
    }
}

