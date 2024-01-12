using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]GameObject dialogueBox;
    [SerializeField]Text dialogueText;

    [SerializeField]int lettersPerSecond;

    public event Action OnShowDialogue;
    public event Action OnHideDialogue;

    public event Action OnDialogueEnd;

    public static DialogueManager Instance { get; private set; }

    private bool hasSword = false;

    public bool HasSword
    {
        get { return hasSword; }
        private set { hasSword = value; }
    }

    public void GiveSword()
    {
        hasSword = true;
    }

    private void Awake()
    {
        Instance = this;
    }

    Dialogue dialogue;
    int currentLine = 0;
    bool isTyping;

    public IEnumerator ShowDialogue(Dialogue dialogue)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialogue?.Invoke();

        this.dialogue = dialogue;
        dialogueBox.SetActive(true);

        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
    }
    public void HandleUpdate()
    {
        if (Input.GetKeyUp(KeyCode.E) && !isTyping)
        {
            ++currentLine;
            if(currentLine < dialogue.Lines.Count)
            {
                StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
            }
            else
            {
                dialogueBox.SetActive(false);
                currentLine = 0;
                OnHideDialogue?.Invoke();
                OnDialogueEnd?.Invoke();
            }
        }
    }

    public IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (var letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }

}
