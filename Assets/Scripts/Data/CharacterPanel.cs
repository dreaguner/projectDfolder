using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    [Header("참조")]
    private PlayerStats playerStats;  // ✅ 이름 통일!

    void OnEnable()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }

        RefreshUI();
    }

    void RefreshUI()
    {
        Debug.Log($"플레이어 이름: {playerStats.playerName}");
        Debug.Log($"공격력: {playerStats.finalAttack}");
        // 원하는 UI 데이터 바인딩 여기에 추가
    }
}
