using UnityEngine;

public class UI_Quest : MonoBehaviour,ISaveable
{
    private GameData currentGameData;
    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_QuestPreview questPreview;
    private UI_QuestSlot[] questSlots;
    public Player_QuestManager questManager{get;private set;}

    private void Awake()
    {
        questSlots = GetComponentsInChildren<UI_QuestSlot>(true);
        questManager=Player.instance.questManager;
    }
    public void SetupQuestUI(QuestDataSO[] questsToSetup)
    {
        foreach(var slot in questSlots)
        slot.gameObject.SetActive(false);

        for(int i=0;i<questsToSetup.Length;i++)
        {
            questSlots[i].gameObject.SetActive(true);
            questSlots[i].SetupQuestSlot(questsToSetup[i]);
        }
        questPreview.MakeQuestPreviewEmpty();
        inventorySlots.UpdateSlots(Player.instance.inventory.itemList);

        UpdateQuestList();
    }

    public void UpdateQuestList()
    {
        #region 自己加的：更新任务列表时，检查是否有任务槽
        if(questSlots == null) return;
        #endregion
        foreach(var slot in questSlots)
        {
            if(slot.questInSlot==null)continue;

            if(slot.gameObject.activeSelf && CanTakeQuest(slot.questInSlot)==false)
                slot.gameObject.SetActive(false);
        }
    }
    private bool CanTakeQuest(QuestDataSO questToCheck)
    {
        bool questActive=questManager.QuestIsActive(questToCheck);

        if(questManager.IsQuestCompleted(questToCheck))
            return false;
            
        if(currentGameData != null)
        {
            bool questCompleted=
                currentGameData.completedQuests.TryGetValue(questToCheck.questSaveId,out bool isCompleted)&&isCompleted;
            return questActive==false && questCompleted==false;
        }
        return questActive == false;
    }
    public UI_QuestPreview GetQuestPreview()=>questPreview;

    public void LoadData(GameData data)
    {
        currentGameData=data;
        UpdateQuestList();
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
