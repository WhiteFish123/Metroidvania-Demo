using UnityEngine;

public enum DialogueActionType
{
    None,
    OpenQuest,
    OpenShop,
    OpenCraft,
    GetQuestReward,
    PlayerMakeChoice,
    CloseDialogue
}
//后续可以添加其他操作类型，但记住一定要在最末尾添加，否则会导致索引错误