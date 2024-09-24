using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    [Tooltip("the game object the will be passed to the game manager as the player prefab")]
    public GameObject character;

    [Tooltip("the colour of the cursour when hovering over this button")]
    public Color hoverColour;

    [Tooltip("the colour of the cursour after it has selected this button")]
    public Color selectedColour;

}
