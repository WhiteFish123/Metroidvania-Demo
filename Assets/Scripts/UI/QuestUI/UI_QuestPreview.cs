using UnityEngine;
using TMPro;

public class UI_QuestPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questGoal;
    [SerializeField] private UI_QuestRewardSlot[] questReward;

    [SerializeField] private GameObject[] additionalObjects;

    public void SetupQuestPreview(QuestDataSO questDataSO)
    {
        EnableAddtionalObjects(true);
        EnableQuestRewardObjects(true);

        questName.text = questDataSO.questName;
        questDescription.text=questDataSO.description;
        questGoal.text=questDataSO.questGoal;
        
        for(int i=0;i<questDataSO.rewardItems.Length;i++)
        {
            questReward[i].gameObject.SetActive(true);
            questReward[i].UpdateSlot(questDataSO.rewardItems[i]);
        }
    }

    private void MakeQuestPreviewEmpty()
    {
        questName.text="";
        questDescription.text="";
        EnableAddtionalObjects(false);
        EnableQuestRewardObjects(false);
    }
    private void EnableAddtionalObjects(bool enable)
    {
        foreach(var obj in additionalObjects)
        {
            obj.SetActive(enable);
        }
    }
    private void EnableQuestRewardObjects(bool enable)
    {
        foreach(var obj in questReward)
        {
            obj.gameObject.SetActive(enable);
        }
    }
}
