using UnityEngine;
using System.Collections.Generic;

public class Player_QuestManager : MonoBehaviour
{
    public List<QuestData> activeQuests;
    public List<QuestData> completedQuests;
    private Entity_DropManager dropManager;


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
}


