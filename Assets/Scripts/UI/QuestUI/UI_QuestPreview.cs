using UnityEngine;
using TMPro;

public class UI_QuestPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questGoal;
    [SerializeField] private UI_QuestRewardSlot[] questReward;

    [SerializeField] private GameObject[] additionalObjects;
    private UI_Quest questUI;
    private QuestDataSO previewQuest;
    public void SetupQuestPreview(QuestDataSO questDataSO)
    {
        questUI=transform.root.GetComponentInChildren<UI_Quest>();
        previewQuest=questDataSO;

        EnableAddtionalObjects(true);
        EnableQuestRewardObjects(false);

        questName.text = questDataSO.questName;
        questDescription.text=questDataSO.description;
        questGoal.text=questDataSO.questGoal;
        
        for(int i=0;i<questDataSO.rewardItems.Length;i++)
        {
            Inventory_Item rewardItem = new Inventory_Item(questDataSO.rewardItems[i].itemData);
            rewardItem.stackSize=questDataSO.rewardItems[i].stackSize;

            questReward[i].gameObject.SetActive(true);
            questReward[i].UpdateSlot(rewardItem);
        }
    }
    public void AcceptQuestBTN()
    {
        MakeQuestPreviewEmpty();

        questUI.questManager.AcceptQuest(previewQuest);
        questUI.UpdateQuestList();
    }
    public void MakeQuestPreviewEmpty()
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
