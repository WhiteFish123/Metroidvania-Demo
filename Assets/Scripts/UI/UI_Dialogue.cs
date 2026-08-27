using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class UI_Dialogue : MonoBehaviour
{
    private UI ui;
    [SerializeField]private Image speakerPortrait;
    [SerializeField]private TextMeshProUGUI speakerName;
    [SerializeField]private TextMeshProUGUI dialogueText;
    [SerializeField]private TextMeshProUGUI dialogueChoices;

    [Space]
    [SerializeField]private float typingSpeed=0.05f;
    private string fullTextToShow;
    private Coroutine typeTextCo;
    private DialogueLineSO currentLine;
    private bool waitingToConfirm;
    private bool canInteract;


    private void Awake()
    {
        ui=GetComponentInParent<UI>();
    }

    public void PlayDialogueLIne(DialogueLineSO line)
    {
        currentLine=line;
        canInteract=false;

        speakerPortrait.sprite=line.speaker.speakerPortrait;
        speakerName.text=line.speaker.speakerName;

        fullTextToShow=line.GetRandomLine();
        typeTextCo=StartCoroutine(TypeTextCo(fullTextToShow));
        StartCoroutine(EnableInteractionCo());
    }
    private void HandleNextAction()
    {
        switch(currentLine.actionType)
        {
            case DialogueActionType.OpenShop:
                ui.SwitchToInGameUI();
                ui.OpenMerchantUI(true);
                break;
        }
    }
    public void DialogueInteraction()
    {
        if(canInteract==false)
            return;

        if(typeTextCo!=null)
        {
            CompleteTyping();
            waitingToConfirm=true;
            return;
        }
        if(waitingToConfirm)
        {
            waitingToConfirm=false;
            HandleNextAction();
        }
    }

    private void CompleteTyping()
    {
        if(typeTextCo!=null)
        {
            StopCoroutine(typeTextCo);
            dialogueText.text=fullTextToShow;
            typeTextCo=null;
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
        waitingToConfirm=true;
        typeTextCo = null;
    }
    private IEnumerator EnableInteractionCo()
    {
        yield return null;
        canInteract=true;
    }
}
