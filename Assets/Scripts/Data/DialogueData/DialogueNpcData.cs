using UnityEngine;
using System;

[Serializable]
public class DialogueNpcData
{
    public RewardType npcRewardType;
    public QuestDataSO[] quests;

    public DialogueNpcData(RewardType rewardType,QuestDataSO[] quests)
    {
        this.npcRewardType=rewardType;
        this.quests=quests;
    }
}
