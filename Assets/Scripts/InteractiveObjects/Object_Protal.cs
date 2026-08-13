using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Portal : MonoBehaviour,ISaveable
{
    public static Object_Portal instance;
    public bool isActive {get;private set;}
    [SerializeField]private Vector2 defaultPosition;//默认位置
    [SerializeField]private string townSceneName= "Level_0";

    [SerializeField]private Transform respawnPoint;//重生点
    [SerializeField]private bool canBeTriggered;//是否可以触发被传送走

    private string currentSceneName;//当前场景的名称
    private bool returningFormTown;//是否从城镇返回

    private void Awake()
    {
        instance = this;
        currentSceneName=SceneManager.GetActiveScene().name;
        transform.position=new Vector3(9999,9999);//默认
    }

    public void ActivatePortal(Vector3 position,int facingDir=1)
    {
        isActive=true;
        transform.position=position;

        if(facingDir==-1)
            transform.Rotate(0,180,0);
    }

    private void UseTeleport()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canBeTriggered==false)
            return;
        
        UseTeleport();
    }
    private void OnTriggerExit2D(Collider2D collision)=>canBeTriggered=true;

    public void SetTrigger(bool trigger)=>canBeTriggered = trigger;

    public Vector2 GetPosition()=>respawnPoint !=null? respawnPoint.position:transform.position;

    private bool InTown()=>currentSceneName==townSceneName;
    public void LoadData(GameData data)
    {
        if(InTown()&&data.inScenePortals.Count>0)
        {
            transform.position=defaultPosition;
            isActive=true;
        }
        else if(data.inScenePortals.TryGetValue(currentSceneName,out Vector3 portalPosition))
        {
            transform.position=portalPosition;
            isActive=true;
        }

        returningFormTown=data.returningFormTown;
    }

    public void SaveData(ref GameData data)
    {
        if(isActive)
        {
            data.inScenePortals[currentSceneName]=transform.position
        }
        else
        {
            data.inScenePortals.Remove(currentSceneName);
        }

        data.portalDestinationSceneName=currentSceneName;
        data.returningFormTown=InTown();
    }
}
