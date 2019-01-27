using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{


    [FMODUnity.EventRef]
    public FMOD.Studio.EventInstance bgmMusic;

    public enum MenuState
    {
        MAIN,
        HOW_TO_PLAY,
        PLAYER_JOIN,
        CREDITS
    }

    private void Start()
    {
        bgmMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/music_menu");
        bgmMusic.start();
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene("Game");
        bgmMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void OnClickCredits()
    {

    }

    public void OnClickExit()
    {
        Application.Quit();
    }


}
