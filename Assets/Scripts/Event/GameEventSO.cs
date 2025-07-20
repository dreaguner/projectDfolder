using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Events/Game Event")]
public class GameEventSO : ScriptableObject
{
    [Header("이벤트 이름")]
    public string eventName;

    [Header("발생 확률 (0~1)")]
    [Range(0f, 1f)]
    public float probability;

    [Header("실행 액션")]
    public UnityEvent eventAction;

    [Header("조건: 성향/저주/축복")]
    public float minMorality;
    public float minOrder;
    public int minCurse;
    public int minBlessing;

    [Header("추가 조건")]
    public string requiredSkillId; // ex: 플레이어가 배운 스킬
    public string requiredTraitCode; // ex: 플레이어가 가진 특성 코드
    public int requiredFloor; // ex: 층 제한

    public bool Condition(PlayerStats player)
    {
        bool match = true;

        if(player.morality < minMorality) match = false;
        if (player.order < minOrder) match = false;
        if (player.curse < minCurse) match = false;
        if (player.blessing < minBlessing) match = false;

        if (!string.IsNullOrEmpty(requiredSkillId) && !player.learnedSkillIds.Contains(requiredSkillId))
            match = false;

        if (!string.IsNullOrEmpty(requiredTraitCode) && !player.traits.Any(t => t.code == requiredTraitCode))
            match = false;

        // 층수 제한 체크 예시
        int currentFloor = GameManager.Instance.currentFloor; // 층 관리 싱글톤 예시
        if (requiredFloor > 0 && currentFloor != requiredFloor)
            match = false;

        return match;
    }
}

