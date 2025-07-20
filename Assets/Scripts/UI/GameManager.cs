using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameDifficulty CurrentDifficulty = GameDifficulty.Normal;

    [Header("플레이어 진행 정보")]
    public int currentFloor = 1;

    [Header("플레이어 참조")]
    public PlayerStats playerStats;

    [Header("참조 가능한 데이터 풀")]
    public SkillInfo skillInfo;   // ScriptableObject
    public StatEffect statEffect; // ScriptableObject

    private Dictionary<string, Trait> traitDict = new();

    private string savePath => Application.persistentDataPath + "/player_save.json";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        LoadTraits();
        Debug.Log("✅ GameManager Instance 준비됨!");
    }

    // ================================
    // ✔️ Trait Resources 로드 & 캐싱
    // ================================
    private void LoadTraits()
    {
        traitDict.Clear();
        Trait[] traits = Resources.LoadAll<Trait>("Traits");
        foreach (var trait in traits)
        {
            traitDict[trait.code] = trait;
        }
        Debug.Log($"[Trait 로드] 총 {traitDict.Count}개 로드됨!");
    }

    // ================================
    // ✔️ 성향/저주/축복 변화 도우미
    // ================================
    public void Adjustmorality(int delta)
    {
        playerStats.morality += delta;
        Debug.Log($"[성향] 현재: {playerStats.morality}");
        EventManager.Instance?.TryTriggerEvent();
    }

    public void Adjustorder(int delta)
    {
        playerStats.order += delta;
        Debug.Log($"[성향] 현재: {playerStats.order}");
        EventManager.Instance?.TryTriggerEvent();
    }

    public void AdjustCurse(int delta)
    {
        playerStats.curse += delta;
        Debug.Log($"[저주] 현재: {playerStats.curse}");
        EventManager.Instance?.TryTriggerEvent();
    }

    public void AdjustBlessing(int delta)
    {
        playerStats.blessing += delta;
        Debug.Log($"[축복] 현재: {playerStats.blessing}");
        EventManager.Instance?.TryTriggerEvent();
    }

    // ================================
    // ✔️ 스킬 추가
    // ================================
    public void AddSkill(string skillId)
    {
        var skill = skillInfo.GetSkillById(skillId);
        if (skill != null && !playerStats.learnedSkillIds.Contains(skillId))
        {
            playerStats.learnedSkillIds.Add(skillId);
            Debug.Log($"[스킬 추가] {skill.skillName} 추가됨!");
        }
        else
        {
            Debug.LogWarning($"[스킬 추가 실패] {skillId} 없음 or 이미 있음");
        }
    }

    // ================================
    // ✔️ 스탯 추가
    // ================================
    public void AddStat(string statId)
    {
        var stat = statEffect.stats.FirstOrDefault(s => s.statId == statId);
        if (stat != null && !playerStats.stats.Any(s => s.name == stat.statName))
        {
            playerStats.stats.Add(new Stat { name = stat.statName, value = (int)stat.effectValue });
            Debug.Log($"[스탯 추가] {stat.statName} 추가됨!");
        }
        else
        {
            Debug.LogWarning($"[스탯 추가 실패] {statId} 없음 or 이미 있음");
        }
    }

    // ================================
    // ✔️ 특성 추가
    // ================================
    public void AddTrait(string traitCode)
    {
        if (traitDict.TryGetValue(traitCode, out Trait trait))
        {
            if (!playerStats.traits.Any(t => t.code == traitCode))
            {
                playerStats.traits.Add(trait);
                Debug.Log($"[특성 추가] {trait.traitName} 추가됨!");
            }
            else
            {
                Debug.LogWarning($"[특성 추가 실패] {traitCode} 이미 있음");
            }
        }
        else
        {
            Debug.LogWarning($"[특성 추가 실패] {traitCode} 없음 (Trait Resources 확인)");
        }
    }

    // ================================
    // ✔️ 플레이어 데이터 저장
    // ================================
    public void SavePlayerData()
    {
        var save = new PlayerSaveData();

        save.playerName = playerStats.playerName;
        save.level = playerStats.level;
        save.age = playerStats.age;

        save.currentFloor = currentFloor;

        save.gold = playerStats.gold;

        save.morality = playerStats.morality;
        save.order = playerStats.order;
        save.curse = playerStats.curse;
        save.blessing = playerStats.blessing;

        save.learnedSkillIds = playerStats.learnedSkillIds.ToList();
        save.traitCodes = playerStats.traits.Select(t => t.code).ToList();
        save.stats = playerStats.stats.Select(s => new StatDataSave { name = s.name, value = s.value }).ToList();

        // ✅ 인벤토리 저장
        save.inventory = InventoryManager.Instance.slots.Select(slot => new InventoryItemSave
        {
            itemId = slot.item.itemCode, // ✅ 반드시 itemCode로!
            quantity = slot.quantity
        }).ToList();

        // ✅ 장비 저장
        save.equipped.weaponId = EquipmentManager.Instance.equippedWeapon?.itemCode;
        save.equipped.headArmorId = EquipmentManager.Instance.headArmor?.itemCode;
        save.equipped.bodyArmorId = EquipmentManager.Instance.bodyArmor?.itemCode;
        // 필요한 슬롯 추가

        var json = JsonUtility.ToJson(save, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"✅ 저장 완료: {savePath}");
    }

    // ================================
    // ✔️ 플레이어 데이터 로드
    // ================================
    public void LoadPlayerData()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("⚠️ 저장 파일 없음! 새로 시작");
            return;
        }

        var json = File.ReadAllText(savePath);
        var save = JsonUtility.FromJson<PlayerSaveData>(json);

        playerStats.playerName = save.playerName;
        playerStats.level = save.level;
        playerStats.age = save.age;

        currentFloor = save.currentFloor;

        playerStats.gold = save.gold;
        playerStats.morality = save.morality;
        playerStats.order = save.order;
        playerStats.curse = save.curse;
        playerStats.blessing = save.blessing;

        playerStats.learnedSkillIds.Clear();
        foreach (var id in save.learnedSkillIds)
            AddSkill(id);

        playerStats.traits.Clear();
        foreach (var code in save.traitCodes)
            AddTrait(code);

        playerStats.stats.Clear();
        foreach (var statSave in save.stats)
            playerStats.stats.Add(new Stat { name = statSave.name, value = statSave.value });

        // ✅ 인벤토리 로드
        InventoryManager.Instance.slots.Clear();
        foreach (var item in save.inventory)
        {
            var itemData = ItemManager.Instance.GetItemByCode(item.itemId);
            InventoryManager.Instance.AddItem(itemData, item.quantity);
        }

        // ✅ 장비 로드
        var weapon = ItemManager.Instance.GetItemByCode(save.equipped.weaponId) as WeaponData;
        EquipmentManager.Instance.EquipWeapon(weapon);

        var head = ItemManager.Instance.GetItemByCode(save.equipped.headArmorId) as ArmorData;
        EquipmentManager.Instance.EquipArmor(head);

        var body = ItemManager.Instance.GetItemByCode(save.equipped.bodyArmorId) as ArmorData;
        EquipmentManager.Instance.EquipArmor(body);

        Debug.Log($"✅ 로드 완료: {savePath}");
    }
}
