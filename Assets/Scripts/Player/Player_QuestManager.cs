using UnityEngine;
using System.Collections.Generic;

public class Player_QuestManager : MonoBehaviour
{
    public List<QuestData> activeQuests;

    public void AddProgress(string questTargetId,int amount = 1)
    {
        foreach(var quest in activeQuests)
        {
            if(quest.questDataSO.questTargetId != questTargetId)
                continue;

            quest.AddQuestProgress(amount);
        }
    }


    public void AcceptQuest(QuestDataSO questDataSO)
    {
        activeQuests.Add(new QuestData(questDataSO));
    }
    public bool QuestIsActive(QuestDataSO questToCheck)
    {
        if(questToCheck==null)
            return false;

        return activeQuests.Find(q=>q.questDataSO == questToCheck)!=null;//判断是否已经接了这个任务
    }
}


