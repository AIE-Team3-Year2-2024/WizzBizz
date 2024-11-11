using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenu : Menu
{
    [Header("Play Menu Stuff")]
    [Tooltip("What scene should be loaded when the start button is pressed.")]
    public string freeForAllScene = "Character";

    public string teamScene = "TeamCharacter";

    // Handle start button press.
    public void FreeForAllButton()
    {
        if (_menuManager)
            _menuManager.FadeToScene(freeForAllScene);
        else
            Debug.LogError("Menu couldn't find the Menu Manager. Is it in the scene?");
    }

    public void TeamButton()
    {
        if (_menuManager)
            _menuManager.FadeToScene(teamScene);
        else
            Debug.LogError("Menu couldn't find the Menu Manager. Is it in the scene?");
    }
}
