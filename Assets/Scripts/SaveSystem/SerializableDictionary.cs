using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys=new List<TKey>();
    [SerializeField]private List<TValue> values=new List<TValue>();

    public void OnAfterDeserialize()
    {
        this.Clear();
        if(keys.Count!=values.Count)
            Debug.LogError("Key and value lists are not of the same size.");
        for(int i=0;i<keys.Count;i++)
        {
            Add(keys[i],values[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach(KeyValuePair<TKey,TValue>pairs in this)
        {
            keys.Add(pairs.Key);
            values.Add(pairs.Value);
        }
    }
}
