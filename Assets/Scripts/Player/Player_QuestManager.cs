using UnityEngine;
using System.Collections.Generic;

public class Player_QuestManager : MonoBehaviour,ISaveable
{
    public List<QuestData> activeQuests;
    public List<QuestData> completedQuests;
    private Entity_DropManager dropManager;
    private Inventory_Player inventory;
    [Header("Quest Database")]
    [SerializeField]private QuestDatabaseSO questDatabase;


    private void Awake()
    {
        dropManager=GetComponent<Entity_DropManager>();
        inventory=GetComponent<Inventory_Player>();
    }
    public void TryGetRewardFrom(RewardType npcType)
    {
        List<QuestData> getRewardQuests=new List<QuestData>();

        foreach(var quest in activeQuests)
        {
            if(quest.questDataSO.questType==QuestType.Delivery)
            {
                var requiredItem=quest.questDataSO.itemToDeliver;
                var requiredAmount=quest.questDataSO.requiredAmount;
                if(inventory.HasItemAmount(requiredItem,requiredAmount))
                {
                    inventory.RemoveItemAmount(requiredItem,requiredAmount);
                    quest.AddQuestProgress(requiredAmount);
                }
            }

            if(quest.CanGetReward()&&quest.questDataSO.rewardType==npcType)
                getRewardQuests.Add(quest);
        }

        foreach(var quest in getRewardQuests)
        {
            GiveQuestReward(quest.questDataSO);
            CompleteQuest(quest);
        }
    }
    private void GiveQuestReward(QuestDataSO questDataSO)
    {
        foreach(var item in questDataSO.rewardItems)
        {
            if(item==null||item.itemData==null)
                continue;


            for(int i=0;i<item.stackSize;i++)
            {
                dropManager.CreateItemDrop(item.itemData);
            }
        }
    }
    public bool HasCompletedQuest()
    {
        for(int i=0;i<activeQuests.Count;i++)
        {
            QuestData quest=activeQuests[i];
            if(quest.questDataSO.questType==QuestType.Delivery)
            {
                var requiredItem=quest.questDataSO.itemToDeliver;
                var requiredAmount=quest.questDataSO.requiredAmount;
                if(inventory.HasItemAmount(requiredItem,requiredAmount))
                    return true;
            }
            if(quest.CanGetReward())
                return true;
        }
        return false;
    }
    public void AddProgress(string questTargetId,int amount = 1)
    {
        List<QuestData>getRewardQuests=new List<QuestData>();

        foreach(var quest in activeQuests)
        {
            if(quest.questDataSO.questTargetId != questTargetId)
                continue;

            if(quest.canGetReward==false)
                quest.AddQuestProgress(amount);

            if(quest.questDataSO.rewardType==RewardType.None && quest.CanGetReward())
                getRewardQuests.Add(quest);

        }
        foreach(var quest in getRewardQuests)
        {
            GiveQuestReward(quest.questDataSO);
            CompleteQuest(quest);
        }
    }

    public int GetQuestProgress(QuestData questToCheck)
    {
        QuestData quest=activeQuests.Find(q=>q==questToCheck);

        return quest!=null ? quest.currentAmount:0;
    }
    public void AcceptQuest(QuestDataSO questDataSO)
    {
        activeQuests.Add(new QuestData(questDataSO));
    }
    public void CompleteQuest(QuestData questData)
    {
        completedQuests.Add(questData);
        activeQuests.Remove(questData);
    }
    public bool QuestIsActive(QuestDataSO questToCheck)
    {
        if(questToCheck==null)
            return false;

        return activeQuests.Find(q=>q.questDataSO == questToCheck)!=null;//判断是否已经接了这个任务
    }
    #region 自己加的
    public bool IsQuestCompleted(QuestDataSO questToCheck)
    {
        return completedQuests.Find(q=>q.questDataSO == questToCheck)!=null;
    }
    #endregion
    public void LoadData(GameData data)
    {
        activeQuests.Clear();

        foreach(var entry in data.activeQuests)
        {
            string questSaveId=entry.Key;
            int progress=entry.Value;

            QuestDataSO questDataSO=questDatabase.GetQuestByID(questSaveId);
            
            if(questDataSO==null)
            {
                Debug.LogError(questSaveId+"was not found in the database!");
                continue;
            }
            QuestData questToLoad=new QuestData(questDataSO);
            questToLoad.currentAmount=progress;

            activeQuests.Add(questToLoad);
        }
        #region 自己加的：加载已完成的任务
        completedQuests.Clear();
        foreach(var entry in data.completedQuests)
        {
            QuestDataSO questDataSO = questDatabase.GetQuestByID(entry.Key);
            if(questDataSO != null)
            completedQuests.Add(new QuestData(questDataSO));
        }
        #endregion
    }

    public void SaveData(ref GameData data)
    {
        data.activeQuests.Clear();
        #region 自己加的
        data.completedQuests.Clear();
        #endregion

        foreach(var quest in activeQuests)
        {
            data.activeQuests.Add(quest.questDataSO.questSaveId,quest.currentAmount);
        }
        foreach(var quest in completedQuests)
        {
            data.completedQuests.Add(quest.questDataSO.questSaveId,true);
        }
    }
    
}


