using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UISelectHandler : MonoBehaviour, ISelectHandler
{
    [Tooltip("On selection callback.")]
    public UnityEvent selectCallback;

    // Stupid workaround to make the UnityUI element do something on selection.
    public void OnSelect(BaseEventData eventData)
    {
        selectCallback.Invoke();
    }
}
