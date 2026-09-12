using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;
    private Player_QuestManager questManager;


    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
        questManager=Player.instance.questManager;
    }
    public override bool TakeDamage(float damage, float elementalDamage,ElementType element, Transform damageDealer)
    {
        //Debug.Log($"[{enemy.name}] 被攻击, 当前状态: {enemy.stateMachine.currentState}, 尝试进入Battle");
        if(canTakeDamage == false) 
            return false;

        bool wasHit = base.TakeDamage(damage,elementalDamage,element, damageDealer);

        if (wasHit == false)
            return false;

        if(damageDealer.GetComponent<Player>() != null)
            //Debug.Log($"[{enemy.name}] 被攻击, 尝试进入Battle");
            enemy.TryEnterBattleState(damageDealer);

        return true;    
    }
    protected override void Die()
    {
        if (isDead) return;
        base.Die();
        questManager.AddProgress(enemy.questTargetId);
    }
}