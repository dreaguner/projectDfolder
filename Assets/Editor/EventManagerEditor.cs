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
            EditorGUILayout.HelpBox("Play ��忡���� �׽�Ʈ ��ư�� Ȱ��ȭ�˴ϴ�.", MessageType.Info);
        }
    }
}
