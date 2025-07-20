using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    [Header("무기 정보")]
    public Image weaponIconImage;
    public Text weaponNameText;

    [Header("스탯")]
    public Text strText;
    public Text agiText;
    public Text magicText;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        RefreshUI();
    }

    [ContextMenu("RefreshUI")]
    public void RefreshUI()
    {
        // ✅ 무기 정보는 EquipmentManager에서 가져오기
        WeaponData weapon = EquipmentManager.Instance.equippedWeapon;

        if (weapon != null)
        {
            weaponIconImage.sprite = weapon.icon;  // ✅ WeaponData에 icon 필드 있어야 함!
            weaponNameText.text = weapon.itemName;
        }
        else
        {
            weaponIconImage.sprite = null;
            weaponNameText.text = "없음";
        }

        // ✅ 플레이어 스탯은 PlayerStats에서 가져오기
        strText.text = playerStats.str.ToString();
        agiText.text = playerStats.dex.ToString();
        magicText.text = playerStats.intelligence.ToString();
    }
}
