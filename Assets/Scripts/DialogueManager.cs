using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[System.Serializable]
public class DialogueLine
{
    [TextArea]
    public string text;
    public List<ChoiceData> choices;
}

[System.Serializable]
public class ChoiceData
{
    public string text;
    public string actionKey;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public TMP_Text choicesText;

    [Header("Dialogue Data")]
    public DialogueLine[] dialogues;

    [Header("Choice Settings")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private int dialogueIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private List<Choice> currentChoices = new List<Choice>();
    private int selectedChoiceIndex = 0;
    private bool isChoosing = false;

    void Start()
    {
        dialoguePanel.SetActive(true);
        if (choicesText != null)
            choicesText.gameObject.SetActive(false);

        StartDialogue();
    }

    void Update()
    {
        if (isChoosing)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedChoiceIndex = (selectedChoiceIndex - 1 + currentChoices.Count) % currentChoices.Count;
                UpdateChoicesText();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedChoiceIndex = (selectedChoiceIndex + 1) % currentChoices.Count;
                UpdateChoicesText();
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.F))
            {
                ConfirmChoice();
            }
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                CompleteTyping();
            }
            else
            {
                NextDialogue();
            }
        }
    }

    void StartDialogue()
    {
        dialogueIndex = 0;
        ShowDialogue(dialogues[dialogueIndex]);
    }

    void ShowDialogue(DialogueLine line)
    {
        string[] parts = line.text.Split(':');
        if (parts.Length >= 2)
        {
            nameText.text = parts[0].Trim();
            StartTyping(parts[1].Trim());
        }
        else
        {
            nameText.text = "";
            StartTyping(line.text);
        }

        if (line.choices != null && line.choices.Count > 0)
        {
            List<Choice> choices = new List<Choice>();
            foreach (var c in line.choices)
            {
                choices.Add(new Choice(
                    c.text,
                    () => ExecuteChoiceAction(c.actionKey)
                ));
            }
            ShowChoices(choices);
        }
    }

    void StartTyping(string sentence)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        int i = 0;
        while (i < sentence.Length)
        {
            if (sentence[i] == '<')
            {
                string tag = "";
                while (i < sentence.Length && sentence[i] != '>')
                    tag += sentence[i++];

                if (i < sentence.Length)
                    tag += sentence[i++];

                dialogueText.text += tag;
            }
            else
            {
                dialogueText.text += sentence[i++];
                yield return new WaitForSeconds(0.05f);
            }
        }

        isTyping = false;
    }

    void CompleteTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        string[] parts = dialogues[dialogueIndex].text.Split(':');
        if (parts.Length >= 2)
            dialogueText.text = parts[1].Trim();
        else
            dialogueText.text = dialogues[dialogueIndex].text;

        isTyping = false;
    }

    void NextDialogue()
    {
        if (isChoosing) return;

        dialogueIndex++;

        if (dialogueIndex < dialogues.Length)
        {
            ShowDialogue(dialogues[dialogueIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        if (choicesText != null)
            choicesText.gameObject.SetActive(false);

        Debug.Log("대화 끝!");
    }

    private void ShowChoices(List<Choice> choices)
    {
        currentChoices = choices;
        selectedChoiceIndex = 0;
        isChoosing = true;

        if (choicesText != null)
            choicesText.gameObject.SetActive(true);

        UpdateChoicesText();
    }

    private void UpdateChoicesText()
    {
        if (choicesText == null) return;

        string display = "";
        for (int i = 0; i < currentChoices.Count; i++)
        {
            string prefix = (i == selectedChoiceIndex) ? "▶ " : "   ";
            string line = prefix + $"<color=#{ColorUtility.ToHtmlStringRGB(i == selectedChoiceIndex ? highlightColor : normalColor)}>{currentChoices[i].text}</color>";
            display += line + "\n";
        }
        choicesText.text = display;
    }

    private void ConfirmChoice()
    {
        isChoosing = false;

        if (choicesText != null)
            choicesText.gameObject.SetActive(false);

        currentChoices[selectedChoiceIndex].action?.Invoke();
    }

    private void ExecuteChoiceAction(string actionKey)
    {
        Debug.Log($"선택된 액션: {actionKey}");

        switch (actionKey)
        {
            case "GoToVillage":
                GoToVillage();
                break;
            case "EnterForest":
                EnterForest();
                break;
            case "GoHome":
                GoHome();
                break;
            default:
                Debug.LogWarning("정의되지 않은 액션: " + actionKey);
                break;
        }

        NextDialogue();
    }

    private void GoToVillage() { Debug.Log("마을로 이동!"); }
    private void EnterForest() { Debug.Log("숲으로 이동!"); }
    private void GoHome() { Debug.Log("집으로 귀환!"); }

    // ✅ 내부 전용 Choice 클래스는 public으로 선언
    [System.Serializable]
    public class Choice
    {
        public string text;
        public Action action;

        public Choice(string text, Action action)
        {
            this.text = text;
            this.action = action;
        }
    }
}
