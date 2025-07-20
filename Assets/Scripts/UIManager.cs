using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("패널들")]
    public GameObject DimBackground;
    public GameObject OptionPanel;
    public GameObject CharacterPanel;
    public GameObject InventoryPanel;
    public GameObject EtcPopup;

    [Header("옵션 버튼")]
    public OptionButton optionButton;  // OptionButton 스크립트 참조

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 시작할 때는 모든 패널 OFF
        CloseAllPanels();
    }

    /// <summary>
    /// 옵션 패널 열기
    /// </summary>
    public void OpenOptionPanel()
    {
        DimBackground.SetActive(true);
        OptionPanel.SetActive(true);
    }

    /// <summary>
    /// 모든 패널 닫기
    /// </summary>
    public void CloseAllPanels()
    {
        DimBackground.SetActive(false);
        OptionPanel.SetActive(false);
        CharacterPanel.SetActive(false);
        InventoryPanel.SetActive(false);
        EtcPopup.SetActive(false);
    }

    /// <summary>
    /// 하나라도 켜져있으면 true
    /// </summary>
    public bool IsAnyPanelOpen()
    {
        return OptionPanel.activeSelf
               || CharacterPanel.activeSelf
               || InventoryPanel.activeSelf
               || EtcPopup.activeSelf;
    }

    /// <summary>
    /// 패널 상태 바뀌면 버튼 아이콘 새로고침
    /// 이거 호출하면 OptionButton 아이콘 갱신됨
    /// </summary>
    private void Update()
    {
        if (optionButton != null)
        {
            optionButton.UpdateButtonIcon();
        }
    }
}
