using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int gold;
    public List<Inventory_Item>itemList;
    public SerializableDictionary<string,int>inventory;
    public GameData()
    {
        inventory=new SerializableDictionary<string,int>();
    }
}
