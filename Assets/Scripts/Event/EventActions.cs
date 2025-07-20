using UnityEngine;

public class EventActions : MonoBehaviour
{
    public void PlayDialogue(string dialogueId)
    {
        Debug.Log($"[다이얼로그 실행]: {dialogueId}");
        // 예시: DialogueManager.Instance.Show(dialogueId);
    }

    public void HealPlayer(int amount)
    {
        Debug.Log($"[HP 회복]: +{amount}");
        // 예시: PlayerStatus.Instance.AddHp(amount);
    }
}
