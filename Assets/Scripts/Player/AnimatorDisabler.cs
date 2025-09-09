using UnityEngine;

public class AnimatorDisabler : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Update()
    {
        if (IsInIdleAndDone())
        {
            _animator.enabled = false;
            enabled = false;
        }
    }
    bool IsInIdleAndDone()
    {
        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName("Idle") && state.normalizedTime >= 1f;
    }
}
