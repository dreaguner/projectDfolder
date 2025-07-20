using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("����� �̺�Ʈ Ǯ")]
    public GameEventPoolSO eventPool;

    public PlayerStats playerStats;

    private void Awake()
    {
        Instance = this;
    }

    public void TryTriggerEvent()
    {
        var possibleEvents = new List<GameEventSO>();

        foreach (var gameEvent in eventPool.events)
        {
            if (gameEvent.Condition(playerStats))
            {
                possibleEvents.Add(gameEvent);
            }
        }

        if (possibleEvents.Count > 0)
        {
            var chosenEvent = possibleEvents[Random.Range(0, possibleEvents.Count)];

            float roll = Random.value;
            if (roll <= chosenEvent.probability)
            {
                Debug.Log($"?? [�̺�Ʈ �߻�]: {chosenEvent.eventName}");
                chosenEvent.eventAction.Invoke();
            }
            else
            {
                Debug.Log($"?? [�̺�Ʈ �̹߻�]: {chosenEvent.eventName} roll={roll} > {chosenEvent.probability}");
            }
        }
        else
        {
            Debug.Log("[�̺�Ʈ ����] ���� ����/����/�ູ ���ǿ� �´� �̺�Ʈ ����!");
        }
    }
}
