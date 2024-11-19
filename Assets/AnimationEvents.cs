using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationEvents : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Specific trigger wrappers because input actions can't have parameters.
    public void SetWhackTrigger(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("Whack");
        }
    }

    // Generic set trigger for use in animation events/scripts.
    public void SetAnimationTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }
}
