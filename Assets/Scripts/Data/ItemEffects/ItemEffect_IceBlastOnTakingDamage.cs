using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Ice blast", fileName = "Item effect data - Ice blast on taking damage")]//创建资源文件 

public class ItemEffect_IceBlastOnTakingDamage : ItemEffect_DataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;

    [Space]
    [SerializeField] private float healthPercentTrigger = .25f;//触发效果的生命值阙值
    [SerializeField] private float cooldown;//冷却时间
    private float lastTimeUsed = -999;//上一次的使用时刻
    [Header("Vfx Objects")]
    [SerializeField] private GameObject iceBlastVfx;
    [SerializeField] private GameObject onHitVfx;


    public override void ExecuteEffect()
    {
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;//用于判断冷却时间是否结束
        bool reachedThreshold = player.health.GetHealthPercent() <= healthPercentTrigger;//用于判断当前生命值是否已低于阙值

        if (noCooldown && reachedThreshold)//如果两个条件都满足了的话
        {
            player.vfx.CreateEffectOf(iceBlastVfx, player.transform);//创建预制体
            lastTimeUsed = Time.time;//更新使用时刻
            DamageEnemiesWithIce();//对敌人造成伤害
        }
    }

    private void DamageEnemiesWithIce()//以自身为中心释放范围冰冻伤害
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);//检测一定范围内的敌人

        foreach (var target in enemies)
        {
            IDamageable damagable = target.GetComponent<IDamageable>();

            if (damagable == null) continue;

            bool targetGotHit = damagable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);

            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

            if (targetGotHit)
                player.vfx.CreateEffectOf(onHitVfx, target.transform);
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.OnTakingDamage += ExecuteEffect;//玩家的某个事件与装备效果的触发事件绑定了
        //OntakingDamage事件现在与装备的效果产生了绑定，触发该事件就可以调用效果了
        lastTimeUsed = -999;//重新装备的时候初始化
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage -= ExecuteEffect;//该事件取消绑定该装备效果
        player = null;//清空对象，防止脚本对象在会话之间保存值和引用
    }
}
