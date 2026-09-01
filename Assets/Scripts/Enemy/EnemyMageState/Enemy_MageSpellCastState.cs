using UnityEngine;

public class Enemy_MageSpellCastState : EnemyState
{
    private Enemy_Mage enemyMage;
    public Enemy_MageSpellCastState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyMage = enemy as Enemy_Mage;
        enemyMage.SetSpellCastPerformed(false);
    }
    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0, 0);
        enemyMage.SetSpellCastPerformed(false);
    }
    public override void Update()
    {
        base.Update();
        
        if(enemyMage.spellCastPerformed)//如果施法完成
            anim.SetBool("spellCast_performed", true);


        if(triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
    public override void Exit()
    {
        base.Exit();
        anim.SetBool("spellCast_performed", false);
    }
}
