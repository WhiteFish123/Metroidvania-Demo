using UnityEngine;

public class AudioManager : MonoBehaviour//播放背景音乐和全局音效
{
    public static AudioManager instance;
    [SerializeField]private AudioDataBaseSO audioDB;
    [SerializeField]private AudioSource bgmSource;
    [SerializeField]private AudioSource sfxSource;
    private Transform player;

    private void Awake()
    {
        if(instance!=null&&instance!=this)
        {
            Destroy(gameObject);
            return;
        }

        instance=this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(string soundName,AudioSource sfxSource,float minDistanceToHearSound=5)
    {
        if(player==null)
        {
            player=Player.instance.transform;
        }
        var data=audioDB.Get(soundName);
        if(data==null)
        {
            Debug.LogError("Attempt to play sound - "+soundName);
             return;
        }

        var clip=data.GetRandomClip();
        if(clip==null) return;

        float maxVolume=data.maxVolume;
        float distance=Vector3.Distance(sfxSource.transform.position,player.position);

        float t=Mathf.Clamp01(1-(distance/minDistanceToHearSound));

        sfxSource.pitch=Random.Range(.95f,1.1f);
        sfxSource.volume=Mathf.Lerp(0,maxVolume,t*t);
        sfxSource.PlayOneShot(clip);
    }

    public void PlayerGlobalSFX(string soundName)
    {
        var data=audioDB.Get(soundName);
        if(data==null) return;

        var clip=data.GetRandomClip();
        if(clip==null) return;

        Debug.Log("Played audio - "+soundName);
        sfxSource.pitch=Random.Range(.95f,1.1f);
        sfxSource.volume=data.maxVolume;
        sfxSource.PlayOneShot(clip);
    }
}
