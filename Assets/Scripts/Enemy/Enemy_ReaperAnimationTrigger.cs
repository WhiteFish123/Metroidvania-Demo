using UnityEngine;

public class Enemy_ReaperAnimationTrigger :Enemy_AnimationTriggers
{
    private Enemy_Reaper enemyReaper;
    protected override void Awake()
    {
        base.Awake();
        enemyReaper = GetComponentInParent<Enemy_Reaper>();
    }
    private void TeleportTrigger()
    {
        enemyReaper.setTeleportTrigger(true);
    }
}
