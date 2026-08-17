using UnityEngine;

public class AudioManager : MonoBehaviour//播放背景音乐和全局音效
{
    public static AudioManager instance;
    [SerializeField]private AudioDataBaseSO audioDB;
    [SerializeField]private AudioSource bgmSource;
    [SerializeField]private AudioSource sfxSource;

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

    public void PlaySFX(string soundName,AudioSource sfxSource)
    {
        var data=audioDB.Get(soundName);
        if(data==null)
        {
            Debug.LogError("Attempt to play sound - "+soundName);
             return;
        }

        var clip=data.GetRandomClip();
        if(clip==null) return;

        sfxSource.pitch=Random.Range(.95f,1.1f);
        sfxSource.clip=clip;
        sfxSource.volume=data.volume;
        sfxSource.PlayOneShot(clip);
    }
}
