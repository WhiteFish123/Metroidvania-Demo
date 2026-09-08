using UnityEngine;

public class Enemy_ReaperBattleState : Enemy_BattleState
{
    private Enemy_Reaper enemyReaper;
    public Enemy_ReaperBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyReaper = enemy as Enemy_Reaper;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer=enemyReaper.maxBattleIdleTime;
    }

    override public void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();

        if(stateTimer<=0)
            stateMachine.ChangeState(enemyReaper.reaperTeleportState);
            
        if (enemy.PlayerDetected())
            UpdateTargetIfNeeded();

        if (WithinAttackRange() && enemy.PlayerDetected()&&CanAttack())
        {
            lastTimeAttacked = Time.time;
            stateMachine.ChangeState(enemyReaper.reaperAttackState);
        }
        else
        {
            float xVelocity=enemy.CanChasePlayer ? enemy.GetBattleMoveSpeed() : 0.001f;

            if(enemy.groundDetected==false)
            {
                xVelocity=0.00001f;
            }

            enemy.SetVelocity(xVelocity* DirectionToPlayer(), rb.linearVelocity.y);
        }
    }
    
}
