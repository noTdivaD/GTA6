using UnityEngine;

public class GrandmaAnimatorController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Call this method from the throw script
    public void PlayFlyingAnimation()
    {
        if (animator)
        {
            animator.SetTrigger("Fly");
        }
    }
}
