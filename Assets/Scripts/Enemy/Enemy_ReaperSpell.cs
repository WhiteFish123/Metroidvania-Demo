using UnityEngine;

public class Enemy_ReaperSpell : MonoBehaviour
{
    private Entity_Combat combat;
    [SerializeField]private LayerMask whatIsTarget;
    [SerializeField]private Collider2D col;
    [SerializeField]private DamageScaleData damageScaleData;

    public void SetupSpell(Entity_Combat combat,DamageScaleData damageScaleData)
    {
        this.combat=combat;
        this.damageScaleData=damageScaleData;
        Destroy(gameObject,1.32f);
    }
    private void EnableCollider()=>col.enabled=true;
    private void DisableCollider()=>col.enabled=false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1<<collision.gameObject.layer)&whatIsTarget)!=0)
        {
            combat.PerformAttackOnTarget(collision.transform,damageScaleData);
            DisableCollider();
        }
        
    }
}
