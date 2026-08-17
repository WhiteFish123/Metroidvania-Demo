using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn();
    }
    public void PlayBTN()
    {
        AudioManager.instance.PlayerGlobalSFX("button_click");
        //GameManager.instance.ContinuePlay();
    }
    public void QuitGameBTN()
    {
        Application.Quit();
    }
}
