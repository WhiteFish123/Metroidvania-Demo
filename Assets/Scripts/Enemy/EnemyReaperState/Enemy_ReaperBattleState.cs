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
    protected override bool CanExitBattle() => false;
    protected override EnemyState GetAttackState() => enemyReaper.reaperAttackState;
    public override void Update()
    {
        stateTimer-=Time.deltaTime;
        UpdateAnimationParameters();
        base.Update();
        if(stateTimer<=0)
            stateMachine.ChangeState(enemyReaper.reaperTeleportState);//cd好了就传送
        
        if(enemy.PlayerDetected())
            UpdateTargetIfNeeded();
        if(WithinAttackRange()&&enemy.PlayerDetected()&&CanAttack())//如果在攻击范围内且玩家被检测到且可以攻击，则进行攻击
        {
            lastTimeAttacked=Time.time;//记录攻击时间
            stateMachine.ChangeState(GetAttackState());
        }
        else
        {
            float xVelocity=enemy.CanChasePlayer ? enemy.GetBattleMoveSpeed():0.001f;//如果可以追玩家，则移动到玩家位置，否则保持原地
            if(enemy.groundDetected==false)//防止从平台边缘掉落
            {
                xVelocity=0.00001f;//保留微笑速度用来翻转
            }
            enemy.SetVelocity(xVelocity* DirectionToPlayer(), rb.linearVelocity.y);//设置速度
        }
    }
    // override public void Update()
    // {
        
    //     // stateTimer -= Time.deltaTime;
    //     // UpdateAnimationParameters();

    //     // if(stateTimer<=0)
    //     //     stateMachine.ChangeState(enemyReaper.reaperTeleportState);
            
    //     // if (enemy.PlayerDetected())
    //     //     UpdateTargetIfNeeded();

    //     // if (WithinAttackRange() && enemy.PlayerDetected()&&CanAttack())
    //     // {
    //     //     lastTimeAttacked = Time.time;
    //     //     stateMachine.ChangeState(enemyReaper.reaperAttackState);
    //     // }
    //     // else
    //     // {
    //     //     float xVelocity=enemy.CanChasePlayer ? enemy.GetBattleMoveSpeed() : 0.001f;

    //     //     if(enemy.groundDetected==false)
    //     //     {
    //     //         xVelocity=0.00001f;
    //     //     }

    //     //     enemy.SetVelocity(xVelocity* DirectionToPlayer(), rb.linearVelocity.y);
    //     // }
    // }
    
}
