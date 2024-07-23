using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerAnimationManager : NetworkBehaviour
{
    [SerializeField] private Animator _animator;

    public void SetAnimatorValue(AnimatorParameterType type, string animatorParameterName, float floatValue = 0, int intValue = 0, bool boolValue = false)
    {
        int paramHash = Animator.StringToHash(animatorParameterName);

        switch (type)
        {
            case AnimatorParameterType.FLOAT:
                _animator.SetFloat(paramHash, floatValue, 0.2f, Time.deltaTime);
                break;
            case AnimatorParameterType.INT:
                _animator.SetInteger(paramHash, intValue);
                break;
            case AnimatorParameterType.BOOL:
                _animator.SetBool(paramHash, boolValue);
                break;
            default:
                break;
        }
    }

    public void HandlePlayAnimation(string animationName)
    {
        _animator.CrossFade(animationName, 0.5f);
    }
}

public enum AnimatorParameterType
{
    FLOAT,
    BOOL,
    INT
}
