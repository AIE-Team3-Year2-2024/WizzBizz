using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Per scene metadata.
public class SceneInfo : MonoBehaviour
{
    [Header("Optional - ")]
    [Tooltip("The menu that should be selected by default. (Optional)")]
    public Menu firstMenu = null; // Used by the menu manager.
}
