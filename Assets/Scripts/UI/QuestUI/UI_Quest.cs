using UnityEngine;

public class UI_Quest : MonoBehaviour
{
    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_QuestPreview questPreview;

    private UI_QuestSlot[] questSlots;
    private void Awake()
    {
        questSlots = GetComponentsInChildren<UI_QuestSlot>(true);
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
    }
    public UI_QuestPreview GetQuestPreview()=>questPreview;
}
