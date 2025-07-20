using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemPanel : MonoBehaviour
{
    public static ItemPanel Instance; // ✅ 반드시 있어야 함!

    private CombatUnit currentPlayer;

    [Header("ItemButton Prefab")]
    public GameObject itemButtonPrefab;   // ✅ 연결: ItemButton.prefab

    [Header("버튼 부모 Container")]
    public Transform buttonContainer;     // ✅ 연결: Vertical Layout Group 붙은 부모

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Open(CombatUnit player)
    {
        currentPlayer = player;
        gameObject.SetActive(true);
        Debug.Log("아이템 패널 열기!");

        CreateItemButtons();
    }

    /// <summary>
    /// 소비 아이템 버튼 동적 생성
    /// </summary>
    private void CreateItemButtons()
    {
        // 1) 기존 버튼 제거
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // 2) InventoryManager 슬롯에서 소비 아이템만 필터
        List<ItemData> consumables = new List<ItemData>();

        foreach (var slot in InventoryManager.Instance.slots)
        {
            if (slot.item is ConsumableData)
            {
                consumables.Add(slot.item);
            }
        }

        // 3) 버튼 생성
        foreach (var item in consumables)
        {
            GameObject btnObj = Instantiate(itemButtonPrefab, buttonContainer);

            btnObj.GetComponentInChildren<Text>().text = item.itemName;

            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnItemSelect(item);
            });
        }
    }

    public void OnItemSelect(ItemData item)
    {
        Debug.Log($"선택된 아이템: {item.itemName}");
        ActionResolver.UseItem(currentPlayer, item);
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
