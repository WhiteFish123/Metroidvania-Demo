using UnityEngine;

public class Enemy_CowIdleState : EnemyState
{
    private Enemy_Cow enemyCow;
    private float lastTimeAttacked = float.NegativeInfinity;

    public Enemy_CowIdleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyCow = enemy as Enemy_Cow;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected() && WithinAttackRange() && CanAttack())
        {
            lastTimeAttacked = Time.time;
            enemy.HandleFlip(DirectionToPlayer());

            if (enemyCow.CanDoSpecialAttack())
                stateMachine.ChangeState(enemyCow.cowSpecialAttackState);
            else
                stateMachine.ChangeState(enemyCow.cowAttackState);
        }
    }

    private bool CanAttack() => Time.time > lastTimeAttacked + enemy.attackCooldown;

    private bool WithinAttackRange()
    {
        Transform player = enemy.GetPlayerReference();
        if (player == null)
            return false;
        return Mathf.Abs(player.position.x - enemy.transform.position.x) < enemy.attackDistance;
    }

    private int DirectionToPlayer()
    {
        Transform player = enemy.GetPlayerReference();
        if (player == null)
            return 0;
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}