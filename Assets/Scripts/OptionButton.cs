using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    [Header("옵션 버튼 상태 아이콘")]
    public Image buttonIcon; // 버튼 안의 Image
    public Sprite openIcon;  // 옵션 열기 상태 아이콘
    public Sprite closeIcon; // X 버튼 아이콘

    /// <summary>
    /// 버튼 클릭 시 호출 → 토글 동작
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
    /// 아이콘 상태 갱신
    /// </summary>
    public void UpdateButtonIcon()
    {
        if (buttonIcon == null)
        {
            Debug.LogWarning("OptionButton: buttonIcon 할당 안 됨! 아이콘 업데이트 생략.");
            return;
        }
        if (openIcon == null || closeIcon == null)
        {
            Debug.LogWarning("OptionButton: openIcon 또는 closeIcon 할당 안 됨! 아이콘 업데이트 생략.");
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
