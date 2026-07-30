using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory_Item itemInSlot { get; private set; }
    protected Inventory_Player inventory;
    protected UI ui;
    protected RectTransform rect;

    [Header("UI Slot Setup")]
    [SerializeField] protected GameObject defaultIcon;
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TextMeshProUGUI itemStackSize;

    protected virtual void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        inventory = FindAnyObjectByType<Inventory_Player>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)//鼠标点击插槽
    {
        if (itemInSlot == null || itemInSlot.itemData.itemType == ItemType.Material)//如果是空插槽或者是材料的话不作为
            return;

        bool alternativeInput = Input.GetKey(KeyCode.LeftControl);//crtl+鼠标点击 可以销毁该物品

        if (alternativeInput)
        {
            inventory.RemoveOneItem(itemInSlot);
        }
        else
        {
            if (itemInSlot.itemData.itemType == ItemType.Consumable)//判断是不是消耗品
            {
                
                inventory.TryUseItem(itemInSlot);//是的话尝试消耗
            }
            else
                inventory.TryEquipItem(itemInSlot);//不是的话（装备类），则尝试装备上身
        }

        if (itemInSlot == null)//空槽不显示信息框
            ui.itemToolTip.ShowToolTip(false, null);
    }

    public void UpdateSlot(Inventory_Item item)//当拾取物品或者使用物品的话，随后执行该函数进行UI刷新
    {
        itemInSlot = item;

        if(defaultIcon != null)
            defaultIcon.gameObject.SetActive(itemInSlot == null);

        if (itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }

        Color color = Color.white; color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite = itemInSlot.itemData.itemIcon;
        itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }
}
