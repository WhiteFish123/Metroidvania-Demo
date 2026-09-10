using UnityEngine;

public class Enemy_CowAttackState : EnemyState
{
    private Enemy_Cow enemyCow;

    public Enemy_CowAttackState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyCow = enemy as Enemy_Cow;
    }

    public override void Enter()
    {
        base.Enter();
        SyncAttackSpeed();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(enemyCow.cowIdleState);
    }
}