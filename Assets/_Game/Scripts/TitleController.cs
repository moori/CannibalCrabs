using Rewired;
using System;
using System.Linq;
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
    public Image[] playerSlotsEmpty;
    public Sprite[] playersprites;
    public Sprite emptySlot;


    public static bool[] players;
    public static int[] playerColors;
    public float[] pTimes;

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

        players = new bool[4];
        playerColors = new int[] { 0, 1, 2, 3 };
        pTimes = new float[] { 0, 0, 0, 0 };

        GoToMain();

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
    public void GoToMain()
    {
        Delay();
        state = MenuState.MAIN;

        mainCanvas.SetActive(true);
        howToPlayCanvas.SetActive(false);
        playerJoinCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }
    public void GoToHowToPlay()
    {
        Delay();
        state = MenuState.HOW_TO_PLAY;

        mainCanvas.SetActive(false);
        howToPlayCanvas.SetActive(true);
        playerJoinCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    public void StartMatch()
    {
        bgmMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene("Game");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }


    private void FixedUpdate()
    {
        if (!canClick)
            return;

        switch (state)
        {
            case MenuState.MAIN:
                break;
            case MenuState.HOW_TO_PLAY:
                if (ReInput.players.GetPlayer(0).GetButtonDown("Submit") ||
                    ReInput.players.GetPlayer(1).GetButtonDown("Submit") ||
                    ReInput.players.GetPlayer(2).GetButtonDown("Submit") ||
                    ReInput.players.GetPlayer(3).GetButtonDown("Submit"))
                {
                    Delay();
                    StartMatch();
                }
                if (ReInput.players.GetPlayer(0).GetButtonDown("Cancel") ||
                    ReInput.players.GetPlayer(1).GetButtonDown("Cancel") ||
                    ReInput.players.GetPlayer(2).GetButtonDown("Cancel") ||
                    ReInput.players.GetPlayer(3).GetButtonDown("Cancel"))
                {
                    Delay();
                    GoToMain();
                }
                break;
            case MenuState.PLAYER_JOIN:
                if (ReInput.players.GetPlayer(0).GetButtonDown("Submit") ||
                    ReInput.players.GetPlayer(1).GetButtonDown("Submit") ||
                    ReInput.players.GetPlayer(2).GetButtonDown("Submit") ||
                    ReInput.players.GetPlayer(3).GetButtonDown("Submit"))
                {
                    GoToHowToPlay();
                }
                if (ReInput.players.GetPlayer(0).GetButtonDown("Cancel") ||
                    ReInput.players.GetPlayer(1).GetButtonDown("Cancel") ||
                    ReInput.players.GetPlayer(2).GetButtonDown("Cancel") ||
                    ReInput.players.GetPlayer(3).GetButtonDown("Cancel"))
                {
                    GoToMain();
                }

                for (int i = 0; i < 4; i++)
                {
                    //if (Input.GetButtonDown($"P{i + 1}_Shoot"))
                    if (ReInput.players.GetPlayer(i).GetButtonDown("Shoot"))
                    {
                        players[i] = true;
                        playerSlots[i].gameObject.SetActive(true);
                        playerSlots[i].color = Player.colors[playerColors[i]];
                        playerSlotsEmpty[i].gameObject.SetActive(false);
                    }
                    //if (Input.GetButtonDown($"P{i + 1}_Sacrifice"))
                    if (ReInput.players.GetPlayer(i).GetButtonDown("Sacrifice"))
                    {
                        players[i] = false;
                        playerSlots[i].gameObject.SetActive(false);
                        playerSlotsEmpty[i].gameObject.SetActive(true);
                    }
                    //if (Input.GetAxisRaw($"P{i + 1}_Horizontal") != 0)
                    if (ReInput.players.GetPlayer(i).GetAxis("Move Horizontal") != 0)
                    {
                        if (Time.time - pTimes[i] > 0.25f && players[i])
                        {
                            playerColors[i] = Player.colors.IndexOf(Player.colors.Where(c => !playerColors.Contains(Player.colors.IndexOf(c))).ToList().GetRandom());
                            //playerColors[i] = UnityEngine.Random.Range(0, Player.colors.Count);
                            playerSlots[i].color = Player.colors[playerColors[i]];
                            pTimes[i] = Time.time;
                        }
                    }
                }

                break;
            case MenuState.CREDITS:
                if (ReInput.players.GetPlayer(0).GetButtonDown("Submit") ||
                    ReInput.players.GetPlayer(1).GetButtonDown("Submit") ||
                    ReInput.players.GetPlayer(2).GetButtonDown("Submit") ||
                    ReInput.players.GetPlayer(3).GetButtonDown("Submit") ||
                    ReInput.players.GetPlayer(0).GetButtonDown("Cancel") ||
                    ReInput.players.GetPlayer(1).GetButtonDown("Cancel") ||
                    ReInput.players.GetPlayer(2).GetButtonDown("Cancel") ||
                    ReInput.players.GetPlayer(3).GetButtonDown("Cancel"))
                {
                    GoToMain();
                }
                break;
            default:
                break;
        }
    }

}
