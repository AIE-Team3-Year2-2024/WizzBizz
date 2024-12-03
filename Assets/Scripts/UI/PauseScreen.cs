using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseScreen : Menu
{
    [SerializeField, Tooltip("this will be the defualt button to hover over")]
    private GameObject _firstSelected;

    [SerializeField, Tooltip("the name of the scene to go to if the game is in team mode")]
    private string _teamLevelName;

    [SerializeField, Tooltip("the name of the scene to go to if the game is not in team mode")]
    private string _freeForAllLevelName;

    public override void Start()
    {
        base.Start();
        EventSystem.current.SetSelectedGameObject(_firstSelected);
    }

    public void QuitButton()
    {
        if (GameManager.Instance.teamMode == true)
        {
            MenuManager.Instance.FadeToScene(_teamLevelName);
        }
        else
        {
            MenuManager.Instance.FadeToScene(_freeForAllLevelName);
        }
        
        //MenuManager.Instance.InitializeManager();
        if (GameManager.Instance.arenaUICanvas != null)
            GameManager.Instance.arenaUICanvas.gameObject.SetActive(false);
    }
}
