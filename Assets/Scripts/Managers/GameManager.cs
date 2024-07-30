using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static reference
    public static GameManager Instance { get; private set; }

    public bool addingControllers;

    [Tooltip("The list of level scenes to be picked randomly (CAPITALIZATION MATTERS)")]
    [SerializeField]
    private string[] _levels;

    [Tooltip("the prefab players use to controll the character screen")]
    [SerializeField]
    private GameObject _cursorPrefab;

    [Tooltip("the prefab that will be used to spawn the players")]
    [SerializeField]
    private GameObject _playerPrefab;

    private int _currentPlayerCount;

    public GameObject canvas;

    // Singleton instantiation
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    //input variables
    private List<Gamepad> _controllers = new List<Gamepad>(); // list of connected controllers
    private List<PlayerController> _activePlayerControllers = new List<PlayerController>(); // currently instantiated players

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(addingControllers)
        {
            for (int i = 0; i < Gamepad.all.Count; i++)
            {
                //check if the current gamepad has a button pressed and is not stored
                if (Gamepad.all[i].allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic) && !_controllers.Contains(Gamepad.all[i]))
                {
                    //store controller and add player to leaderboard
                    _controllers.Add(Gamepad.all[i]);

                    GameObject newPlayer = PlayerInput.Instantiate(_cursorPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[i]).gameObject;
                    newPlayer.transform.SetParent(canvas.transform);
                    _currentPlayerCount++;
                }
            }
        }
    }

    /// <summary>
    /// removes the players controller from the controller list and updates the player count
    /// </summary>
    /// <param name="player"></param>
    public void DissconectCursor(PlayerInput player)
    {
        foreach(InputDevice input in player.devices)
        {
            foreach(Gamepad controller in _controllers)
            {
                if(input == controller)
                {
                    _controllers.Remove(controller);
                    _currentPlayerCount--;
                    return;
                }
            }
        }
    }

    public int GetCurrntPlayerCount()
    {
        return _currentPlayerCount;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }


    public IEnumerator StartGameRoutine()
    {
        SceneManager.LoadScene(_levels[Random.Range(0, _levels.Length)]);

        //theese are here so that the playwers get spaw2ned in the new scene and not the old one
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        for (int i = 0; i < _controllers.Count; i++)
        {
            GameObject newPlayer = PlayerInput.Instantiate(_playerPrefab, controlScheme: "Gamepad", pairWithDevice: _controllers[i]).gameObject;
            _activePlayerControllers.Add(newPlayer.GetComponent<PlayerController>());
        }
        addingControllers = false;
    }
}
