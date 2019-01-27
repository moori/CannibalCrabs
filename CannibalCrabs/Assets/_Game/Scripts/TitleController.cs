using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{


    [FMODUnity.EventRef]
    public FMOD.Studio.EventInstance bgmMusic;

    public MenuState state = MenuState.MAIN;

    public GameObject mainCanvas;
    public GameObject creditsCanvas;
    public GameObject howToPlayCanvas;
    public GameObject playerJoinCanvas;

    public Image[] playerSlots;
    public Sprite[] playersprites;
    public Sprite emptySlot;


    public static bool[] players = new bool[4];

    private bool canClick = true;

    public enum MenuState
    {
        MAIN,
        HOW_TO_PLAY,
        PLAYER_JOIN,
        CREDITS
    }

    public void Delay()
    {
        canClick = false;
        this.DelayedAction(0.3f, () => canClick = true);
    }

    private void Start()
    {
        bgmMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/music_menu");
        bgmMusic.start();
    }

    public void OnClickPlay()
    {
        Delay();
        state = MenuState.PLAYER_JOIN;

        mainCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        playerJoinCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }

    public void OnClickCredits()
    {
        Delay();
        state = MenuState.CREDITS;

        mainCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        playerJoinCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }


    private void Update()
    {
        if (!canClick)
            return;

        switch (state)
        {
            case MenuState.MAIN:
                break;
            case MenuState.HOW_TO_PLAY:
                if (Input.GetButtonDown("Submit"))
                {
                    bgmMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                    SceneManager.LoadScene("Game");
                }
                break;
            case MenuState.PLAYER_JOIN:
                if (Input.GetButtonDown("Start"))
                {
                    Delay();
                    state = MenuState.HOW_TO_PLAY;

                    mainCanvas.SetActive(false);
                    howToPlayCanvas.SetActive(true);
                    playerJoinCanvas.SetActive(false);
                    creditsCanvas.SetActive(false);
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    Delay();
                    state = MenuState.MAIN;

                    mainCanvas.SetActive(true);
                    howToPlayCanvas.SetActive(false);
                    playerJoinCanvas.SetActive(false);
                    creditsCanvas.SetActive(false);
                }

                if (Input.GetButtonDown("P1_Shoot"))
                {
                    players[0] = true;
                    playerSlots[0].sprite = playersprites[0];
                    playerSlots[0].SetNativeSize();
                }
                if (Input.GetButtonDown("P2_Shoot"))
                {
                    players[1] = true;
                    playerSlots[1].sprite = playersprites[1];
                    playerSlots[1].SetNativeSize();
                }
                if (Input.GetButtonDown("P3_Shoot"))
                {
                    players[2] = true;
                    playerSlots[2].sprite = playersprites[2];
                    playerSlots[2].SetNativeSize();
                }
                if (Input.GetButtonDown("P4_Shoot"))
                {
                    players[3] = true;
                    playerSlots[3].sprite = playersprites[3];
                    playerSlots[3].SetNativeSize();
                }
                break;
            case MenuState.CREDITS:
                if (Input.GetButtonDown("Submit"))
                {
                    Delay();
                    state = MenuState.MAIN;

                    mainCanvas.SetActive(true);
                    howToPlayCanvas.SetActive(false);
                    playerJoinCanvas.SetActive(false);
                    creditsCanvas.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

}
