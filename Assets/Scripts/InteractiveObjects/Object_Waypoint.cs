using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    [Space]
    public RespawnType waypointType;
    [SerializeField] private RespawnType conntedWaypoint;
    [SerializeField]private bool canBeTriggered=true;

    private void OnValidate()
    {
        gameObject.name = $"Object_Waypoint - "+waypointType.ToString()+" - "+transferToScene;

        if(waypointType==RespawnType.Enter)
            conntedWaypoint=RespawnType.Exit;

        if(waypointType==RespawnType.Exit)
            conntedWaypoint=RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canBeTriggered==false)
            return;
        SaveManager.instance.SaveData();
        SceneManager.LoadScene(transferToScene);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        canBeTriggered=true;
    }
}
