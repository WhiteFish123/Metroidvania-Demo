using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Vector3 lastDeathPosition;//保存上次的死亡地点

    private void Awake()
    {
        if(instance!=null&&instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLastDeathPosition(Vector3 position)=>lastDeathPosition=position;//设置上次的死亡地点
    
    public void RestartScene()
    {
        SaveManager.instance.SaveData();

        string sceneName=SceneManager.GetActiveScene().name;//获取当前场景的名称
        ChangeScene(sceneName,RespawnType.None);//重新加载当前场景
    }
    public void ChangeScene(string sceneName,RespawnType respawnType)
    {
        StartCoroutine(ChangeSceneCo(sceneName,respawnType));
    }

    private IEnumerator ChangeSceneCo(string sceneName,RespawnType respawnType)
    {
        //Fade
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(.2f);

        Vector3 position=GetNewPlayerPosition(respawnType);//获取新的玩家位置

        if(position!=Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }

    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if(type==RespawnType.None)
        {
            var data=SaveManager.instance.GetGameData();//获取游戏数据
            var checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
            var unlockedcheckpoints=checkpoints//获取所有检查点
                .Where(cp=>data.unlockedCheckpoints.TryGetValue(cp.GetCheckpointId(),out bool unlocked)&&unlocked)//获取所有解锁的检查点
                .Select(cp=>cp.GetPosition())//获取所有解锁的检查点的位置
                .ToList();//将所有解锁的检查点的位置转换为列表

            var enterWaypoints=FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)//获取所有路径点
                .Where(wp=>wp.GetWaypointType()==RespawnType.Enter)//获取所有进入点
                .Select(wp=>wp.GetPositionAndSetTriggerFalse())//获取所有进入点的位置
                .ToList();//将所有进入点的位置转换为列表

            var selectedPositions=unlockedcheckpoints.Concat(enterWaypoints).ToList();
            if(selectedPositions.Count==0)
                return Vector3.zero;

            return selectedPositions.
                OrderBy(position => Vector3.Distance(position,lastDeathPosition))//根据当前可重生的位置和上次死亡的位置的距离排序
                .First();//返回距离最近的重生点
        }
        return GetWaypointPosition(type);
    }

    private Vector3 GetWaypointPosition(RespawnType type)//获取重生点的位置
    {
        var waypoints =FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);//获取所有路径点

        foreach(var point in waypoints)//遍历所有路径点
        {
            if(point.GetWaypointType()==type)//如果路径点的类型与指定的类型相同
                return point.GetPositionAndSetTriggerFalse();//返回路径点的位置

        }
        return Vector3.zero;
    }
}
