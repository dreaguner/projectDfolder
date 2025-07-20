using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public TurnManager turnManager;

    public CombatUnit playerUnit;
    public CombatUnit mobUnit;

    [Header("몹 데이터")]
    public MobData mobData; // Inspector에서 몹 데이터 ScriptableObject 연결

    [Header("몹 프리팹")]
    public GameObject mobPrefab; // Inspector에서 몹 프리팹 연결

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // ✅ 1) 몹 프리팹 인스턴스화
        GameObject mobGO = Instantiate(mobPrefab);

        // ✅ 2) 프리팹에서 앵커 매니저 가져오기
        MobPartAnchorManager anchorManager = mobGO.GetComponent<MobPartAnchorManager>();

        if (anchorManager == null)
        {
            Debug.LogError("몹 프리팹에 MobPartAnchorManager가 없습니다!");
            return;
        }

        // ✅ 3) CombatUnit 생성자에 넘겨서 몹 CombatUnit 만들기
        mobUnit = new CombatUnit(mobData, anchorManager);

        // ✅ 4) 턴 매니저에 몹 CombatUnit 등록
        turnManager.units.Add(mobUnit);

        Debug.Log($"BattleManager: 몹 CombatUnit 생성 완료! {mobUnit.unitName}");
    }

    private void Update()
    {
        turnManager.UpdateTurnGauges(Time.deltaTime);

        var unit = turnManager.GetNextActingUnit();
        if (unit != null)
        {
            Debug.Log($"{unit.unitName} 턴 시작!");

            // TODO: 행동 처리
            turnManager.SpendTurn(unit);

            // ✅ 몹 전멸 체크 → 전투 종료
            bool hasAliveEnemies = turnManager.units.Exists(u => !u.isPlayer);
            if (!hasAliveEnemies)
            {
                EndBattle();
            }

            // (선택) 플레이어 전멸 체크
            // bool hasAlivePlayers = turnManager.units.Exists(u => u.isPlayer);
            // if (!hasAlivePlayers)
            // {
            //     Debug.Log("💀 플레이어 전멸! GameOver 처리 필요");
            // }
        }
    }

    public void EndBattle()
    {
        Debug.Log("🎉 전투 종료!");

        // ✅ TODO: UI 전환, 드롭 보상, 씬 이동 등 후속 처리
        // 예시: SceneManager.LoadScene("FieldScene");
    }
}
