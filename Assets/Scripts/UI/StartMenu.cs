using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : Menu
{
    [Header("Start Menu Stuff - ")]

    public CanvasGroup menuGroup = null;
    public CanvasGroup exitPromptGroup = null;
    public Button exitFirstButton = null;

    public override void Start()
    {
        base.Start();

        menuGroup.alpha = 1.0f;
        menuGroup.interactable = true;

        exitPromptGroup.alpha = 0.0f;
        exitPromptGroup.interactable = false;
    }

    

    // Handle exit button press.
    public void ExitGameButton(bool confirm = false)
    {
        if (confirm)
        {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
        }
        else
        {
            if (exitPromptGroup && menuGroup)
            {
                menuGroup.interactable = false;
                menuGroup.alpha = 0.0f;
                exitPromptGroup.interactable = true;
                exitPromptGroup.alpha = 1.0f;

                _lastSelected = _menuManager._primaryEventSystem.currentSelectedGameObject.GetComponent<Selectable>();
                _menuManager._primaryEventSystem.SetSelectedGameObject(exitFirstButton.gameObject);
            }
        }
    }

    public void CancelExitButton()
    {
        if (exitPromptGroup && menuGroup)
        {
            menuGroup.interactable = true;
            menuGroup.alpha = 1.0f;
            exitPromptGroup.interactable = false;
            exitPromptGroup.alpha = 0.0f;

            _menuManager._primaryEventSystem.SetSelectedGameObject(_lastSelected.gameObject);
        }
    }
}
