using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class GameManager : MonoBehaviour
{
    // Static reference
    public static GameManager Instance { get; private set; }

    public bool addingControllers;

    [Tooltip("The list of level scenes to be picked randomly (CAPITALIZATION MATTERS)")]
    [SerializeField]
    private string[] levels;

    [Tooltip("the prefab players use to controll the character screen")]
    [SerializeField]
    private GameObject cursorPrefab;

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
    }

    //input variables
    private List<Gamepad> m_controllers = new List<Gamepad>(); // list of connected controllers
    private List<PlayerController> m_activePlayerControllers; // currently instantiated players

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
                if (Gamepad.all[i].allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic) && !m_controllers.Contains(Gamepad.all[i]))
                {
                    //store controller and add player to leaderboard
                    m_controllers.Add(Gamepad.all[i]);

                    GameObject newPlayer = PlayerInput.Instantiate(cursorPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[i]).gameObject;
                    newPlayer.transform.SetParent(canvas.transform);
                }
            }
        }
    }
}
