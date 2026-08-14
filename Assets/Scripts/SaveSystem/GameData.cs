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

    public SerializableDictionary<string,bool>unlockedCheckpoints;//检查点的id->是否解锁
    public SerializableDictionary<string,Vector3>inScenePortals;//传送的id->传送的位置

    public string portalDestinationSceneName;
    public bool returningFormTown;

    public string lastScenePlayed;
    public Vector3 lastPlayerPosition;

    public GameData()
    {
        inventory=new SerializableDictionary<string,int>();
        storageItems=new SerializableDictionary<string,int>();
        storageMaterials=new SerializableDictionary<string,int>();

        equipedItems=new SerializableDictionary<string,ItemType>();

        skillTreeUI=new SerializableDictionary<string,bool>();
        skillUpgrades=new SerializableDictionary<SkillType,SkillUpgradeType>();

        unlockedCheckpoints=new SerializableDictionary<string,bool>();
        inScenePortals=new SerializableDictionary<string,Vector3>();
    }
}
