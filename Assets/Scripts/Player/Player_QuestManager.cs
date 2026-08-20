using UnityEngine;
using System.Collections.Generic;

public class Player_QuestManager : MonoBehaviour,ISaveable
{
    public List<QuestData> activeQuests;
    public List<QuestData> completedQuests;
    private Entity_DropManager dropManager;
    [Header("Quest Database")]
    [SerializeField]private QuestDatabaseSO questDatabase;


    private void Awake()
    {
        dropManager=GetComponent<Entity_DropManager>();
    }
    public void TryGiveRewardFrom(RewardType npcType)
    {
        List<QuestData> getRewardQuests=new List<QuestData>();

        foreach(var quest in activeQuests)
        {
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

    public void AddProgress(string questTargetId,int amount = 1)
    {
        List<QuestData>getRewardQuests=new List<QuestData>();

        foreach(var quest in activeQuests)
        {
            if(quest.questDataSO.questTargetId != questTargetId)
                continue;

            quest.AddQuestProgress(amount);
            if(quest.questDataSO.rewardType==RewardType.None && quest.CanGetReward())
            {
                getRewardQuests.Add(quest);
            }
             
        }
        foreach(var quest in getRewardQuests)
        {
            GiveQuestReward(quest.questDataSO);
            CompleteQuest(quest);
        }
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


