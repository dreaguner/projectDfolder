using UnityEngine;

[CreateAssetMenu(fileName = "NewEventPool", menuName = "Events/Event Pool")]
public class GameEventPoolSO : ScriptableObject
{
    [Header("�̺�Ʈ ����Ʈ")]
    public GameEventSO[] events;
}

