using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UI_Dialogue : MonoBehaviour
{
    [SerializeField]private Image speakerPortrait;
    [SerializeField]private TextMeshProUGUI speakerName;
    [SerializeField]private TextMeshProUGUI dialogueText;
    [SerializeField]private TextMeshProUGUI dialogueChoices;

    [Space]
    [SerializeField]private float typingSpeed=0.05f;
    private Coroutine typeTextCo;
    private string fullTextToShow;

    public void PlayDialogueLIne(DialogueLineSO line)
    {
        speakerPortrait.sprite=line.speaker.speakerPortrait;
        speakerName.text=line.speaker.speakerName;
        fullTextToShow=line.GetRandomLine();
        typeTextCo=StartCoroutine(TypeTextCo(fullTextToShow));
    }

    public void DialogueInteraction()
    {
        if(typeTextCo!=null&&dialogueText.text.Length>4)
        {
            CompleteTyping();
            return;
        }
    }

    private void CompleteTyping()
    {
        if(typeTextCo!=null)
        {
            StopCoroutine(typeTextCo);
            dialogueText.text=fullTextToShow;
        }
    }
    private IEnumerator TypeTextCo(string text)
    {
        dialogueText.text="";
        foreach(char letter in text)
        {
            dialogueText.text+=letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
