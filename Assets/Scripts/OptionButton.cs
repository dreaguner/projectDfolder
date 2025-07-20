using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    [Header("�ɼ� ��ư ���� ������")]
    public Image buttonIcon; // ��ư ���� Image
    public Sprite openIcon;  // �ɼ� ���� ���� ������
    public Sprite closeIcon; // X ��ư ������

    /// <summary>
    /// ��ư Ŭ�� �� ȣ�� �� ��� ����
    /// </summary>
    public void OnClickToggle()
    {
        if (UIManager.Instance.IsAnyPanelOpen())
        {
            UIManager.Instance.CloseAllPanels();
        }
        else
        {
            UIManager.Instance.OpenOptionPanel();
        }

        UpdateButtonIcon();
    }

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    public void UpdateButtonIcon()
    {
        if (buttonIcon == null)
        {
            Debug.LogWarning("OptionButton: buttonIcon �Ҵ� �� ��! ������ ������Ʈ ����.");
            return;
        }
        if (openIcon == null || closeIcon == null)
        {
            Debug.LogWarning("OptionButton: openIcon �Ǵ� closeIcon �Ҵ� �� ��! ������ ������Ʈ ����.");
            return;
        }

        if (UIManager.Instance != null && UIManager.Instance.IsAnyPanelOpen())
        {
            buttonIcon.sprite = closeIcon;
        }
        else
        {
            buttonIcon.sprite = openIcon;
        }
    }

}
