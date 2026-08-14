using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    public void PlayBTN()
    {
        GameManager.instance.ContinuePlay();
    }
    public void QuitGameBTN()
    {
        Application.Quit();
    }
}
