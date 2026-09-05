using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SetGravityScale(rb.gravityScale*player.gravityScaleMultiplier);
        rb.linearVelocity=new Vector2(rb.linearVelocity.x,Mathf.Max(rb.linearVelocity.y,player.maxFallSpeed));
    }
    public override void Update()
    {
        base.Update();

        if (player.groundDetected)
            stateMachine.ChangeState(player.idleState);

        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
    public override void Exit()
    {
        base.Exit();
        SetGravityScale(rb.gravityScale/player.gravityScaleMultiplier);
    }
}
