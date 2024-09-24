using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    /// <summary>
    /// function for calling unpause on the game manager
    /// </summary>
    public void UnPause()
    {
        GameManager.Instance.UnPause();
    }
}
