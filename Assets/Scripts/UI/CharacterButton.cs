using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    public GameObject character;

    public Color hoverColour;

    public Color selectedColour;

    public Color lockedColour = Color.gray;

    [HideInInspector] public int whosLockedIn = -1;
    [HideInInspector] public bool isLocked = false;

}
