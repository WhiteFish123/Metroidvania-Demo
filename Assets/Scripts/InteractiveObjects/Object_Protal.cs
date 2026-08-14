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
    private string returnSceneName;
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
        SaveManager.instance.GetGameData().inScenePortals.Clear();

        if(facingDir==-1)
            transform.Rotate(0,180,0);
    }
    public void DisableIfNeeded()
    {
        if(returningFormTown==false)//如果不是从城镇返回，就不管
            return;
        
        SaveManager.instance.GetGameData().inScenePortals.Remove(currentSceneName);

        isActive=false;
        transform.position=new Vector3(9999,9999);//默认
    }

    private void UseTeleport()
    {
        string destinationScene=InTown() ? returnSceneName : townSceneName;//如果在城镇，就返回到返回场景，否则就返回到城镇场景
    
        GameManager.instance.ChangeScene(destinationScene,RespawnType.Portal);
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
        returnSceneName=data.portalDestinationSceneName;//返回场景的名称
    }

    public void SaveData(ref GameData data)
    {
        data.returningFormTown=InTown();

        if(isActive&&InTown()==false)
        {
            data.inScenePortals[currentSceneName]=transform.position;
            data.portalDestinationSceneName=currentSceneName;
        }
        else
        {
            data.inScenePortals.Remove(currentSceneName);
        }

    }
}
