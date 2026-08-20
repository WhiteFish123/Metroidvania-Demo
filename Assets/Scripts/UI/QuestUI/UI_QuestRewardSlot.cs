using UnityEngine;
using UnityEngine.EventSystems;
public class UI_QuestRewardSlot : UI_ItemSlot
{
    //不应该有点击的效果
    public override void OnPointerDown(PointerEventData eventData)
    {
        
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot,false,false,false);
    }
}
