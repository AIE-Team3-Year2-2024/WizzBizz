using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : Menu
{
    [Header("Start Menu Stuff - ")]
    [Tooltip("What scene should be loaded when the start button is pressed.")]
    public string startGameScene = "Character";

    // Handle start button press.
    public void StartGameButton()
    {
        if (_menuManager)
            _menuManager.FadeToScene(startGameScene);
        else
            Debug.LogError("Menu couldn't find the Menu Manager. Is it in the scene?");
    }

    // Handle exit button press.
    public void ExitGameButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
