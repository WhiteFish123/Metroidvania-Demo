using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    private bool boostApplied;
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        boostApplied = false;
        player.SetVelocity(player.wallJumpForce.x * -player.facingDir, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();
        float targetX=player.moveInput.x != 0 ?
            player.moveInput.x*player.moveSpeed:
            rb.linearVelocity.x;
        float newX=Mathf.Lerp(rb.linearVelocity.x,targetX,player.wallJumpLerp*Time.deltaTime);
        player.SetVelocity(newX,rb.linearVelocity.y);
        
        if (!boostApplied && player.moveInput.x != 0 && Mathf.Sign(player.moveInput.x) != player.facingDir)
        {
            boostApplied = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * player.wallJumpBackBoost);
        }
        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);

        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
