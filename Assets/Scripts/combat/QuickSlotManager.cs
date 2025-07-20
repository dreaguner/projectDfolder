using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    public static QuickSlotManager Instance { get; private set; }

    public WeaponData[] quickSlots;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool TryChangeWeapon()
    {
        // 예시: 첫번째 슬롯 무기 교체
        var weapon = quickSlots[0];
        if (weapon != null)
        {
            EquipmentManager.Instance.EquipWeapon(weapon);
            return true;
        }
        Debug.LogWarning("퀵슬롯에 무기가 없음!");
        return false;
    }
}
