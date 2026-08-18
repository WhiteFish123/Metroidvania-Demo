using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Audio/Audio Database")]
public class AudioDataBaseSO : ScriptableObject
{
     public List<AudioClipData> player;
     public List<AudioClipData> uiAudio;

     [Header("Music Lists")]
     public List<AudioClipData>mainMenuMusic;//主菜单背景音乐组
     public List<AudioClipData>levelMusic;//关卡背景音乐组

     private Dictionary<string,AudioClipData> clipCollection;

    private void OnEnable()
    {
        clipCollection=new Dictionary<string,AudioClipData>();

        AddToCollection(player);
        AddToCollection(uiAudio);
        AddToCollection(mainMenuMusic);
        AddToCollection(levelMusic);
    }

    public AudioClipData Get(string groupName)
    {
        return clipCollection.TryGetValue(groupName,out var data)?data:null;
    }
     private void AddToCollection(List<AudioClipData> listToAdd)
    {
        foreach(var data in listToAdd)
        {
            if(data!=null&&clipCollection.ContainsKey(data.audioName)==false)
            {
                clipCollection.Add(data.audioName,data);
            }
        }
    }
}

[System.Serializable]
public class AudioClipData
{
    public string audioName;
    public List<AudioClip> clips=new List<AudioClip>();
    [Range(0,1f)]public float maxVolume=1f;

    public AudioClip GetRandomClip()
    {
        if(clips==null||clips.Count==0)
        {
            return null;
        }

        return clips[Random.Range(0,clips.Count)];
    }
}