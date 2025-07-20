using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("�гε�")]
    public GameObject DimBackground;
    public GameObject OptionPanel;
    public GameObject CharacterPanel;
    public GameObject InventoryPanel;
    public GameObject EtcPopup;

    [Header("�ɼ� ��ư")]
    public OptionButton optionButton;  // OptionButton ��ũ��Ʈ ����

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // ������ ���� ��� �г� OFF
        CloseAllPanels();
    }

    /// <summary>
    /// �ɼ� �г� ����
    /// </summary>
    public void OpenOptionPanel()
    {
        DimBackground.SetActive(true);
        OptionPanel.SetActive(true);
    }

    /// <summary>
    /// ��� �г� �ݱ�
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
    /// �ϳ��� ���������� true
    /// </summary>
    public bool IsAnyPanelOpen()
    {
        return OptionPanel.activeSelf
               || CharacterPanel.activeSelf
               || InventoryPanel.activeSelf
               || EtcPopup.activeSelf;
    }

    /// <summary>
    /// �г� ���� �ٲ�� ��ư ������ ���ΰ�ħ
    /// �̰� ȣ���ϸ� OptionButton ������ ���ŵ�
    /// </summary>
    private void Update()
    {
        if (optionButton != null)
        {
            optionButton.UpdateButtonIcon();
        }
    }
}
