using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public void UnPause()
    {
        GameManager.Instance.UnPause();
    }
}
