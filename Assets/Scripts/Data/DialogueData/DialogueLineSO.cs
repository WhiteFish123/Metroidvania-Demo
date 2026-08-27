using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Dialogue Data/New Line Data",fileName="Line - ")]
public class DialogueLineSO : ScriptableObject
{
    [Header("Dialogue info")]
    public string dialogueGroupName;
    public DialogueSpeakerSO speaker;

    [Header("Text options")]
    [TextArea]public string[] textLine;

    [Header("Answer setup")]
    public bool playerCanAnswer;//如果玩家可以做选择的话，值为true
    public DialogueLineSO[] answerLine;//玩家可以选择的对话行

    public string GetRandomLine()
    {
        return textLine[Random.Range(0,textLine.Length)];
    }
}
