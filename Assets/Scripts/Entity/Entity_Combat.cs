using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;
    private Entity_SFX sfx;
    
    private Entity_VFX vfx;
    private Entity_Stats stats;

    public DamageScaleData basicAttackScale;

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;


    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        sfx = GetComponent<Entity_SFX>();

        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {   
        bool anyHit = false;

        foreach (var target in GetDetectedColliders(whatIsTarget))
        {
            AttackData attackData = stats.GetAttackData(basicAttackScale);
            if (ApplyDamageToTarget(target.transform, attackData))
                anyHit = true;
        }

        if (anyHit == false)
            sfx?.PlayAttackMiss();
    }

    private bool ApplyDamageToTarget(Transform target, AttackData attackData)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable == null)
            return false;

        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

        float physicalDamage = attackData.phyiscalDamage;
        float elementalDamage = attackData.elementalDamage;
        ElementType element = attackData.element;
        bool targetGotHit = damageable.TakeDamage(physicalDamage, elementalDamage, element, transform);

        if (element != ElementType.None)
            statusHandler?.ApplyStatusEffect(element, attackData.effectData);

        if (targetGotHit)
        {
            OnDoingPhysicalDamage?.Invoke(attackData.phyiscalDamage);
            vfx.CreateOnHitVFX(target.transform, attackData.isCrit, attackData.element);
            sfx?.PlayAttackHit();
        }

        return targetGotHit;
    }

    public void PerformAttackOnTarget(Transform target, DamageScaleData damageScaleData = null)
    {
        DamageScaleData scale = damageScaleData ?? basicAttackScale;
        AttackData attackData = stats.GetAttackData(scale);

        bool hit = ApplyDamageToTarget(target, attackData);

        if (hit == false)
            sfx?.PlayAttackMiss();
    }

    protected Collider2D[] GetDetectedColliders(LayerMask whatToDetect)
    {
        return Physics2D.OverlapCircleAll(targetCheck.position,targetCheckRadius, whatToDetect);
    }

    public void PerformAttackWithRadius(float customRadius)
    {
        float originalRadius = targetCheckRadius;
        targetCheckRadius = customRadius;
        PerformAttack();
        targetCheckRadius = originalRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}