using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WinScreen : MonoBehaviour
{
    [Tooltip("the array of images where the winners will be displayed MUST BE IN ORDER FROM WINNER TO FOURTH PLACE"), SerializeField]
    private RawImage[] _winnerImages;

    [Tooltip("the render texture of the raccoon"), SerializeField]
    private RenderTexture _raccoonRenderTexture;

    [Tooltip("the render texture of the penguin"), SerializeField]
    private RenderTexture _penguinRenderTexture;

    [Tooltip("the render texture of the lizard"), SerializeField]
    private RenderTexture _lizardRenderTexture;

    [Tooltip("the render texture of the frog"), SerializeField]
    private RenderTexture _frogRenderTexture;

    [Tooltip("how long this scene will show up for"), SerializeField]
    private float _sceneTime;

    [Tooltip("the scene to be loaded after this scene is done"), SerializeField]
    private string _afterWinScene;

    // Start is called before the first frame update
    void Start()
    {
        List<PlayerData> _gameWonData = GameManager.Instance.GetSortedPlayerData();
        int count = 0;
        for(int i = 0; i < _gameWonData.Count; i++)
        {
            if (_winnerImages[i] == null)
            {
                i += 999;
                return;
            }
            count++;
            switch(_gameWonData[i].characterSelect.name)
            {
                case ("Raccoon"):
                    {
                        _winnerImages[i].texture = _raccoonRenderTexture;
                        break;
                    }
                case ("Penguin"):
                    {
                        _winnerImages[i].texture = _penguinRenderTexture;
                        break;
                    }
                case ("Lizard"):
                    {
                        _winnerImages[i].texture = _lizardRenderTexture;
                        break;
                    }
                case ("Frog"):
                    {
                        _winnerImages[i].texture = _frogRenderTexture;
                        break;
                    }
            }
        }

        for (int i = count; i < 4; i++)
        {
            Destroy(_winnerImages[i].gameObject);
        }
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(_sceneTime);
        SceneManager.LoadScene(_afterWinScene);
    }
}
