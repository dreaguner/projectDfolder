using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEventSO))]
public class GameEventSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameEventSO gameEvent = (GameEventSO)target;

        EditorGUILayout.LabelField("�̺�Ʈ �̸�", EditorStyles.boldLabel);
        gameEvent.eventName = EditorGUILayout.TextField(gameEvent.eventName);

        EditorGUILayout.Space();
        gameEvent.probability = EditorGUILayout.Slider("�߻� Ȯ��", gameEvent.probability, 0f, 1f);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("����", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Condition() �޼��带 Ŀ�����ϰų� �ڵ忡�� ó���ϼ���.", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("���� �׼�", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("eventAction"), true);

        serializedObject.ApplyModifiedProperties();
    }
}

