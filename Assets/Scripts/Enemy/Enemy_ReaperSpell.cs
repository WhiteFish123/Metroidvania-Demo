using UnityEngine;

public class Enemy_ReaperSpell : Enemy_ProjectileBase
{
    [SerializeField]private DamageScaleData damageScaleData;

    public void SetupSpell(Entity_Combat combat,DamageScaleData damageScaleData)
    {
        this.combat=combat;
        this.damageScaleData=damageScaleData;
        Destroy(gameObject,1.32f);
    }
    private void EnableCollider()=>col.enabled=true;
    private void DisableCollider()=>col.enabled=false;
    protected override void OnHitTarget(Transform target)
    {
        combat.PerformAttackOnTarget(target,damageScaleData);
        DisableCollider();
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if(((1<<collision.gameObject.layer)&whatIsTarget)!=0)
    //     {
    //         combat.PerformAttackOnTarget(collision.transform,damageScaleData);
    //         DisableCollider();
    //     }
        
    // }
}
