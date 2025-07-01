using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Window:MonoBehaviour
{
    [SerializeField] protected bool isTimed;
    public abstract bool CheckAnswer();

    public bool IsTimed()
    {
        return isTimed;
    }
}
