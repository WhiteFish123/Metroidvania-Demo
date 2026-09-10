using UnityEngine;

public class Enemy_Cow : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }

    public Enemy_CowIdleState cowIdleState { get; private set; }
    public Enemy_CowAttackState cowAttackState { get; private set; }
    public Enemy_CowSpecialAttackState cowSpecialAttackState { get; private set; }

    [Header("Cow Special Attack")]
    [SerializeField] private float specialAttackRadius = 3f;
    [SerializeField] private float specialAttackCooldown = 10f;
    public float lastTimeSpecialAttacked = float.NegativeInfinity;

    protected override void Awake()
    {
        base.Awake();

        cowIdleState = new Enemy_CowIdleState(this, stateMachine, "idle");
        cowAttackState = new Enemy_CowAttackState(this, stateMachine, "attack");
        cowSpecialAttackState = new Enemy_CowSpecialAttackState(this, stateMachine, "specialAttack");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(cowIdleState);
        Flip();
    }

    public bool CanDoSpecialAttack() => Time.time > lastTimeSpecialAttacked + specialAttackCooldown;
    public void SetSpecialAttackOnCooldown() => lastTimeSpecialAttacked = Time.time;

    public override void SpecialAttack()
    {
        combat.PerformAttackWithRadius(specialAttackRadius);
    }

    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;
        stateMachine.ChangeState(stunnedState);
    }
}