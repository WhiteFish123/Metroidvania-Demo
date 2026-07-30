using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Heal effect", fileName = "Item effect data - Heal")]

public class ItemEffect_Heal : ItemEffect_DataSO
{
    [SerializeField] private float healPercent = .1f;

    public override void ExecuteEffect()//覆写原函数，补充上自己的效果
    {
        Player player = FindFirstObjectByType<Player>();

        float healAmount = player.stats.GetMaxHealth() * healPercent;//计算该道具能给玩家回多少血

        player.health.IncreaseHealth(healAmount);//正式回血
    }
}
