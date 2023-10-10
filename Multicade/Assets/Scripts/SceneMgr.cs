using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr inst;

    private void Awake()
    {
        inst = this;
    }

    public void loadHelpMenu()
    {

    }

    public void loadMainMenu()
    {
        GameMgr.inst.isBlackjack = false;
        GameMgr.inst.isPoker = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void loadBlackjack ()
    {
        GameMgr.inst.isBlackjack = true;
        SceneManager.LoadScene("Blackjack");
        AudioMgr.inst.playSelectSound();
    }

    public void loadPoker()
    {
        AudioMgr.inst.playSelectSound();
        GameMgr.inst.isPoker = true;
        SceneManager.LoadScene("Poker");
    }
}
