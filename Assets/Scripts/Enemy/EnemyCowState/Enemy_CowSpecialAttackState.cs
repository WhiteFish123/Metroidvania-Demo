using UnityEngine;

public class Enemy_CowSpecialAttackState : EnemyState
{
    private Enemy_Cow enemyCow;

    public Enemy_CowSpecialAttackState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyCow = enemy as Enemy_Cow;
    }

    public override void Enter()
    {
        base.Enter();
        SyncAttackSpeed();
        enemyCow.SetVelocity(0, 0);
        enemyCow.SetSpecialAttackOnCooldown();
        stateTimer = 1.5f;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled || stateTimer < 0)
            stateMachine.ChangeState(enemyCow.cowIdleState);
    }
}