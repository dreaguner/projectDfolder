using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("사용할 이벤트 풀")]
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
                Debug.Log($"?? [이벤트 발생]: {chosenEvent.eventName}");
                chosenEvent.eventAction.Invoke();
            }
            else
            {
                Debug.Log($"?? [이벤트 미발생]: {chosenEvent.eventName} roll={roll} > {chosenEvent.probability}");
            }
        }
        else
        {
            Debug.Log("[이벤트 없음] 현재 성향/저주/축복 조건에 맞는 이벤트 없음!");
        }
    }
}
