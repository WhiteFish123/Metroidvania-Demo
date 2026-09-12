using UnityEngine;

public class Enemy_ArcherElfArrow : Enemy_ProjectileBase , ICounterable
{
    private Rigidbody2D rb;
    private Animator anim;

    public bool CanBeCountered => true;

    public void SetupArrow(float xVelocity,Entity_Combat combat)
    {
        rb=GetComponent<Rigidbody2D>();
        col=GetComponent<Collider2D>();
        anim=GetComponentInChildren<Animator>();

        this.combat=combat;
        rb.linearVelocity=new Vector2(xVelocity,rb.linearVelocity.y);

        if(rb.linearVelocity.x<0)
            transform.Rotate(0,180,0);
    }
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if(((1<<collision.gameObject.layer)&whatIsTarget.value)!=0)
    //     {
    //         combat.PerformAttackOnTarget(collision.transform);
    //         StuckIntoTarget(collision.transform);
    //     }
        
    // }
    protected override void OnHitTarget(Transform target)
    {
        combat.PerformAttackOnTarget(target);
        StuckIntoTarget(target);
    }
    private void StuckIntoTarget(Transform target)
    {
        rb.linearVelocity=Vector2.zero;
        rb.bodyType=RigidbodyType2D.Kinematic;
        col.enabled=false;
        anim.enabled=false;

        transform.parent=target;

        Destroy(gameObject,3);
    }

    public void HandleCounter()
    {
        rb.linearVelocity=new Vector2(rb.linearVelocity.x*-1,0);
        transform.Rotate(0,180,0);

        //让箭可以伤害敌人
        int enemyLayer=LayerMask.NameToLayer("Enemy");

        whatIsTarget = whatIsTarget | (1<<enemyLayer);
    }
}
