using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    [Space]
    [SerializeField] private RespawnType waypointType;
    [SerializeField] private RespawnType conntedWaypoint;
    [SerializeField]private Transform respawnPoint;
    [SerializeField]private bool canBeTriggered=true;

    public Vector3 GetPositionAndSetTriggerFalse()
    {

        canBeTriggered=false;
        return respawnPoint==null?transform.position:respawnPoint.position;
    }

    public RespawnType GetWaypointType()=>waypointType;

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
        GameManager.instance.ChangeScene(transferToScene,conntedWaypoint);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        canBeTriggered=true;
    }
}
