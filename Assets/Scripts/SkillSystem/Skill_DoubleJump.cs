using UnityEngine;

public class Skill_DoubleJump : Skill_Base
{
    public int maxExtraJumps = 2;
    public int remainingJumps { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        remainingJumps = maxExtraJumps;
    }

    public override bool CanUseSkill()
    {
        if (remainingJumps <= 0)
        {
            Debug.Log("[DoubleJump] CanUseSkill=false (remainingJumps=0)");
            return false;
        }
        bool baseResult = base.CanUseSkill();
        //Debug.Log($"[DoubleJump] CanUseSkill={baseResult} (remainingJumps={remainingJumps})");
        return baseResult;
    }

    public void ConsumeJump() => remainingJumps--;
    public void ResetJumps() => remainingJumps = maxExtraJumps;
}