using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_QuestSlot : MonoBehaviour
{
     [SerializeField] private TextMeshProUGUI questName;
     [SerializeField] private Image[] rewardQuickPrewview;

     private QuestDataSO questInSlot;
     private UI_QuestPreview questPreview;

     public void SetupQuestSlot(QuestDataSO questDataSO)
    {
        questPreview=transform.root.GetComponentInChildren<UI_Quest>().GetQuestPreview();

        questInSlot = questDataSO;

        questName.text = questInSlot.questName;

        foreach(var previewIcon in rewardQuickPrewview)
        {
            previewIcon.gameObject.SetActive(false);
        }
        for(int i=0;i<questInSlot.rewardItems.Length;i++)
        {
            if(questDataSO.rewardItems[i]==null||questDataSO.rewardItems[i].itemData==null)
                continue;
            
            Image slot=rewardQuickPrewview[i];

            slot.gameObject.SetActive(true);
            slot.sprite=questDataSO.rewardItems[i].itemData.itemIcon;
            slot.GetComponentInChildren<TextMeshProUGUI>().text=questDataSO.rewardItems[i].stackSize.ToString();
        }
    }

    public void UpdateQuestPreview()
    {
        questPreview.SetupQuestPreview(questInSlot);
    }
}
