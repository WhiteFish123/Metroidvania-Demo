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
    [SerializeField]private TextMeshProUGUI[] dialogueChoicesText;

    [Space]
    [SerializeField]private float typingSpeed=0.05f;
    private string fullTextToShow;
    private Coroutine typeTextCo;
    private DialogueLineSO currentLine;
    private DialogueLineSO[] currentChoices;
    private DialogueLineSO selectedChoice;
    private int selectedChoiceIndex;
    private bool waitingToConfirm;
    private bool canInteract;


    private void Awake()
    {
        ui=GetComponentInParent<UI>();
    }

    public void PlayDialogueLIne(DialogueLineSO line)
    {
        currentLine=line;
        currentChoices=line.choiceLines;
        canInteract=false;
        HideAllChoices();

        speakerPortrait.sprite=line.speaker.speakerPortrait;
        speakerName.text=line.speaker.speakerName;

        fullTextToShow=line.actionType==DialogueActionType.None||line.actionType==DialogueActionType.PlayerMakeChoice?
            line.GetRandomLine() : line.actionLine;
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
            case DialogueActionType.PlayerMakeChoice:
                if(selectedChoice==null)
                {
                    selectedChoiceIndex=0;
                    ShowChoices();
                }
                else
                {
                    DialogueLineSO selectedChoice=currentChoices[selectedChoiceIndex];
                    PlayDialogueLIne(selectedChoice);
                    selectedChoice=null;
                }
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
    private void ShowChoices()
    {
        for(int i=0;i<dialogueChoicesText.Length;i++)
        {
            if(i<currentChoices.Length)//如果索引还在选项范围内
            {
                DialogueLineSO choice=currentChoices[i];
                string choiceText=choice.GetFirstLine();
                dialogueChoicesText[i].gameObject.SetActive(true);
                dialogueChoicesText[i].text=selectedChoiceIndex == i ? 
                    $"<color=yellow>{i+1 } { choiceText}" : 
                    $"{i+1 }{choiceText}";
            }
            else
            {
                dialogueChoicesText[i].gameObject.SetActive(false);
            }
        }
        selectedChoice=currentChoices[selectedChoiceIndex];//更新选中的选项
    }
    private void HideAllChoices()
    {
        foreach(var obj in dialogueChoicesText)
            obj.gameObject.SetActive(false);
    }

    public void NavigateChoice(int direction)
    {
        if(currentChoices==null||currentChoices.Length<=1)
            return;
        
        selectedChoiceIndex=selectedChoiceIndex+direction;
        selectedChoiceIndex=Mathf.Clamp(selectedChoiceIndex,0,currentChoices.Length-1);
        ShowChoices();//刷新显示的选项
    }
    private IEnumerator TypeTextCo(string text)
    {
        dialogueText.text="";
        foreach(char letter in text)
        {
            dialogueText.text+=letter;
            yield return new WaitForSeconds(typingSpeed);
        }   
        if(currentLine.actionType!=DialogueActionType.PlayerMakeChoice)
        {
            waitingToConfirm=true; 
        }
        else
        {
            yield return new WaitForSeconds(.2f);
            HandleNextAction();
        }
        typeTextCo = null;
    }
    private IEnumerator EnableInteractionCo()
    {
        yield return null;
        canInteract=true;
    }
}
