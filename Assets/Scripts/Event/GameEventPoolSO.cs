using UnityEngine;

[CreateAssetMenu(fileName = "NewEventPool", menuName = "Events/Event Pool")]
public class GameEventPoolSO : ScriptableObject
{
    [Header("이벤트 리스트")]
    public GameEventSO[] events;
}

