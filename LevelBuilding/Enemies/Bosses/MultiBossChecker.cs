using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiBossChecker : MonoBehaviour
{
    public int numberOfCheckers;

    [Header("Events")]
    public UnityEvent onCheckedAll;

    private bool[] _checkers;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Add checker to array. 
    /// </summary>
    public void AddChecker()
    {
        for (int i = 0; i < _checkers.Length; i++)
        {
            if (! _checkers[i])
            {
                _checkers[i] = true;
                break;
            }
        }
    }

    /// <summary>
    /// Check all checkers.
    /// </summary>
    /// <returns></returns>
    private bool CheckCheckers()
    {
        foreach (bool checker in _checkers)
        {
            if (! checker)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Call this method to invoke event
    /// if all conditional checkers have been
    /// meet.
    /// </summary>
    public void CheckIfTriggerEvent()
    {
        if (CheckCheckers())
        {
            onCheckedAll?.Invoke();
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _checkers = new bool[numberOfCheckers];
    }
}
