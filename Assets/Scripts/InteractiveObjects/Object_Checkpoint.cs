using UnityEngine;

public class Object_Checkpoint : MonoBehaviour,ISaveable
{
    private Object_Checkpoint[] allCheckpoints;
    private Animator anim;

    private void Awake()
    {
        allCheckpoints=FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
        anim = GetComponentInChildren<Animator>();
    }
    
    public void ActivateCheckpoint(bool activate)
    {
        anim.SetBool("isActive",activate);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(var point in allCheckpoints)
            point.ActivateCheckpoint(false);

        SaveManager.instance.GetGameData().savedCheckpoint=transform.position;
        ActivateCheckpoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active=data.savedCheckpoint==transform.position;
        ActivateCheckpoint(active);

        if(active)
            Player.instance.TeleportPlayer(data.savedCheckpoint);
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
