using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroy the bubble explosion effect gameObject
/// </summary>
public class DestroyInSeconds : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1.5f);
    }
}
