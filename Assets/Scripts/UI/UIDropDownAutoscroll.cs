using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Modified from https://gist.github.com/emredesu/af597de14a4377e1ecf96b6f7b6cc506
[RequireComponent(typeof(ScrollRect))]
public class UIDropDownAutoscroll : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    public float scrollSpeed = 10f;
    private bool mouseOver = false;

    private List<Selectable> m_Selectables = new List<Selectable>();
    private List<CanvasGroup> m_Groups = new List<CanvasGroup>();
    private ScrollRect m_ScrollRect;

    private Vector2 m_NextScrollPosition = Vector2.up;
    void OnEnable()
    {
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
            m_ScrollRect.content.GetComponentsInChildren(m_Groups);
        }
    }
    void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
    }
    void Start()
    {
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
            m_ScrollRect.content.GetComponentsInChildren(m_Groups);
        }
        ScrollToSelected(true);
    }
    void Update()
    {
        // If we are on mobile and we do not have a gamepad connected, do not do anything.
        if (SystemInfo.deviceType == DeviceType.Handheld && Gamepad.all.Count <= 1)
        {
            return;
        }

        // Scroll via input.
        InputScroll();
        if (!mouseOver)
        {
            // Lerp scrolling code.
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, scrollSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }
    }

#nullable enable
    void InputScroll()
    {
        if (m_Selectables.Count > 0)
        {
            //Keyboard? currentKeyboard = Keyboard.current;
            //Gamepad? currentGamepad = Gamepad.current;

            /*if (currentKeyboard != null)
            {
                if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                {
                    ScrollToSelected(false);
                }
            }*/

            // TODO: Pass in gamepad maybe?
            if (MenuManager.Instance)
            {
                if (MenuManager.Instance._primaryController.devices.Any(x => x is Gamepad g && !x.synthetic && 
                    (g.leftStick.y.magnitude != 0.0f || g.dpad.up.isPressed || g.dpad.down.isPressed)))
                {
                    ScrollToSelected(false);
                }
            }
        }
    }
#nullable disable
    void ScrollToSelected(bool quickScroll)
    {
        int selectedIndex = -1, groupIndex = -1;
        Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;

        if (selectedElement)
        {
            selectedIndex = m_Selectables.IndexOf(selectedElement);
            CanvasGroup group = selectedElement.GetComponent<CanvasGroup>();;
            if (group) groupIndex = m_Groups.IndexOf(group);
        }

        if (groupIndex > -1)
        {
            if (quickScroll)
            {
                m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (groupIndex / ((float)m_Groups.Count - 1)));
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            }
            else
            {
                m_NextScrollPosition = new Vector2(0, 1 - (groupIndex / ((float)m_Groups.Count - 1)));
            }
        }
        else if (selectedIndex > -1)
        {
            if (quickScroll)
            {
                m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            }
            else
            {
                m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
            }
        }
    }

    /*public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        ScrollToSelected(false);
    }*/
}
