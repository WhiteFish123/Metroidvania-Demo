using UnityEngine;

public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();

        if (player.moveInput.x != 0)
            player.SetVelocity(player.moveInput.x * (player.moveSpeed * player.inAirMoveMultiplier), rb.linearVelocity.y);
        if (input.Player.Jump.WasPressedThisFrame())
        {
            bool canUse = skillManager.doubleJump.CanUseSkill();
            bool isFallState = stateMachine.currentState == player.fallState;
            //Debug.Log($"[DoubleJump] JumpPressed | CanUseSkill={canUse} | IsFallState={isFallState} | CurrentState={stateMachine.currentState.GetType().Name}");
            if (canUse && isFallState)
            {
                //Debug.Log("[DoubleJump] Triggered!");
                skillManager.doubleJump.ConsumeJump();
                skillManager.doubleJump.SetSkillOnCooldown();
                player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
            }
        }
        if (input.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpAttackState);
    }
}