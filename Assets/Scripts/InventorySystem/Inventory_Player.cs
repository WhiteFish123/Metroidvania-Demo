using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public event Action<int> OnQuickSlotUsed;
    public int gold = 10000;

    public List<Inventory_EquipmentSlot> equipList;
    public Inventory_Storage storage { get ; private set; }

    [Header("Quick Item Slots")]
    public Inventory_Item[] quickItems = new Inventory_Item[2];

    protected override void Awake()
    {
        base.Awake();
        storage = FindFirstObjectByType<Inventory_Storage>();
    }

    public void SetQuickItemInSlot(int slotNumber, Inventory_Item itemToSet)
    {
        quickItems[slotNumber - 1] = itemToSet;
        TriggerUpdateUI();
    }

    public void TryUseQuickItemInSlot(int passedSlotNumber)
    {
        int slotNumber = passedSlotNumber - 1;
        var itemToUse = quickItems[slotNumber];

        if (itemToUse == null)
            return;

        TryUseItem(itemToUse);

        if (FindItem(itemToUse) == null)
        {
            quickItems[slotNumber] = FindSameItem(itemToUse);
        }

        TriggerUpdateUI();
        OnQuickSlotUsed?.Invoke(slotNumber);
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

        // STEP 1 : Try to find empty slot and equip item
        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }

        // STEP 2: No empty slots ? Replace first one
        var slotToReplace = matchingSlots[0];
        var itemToUneqip = slotToReplace.equipedItem;

        UnequipItem(itemToUneqip,slotToReplace != null);
        EquipItem(inventoryItem, slotToReplace);
    }

    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        float savaedHealthPercent = player.health.GetHealthPercent();

        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(player.stats);
        slot.equipedItem.AddItemEffect(player);

        player.health.SetHealthToPercent(savaedHealthPercent);
        RemoveOneItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip,bool replacingItem = false)
    {
        if (CanAddItem(itemToUnequip) == false && replacingItem  == false)
        {
            Debug.Log("No space!");
            return;
        }

        float savedHealthPercent = player.health.GetHealthPercent();
        var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

        if(slotToUnequip != null)
            slotToUnequip.equipedItem = null;

        itemToUnequip.RemoveModifiers(player.stats);
        itemToUnequip.RemoveItemEffect();

        player.health.SetHealthToPercent(savedHealthPercent);
        AddItem(itemToUnequip);
    }
}
