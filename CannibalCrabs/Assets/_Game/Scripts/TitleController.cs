﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

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


    public static bool[] players;

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

        TitleController.players = new bool[4];

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
                    Delay();
                    StartMatch();
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    Delay();
                    GoToMain();
                }
                break;
            case MenuState.PLAYER_JOIN:
                if (Input.GetButtonDown("Start"))
                {
                    GoToHowToPlay();
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    GoToMain();
                }

                for (int i = 0; i < 4; i++)
                {
                    if (Input.GetButtonDown($"P{i + 1}_Shoot"))
                    {
                        players[i] = true;
                        playerSlots[i].sprite = playersprites[i];
                        playerSlots[i].SetNativeSize();
                    }
                    if (Input.GetButtonDown($"P{i + 1}_Sacrifice"))
                    {
                        players[i] = false;
                        playerSlots[i].sprite = emptySlot;
                        playerSlots[i].SetNativeSize();
                    }
                }

                break;
            case MenuState.CREDITS:
                if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
                {
                    GoToMain();
                }
                break;
            default:
                break;
        }
    }

}
