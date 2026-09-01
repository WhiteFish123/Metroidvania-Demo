using UnityEngine;

public class Enemy_MageProjectile : MonoBehaviour
{
    private Entity_Combat combat;
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;
    [SerializeField] private float arcHeight = 2f;
    [SerializeField] private LayerMask whatCanCollideWith;

    public void SetupProjectile(Transform target,Entity_Combat combat)
    {
        this.combat=combat;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        anim.enabled=false;
        Vector2 velocity = CalculateBallisticVelocity(transform.position, target.position);
        rb.linearVelocity=velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1<<collision.gameObject.layer)&whatCanCollideWith.value)!=0)
        {
            combat.PerformAttackOnTarget(collision.transform);

            rb.linearVelocity=Vector2.zero;
            rb.gravityScale=0;
            anim.enabled=true;
            col.enabled=false;
            Destroy(gameObject,2f);
        }
    }

    private Vector2 CalculateBallisticVelocity(Vector2 start, Vector2 end)
    {
        //获取有效重力加速度（全局重力 × 此刚体的重力缩放）
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        //水平与垂直位移
        float displacementY = end.y - start.y;
        float displacementX = end.x - start.x;

        float peakHeight = Mathf.Max(arcHeight,end.y-start.y+.1f);
        //从起点飞到弧顶所需时间
        float timeToApex = Mathf.Sqrt(2 * peakHeight / gravity);

        //从弧顶落到目标高度所需时间
        float timeFromApex = Mathf.Sqrt(2 * (peakHeight - displacementY) / gravity);

        //总飞行时间 = 上升 + 下降
        float totalTime = timeToApex + timeFromApex;

        //垂直初速度：刚好能飞到弧顶高度
        float velocityY = Mathf.Sqrt(2 * gravity * peakHeight);

        //水平初速度：水平距离 / 总飞行时间
        float velocityX = displacementX / totalTime;

        //返回合成速度
        return new Vector2(velocityX, velocityY);
    }
}
