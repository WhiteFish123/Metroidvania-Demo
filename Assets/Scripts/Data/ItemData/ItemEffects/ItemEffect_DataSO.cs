using UnityEngine;

public class ItemEffect_DataSO : ScriptableObject
{
    [TextArea]
    public string effectDescription;
    protected Player player;

    public virtual bool CanBeUsed(Player player)
    {
        return true;
    }

    public virtual void ExecuteEffect()//用来覆写的，各个消耗品覆写并补充自身的效果
    {

    }

    public virtual void Subscribe(Player player)
    {
        this.player = player;
    }

    public virtual void Unsubscribe()
    {

    }

}
