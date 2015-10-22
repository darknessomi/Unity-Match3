using UnityEngine;
using System.Collections;

public class glass : MonoBehaviour
{

    public Animator amin;

    /// <summary>
    /// disable animator
    /// </summary>
    public void DisableAnimator()
    {
        amin.enabled = false;
    }
}
