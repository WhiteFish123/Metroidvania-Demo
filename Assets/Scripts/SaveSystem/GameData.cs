using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int gold;
    public List<Inventory_Item>itemList;
    public SerializableDictionary<string,int>inventory;//物品的存储ID->物品的数量
    public SerializableDictionary<string,int>storageItems;
    public SerializableDictionary<string,int>storageMaterials;

    public SerializableDictionary<string,ItemType>EquipedItems;//物品的id->物品的类型
    public GameData()
    {
        inventory=new SerializableDictionary<string,int>();
        storageItems=new SerializableDictionary<string,int>();
        storageMaterials=new SerializableDictionary<string,int>();

        EquipedItems=new SerializableDictionary<string,ItemType>();
    }
}
