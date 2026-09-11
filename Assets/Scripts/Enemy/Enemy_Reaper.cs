using System.Collections;
using UnityEngine;

public class Enemy_Reaper : Enemy , ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }

    public Enemy_ReaperBattleState reaperBattleState { get; private set; }
    public Enemy_ReaperTeleportState reaperTeleportState { get; private set; }
    public Enemy_ReaperAttackState reaperAttackState { get; private set; }
    public Enemy_ReaperSpellCastState reaperSpellCastState { get; private set; }

    [Header("Reaper specifics")]
    public float maxBattleIdleTime=5;

    [Header("Reaper SpellCast")]
    [SerializeField]private DamageScaleData spellDamageScale;
    [SerializeField]private GameObject spellCastPrefab;
    [SerializeField]private int amountToCast=6;
    [SerializeField]private float spellCastRate=1.2f;
    [SerializeField]private float spellCastStateCooldown=10;
    [SerializeField]private Vector2 playerOffsetPrediction;
    public float lastTimeCastedSpells=float.NegativeInfinity;
    public bool spellCastPerformed{get;private set;}
    private Player playerScript;

    [Header("Reaper Teleport")]
    [SerializeField] private BoxCollider2D arenaBounds;
    [SerializeField] private float offsetCenterY=1.748f;
    [SerializeField] private float chanceToTeleport=0.25f;
    private float defaultTeleportChance;

    public bool teleportTrigger{get;private set;}

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        reaperBattleState = new Enemy_ReaperBattleState(this, stateMachine, "battle");
        reaperAttackState = new Enemy_ReaperAttackState(this, stateMachine, "attack");
        reaperTeleportState = new Enemy_ReaperTeleportState(this, stateMachine, "teleport");
        reaperSpellCastState = new Enemy_ReaperSpellCastState(this, stateMachine, "spellCast");
        
        battleState = reaperBattleState;

        
    }

    protected override void Start()
    {
        base.Start();
        arenaBounds.transform.parent=null;
        defaultTeleportChance=chanceToTeleport;

        stateMachine.Initialize(idleState);

    }
   

    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }
    public override void TryEnterBattleState(Transform player)
    {
        if(stateMachine.currentState==reaperSpellCastState)
            return;
        base.TryEnterBattleState(player);
    }
    override public void SpecialAttack()
    {
        StartCoroutine(CastSpelCol());
    }

    private IEnumerator CastSpelCol()
    {
        if(playerScript==null)
            playerScript= player.GetComponent<Player>();

        for(int i=0;i<amountToCast;i++)
        {
            bool playerMoving=playerScript.rb.linearVelocity.magnitude>0;
            float xOffset=playerMoving?playerOffsetPrediction.x*playerScript.facingDir:0;
            Vector3 spellPosition=player.transform.position+new Vector3(xOffset,playerOffsetPrediction.y);

            Enemy_ReaperSpell spell
                =Instantiate(spellCastPrefab,spellPosition,Quaternion.identity).GetComponent<Enemy_ReaperSpell>();
           
            spell.SetupSpell(combat,spellDamageScale);
            yield return new WaitForSeconds(spellCastRate);
        }
        
        SetSpellCastPerformed(true);
    }

    public void SetSpellCastPerformed(bool spellCastStatus)=>spellCastPerformed=spellCastStatus;
    public void SetSpellCastOnCooldown()=>lastTimeCastedSpells=Time.time;
    public bool CanDoSpellCast()=>Time.time>lastTimeCastedSpells+spellCastStateCooldown;
    public bool shouldTeleport()
    {
        if(Random.value<chanceToTeleport)
        {
            chanceToTeleport=defaultTeleportChance;
            return true;
        }
        else
        {
            chanceToTeleport=chanceToTeleport+0.05f;
            return false;
        }
    }

    public void setTeleportTrigger(bool triggerStatus)=>teleportTrigger=triggerStatus;

    public Vector3 FindTeleportPoint()
    {
        int maxAttempts=10;
        float bossWithColliderHalf = col.bounds.size.x / 2;
        for(int i=0;i<maxAttempts;i++)//获取随机位置作为传送终点
        {
            float randomX=Random.Range(arenaBounds.bounds.min.x + bossWithColliderHalf, 
                                       arenaBounds.bounds.max.x - bossWithColliderHalf);

            Vector2 raycastPoint = new Vector2(randomX,arenaBounds.bounds.max.y);
            RaycastHit2D hit=Physics2D.Raycast(raycastPoint, Vector2.down,Mathf.Infinity,whatIsGround);//从随机点向下发射射线，检测地面 

            if(hit.collider!=null)
            {
                return hit.point+new Vector2(0,offsetCenterY);
            }
        }
        return transform.position;//如果没有找到合适的传送点，则返回当前的位置(假传送)
    }

}
