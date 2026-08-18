using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn();//画面淡入
        AudioManager.instance.StartBGM("playlist_mainMenu");//开始播放主菜单背景音乐
    }
    public void PlayBTN()
    {
        AudioManager.instance.PlayerGlobalSFX("button_click");//播放按钮点击音效
        GameManager.instance.ContinuePlay();//加载上次存档继续游戏
    }
    public void QuitGameBTN()
    {
        Application.Quit();
    }
}