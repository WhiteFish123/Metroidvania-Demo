using UnityEngine;


public abstract class Enemy_ProjectileBase : MonoBehaviour
{
    protected Entity_Combat combat;
    [SerializeField] protected LayerMask whatIsTarget;
    [SerializeField] protected Collider2D col;

    protected virtual void OnTriggerEnter2D(Collider2D collision)//当与目标碰撞时调用
    {
        if (((1 << collision.gameObject.layer) & whatIsTarget.value) != 0)
            OnHitTarget(collision.transform);//各自调用特事故方法
    }

    protected abstract void OnHitTarget(Transform target);
}