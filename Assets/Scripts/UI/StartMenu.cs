using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : Menu
{
    public string startGameScene = "Character";

    public void StartGameButton()
    {
        if (_menuManager)
            _menuManager.FadeToScene(startGameScene);
        else
            Debug.LogError("Menu couldn't find the Menu Manager. Is it in the scene?");
    }

    public void ExitGameButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
