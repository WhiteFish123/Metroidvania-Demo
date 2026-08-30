using UnityEngine;

public class EnemySlimeDeadState : Enemy_DeadState
{
    private Enemy_Slime enemySlime;
    public EnemySlimeDeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemySlime = enemy as Enemy_Slime;//将敌人转换为小史莱姆类型
    }
    public override void Enter()
    {
        base.Enter();
        enemySlime.CreateSlimeOnDeath();
    }
}
