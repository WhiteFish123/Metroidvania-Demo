using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private Inventory_Storage storage;

    public enum StorageSlotType { StorageSlot,PlayerInventorySlot}//区分是仓储中的插槽还是玩家背包的插槽
    public StorageSlotType slotType;//enum实例
    public void SetStorage(Inventory_Storage storage) => this.storage = storage;

    public override void OnPointerDown(PointerEventData eventData)//(覆写原本的角色界面的)鼠标点击操作
    {
        if (itemInSlot == null)
            return;

        bool transferFullStack = Input.GetKey(KeyCode.LeftControl);//如果按住左ctrl，变成整个槽位移动

        if (slotType == StorageSlotType.StorageSlot)
            storage.FromStorageToPlayer(itemInSlot,transferFullStack);

        if (slotType == StorageSlotType.PlayerInventorySlot)
            storage.FromPlayerToStorage(itemInSlot,transferFullStack);

        ui.itemToolTip.ShowToolTip(false, null);
    }

}
