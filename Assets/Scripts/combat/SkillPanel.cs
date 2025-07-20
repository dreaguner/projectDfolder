using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    public static SkillPanel Instance;

    private CombatUnit currentPlayer;

    [Header("SkillButton Prefab")]
    public GameObject skillButtonPrefab;   // ✅ 연결: SkillButton.prefab

    [Header("버튼 부모 Container")]
    public Transform buttonContainer;      // ✅ 연결: Vertical Layout Group 붙은 빈 오브젝트


    private void Awake()
    {
        Instance = this;
    }

    public void Open(CombatUnit player)
    {
        currentPlayer = player;
        gameObject.SetActive(true);

        CreateSkillButtons();
    }

    /// <summary>
    /// 스킬 버튼을 동적으로 생성한다
    /// </summary>
    private void CreateSkillButtons()
    {
        // ✅ 1) 기존 버튼들 전부 제거
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // ✅ 2) SkillInfo에서 ID로 SkillData 가져오기
        SkillInfo skillInfo = Resources.Load<SkillInfo>("SkillInfo");
        skillInfo.Initialize();

        foreach (string id in currentPlayer.playerStats.learnedSkillIds)
        {
            SkillData skill = skillInfo.GetSkillById(id);
            if (skill == null) continue;

            // ✅ 3) 버튼 Prefab 복제 & 부모에 붙이기
            GameObject btnObj = Instantiate(skillButtonPrefab, buttonContainer);

            // ✅ 4) 버튼 텍스트 바꿔주기
            btnObj.GetComponentInChildren<Text>().text = skill.skillName;

            // ✅ 5) 버튼 OnClick 연결
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnSkillSelect(skill);
            });
        }
    }

    /// <summary>
    /// 버튼 눌렀을 때 호출
    /// </summary>
    public void OnSkillSelect(SkillData skill)
    {
        Debug.Log($"스킬 사용: {skill.skillName}");
        ActionResolver.UseSkill(currentPlayer, skill, BattleManager.Instance.mobUnit);
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
