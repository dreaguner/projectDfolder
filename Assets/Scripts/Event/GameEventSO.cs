using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Events/Game Event")]
public class GameEventSO : ScriptableObject
{
    [Header("�̺�Ʈ �̸�")]
    public string eventName;

    [Header("�߻� Ȯ�� (0~1)")]
    [Range(0f, 1f)]
    public float probability;

    [Header("���� �׼�")]
    public UnityEvent eventAction;

    [Header("����: ����/����/�ູ")]
    public float minMorality;
    public float minOrder;
    public int minCurse;
    public int minBlessing;

    [Header("�߰� ����")]
    public string requiredSkillId; // ex: �÷��̾ ��� ��ų
    public string requiredTraitCode; // ex: �÷��̾ ���� Ư�� �ڵ�
    public int requiredFloor; // ex: �� ����

    public bool Condition(PlayerStats player)
    {
        bool match = true;

        if(player.morality < minMorality) match = false;
        if (player.order < minOrder) match = false;
        if (player.curse < minCurse) match = false;
        if (player.blessing < minBlessing) match = false;

        if (!string.IsNullOrEmpty(requiredSkillId) && !player.learnedSkillIds.Contains(requiredSkillId))
            match = false;

        if (!string.IsNullOrEmpty(requiredTraitCode) && !player.traits.Any(t => t.code == requiredTraitCode))
            match = false;

        // ���� ���� üũ ����
        int currentFloor = GameManager.Instance.currentFloor; // �� ���� �̱��� ����
        if (requiredFloor > 0 && currentFloor != requiredFloor)
            match = false;

        return match;
    }
}

