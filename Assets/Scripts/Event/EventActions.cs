using UnityEngine;

public class EventActions : MonoBehaviour
{
    public void PlayDialogue(string dialogueId)
    {
        Debug.Log($"[���̾�α� ����]: {dialogueId}");
        // ����: DialogueManager.Instance.Show(dialogueId);
    }

    public void HealPlayer(int amount)
    {
        Debug.Log($"[HP ȸ��]: +{amount}");
        // ����: PlayerStatus.Instance.AddHp(amount);
    }
}
