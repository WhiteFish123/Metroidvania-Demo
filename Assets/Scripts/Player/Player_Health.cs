using UnityEngine;

public class Player_Health : Entity_Health
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
            Die();
    }
    protected override void Die()
    {
        base.Die();
        //Player.OnPlayerDeath?.Invoke();

        //GameManager.instance.SetLastPlayerPosition(transform.position);//设置上次的死亡地点为当前玩家的位置
        GameManager.instance.RestartScene();//重新加载当前场景
    }
}
