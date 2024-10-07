using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    [SerializeField, Tooltip("the name of the level to load")]
    private string _level;

    /// <summary>
    /// will load the inputted scene
    /// </summary>
    public void LoadScene()
    {
        SceneManager.LoadScene(_level);
        if (GameManager.Instance.arenaUICanvas != null)
            GameManager.Instance.arenaUICanvas.gameObject.SetActive(false);
    }
}
