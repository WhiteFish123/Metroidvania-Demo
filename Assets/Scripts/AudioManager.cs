using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField]private AudioDataBaseSO audioDB;//音频数据库（ScriptableObject）
    [SerializeField]private AudioSource bgmSource;//背景音乐播放源（循环、淡入淡出）
    [SerializeField]private AudioSource sfxSource;//全局音效播放源（PlayOneShot、可叠加）
    [Space]
    private Transform player;//玩家位置缓存，用于计算音效距离衰减
    private AudioClip lastMusicPlayed;//上一首播放的曲目，避免连续两首相同
    private string currentBgGroupName;//当前背景音乐组名，防重复触发切换
    private Coroutine currentBgmCo;//当前正在执行的音乐切换协程
    [SerializeField]private bool bgmShouldPlay;//总开关：控制背景音乐是否应该播放

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

    private void Update()
    {
        //当前曲目播放完毕且总开关打开 → 自动播放下一首（循环播放）
        if(bgmSource.isPlaying==false&&bgmShouldPlay)
        {
            if(string.IsNullOrEmpty(currentBgGroupName)==false)
                NextBGM(currentBgGroupName);
        }
        //总开关关闭但音乐还在播放 → 主动停止
        if(bgmSource.isPlaying&&bgmShouldPlay==false)
            StopMusic();
    }

    //外部入口：开始播放指定音乐组的背景音乐
    public void StartBGM(string musicGoup)
    {
        bgmShouldPlay=true;//打开总开关
        
        if(musicGoup==currentBgGroupName)//如果已经在播同一组音乐
            return;//忽略，避免重复触发淡入淡出

        NextBGM(musicGoup);//切换到新音乐组
    }

    //切换到指定音乐组的下一首曲目
    public void NextBGM(string musicGroup)
    {
        bgmShouldPlay=true;//确保总开关打开
        currentBgGroupName=musicGroup;//更新当前组名

        if(currentBgmCo!=null)
            StopCoroutine(currentBgmCo);//停掉旧的切换协程，防止冲突

        currentBgmCo=StartCoroutine(SwitchMusicCo(musicGroup));//启动新的切换协程
    }

    //停止所有背景音乐
    public void StopMusic()
    {
        bgmShouldPlay=false;//关闭总开关

        StartCoroutine(FadeVolumeCo(bgmSource,0,1));//1秒淡出到静音
        if(currentBgmCo!=null)
            StopCoroutine(currentBgmCo);//停止正在进行的切换协程
    }

    //核心协程：执行完整的曲目切换流程（淡出 → 换曲 → 淡入）
    private IEnumerator SwitchMusicCo(string musicGroup)
    {
        AudioClipData data=audioDB.Get(musicGroup);//从数据库查找音频组
        AudioClip nextmusic=data.GetRandomClip();//随机选一首

        if(data==null||data.clips.Count==0)//数据库中没有此组
        {
            Debug.Log("No music found for group - "+musicGroup);
            yield break;//退出协程
        }

        if(data.clips.Count>1)//同组有多首曲目
        {
            while(nextmusic==lastMusicPlayed)//循环直到选出不同的曲目
                nextmusic=data.GetRandomClip();//避免连续两首相同
        }

        if(bgmSource.isPlaying)//如果当前正在播放
            yield return FadeVolumeCo(bgmSource,0,1);//等待1秒淡出完成

        lastMusicPlayed=nextmusic;//记住当前曲目（下次避免重复）
        bgmSource.clip=nextmusic;//加载新音频
        bgmSource.volume=0;//音量从0开始
        bgmSource.Play();//开始播放（此时无声）
        
        StartCoroutine(FadeVolumeCo(bgmSource,data.maxVolume,1));//1秒淡入到目标音量
    }
    
    //通用工具：平滑过渡 AudioSource 的音量到目标值
    private IEnumerator  FadeVolumeCo(AudioSource source,float targetVolume,float duration)
    {
        float time=0;
        float startVolume=source.volume;//记录起始音量

        while(time<duration)
        {
            time+=Time.deltaTime;
            source.volume=Mathf.Lerp(startVolume,targetVolume,time/duration);//线性插值
            yield return null;//每帧更新一次
        }
        source.volume=targetVolume;//兜底：确保精确到达目标值
        yield return null;
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