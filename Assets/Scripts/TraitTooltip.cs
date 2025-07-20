using UnityEngine;
using UnityEngine.EventSystems;

public class TraitTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string description;

    public void Setup(string desc)
    {
        description = desc;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"ÅøÆÁ: {description}");
        // ÅøÆÁ UI°¡ µû·Î ÀÖÀ¸¸é TooltipManager.Instance.ShowTooltip(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ÅøÆÁ ´İ±â");
        // TooltipManager.Instance.HideTooltip();
    }
}
