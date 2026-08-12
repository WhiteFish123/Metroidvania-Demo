using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

        Vector3 position=GetWaypointPosition(respawnType);
        if(position!=Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }

    private Vector3 GetWaypointPosition(RespawnType type)
    {
        var waypoints =FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach(var point in waypoints)
        {
            if(point.GetWaypointType()==type)
            {
                point.SetCanBeTriggered(false);
                return point.GetPosition();
            }
        }
        return Vector3.zero;
    }
}
