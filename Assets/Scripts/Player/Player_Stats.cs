using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private List<string> activeBuff = new List<string>();//当前已被激活的buff
    private Inventory_Player inventory;

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<Inventory_Player>();
    }

    public bool CanApplyBuffOf(string source)
    {
        return activeBuff.Contains(source) == false;//如果当前已经有该来源的buff了就不能再叠加使用了
    }

    public void ApplyBuff(BuffEffectData[] buffsToAply, float duration, string source)//buff类型，数值，持续时间，来源
    {
        StartCoroutine(BuffCo(buffsToAply, duration, source));
    }

    private IEnumerator BuffCo(BuffEffectData[] buffsToApply, float duration, string source)//提供buff的协程 
    {
        activeBuff.Add(source);

        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).AddModifier(buff.value, source);
        }

        yield return new WaitForSeconds(duration);//等待持续时间结束
        //移除时间到了的buff
        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).RemoveModifier(source);
        }

        inventory.TriggerUpdateUI();
        activeBuff.Remove(source);
    }
}
