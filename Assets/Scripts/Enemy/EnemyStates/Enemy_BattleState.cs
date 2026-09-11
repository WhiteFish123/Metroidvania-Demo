using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    protected Transform player;
    protected Transform lastTarget;
    protected float lastTimeWasInBattle;
    protected float lastTimeAttacked=float.NegativeInfinity;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        UpdateBattleTimer();

        if(player == null)
            player = enemy.GetPlayerReference();

        float dist = DistanceToPlayer();
        bool shouldRetreat = dist < enemy.minRetreatDistance;
        //Debug.Log($"[Retreat] {enemy.name} Enter BattleState | dist={dist:F2} | minRetreat={enemy.minRetreatDistance} | shouldRetreat={shouldRetreat}");

        if (shouldRetreat)
        {
            ShortRetreat();
        }
    }

    protected void ShortRetreat()
    {
        float x=(enemy.retreatVelocity.x * enemy.activeSlowMultiplier) * -DirectionToPlayer();
        float y=enemy.retreatVelocity.y;
        //Debug.Log($"[Retreat] {enemy.name} ShortRetreat | retreatVelocity={enemy.retreatVelocity} | slowMultiplier={enemy.activeSlowMultiplier} | finalVelocity=({x:F2},{y:F2})");
        rb.linearVelocity = new Vector2(x, y);
        enemy.HandleFlip(DirectionToPlayer());
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }

        if (CanExitBattle())
            stateMachine.ChangeState(enemy.idleState);

        if (WithinAttackRange() && enemy.PlayerDetected()&&CanAttack())
        {
            lastTimeAttacked = Time.time;
            stateMachine.ChangeState(GetAttackState());
        }
        else
        {
            float xVelocity=enemy.CanChasePlayer ? enemy.GetBattleMoveSpeed() : 0.001f;
            enemy.SetVelocity(xVelocity* DirectionToPlayer(), rb.linearVelocity.y);
        }
    }

    protected bool CanAttack()=>Time.time> lastTimeAttacked + enemy.attackCooldown;

    protected void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetected() == false)
            return;

        Transform newTarget = enemy.PlayerDetected().transform;

        if (newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
        }
    }

    protected void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;

    protected bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;
    protected virtual bool CanExitBattle()=>BattleTimeIsOver();
    protected virtual EnemyState GetAttackState()=>enemy.attackState;
    protected bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;
    protected bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    protected float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    protected int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }

}