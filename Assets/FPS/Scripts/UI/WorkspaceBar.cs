using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;

public class WorkspaceBar : MonoBehaviour
{

    [Tooltip("The floating bar pivot transform")]
    public Transform BarPivot;



    void Update()
    {
        // rotate bar to face the camera/player
        BarPivot.LookAt(Camera.main.transform.position);

    }
}
