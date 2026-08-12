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
    public SerializableDictionary<string,ItemType>equipedItems;//物品的id->物品的类型

    public int skillPoints;
    public SerializableDictionary<string,bool>skillTreeUI;//技能的id->技能是否解锁
    public SerializableDictionary<SkillType,SkillUpgradeType>skillUpgrades;//技能的类型->技能的升级类型

    public Vector3 savedCheckpoint;

    public GameData()
    {
        inventory=new SerializableDictionary<string,int>();
        storageItems=new SerializableDictionary<string,int>();
        storageMaterials=new SerializableDictionary<string,int>();

        equipedItems=new SerializableDictionary<string,ItemType>();

        skillTreeUI=new SerializableDictionary<string,bool>();
        skillUpgrades=new SerializableDictionary<SkillType,SkillUpgradeType>();
    }
}
