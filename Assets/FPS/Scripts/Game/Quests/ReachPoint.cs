using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;

public class ReachPoint : MonoBehaviour
{
    public Action OnPointReached;

    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Player"))
        {
            OnPointReached?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnPointReached = null;
    }
}
