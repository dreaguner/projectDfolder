using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionPanel : MonoBehaviour
{
    public static PlayerActionPanel Instance;

    [Header("UI")]
    public Text mobNameText;
    public Text partNameText;
    public Text partDetailText;

    private CombatUnit currentPlayer;
    private CombatUnit targetMob;

    private bool isSelectingPart = false;
    private int selectedPartIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void Open(CombatUnit player)
    {
        currentPlayer = player;
        targetMob = BattleManager.Instance.mobUnit;

        mobNameText.text = "";
        partNameText.text = "";
        partDetailText.text = "";

        isSelectingPart = false; // 기본은 버튼 선택 상태
        gameObject.SetActive(true);

        Debug.Log($"PlayerActionPanel 열림! {currentPlayer.unitName} 턴");
    }

    private void Update()
    {
        if (!isSelectingPart || !gameObject.activeSelf || targetMob == null) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            selectedPartIndex = Mathf.Max(0, selectedPartIndex - 1);
            UpdatePartInfo();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            selectedPartIndex = Mathf.Min(targetMob.mobParts.Count - 1, selectedPartIndex + 1);
            UpdatePartInfo();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            var selectedPart = targetMob.mobParts[selectedPartIndex];
            Debug.Log($"[선택 확정] {selectedPart.part} 공격!");
            ActionResolver.ResolveBasicAttack(currentPlayer, targetMob, selectedPart);
            Close();
        }
    }

    public void OnAttackButton()
    {
        Debug.Log("기본 공격 선택! 파츠 선택 모드 진입");
        isSelectingPart = true;
        selectedPartIndex = 0;
        UpdatePartInfo();
    }

    private void UpdatePartInfo()
    {
        if (targetMob == null) return;

        var part = targetMob.mobParts[selectedPartIndex];

        mobNameText.text = $"몹: {targetMob.unitName}";
        partNameText.text = $"파츠: {part.part}";

        if (HasObservationSkillOrTrait(currentPlayer))
        {
            string effects = part.effects != null && part.effects.Count > 0
                ? string.Join(", ", part.effects.Select(e => e.type))
                : "없음";

            partDetailText.text = $"HP: {part.currentHP}/{part.parthp}\n" +
                                  $"회피율: {part.parteva}%\n" +
                                  $"특성: {effects}";
        }
        else
        {
            partDetailText.text = "🔒 상세 정보 확인 불가";
        }

        Debug.Log($"[UpdatePartInfo] {part.part} 선택됨 (인덱스 {selectedPartIndex})");
    }

    private bool HasObservationSkillOrTrait(CombatUnit player)
    {
        return player.traits.Any(t => t.code == "해부학")
            || player.playerStats.learnedSkillIds.Contains("관찰");
    }

    public void OnSkillButton()
    {
        Debug.Log("스킬 선택!");
        SkillPanel.Instance.Open(currentPlayer);
        Close();
    }

    public void OnUseItemButton()
    {
        Debug.Log("아이템 선택!");
        ItemPanel.Instance.Open(currentPlayer);
        Close();
    }

    public void OnChangeWeaponButton()
    {
        ActionResolver.ChangeWeapon(currentPlayer);
        Close();
    }

    public void OnObserveButton()
    {
        Debug.Log("관찰 선택!");
        currentPlayer.SpendTurn();
        Close();
    }

    public void OnRunAwayButton()
    {
        ActionResolver.TryEscape(currentPlayer, BattleManager.Instance.mobUnit);
        Close();
    }

    public void Close()
    {
        isSelectingPart = false;
        gameObject.SetActive(false);
    }
}
