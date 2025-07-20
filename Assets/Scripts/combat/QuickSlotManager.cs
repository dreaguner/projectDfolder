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
        // ����: ù��° ���� ���� ��ü
        var weapon = quickSlots[0];
        if (weapon != null)
        {
            EquipmentManager.Instance.EquipWeapon(weapon);
            return true;
        }
        Debug.LogWarning("�����Կ� ���Ⱑ ����!");
        return false;
    }
}
