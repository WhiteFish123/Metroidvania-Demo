using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_QuestSlot : MonoBehaviour
{
     [SerializeField] private TextMeshProUGUI questName;
     [SerializeField] private Image[] rewardQuickPrewview;

     private QuestDataSO questInSlot;

     public void SetupQuestSlot(QuestDataSO questDataSO)
    {
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
        Debug.Log("Setup quest preview");
    }
}
